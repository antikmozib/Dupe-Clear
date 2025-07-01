﻿// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using DupeClear.Helpers;
using DupeClear.Models;
using DupeClear.Models.Events;
using DupeClear.Models.Finder;
using DupeClear.Models.MessageBox;
using DupeClear.Models.Serializable;
using DupeClear.Native;
using EnumsNET;
using Mozib.AppUpdater;
using Mozib.AppUpdater.Helpers;
using Mozib.AppUpdater.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static DupeClear.Helpers.Common;

namespace DupeClear.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private const int ProgressReporterDelay = 250;

    private const int EtaReporterDelay = 5000;

    #region Fields

    private readonly UserData _userData;

    private readonly string _userDataFile = Path.Combine(
        GetAppUserDataDirectory(),
        Constants.UserPreferencesFileName);

    private readonly IFileService? _fileService;

    private readonly UpdateServiceProvider _updateService;

    private FinderOption _finderOptions;

    private MarkingCriteria _lastValidMarkingCriteria;

    private string _lastAddedDirectory;

    private int _oldPreviewPaneWidth;

    private IAsyncRelayCommand? _cancellableOperationCommand = null;

    #endregion // Fields

    #region Properties

    public Func<string, Task<IEnumerable<string>>>? AsyncDirectoryPicker { get; set; }

    public FileSaverDelegate? AsyncFileSaver { get; set; }

    public FilePickerDelegate? AsyncFilePicker { get; set; }

    public Func<MessageBoxViewModel, Task<MessageBoxResult>>? MessageBox { get; set; }

    private bool _includeSubdirectories;
    /// <summary>
    /// Include subdirectories of the included directories.
    /// </summary>
    public bool IncludeSubdirectories
    {
        get => _includeSubdirectories;
        set
        {
            if (_includeSubdirectories != value)
            {
                _includeSubdirectories = value;

                foreach (var dir in IncludedDirectories)
                {
                    dir.IncludeSubdirectories = value;
                }

                OnPropertyChanged();
            }
        }
    }

    public bool MatchSameFileName
    {
        get => _finderOptions.HasOption(FinderOption.SameFileName);
        set
        {
            if (value == false && _finderOptions.HasOption(FinderOption.SameFileName))
            {
                _finderOptions = _finderOptions.RemoveOption(FinderOption.SameFileName);
            }
            else if (value == true && !_finderOptions.HasOption(FinderOption.SameFileName))
            {
                _finderOptions = _finderOptions.AddOption(FinderOption.SameFileName);
            }

            if (value == false && !string.IsNullOrWhiteSpace(FileNamePattern))
            {
                if (!SavedFileNamePatterns.Contains(FileNamePattern))
                {
                    SavedFileNamePatterns.Add(FileNamePattern);
                }

                FileNamePattern = null;
            }

            OnPropertyChanged();
        }
    }

    public bool MatchSameContents
    {
        get => _finderOptions.HasOption(FinderOption.SameContents);
        set
        {
            if (value == false && _finderOptions.HasOption(FinderOption.SameContents))
            {
                _finderOptions = _finderOptions.RemoveOption(FinderOption.SameContents);
            }
            else if (value == true && !_finderOptions.HasOption(FinderOption.SameContents))
            {
                _finderOptions = _finderOptions.AddOption(FinderOption.SameContents);
                if (!MatchSameSize)
                {
                    MatchSameSize = true;
                }
            }

            OnPropertyChanged();
        }
    }

    public bool MatchSameType
    {
        get => _finderOptions.HasOption(FinderOption.SameType);
        set
        {
            if (value == false && _finderOptions.HasOption(FinderOption.SameType))
            {
                _finderOptions = _finderOptions.RemoveOption(FinderOption.SameType);
            }
            else if (value == true && !_finderOptions.HasOption(FinderOption.SameType))
            {
                _finderOptions = _finderOptions.AddOption(FinderOption.SameType);
            }

            OnPropertyChanged();
        }
    }

    public bool MatchSameSize
    {
        get => _finderOptions.HasOption(FinderOption.SameSize);
        set
        {
            if (value == false && _finderOptions.HasOption(FinderOption.SameSize))
            {
                _finderOptions = _finderOptions.RemoveOption(FinderOption.SameSize);
                if (MatchSameContents)
                {
                    MatchSameContents = false;
                }
            }
            else if (value == true && !_finderOptions.HasOption(FinderOption.SameSize))
            {
                _finderOptions = _finderOptions.AddOption(FinderOption.SameSize);
            }

            OnPropertyChanged();
        }
    }

    public bool MatchAcrossDirectories
    {
        get => _finderOptions.HasOption(FinderOption.AcrossDirectories);
        set
        {
            if (value == false && _finderOptions.HasOption(FinderOption.AcrossDirectories))
            {
                _finderOptions = _finderOptions.RemoveOption(FinderOption.AcrossDirectories);
            }
            else if (value == true && !_finderOptions.HasOption(FinderOption.AcrossDirectories))
            {
                _finderOptions = _finderOptions.AddOption(FinderOption.AcrossDirectories);
            }

            OnPropertyChanged();
        }
    }

    private string? _includedExtensions;
    public string IncludedExtensions
    {
        get => _includedExtensions ?? string.Empty;
        set
        {
            if (_includedExtensions != value)
            {
                _includedExtensions = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _fileNamePattern;
    public string? FileNamePattern
    {
        get => _fileNamePattern;
        set
        {
            if (_fileNamePattern != value)
            {
                _fileNamePattern = value;
                if (!string.IsNullOrWhiteSpace(value) && !MatchSameFileName)
                {
                    MatchSameFileName = true;
                }

                OnPropertyChanged();
            }
        }
    }

    public long MinimumFileLength { get; set; }

    public bool MatchDateCreated { get; set; }

    public bool MatchDateModified { get; set; }

    public DateTime? DateCreatedFrom { get; set; }

    public DateTime? DateCreatedTo { get; set; }

    public DateTime? DateModifiedFrom { get; set; }

    public DateTime? DateModifiedTo { get; set; }

    private bool _excludeSubdirectories;
    /// <summary>
    /// Include subdirectories of the excluded directories.
    /// </summary>
    public bool ExcludeSubdirectories
    {
        get => _excludeSubdirectories;
        set
        {
            if (_excludeSubdirectories != value)
            {
                _excludeSubdirectories = value;

                foreach (var dir in ExcludedDirectories)
                {
                    dir.IncludeSubdirectories = value;
                }

                OnPropertyChanged();
            }
        }
    }

    public bool ExcludeSystemFiles
    {
        get => _finderOptions.HasOption(FinderOption.ExcludeSystemFiles);
        set
        {
            if (value == false && _finderOptions.HasOption(FinderOption.ExcludeSystemFiles))
            {
                _finderOptions = _finderOptions.RemoveOption(FinderOption.ExcludeSystemFiles);
            }
            else if (value == true && !_finderOptions.HasOption(FinderOption.ExcludeSystemFiles))
            {
                _finderOptions = _finderOptions.AddOption(FinderOption.ExcludeSystemFiles);
            }

            OnPropertyChanged();
        }
    }

    public bool ExcludeHiddenFiles
    {
        get => _finderOptions.HasOption(FinderOption.ExcludeHiddenFiles);
        set
        {
            if (value == false && _finderOptions.HasOption(FinderOption.ExcludeHiddenFiles))
            {
                _finderOptions = _finderOptions.RemoveOption(FinderOption.ExcludeHiddenFiles);
            }
            else if (value == true && !_finderOptions.HasOption(FinderOption.ExcludeHiddenFiles))
            {
                _finderOptions = _finderOptions.AddOption(FinderOption.ExcludeHiddenFiles);
            }

            OnPropertyChanged();
        }
    }

    public string? ExcludedExtensions { get; set; }

    public bool AdditionalOptionsExpanded { get; set; }

    public ObservableCollection<string?> SavedFileNamePatterns { get; } = [];

    public ObservableCollection<string> SavedIncludedExtensions { get; } = [];

    public ObservableCollection<string?> SavedExcludedExtensions { get; } = [];

    public bool AreAllFilesLocked => DuplicateFiles.Any(x => !x.IsLocked);

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        private set
        {
            if (_isBusy != value)
            {
                _isBusy = value;

                Dispatcher.UIThread.Invoke(() =>
                {
                    AddDirectoryForInclusionCommand.NotifyCanExecuteChanged();
                    RemoveIncludedDirectoryCommand.NotifyCanExecuteChanged();
                    ApplyMarkingToSelectedIncludedDirectoriesCommand.NotifyCanExecuteChanged();
                    InvertMakingOfSelectedIncludedDirectoryCommand.NotifyCanExecuteChanged();
                    AddDirectoryForExclusionCommand.NotifyCanExecuteChanged();
                    RemoveExcludedDirectoryCommand.NotifyCanExecuteChanged();
                    ApplyMarkingToSelectedExcludedDirectoriesCommand.NotifyCanExecuteChanged();
                    InvertMakingOfSelectedExcludedDirectoryCommand.NotifyCanExecuteChanged();
                    MoveDirectoryUpCommand.NotifyCanExecuteChanged();
                    MoveDirectoryDownCommand.NotifyCanExecuteChanged();
                    SearchCommand.NotifyCanExecuteChanged();
                    CancelOperationCommand.NotifyCanExecuteChanged();
                    KeepEarliestCreatedCommand.NotifyCanExecuteChanged();
                    KeepLatestCreatedCommand.NotifyCanExecuteChanged();
                    KeepEarliestModifiedCommand.NotifyCanExecuteChanged();
                    KeepLatestModifiedCommand.NotifyCanExecuteChanged();
                    DeleteMarkedFilesCommand.NotifyCanExecuteChanged();
                    ExportCommand.NotifyCanExecuteChanged();
                    ImportCommand.NotifyCanExecuteChanged();
                    OpenCommand.NotifyCanExecuteChanged();
                    OpenContainingFolderCommand.NotifyCanExecuteChanged();
                    DelistCommand.NotifyCanExecuteChanged();
                    DelistGroupCommand.NotifyCanExecuteChanged();
                    LockGroupCommand.NotifyCanExecuteChanged();
                    UnlockGroupCommand.NotifyCanExecuteChanged();
                    MarkAllFromThisDirectoryCommand.NotifyCanExecuteChanged();
                    UnmarkAllFromThisDirectoryCommand.NotifyCanExecuteChanged();
                    ApplyMarkingToSelectedSearchResultsCommand.NotifyCanExecuteChanged();
                    InvertMarkingOfSelectedSearchResultCommand.NotifyCanExecuteChanged();
                    RefreshCommand.NotifyCanExecuteChanged();
                    FindWithinSearchResultsCommand.NotifyCanExecuteChanged();
                    ClearFindWithinSearchResultsItemsCommand.NotifyCanExecuteChanged();
                });

                OnPropertyChanged();
            }
        }
    }

    public bool IsSearching => SearchCommand.IsRunning;

    public bool OperationCanBeCanceled => _cancellableOperationCommand != null;

    private string _primaryStatus = "Ready";
    public string PrimaryStatus
    {
        get => _primaryStatus;
        set
        {
            if (_primaryStatus != value)
            {
                _primaryStatus = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _secondaryStatus;
    public string? SecondaryStatus
    {
        get => _secondaryStatus;
        set
        {
            if (_secondaryStatus != value)
            {
                _secondaryStatus = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _tertiaryStatus;
    public string? TertiaryStatus
    {
        get => _tertiaryStatus;
        set
        {
            if (_tertiaryStatus != value)
            {
                _tertiaryStatus = value;
                OnPropertyChanged();
            }
        }
    }

    private string? _quaternaryStatus;
    public string? QuaternaryStatus
    {
        get => _quaternaryStatus;
        set
        {
            if (_quaternaryStatus != value)
            {
                _quaternaryStatus = value;
                OnPropertyChanged();
            }
        }
    }

    private double _progress;
    public double Progress
    {
        get => _progress;
        set
        {
            if (_progress != value)
            {
                _progress = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _showFindWithinSearchResultsPane;
    public bool ShowFindWithinSearchResultsPane
    {
        get => _showFindWithinSearchResultsPane;
        set
        {
            if (_showFindWithinSearchResultsPane != value)
            {
                _showFindWithinSearchResultsPane = value;
                if (value == true)
                {
                    FindWithinSearchResultsPaneHeight = 256;
                }
                else
                {
                    FindWithinSearchResultsPaneHeight = 0;
                    FindWithinSearchResultsLookFor = null;
                    FindWithinSearchResultsItems.Clear();
                }

                OnPropertyChanged();
            }
        }
    }

    private int _findWithinSearchResultsPaneHeight;
    public int FindWithinSearchResultsPaneHeight
    {
        get => _findWithinSearchResultsPaneHeight;
        set
        {
            if (_findWithinSearchResultsPaneHeight != value)
            {
                _findWithinSearchResultsPaneHeight = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _showPreview;
    public bool ShowPreview
    {
        get => _showPreview;
        set
        {
            if (_showPreview != value)
            {
                _showPreview = value;
                if (value == false)
                {
                    PreviewPaneWidth = 0;
                }
                else
                {
                    PreviewPaneWidth = _oldPreviewPaneWidth;
                }

                OnPropertyChanged();
            }
        }
    }

    private int _previewPaneWidth;
    public int PreviewPaneWidth
    {
        get => _previewPaneWidth;
        set
        {
            if (_previewPaneWidth != value)
            {
                _previewPaneWidth = value;
                if (value > 0)
                {
                    _oldPreviewPaneWidth = value;
                }

                OnPropertyChanged();
            }
        }
    }

    private Bitmap? _previewImage;
    public Bitmap? PreviewImage
    {
        get => _previewImage;
        private set
        {
            if (_previewImage != value)
            {
                _previewImage = value;
                OnPropertyChanged();
            }
        }
    }

    private MarkingCriteria _selectedMarkingOption;
    public MarkingCriteria SelectedMarkingCriteria
    {
        get => _selectedMarkingOption;
        set
        {
            if (_selectedMarkingOption != value)
            {
                _selectedMarkingOption = value;

                if (value != MarkingCriteria.Custom)
                {
                    _lastValidMarkingCriteria = value;
                }

                OnPropertyChanged();
            }
        }
    }

    private string? _findWithinSearchResultsLookFor;
    public string? FindWithinSearchResultsLookFor
    {
        get => _findWithinSearchResultsLookFor;
        set
        {
            if (_findWithinSearchResultsLookFor != value)
            {
                _findWithinSearchResultsLookFor = value;

                Dispatcher.UIThread.Invoke(() =>
                {
                    FindWithinSearchResultsCommand.NotifyCanExecuteChanged();
                    ClearFindWithinSearchResultsItemsCommand.NotifyCanExecuteChanged();
                });

                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<DuplicateFile> FindWithinSearchResultsItems { get; } = [];

    private DuplicateFile? _selectedFindWithinSearchResultsItem;
    public DuplicateFile? SelectedFindWithinSearchResultsItem
    {
        get => _selectedFindWithinSearchResultsItem;
        set
        {
            if (_selectedFindWithinSearchResultsItem != value)
            {
                _selectedFindWithinSearchResultsItem = value;
                if (value != null)
                {
                    if (DuplicateFiles.Contains(value))
                    {
                        SelectedDuplicateFile = value;
                    }
                }

                OnPropertyChanged();
            }
        }
    }

    private Theme _theme;
    public Theme Theme
    {
        get => _theme;
        private set
        {
            if (_theme != value)
            {
                _theme = value;
                OnPropertyChanged();
            }
        }
    }

    public bool AutoUpdateCheck { get; set; }

    public ObservableCollection<SearchDirectory> IncludedDirectories { get; } = [];

    private SearchDirectory? _selectedIncludedDirectory = null;
    public SearchDirectory? SelectedIncludedDirectory
    {
        get => _selectedIncludedDirectory;
        set
        {
            if (_selectedIncludedDirectory != value)
            {
                _selectedIncludedDirectory = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<SearchDirectory> SelectedIncludedDirectories { get; } = [];

    public ObservableCollection<SearchDirectory> ExcludedDirectories { get; } = [];

    public SearchDirectory? SelectedExcludedDirectory { get; set; } = null;

    public ObservableCollection<SearchDirectory> SelectedExcludedDirectories { get; } = [];

    public ObservableCollection<DuplicateFile> DuplicateFiles { get; } = [];

    private DuplicateFile? _selectedDuplicateFile;
    public DuplicateFile? SelectedDuplicateFile
    {
        get => _selectedDuplicateFile;
        set
        {
            if (_selectedDuplicateFile != value)
            {
                _selectedDuplicateFile = value;

                PreviewImage = null;
                if (value != null && File.Exists(value.FullName))
                {
                    try
                    {
                        PreviewImage = _fileService?.GetPreview(value.FullName);
                    }
                    catch
                    {

                    }
                }

                OpenCommand.NotifyCanExecuteChanged();
                OpenContainingFolderCommand.NotifyCanExecuteChanged();
                DelistCommand.NotifyCanExecuteChanged();
                DelistGroupCommand.NotifyCanExecuteChanged();
                MarkAllFromThisDirectoryCommand.NotifyCanExecuteChanged();
                UnmarkAllFromThisDirectoryCommand.NotifyCanExecuteChanged();

                OnPropertyChanged();
            }
        }
    }

    #endregion // Properties

    #region Events

    public event EventHandler? InvalidExtensionsToIncludeEntered;

    public event EventHandler? InvalidExtensionsToExcludeEntered;

    public event EventHandler? SearchPerformed;

    public event FindWithinSearchResultsSearchPerformedEventHandler? FindWithinResultsPerformed;

    public event EventHandler? Closed;

    #endregion // Events

    #region Ctor

    public MainViewModel() : this(null)
    { }

    public MainViewModel(IFileService? fileService)
    {
        _fileService = fileService;
#if DEBUG
        _updateService = new UpdateServiceProvider(
            Constants.UpdateApiAddress,
            Constants.UpdateApiAppId,
            FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).ProductVersion,
            AppArchitecture.X86_64,
            InstallMethod.Installer);
#else
        _updateService = new UpdateServiceProvider(
            Assembly.GetEntryAssembly()!.Location,
            Constants.UpdateApiAddress,
            Constants.UpdateApiAppId,
            IntPtr.Size == 8 ? AppArchitecture.X86_64 : AppArchitecture.X86_32);
#endif

        DuplicateFiles.CollectionChanged += DuplicateFiles_CollectionChanged;
        IncludedDirectories.CollectionChanged += IncludedDirectories_CollectionChanged;
        ExcludedDirectories.CollectionChanged += ExcludedDirectories_CollectionChanged;
        SelectedIncludedDirectories.CollectionChanged += SelectedIncludedDirectories_CollectionChanged;
        SelectedExcludedDirectories.CollectionChanged += SelectedExcludedDirectories_CollectionChanged;
        FindWithinSearchResultsItems.CollectionChanged += FindWithinSearchResultsItems_CollectionChanged;

        UserData userData;
        if (File.Exists(_userDataFile))
        {
            var jsonString = File.ReadAllText(_userDataFile);
            userData = JsonSerializer.Deserialize<UserData>(jsonString) ?? new UserData();
        }
        else
        {
            userData = new UserData();
        }

        _userData = userData;

        ChangeTheme((Theme)userData.Theme);

        _lastAddedDirectory = userData.LastAddedDirectory;
        _oldPreviewPaneWidth = userData.PreviewPaneWidth;

        foreach (var item in userData.IncludedDirectories)
        {
            if (!string.IsNullOrEmpty(item.FullName))
            {
                IncludedDirectories.Add(new SearchDirectory(item.FullName, _fileService)
                {
                    IsMarked = item.IsMarked,
                    IncludeSubdirectories = IncludeSubdirectories
                });
            }
        }

        foreach (var item in userData.ExcludedDirectories)
        {
            if (!string.IsNullOrEmpty(item.FullName))
            {
                ExcludedDirectories.Add(new SearchDirectory(item.FullName, _fileService)
                {
                    IsMarked = item.IsMarked,
                    IncludeSubdirectories = userData.ExcludeSubdirectories
                });
            }
        }

        userData.SavedFileNamePatterns.ForEach(x => SavedFileNamePatterns.Add(x));
        userData.SavedIncludedExtensions.ForEach(x => SavedIncludedExtensions.Add(x));
        userData.SavedExcludedExtensions.ForEach(x => SavedExcludedExtensions.Add(x));

        IncludeSubdirectories = userData.IncludeSubdirectories;
        ExcludeSubdirectories = userData.ExcludeSubdirectories;
        FileNamePattern = userData.FileNamePattern;
        MinimumFileLength = userData.MinFileLength;
        SelectedMarkingCriteria = (MarkingCriteria)userData.LastSelectedMarkingCriteria;
        AdditionalOptionsExpanded = userData.AdditionalOptionsExpanded;
        IncludedExtensions = userData.IncludedExtensions;
        ExcludedExtensions = userData.ExcludedExtensions;
        ShowPreview = userData.ShowPreview;
        AutoUpdateCheck = userData.AutoUpdateCheck;
        DateModifiedFrom = userData.DateModifiedFrom;
        DateModifiedTo = userData.DateModifiedTo;
        DateCreatedFrom = userData.DateCreateFrom;
        DateCreatedTo = userData.DateCreateTo;

        if (userData.MatchSameFileName)
        {
            _finderOptions = _finderOptions.AddOption(FinderOption.SameFileName);
        }

        if (userData.MatchSameContents)
        {
            _finderOptions = _finderOptions.AddOption(FinderOption.SameContents);
        }

        if (userData.MatchSameType)
        {
            _finderOptions = _finderOptions.AddOption(FinderOption.SameType);
        }

        if (userData.MatchSameSize)
        {
            _finderOptions = _finderOptions.AddOption(FinderOption.SameSize);
        }

        if (userData.MatchAcrossDirectories)
        {
            _finderOptions = _finderOptions.AddOption(FinderOption.AcrossDirectories);
        }

        if (userData.ExcludeSystemFiles)
        {
            _finderOptions = _finderOptions.AddOption(FinderOption.ExcludeSystemFiles);
        }

        if (userData.ExcludeHiddenFiles)
        {
            _finderOptions = _finderOptions.AddOption(FinderOption.ExcludeHiddenFiles);
        }

        // Check for updates.
        if (AutoUpdateCheck)
        {
            Task.Run(async () => await CheckForUpdatesAsync(true));
        }
    }

    #endregion // Ctor

    #region Event Handlers

    private void IncludedDirectories_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems!.Cast<INotifyPropertyChanged>())
            {
                item.PropertyChanged += IncludedDirectory_PropertyChanged;
            }
        }
        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        {
            foreach (var item in e.OldItems!.Cast<INotifyPropertyChanged>())
            {
                item.PropertyChanged -= IncludedDirectory_PropertyChanged;
            }
        }

        UpdateIsDirectoryExcluded();

        Dispatcher.UIThread.Invoke(() =>
        {
            MoveDirectoryUpCommand.NotifyCanExecuteChanged();
            MoveDirectoryDownCommand.NotifyCanExecuteChanged();
            SearchCommand.NotifyCanExecuteChanged();
        });
    }

    private void IncludedDirectory_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SearchDirectory.IsMarked))
        {
            Dispatcher.UIThread.Invoke(SearchCommand.NotifyCanExecuteChanged);
        }
    }

    private void ExcludedDirectories_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems!.Cast<INotifyPropertyChanged>())
            {
                item.PropertyChanged += ExcludedDirectory_PropertyChanged;
            }
        }
        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        {
            foreach (var item in e.OldItems!.Cast<INotifyPropertyChanged>())
            {
                item.PropertyChanged -= ExcludedDirectory_PropertyChanged;
            }
        }

        UpdateIsDirectoryExcluded();
    }

    private void ExcludedDirectory_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(SearchDirectory.IncludeSubdirectories):
            case nameof(SearchDirectory.IsMarked):
                UpdateIsDirectoryExcluded();

                break;

            default:
                break;
        }
    }

    private void SelectedIncludedDirectories_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.UIThread.Invoke(RemoveIncludedDirectoryCommand.NotifyCanExecuteChanged);
    }

    private void SelectedExcludedDirectories_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.UIThread.Invoke(RemoveExcludedDirectoryCommand.NotifyCanExecuteChanged);
    }

    private void DuplicateFiles_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
        {
            foreach (var item in e.NewItems!.Cast<INotifyPropertyChanged>())
            {
                item.PropertyChanged += DuplicateFile_PropertyChanged;
            }
        }
        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
        {
            foreach (var item in e.OldItems!.Cast<INotifyPropertyChanged>())
            {
                item.PropertyChanged -= DuplicateFile_PropertyChanged;
            }
        }

        SelectedMarkingCriteria = MarkingCriteria.Custom;
        Dispatcher.UIThread.Invoke(() =>
        {
            ExportCommand.NotifyCanExecuteChanged();
            MarkAllCommand.NotifyCanExecuteChanged();
            UnmarkAllCommand.NotifyCanExecuteChanged();
            DeleteMarkedFilesCommand.NotifyCanExecuteChanged();
            RefreshCommand.NotifyCanExecuteChanged();
        });

        OnPropertyChanged(nameof(AreAllFilesLocked));
    }

    private void DuplicateFile_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(DuplicateFile.IsDeleted):
            case nameof(DuplicateFile.IsMarked):
                SelectedMarkingCriteria = MarkingCriteria.Custom;
                Dispatcher.UIThread.Invoke(() =>
                {
                    MarkAllCommand.NotifyCanExecuteChanged();
                    UnmarkAllCommand.NotifyCanExecuteChanged();
                    DeleteMarkedFilesCommand.NotifyCanExecuteChanged();
                });

                if (e.PropertyName == nameof(DuplicateFile.IsDeleted))
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        KeepEarliestCreatedCommand.NotifyCanExecuteChanged();
                        KeepLatestCreatedCommand.NotifyCanExecuteChanged();
                        KeepEarliestModifiedCommand.NotifyCanExecuteChanged();
                        KeepLatestModifiedCommand.NotifyCanExecuteChanged();
                        DeleteMarkedFilesCommand.NotifyCanExecuteChanged();
                        OpenCommand.NotifyCanExecuteChanged();
                    });
                }

                break;

            case nameof(DuplicateFile.IsLocked):
                OnPropertyChanged(nameof(AreAllFilesLocked));

                break;

            default:
                break;
        }
    }

    private void FindWithinSearchResultsItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.UIThread.Invoke(ClearFindWithinSearchResultsItemsCommand.NotifyCanExecuteChanged);
    }

    protected void RaiseEvent(EventHandler? handler)
    {
        handler?.Invoke(this, new EventArgs());
    }

    protected void RaiseFindWithinResultsPerformedEvent(FindWithinSearchResultsSearchPerformedEventHandler? handler, FindWithinSearchResultsSearchPerformedEventArgs e)
    {
        handler?.Invoke(this, e);
    }

    #endregion // Event Handlers

    #region Command Implementations

    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private async Task AddDirectoryForInclusionAsync(object? arg)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        if (AsyncDirectoryPicker != null)
        {
            AddDirectoryForInclusion(await AsyncDirectoryPicker.Invoke(_lastAddedDirectory));
        }
    }

    [RelayCommand(CanExecute = nameof(CanRemoveIncludedDirectory))]
    private void RemoveIncludedDirectory(object? arg)
    {
        if (!CanRemoveIncludedDirectory(arg))
        {
            return;
        }

        foreach (var item in SelectedIncludedDirectories.ToArray())
        {
            IncludedDirectories.Remove(item);
        }

        SelectedIncludedDirectories.Clear();
    }

    private bool CanRemoveIncludedDirectory(object? arg)
    {
        return !IsBusy && SelectedIncludedDirectories.Any();
    }

    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private void ApplyMarkingToSelectedIncludedDirectories(object? arg)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        if (arg is SearchDirectory dir)
        {
            if (SelectedIncludedDirectories.Contains(dir))
            {
                foreach (var item in SelectedIncludedDirectories.Where(x => x != dir && x.IsMarked != dir.IsMarked))
                {
                    item.IsMarked = dir.IsMarked;
                }
            }
        }
    }

    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private void InvertMakingOfSelectedIncludedDirectory(object? arg)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        if (SelectedIncludedDirectory != null)
        {
            SelectedIncludedDirectory.IsMarked = !SelectedIncludedDirectory.IsMarked;
            ApplyMarkingToSelectedIncludedDirectoriesCommand.Execute(SelectedIncludedDirectory);
        }
    }

    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private async Task AddDirectoryForExclusionAsync(object? arg)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        if (AsyncDirectoryPicker != null)
        {
            await Task.Run(async () =>
            {
                var dirs = await AsyncDirectoryPicker.Invoke(_lastAddedDirectory);
                SetBusy("Adding...");
                AddDirectoryForExclusion(dirs);
                SetBusy(false);
            });
        }
    }

    [RelayCommand(CanExecute = nameof(CanRemoveExcludedDirectory))]
    private void RemoveExcludedDirectory(object? arg)
    {
        if (!CanRemoveExcludedDirectory(arg))
        {
            return;
        }

        foreach (var item in SelectedExcludedDirectories.ToArray())
        {
            ExcludedDirectories.Remove(item);
        }

        SelectedExcludedDirectories.Clear();
    }

    private bool CanRemoveExcludedDirectory(object? arg)
    {
        return !IsBusy && SelectedExcludedDirectories.Any();
    }

    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private void ApplyMarkingToSelectedExcludedDirectories(object? arg)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        if (arg is SearchDirectory dir)
        {
            if (SelectedExcludedDirectories.Contains(dir))
            {
                foreach (var item in SelectedExcludedDirectories.Where(x => x != dir && x.IsMarked != dir.IsMarked))
                {
                    item.IsMarked = dir.IsMarked;
                }
            }
        }
    }

    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private void InvertMakingOfSelectedExcludedDirectory(object? arg)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        if (SelectedExcludedDirectory != null)
        {
            SelectedExcludedDirectory.IsMarked = !SelectedExcludedDirectory.IsMarked;
            ApplyMarkingToSelectedExcludedDirectoriesCommand.Execute(SelectedExcludedDirectory);
        }
    }

    [RelayCommand(CanExecute = nameof(CanMoveDirectoryUp))]
    private void MoveDirectoryUp(object? arg)
    {
        if (!CanMoveDirectoryUp(arg))
        {
            return;
        }

        if (arg is SearchDirectory dir)
        {
            var dirIndex = IncludedDirectories.IndexOf(dir);
            if (dirIndex > 0)
            {
                IncludedDirectories.Remove(dir);
                IncludedDirectories.Insert(dirIndex - 1, dir);

                SelectedIncludedDirectories.Clear();
                SelectedIncludedDirectory = dir;
            }
        }
    }

    private bool CanMoveDirectoryUp(object? arg)
    {
        return !IsBusy
            && arg is SearchDirectory dir
            && IncludedDirectories.IndexOf(dir) > 0;
    }

    [RelayCommand(CanExecute = nameof(CanMoveDirectoryDown))]
    private void MoveDirectoryDown(object? arg)
    {
        if (!CanMoveDirectoryDown(arg))
        {
            return;
        }

        if (arg is SearchDirectory dir)
        {
            var dirIndex = IncludedDirectories.IndexOf(dir);
            if (dirIndex < IncludedDirectories.Count - 1)
            {
                IncludedDirectories.Remove(dir);
                IncludedDirectories.Insert(dirIndex + 1, dir);

                SelectedIncludedDirectories.Clear();
                SelectedIncludedDirectory = dir;
            }
        }
    }

    private bool CanMoveDirectoryDown(object? arg)
    {
        return !IsBusy
            && arg is SearchDirectory dir
            && IncludedDirectories.IndexOf(dir) < IncludedDirectories.Count - 1;
    }

    [RelayCommand(CanExecute = nameof(CanSearch))]
    private async Task SearchAsync(object? arg, CancellationToken ct)
    {
        if (!CanSearch(arg))
        {
            return;
        }

        if (!_finderOptions.HasOption(FinderOption.SameFileName)
            && !_finderOptions.HasOption(FinderOption.SameContents)
            && string.IsNullOrWhiteSpace(FileNamePattern))
        {
            MessageBox?.Invoke(new MessageBoxViewModel()
            {
                Title = "Invalid Search Criteria",
                Message = "At least one option from \"Match Same Filename\" and \"Match Same Contents\" must be specified.",
                Icon = MessageBoxIcon.Error
            });

            return;
        }

        IEnumerable<string>? exts = null;
        if (IncludedExtensions.Trim() == "")
        {
            IncludedExtensions = "*.*";
        }

        if (!string.IsNullOrWhiteSpace(FileNamePattern) && !SavedFileNamePatterns.Contains(FileNamePattern))
        {
            SavedFileNamePatterns.Add(FileNamePattern);
        }

        try
        {
            if (IncludedExtensions.Trim() != "*.*")
            {
                exts = BuildExtensionList(IncludedExtensions);
                if (!SavedIncludedExtensions.Contains(IncludedExtensions))
                {
                    SavedIncludedExtensions.Add(IncludedExtensions);
                }
            }
        }
        catch
        {
            MessageBox?.Invoke(new MessageBoxViewModel()
            {
                Title = "Invalid Search Criteria",
                Message = "The extensions must be of the valid format.",
                Icon = MessageBoxIcon.Error
            });

            RaiseEvent(InvalidExtensionsToIncludeEntered);

            return;
        }

        IEnumerable<string>? excludedExts = null;
        try
        {
            if (!string.IsNullOrWhiteSpace(ExcludedExtensions))
            {
                excludedExts = BuildExtensionList(ExcludedExtensions);
                if (!SavedExcludedExtensions.Contains(ExcludedExtensions))
                {
                    SavedExcludedExtensions.Add(ExcludedExtensions);
                }
            }
        }
        catch
        {
            MessageBox?.Invoke(new MessageBoxViewModel()
            {
                Title = "Invalid Search Criteria",
                Message = "The extensions to exclude must be of the valid format.",
                Icon = MessageBoxIcon.Error
            });

            RaiseEvent(InvalidExtensionsToExcludeEntered);

            return;
        }

        if ((MatchDateCreated
            && DateCreatedFrom.HasValue
            && DateCreatedTo.HasValue
            && DateCreatedFrom > DateCreatedTo)
            || (MatchDateModified
                && DateModifiedFrom.HasValue
                && DateModifiedTo.HasValue
                && DateModifiedFrom > DateModifiedTo))
        {
            MessageBox?.Invoke(new MessageBoxViewModel()
            {
                Title = "Invalid Search Criteria",
                Message = "The selected dates must be valid and in the correct order.",
                Icon = MessageBoxIcon.Error
            });

            return;
        }

        if (await GetIfShouldClearResults("Search") == false)
        {
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        SetBusy(SearchCommand);
        FinderResult? result = null;
        FinderProgress dfProgress = default;
        var progressReporterTask = Task.Run(async () =>
        {
            while (IsBusy)
            {
                var delay = Task.Delay(ProgressReporterDelay);
                if (dfProgress.TotalCount > 0)
                {
                    Progress = dfProgress.ProgressCount / (double)dfProgress.TotalCount * 100.0;
                    PrimaryStatus = $"Searching: {Path.GetDirectoryName(dfProgress.CurrentFileName)}";
                    SecondaryStatus = $"Duplicates: {dfProgress.DuplicateCount:N0} ({dfProgress.DuplicateLength.ConvertLengthToString()})";
                    TertiaryStatus = $"Searched: {dfProgress.ProgressCount:N0}/{dfProgress.TotalCount:N0} ({Convert.ToInt32(Progress)}%)";
                }

                await delay;
            }
        });

        var etaReporterTask = Task.Run(async () =>
        {
            var etaStopwatch = Stopwatch.StartNew();
            while (IsBusy)
            {
                var delay = Task.Delay(EtaReporterDelay);
                if (dfProgress.ProgressLength > 0 && dfProgress.ProgressLength < dfProgress.TotalLength)
                {
                    var msRemaining = etaStopwatch.ElapsedMilliseconds / (double)dfProgress.ProgressLength * (dfProgress.TotalLength - dfProgress.ProgressLength);
                    QuaternaryStatus = $"ETA: {msRemaining.ConvertMillisecondsToString()}";
                }

                await delay;
            }
        });

        PrimaryStatus = "Preparing...";
        DuplicateFiles.Clear();
        try
        {
            await Task.Run(async () =>
            {
                result = await FinderService.FindAsync(
                    IncludedDirectories.Where(x => x.IsMarked),
                    ExcludedDirectories.Where(x => x.IsMarked),
                    _finderOptions,
                    FileNamePattern,
                    MinimumFileLength,
                    exts,
                    excludedExts,
                    MatchDateCreated ? DateCreatedFrom : null,
                    MatchDateCreated ? DateCreatedTo : null,
                    MatchDateModified ? DateModifiedFrom : null,
                    MatchDateModified ? DateModifiedTo : null,
                    _fileService,
                    new Progress<FinderProgress>(p =>
                    {
                        dfProgress = p;
                    }),
                    ct);
            });
        }
        catch (OperationCanceledException)
        {

        }

        if (result != null)
        {
            result.Files.ForEach(x => DuplicateFiles.Add(x));
            switch (_lastValidMarkingCriteria)
            {
                case MarkingCriteria.EarliestModified:
                    await MarkFilesKeepEarliestModifiedAsync(result.Files);

                    SelectedMarkingCriteria = MarkingCriteria.EarliestModified;

                    break;

                case MarkingCriteria.LatestModified:
                    await MarkFilesKeepLatestModifiedAsync(result.Files);

                    SelectedMarkingCriteria = MarkingCriteria.LatestModified;

                    break;

                case MarkingCriteria.EarliestCreated:
                    await MarkFilesKeepEarliestCreatedAsync(result.Files);

                    SelectedMarkingCriteria = MarkingCriteria.EarliestCreated;

                    break;

                case MarkingCriteria.LatestCreated:
                    await MarkFilesKeepLatestCreatedAsync(result.Files);

                    SelectedMarkingCriteria = MarkingCriteria.LatestCreated;

                    break;

                case MarkingCriteria.LargestLength:
                    await MarkFilesKeepLargestLengthAsync(result.Files);

                    SelectedMarkingCriteria = MarkingCriteria.LargestLength;

                    break;

                case MarkingCriteria.SmallestLength:
                    await MarkFilesKeepSmallestLengthAsync(result.Files);

                    SelectedMarkingCriteria = MarkingCriteria.SmallestLength;

                    break;

                case MarkingCriteria.MoreLetters:
                    await MarkFilesKeepMoreLettersAsync(result.Files);

                    SelectedMarkingCriteria = MarkingCriteria.MoreLetters;

                    break;

                case MarkingCriteria.LessLetters:
                    await MarkFilesKeepLessLettersAsync(result.Files);

                    SelectedMarkingCriteria = MarkingCriteria.LessLetters;

                    break;

                default:
                    await MarkFilesKeepLatestModifiedAsync(result.Files);

                    SelectedMarkingCriteria = MarkingCriteria.LatestModified;

                    break;
            }
        }

        SetBusy(false);
        stopwatch.Stop();
        if (result != null)
        {
            StringBuilder message = new StringBuilder(
                $"Files searched: {dfProgress.ProgressCount:N0}"
                + $"\nDuration: {stopwatch.Elapsed.ToString(@"hh\:mm\:ss")}");

            if (result.ExcludedDirectories.Count > 0 || result.Errors.Count > 0)
            {
                message.Append('\n');

                if (result.ExcludedDirectories.Count > 0)
                {
                    message.Append($"\nFolders excluded: {result.ExcludedDirectories.Count:N0}");
                }

                if (result.Errors.Count > 0)
                {
                    message.Append($"\nErrors: {result.Errors.Count:N0}");
                }
            }

            if (result.DuplicateCount > 0)
            {
                message.Append($"\n\nMarking criteria applied: {((MarkingCriteria)SelectedMarkingCriteria).AsString(EnumFormat.Description)}");
            }

            MessageBox?.Invoke(new MessageBoxViewModel()
            {
                Title = ct.IsCancellationRequested ? "Search Interrupted" : "Search Completed",
                Header = result.DuplicateCount > 0
                    ? $"Duplicates found: {result.DuplicateCount:N0} ({result.Files.Sum(x => x.Length).ConvertLengthToString()})"
                    : "No duplicate files were found.",
                Message = message.ToString(),
                Icon = result.Errors.Count == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning,
                CustomButton1Content = result.ExcludedDirectories.Count == 0 ? null : "_View Exclusions",
                CustomButton1Action = result.ExcludedDirectories.Count == 0 ? null : new Action(() =>
                {
                    MessageBox?.Invoke(new MessageBoxViewModel()
                    {
                        Title = "Exclusions",
                        Message = $"The following {(result.ExcludedDirectories.Count > 1 ? "folders were" : "folder was")} excluded from the search:",
                        SecondaryMessage = $"{string.Join("\n\n", result.ExcludedDirectories.Select(x => $"\t{x.FullName}"))}",
                        SecondaryMessageWrapped = false
                    });
                }),
                CustomButton2Content = result.Errors.Count == 0 ? null : "View _Errors",
                CustomButton2Action = result.Errors.Count == 0 ? null : new Action(() =>
                {
                    MessageBox?.Invoke(new MessageBoxViewModel()
                    {
                        Title = "Errors",
                        SecondaryMessage = string.Join("\n\n", result.Errors.Select(x => $"{x.Key}: {x.Value}")),
                        SecondaryMessageWrapped = false
                    });
                })
            });

            if (result.Files.Count > 0)
            {
                RaiseEvent(SearchPerformed);
            }
        }
    }

    private bool CanSearch(object? arg)
    {
        return !IsBusy && IncludedDirectories.Any(x => x.IsMarked);
    }

    [RelayCommand(CanExecute = nameof(CanCancelOperation))]
    private void CancelOperation(object? arg)
    {
        if (!CanCancelOperation(arg))
        {
            return;
        }

        if (_cancellableOperationCommand != null && _cancellableOperationCommand.IsRunning)
        {
            _cancellableOperationCommand.Cancel();
        }
    }

    private bool CanCancelOperation(object? arg)
    {
        return OperationCanBeCanceled;
    }

    [RelayCommand]
    private void HideFindWithinSearchResultsPane(object? arg)
    {
        ShowFindWithinSearchResultsPane = false;
    }

    [RelayCommand(CanExecute = nameof(CanAutoMark))]
    private async Task KeepEarliestModifiedAsync(object? arg, CancellationToken ct)
    {
        if (!CanAutoMark(arg))
        {
            return;
        }

        SetBusy("Marking...", KeepEarliestModifiedCommand);
        if (arg == null)
        {
            await MarkFilesKeepEarliestModifiedAsync(DuplicateFiles, ct);

            SelectedMarkingCriteria = MarkingCriteria.EarliestModified;
        }
        else if (arg is IList items)
        {
            await MarkFilesKeepEarliestModifiedAsync(items.Cast<DuplicateFile>(), ct);
        }

        SetBusy(false);
    }

    [RelayCommand(CanExecute = nameof(CanAutoMark))]
    private async Task KeepLatestModifiedAsync(object? arg, CancellationToken ct)
    {
        if (!CanAutoMark(arg))
        {
            return;
        }

        SetBusy("Marking...", KeepLatestModifiedCommand);
        if (arg == null)
        {
            await MarkFilesKeepLatestModifiedAsync(DuplicateFiles, ct);

            SelectedMarkingCriteria = MarkingCriteria.LatestModified;
        }
        else if (arg is IList items)
        {
            await MarkFilesKeepLatestModifiedAsync(items.Cast<DuplicateFile>(), ct);
        }

        SetBusy(false);
    }

    [RelayCommand(CanExecute = nameof(CanAutoMark))]
    private async Task KeepEarliestCreatedAsync(object? arg, CancellationToken ct)
    {
        if (!CanAutoMark(arg))
        {
            return;
        }

        SetBusy("Marking...", KeepEarliestCreatedCommand);
        if (arg == null)
        {
            await MarkFilesKeepEarliestCreatedAsync(DuplicateFiles, ct);

            SelectedMarkingCriteria = MarkingCriteria.EarliestCreated;
        }
        else if (arg is IList items)
        {
            await MarkFilesKeepEarliestCreatedAsync(items.Cast<DuplicateFile>(), ct);
        }

        SetBusy(false);
    }

    [RelayCommand(CanExecute = nameof(CanAutoMark))]
    private async Task KeepLatestCreatedAsync(object? arg, CancellationToken ct)
    {
        if (!CanAutoMark(arg))
        {
            return;
        }

        SetBusy("Marking...", KeepLatestCreatedCommand);
        if (arg == null)
        {
            await MarkFilesKeepLatestCreatedAsync(DuplicateFiles, ct);

            SelectedMarkingCriteria = MarkingCriteria.LatestCreated;
        }
        else if (arg is IList items)
        {
            await MarkFilesKeepLatestCreatedAsync(items.Cast<DuplicateFile>(), ct);
        }

        SetBusy(false);
    }

    [RelayCommand(CanExecute = nameof(CanAutoMark))]
    private async Task KeepLargestLengthAsync(object? arg, CancellationToken ct)
    {
        if (!CanAutoMark(arg))
        {
            return;
        }

        SetBusy("Marking...", KeepLargestLengthCommand);
        if (arg == null)
        {
            await MarkFilesKeepLargestLengthAsync(DuplicateFiles, ct);

            SelectedMarkingCriteria = MarkingCriteria.LargestLength;
        }
        else if (arg is IList items)
        {
            await MarkFilesKeepLargestLengthAsync(items.Cast<DuplicateFile>(), ct);
        }

        SetBusy(false);
    }

    [RelayCommand(CanExecute = nameof(CanAutoMark))]
    private async Task KeepSmallestLengthAsync(object? arg, CancellationToken ct)
    {
        if (!CanAutoMark(arg))
        {
            return;
        }

        SetBusy("Marking...", KeepSmallestLengthCommand);
        if (arg == null)
        {
            await MarkFilesKeepSmallestLengthAsync(DuplicateFiles, ct);

            SelectedMarkingCriteria = MarkingCriteria.SmallestLength;
        }
        else if (arg is IList items)
        {
            await MarkFilesKeepSmallestLengthAsync(items.Cast<DuplicateFile>(), ct);
        }

        SetBusy(false);
    }

    [RelayCommand(CanExecute = nameof(CanAutoMark))]
    private async Task KeepMoreLettersAsync(object? arg, CancellationToken ct)
    {
        if (!CanAutoMark(arg))
        {
            return;
        }

        SetBusy("Marking...", KeepMoreLettersCommand);
        if (arg == null)
        {
            await MarkFilesKeepMoreLettersAsync(DuplicateFiles, ct);

            SelectedMarkingCriteria = MarkingCriteria.MoreLetters;
        }
        else if (arg is IList items)
        {
            await MarkFilesKeepMoreLettersAsync(items.Cast<DuplicateFile>(), ct);
        }

        SetBusy(false);
    }

    [RelayCommand(CanExecute = nameof(CanAutoMark))]
    private async Task KeepLessLettersAsync(object? arg, CancellationToken ct)
    {
        if (!CanAutoMark(arg))
        {
            return;
        }

        SetBusy("Marking...", KeepLessLettersCommand);
        if (arg == null)
        {
            await MarkFilesKeepLessLettersAsync(DuplicateFiles, ct);

            SelectedMarkingCriteria = MarkingCriteria.LessLetters;
        }
        else if (arg is IList items)
        {
            await MarkFilesKeepLessLettersAsync(items.Cast<DuplicateFile>(), ct);
        }

        SetBusy(false);
    }

    private bool CanAutoMark(object? arg)
    {
        // Can auto mark if at least one group in itemsToCheck has two or more undeleted files, and
        // - if operating on selected files, all selected files are either locked or unlocked (not a mix of both), or
        // - if operating on all files, all files are either locked or unlocked (not a mix of both).

        if (!IsBusy)
        {
            var considerLockedFiles = true;
            IEnumerable<DuplicateFile> itemsToCheck;
            if (arg is IList items)
            {
                itemsToCheck = items.Cast<DuplicateFile>();
            }
            else
            {
                itemsToCheck = DuplicateFiles;
            }

            if (itemsToCheck.Any(x => x.IsLocked) && itemsToCheck.Any(x => !x.IsDeleted && !x.IsLocked))
            {
                considerLockedFiles = false;
            }

            if (!considerLockedFiles)
            {
                return itemsToCheck.GroupBy(x => x.Group).Where(g => !g.Any(f => f.IsLocked)).Any(g => g.Count(f => !f.IsDeleted) > 1);
            }
            else
            {
                return itemsToCheck.GroupBy(x => x.Group).Any(g => g.Count(f => !f.IsDeleted) > 1);
            }
        }

        return false;
    }

    [RelayCommand(CanExecute = nameof(CanMarkAll))]
    private void MarkAll(object? arg)
    {
        if (!CanMarkAll(arg))
        {
            return;
        }

        // If all files are locked, mark all files.
        // If there is one or more unlocked files, mark only the unlocked files.

        var itemsToMark = DuplicateFiles.All(x => x.IsLocked) ? DuplicateFiles : DuplicateFiles.Where(x => !x.IsLocked);
        itemsToMark.Where(x => !x.IsDeleted && !x.IsMarked).ForEach(x => x.IsMarked = true);
    }

    private bool CanMarkAll(object? arg)
    {
        // If all DuplicateFiles are locked, can MarkAll if any one is unmarked.
        // If there are any unlocked items in DuplicateFiles, can MarkAll if any one of the unlocked items is unmarked.

        return !IsBusy
            && ((DuplicateFiles.All(x => x.IsLocked) && DuplicateFiles.Any(x => !x.IsDeleted && !x.IsMarked))
                || (DuplicateFiles.Any(x => !x.IsDeleted && !x.IsLocked && !x.IsMarked)));
    }

    [RelayCommand(CanExecute = nameof(CanUnmarkAll))]
    private void UnmarkAll(object? arg)
    {
        if (!CanUnmarkAll(arg))
        {
            return;
        }

        // If all files are locked, unmark all files.
        // If there is one or more unlocked files, unmark only the unlocked files.

        var itemsToUnmark = DuplicateFiles.All(x => x.IsLocked) ? DuplicateFiles : DuplicateFiles.Where(x => !x.IsLocked);
        itemsToUnmark.Where(x => !x.IsDeleted && x.IsMarked).ForEach(x => x.IsMarked = false);
    }

    private bool CanUnmarkAll(object? arg)
    {
        // If all DuplicateFiles are locked, can UnmarkAll if any one is marked.
        // If there are any unlocked items in DuplicateFiles, can UnmarkAll if any one of the unlocked items is marked.

        return !IsBusy
            && ((DuplicateFiles.All(x => x.IsLocked) && DuplicateFiles.Any(x => !x.IsDeleted && x.IsMarked))
                || (DuplicateFiles.Any(x => !x.IsDeleted && !x.IsLocked && x.IsMarked)));
    }

    [RelayCommand(CanExecute = nameof(CanDeleteMarkedFiles))]
    private async Task DeleteMarkedFilesAsync(object? arg, CancellationToken ct)
    {
        if (!CanDeleteMarkedFiles(arg))
        {
            return;
        }

        if (_fileService != null)
        {
            if (MessageBox != null)
            {
                var markedFileCount = DuplicateFiles.Where(x => x.IsMarked).Count();
                var header = $"Move {markedFileCount:N0} marked {(markedFileCount > 1 ? "files" : "file")} to the {_fileService.RecycleBinLabel}?";
                string? message = null;
                string? secondaryMessage = null;

                // Undeletable files are undeleted files from groups where all undeleted files have been marked for
                // deletion.
                var undeleteableFiles = DuplicateFiles
                    .GroupBy(x => x.Group)
                    .Where(g => g.Any(x => !x.IsDeleted))
                    .Where(g => g.Where(x => !x.IsDeleted).All(x => x.IsMarked))
                    .SelectMany(g => g);
                if (undeleteableFiles.Any())
                {
                    message = "WARNING: The following sets of duplicate files have all existing files marked for deletion. "
                        + "It is highly recommended to leave at least one existing file from each set unmarked.";

                    secondaryMessage = string.Join(
                        "\n\n",
                        undeleteableFiles
                        .GroupBy(x => x.Group)
                        .Select(g => $"Group {g.Key}\n{string.Join("\n", g.Where(x => !x.IsDeleted).Select(x => $"\t{x.FullName}"))}"));
                }

                var msgBox = await MessageBox.Invoke(new MessageBoxViewModel()
                {
                    Title = "Delete",
                    Header = header.ToString(),
                    Message = message,
                    SecondaryMessage = secondaryMessage,
                    SecondaryMessageWrapped = false,
                    Icon = !string.IsNullOrEmpty(secondaryMessage) ? MessageBoxIcon.Warning : MessageBoxIcon.Question,
                    Buttons = MessageBoxButton.YesNo,
                    DefaultButton = !string.IsNullOrEmpty(secondaryMessage) ? MessageBoxDefaultButton.No : MessageBoxDefaultButton.Yes,
                });

                if (msgBox.DialogResult != true)
                {
                    return;
                }
            }

            SetBusy(DeleteMarkedFilesCommand);
            FinderResult? result = null;
            FinderProgress dfProgress = default;
            var progressReporterTask = Task.Run(async () =>
            {
                while (IsBusy)
                {
                    var delay = Task.Delay(ProgressReporterDelay);
                    try
                    {
                        if (dfProgress.TotalCount > 0)
                        {
                            Progress = Math.Round(dfProgress.ProgressCount / (double)dfProgress.TotalCount * 100.0, 1);
                        }

                        PrimaryStatus = $"Deleting: {dfProgress.ProgressCount:N0}/{dfProgress.TotalCount:N0} ({Convert.ToInt32(Progress)}%)";
                    }
                    catch
                    {

                    }

                    await delay;
                }
            });

            await Task.Run(async () =>
            {
                try
                {
                    result = await FinderService.DeleteFilesAsync(
                        DuplicateFiles.Where(x => x.IsMarked),
                        _fileService,
                        new Progress<FinderProgress>(p => dfProgress = p),
                        ct);
                }
                catch (OperationCanceledException)
                {

                }

                PrimaryStatus = "Refreshing...";

                Parallel.ForEach(DuplicateFiles, f => f.Refresh());
                UnlockDeletedGroups();
            });

            SetBusy(false);
            if (result != null && MessageBox != null)
            {
                await MessageBox.Invoke(new MessageBoxViewModel()
                {
                    Title = "Delete",
                    Header = $"Files deleted: {result.Files.Count:N0} ({result.Files.Where(x => x.Length.HasValue).Sum(x => x.Length).ConvertLengthToString()})",
                    Message = $"{(result.Errors.Count > 0 ? $"Errors: {result.Errors.Count:N0}\n\n" : "")}",
                    Icon = MessageBoxIcon.Information,
                    CustomButton1Content = result.Errors.Count == 0 ? null : "_View Errors",
                    CustomButton1Action = result.Errors.Count == 0 ? null : new Action(async () =>
                    {
                        await MessageBox.Invoke(new MessageBoxViewModel()
                        {
                            Title = "Errors",
                            SecondaryMessage = string.Join("\n\n", result.Errors.Select(x => $"{x.Key} - {x.Value}")),
                            SecondaryMessageWrapped = false
                        });
                    })
                });
            }
        }
    }

    private bool CanDeleteMarkedFiles(object? arg)
    {
        return !IsBusy && DuplicateFiles.Any(x => x.IsMarked && !x.IsDeleted);
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportAsync(object? arg, CancellationToken ct)
    {
        if (!CanExport(arg))
        {
            return;
        }

        if (AsyncFileSaver != null)
        {
            var fileName = await AsyncFileSaver.Invoke(
                "Export",
                $"Dupe Clear Search Result {DateTime.Now.ToString("yyMMdd-HHmmss")}{Constants.SearchResultsFileExtension}");

            if (!string.IsNullOrEmpty(fileName))
            {
                SetBusy("Exporting...", ExportCommand);
                try
                {
                    await Task.Run(() =>
                    {
                        var serializer = new XmlSerializer(typeof(SerializableDuplicateFileList));
                        var files = new SerializableDuplicateFileList() { MarkingCriteria = (int)SelectedMarkingCriteria };
                        foreach (var file in DuplicateFiles)
                        {
                            ct.ThrowIfCancellationRequested();

                            files.Files.Add(new SerializableDuplicateFile()
                            {
                                Created = file.Created,
                                FullName = file.FullName,
                                Group = file.Group,
                                IsDeleted = file.IsDeleted,
                                IsHidden = file.IsHidden,
                                IsLocked = file.IsLocked,
                                IsMarked = file.IsMarked,
                                IsSystemFile = file.IsSystemFile,
                                Length = file.Length,
                                Modified = file.Modified,
                            });
                        }

                        using (var writer = new StreamWriter(fileName))
                        {
                            serializer.Serialize(writer, files);
                        }
                    });
                }
                catch (Exception ex)
                {
                    if (ex is not OperationCanceledException)
                    {
                        MessageBox?.Invoke(new MessageBoxViewModel()
                        {
                            Title = "Export",
                            Message = "An error occurred while attempting to export data.",
                            SecondaryMessage = ex.Message,
                            Icon = MessageBoxIcon.Error
                        });
                    }
                }
                finally
                {
                    SetBusy(false);
                }
            }
        }
    }

    private bool CanExport(object? arg)
    {
        return !IsBusy && DuplicateFiles.Count > 0;
    }

    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private async Task ImportAsync(object? arg, CancellationToken ct)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        if (AsyncFilePicker != null)
        {
            if (await GetIfShouldClearResults("Import") == false)
            {
                return;
            }

            var fileName = await AsyncFilePicker.Invoke("Import");
            await ImportAsync(fileName, ct);
        }
    }

    [RelayCommand(CanExecute = nameof(CanOpen))]
    private async Task OpenAsync(object? arg)
    {
        if (!CanOpen(arg))
        {
            return;
        }

        var selectedItems = new List<DuplicateFile>();
        if (arg is IList items)
        {
            selectedItems.AddRange(items.Cast<DuplicateFile>());
        }
        else if (arg is DuplicateFile item)
        {
            selectedItems.Add(item);
        }

        var count = selectedItems.Count;
        if (count > 0)
        {
            if (MessageBox != null && count > 2)
            {
                var msgBox = await MessageBox.Invoke(new MessageBoxViewModel()
                {
                    Title = "Open",
                    Message = $"Open {count} files?",
                    Icon = MessageBoxIcon.Warning,
                    Buttons = MessageBoxButton.YesNo,
                    DefaultButton = MessageBoxDefaultButton.No
                });

                if (msgBox.DialogResult != true)
                {
                    return;
                }
            }

            foreach (var item in selectedItems)
            {
                _fileService?.LaunchFile(item.FullName);
            }
        }
    }

    private bool CanOpen(object? arg)
    {
        return !IsBusy
            && ((arg is IList items && items.Cast<DuplicateFile>().Any(x => !x.IsDeleted))
                || (arg is DuplicateFile item && !item.IsDeleted));
    }

    [RelayCommand(CanExecute = nameof(CanOpenContainingFolder))]
    private async Task OpenContainingFolderAsync(object? arg)
    {
        if (!CanOpenContainingFolder(arg))
        {
            return;
        }

        if (arg is IList items)
        {
            var selectedItems = items.Cast<DuplicateFile>().Where(x => Directory.Exists(x.DirectoryName));
            var count = selectedItems.Count();
            if (MessageBox != null && count > 2)
            {
                var msgBox = await MessageBox.Invoke(new MessageBoxViewModel()
                {
                    Title = "Open Containing Folder",
                    Message = $"Open {count} folders?",
                    Icon = MessageBoxIcon.Warning,
                    Buttons = MessageBoxButton.YesNo,
                    DefaultButton = MessageBoxDefaultButton.No
                });

                if (msgBox.DialogResult != true)
                {
                    return;
                }
            }

            foreach (var item in selectedItems)
            {
                _fileService?.LaunchContainingDirectory(item.FullName);
            }
        }
    }

    private bool CanOpenContainingFolder(object? arg)
    {
        return !IsBusy && arg != null;
    }

    [RelayCommand(CanExecute = nameof(CanDelist))]
    private async Task DelistAsync(object? arg)
    {
        if (!CanDelist(arg))
        {
            return;
        }

        if (arg is IList items)
        {
            SetBusy("Delisting...");
            await Task.Run(async () =>
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var item in items.Cast<DuplicateFile>().ToArray())
                    {
                        DuplicateFiles.Remove(item);
                    }
                });

                UpdateGroupIds();
            });

            SetBusy(false);
        }
    }

    private bool CanDelist(object? arg)
    {
        return !IsBusy && SelectedDuplicateFile != null;
    }

    [RelayCommand(CanExecute = nameof(CanDelistGroup))]
    private async Task DelistGroupAsync(object? arg)
    {
        if (!CanDelistGroup(arg))
        {
            return;
        }

        if (arg is IList items)
        {
            SetBusy("Delisting...");
            await Task.Run(async () =>
            {
                var groupsToRemove = new List<int?>();
                foreach (var groupNum in items.Cast<DuplicateFile>().GroupBy(x => x.Group).Select(g => g.Key))
                {
                    groupsToRemove.Add(groupNum);
                }

                var itemsToRemove = DuplicateFiles.Where(x => groupsToRemove.Contains(x.Group)).ToList();

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var item in itemsToRemove)
                    {
                        DuplicateFiles.Remove(item);
                    }
                });

                UpdateGroupIds();
            });

            SetBusy(false);
        }
    }

    private bool CanDelistGroup(object? arg)
    {
        return !IsBusy && SelectedDuplicateFile != null;
    }

    [RelayCommand(CanExecute = nameof(CanDelistDirectory))]
    private async Task DelistDirectoryAsync(object? arg)
    {
        if (!CanDelistDirectory(arg))
        {
            return;
        }

        if (arg is IList items)
        {
            var selectedFiles = items.Cast<DuplicateFile>();
            var dirsToRemove = selectedFiles.Select(x => x.DirectoryName).Distinct();
            var filesToRemove = new List<DuplicateFile>();
            var filesFromSubdirs = DuplicateFiles.Where(x =>
                !selectedFiles.Contains(x)
                && dirsToRemove.Any(y =>
                    !string.IsNullOrEmpty(y)
                    && string.Compare(y, x.DirectoryName, true) != 0
                    && x.DirectoryName.IsSameAsOrSubdirectoryOf(y))
                && !selectedFiles.Any(y => string.Compare(y.DirectoryName, x.DirectoryName) == 0));
            var dirsOfFilesFromSubdirs = filesFromSubdirs.Select(x => x.DirectoryName).Distinct();
            var includeFilesFromSubdirs = false;

            if (filesFromSubdirs.Any())
            {
                if (MessageBox != null)
                {
                    var msgBoxVM = new MessageBoxViewModel()
                    {
                        Title = "Delist",
                        Message = $"Delist files from the following folder{(dirsOfFilesFromSubdirs.Count() > 1 ? "s" : "")} as well?",
                        SecondaryMessage = string.Join('\n', dirsOfFilesFromSubdirs),
                        SecondaryMessageWrapped = false,
                        Icon = MessageBoxIcon.Question,
                        Buttons = MessageBoxButton.YesNoCancel,
                    };

                    var msgBox = await MessageBox.Invoke(msgBoxVM);
                    if (msgBox.DialogResult == null)
                    {
                        return;
                    }
                    else
                    {
                        includeFilesFromSubdirs = msgBox.DialogResult.Value;
                    }
                }
            }

            SetBusy("Delisting...");
            await Task.Run(async () =>
            {
                foreach (var dir in dirsToRemove)
                {
                    filesToRemove.AddRange(
                        DuplicateFiles.Where(x => !string.IsNullOrWhiteSpace(x.DirectoryName)
                            && !string.IsNullOrWhiteSpace(dir)
                            && ((includeFilesFromSubdirs && x.DirectoryName.IsSameAsOrSubdirectoryOf(dir))
                                || !includeFilesFromSubdirs && string.Compare(x.DirectoryName, dir, true) == 0)));
                }

                await Dispatcher.UIThread.InvokeAsync(() => filesToRemove.Distinct().ForEach(f => DuplicateFiles.Remove(f)));

                UnmarkOrphanedFiles();
                UpdateGroupIds();
            });

            SetBusy(false);
        }
    }

    private bool CanDelistDirectory(object? arg)
    {
        return !IsBusy && arg != null;
    }

    [RelayCommand(CanExecute = nameof(CanLockGroup))]
    private async Task LockGroupAsync(object? arg)
    {
        if (!CanLockGroup(arg))
        {
            return;
        }

        if (arg is IList items)
        {
            SetBusy("Locking...");
            await Task.Run(() =>
            {
                var selectedGroups = items.Cast<DuplicateFile>().Select(x => x.Group).Distinct();
                var lockableGroups = DuplicateFiles
                    .GroupBy(x => x.Group)
                    .Where(x => selectedGroups.Contains(x.Key))
                    .Where(x => !x.Any(y => y.IsDeleted));
                foreach (var item in lockableGroups)
                {
                    item.ForEach(f =>
                    {
                        if (!f.IsLocked)
                        {
                            f.IsLocked = true;
                        }
                    });
                }
            });

            SetBusy(false);
        }
    }

    private bool CanLockGroup(object? arg)
    {
        if (!IsBusy && arg is IList items)
        {
            var selectedGroups = items.Cast<DuplicateFile>().Select(x => x.Group).Distinct();
            var lockableGroups = DuplicateFiles
                .GroupBy(x => x.Group)
                .Where(x => selectedGroups.Contains(x.Key))
                .Where(x => !x.Any(y => y.IsDeleted))
                .Where(x => x.Any(y => !y.IsLocked));

            return lockableGroups.Any();
        }

        return false;
    }

    [RelayCommand(CanExecute = nameof(CanUnlockGroup))]
    private async Task UnlockGroupAsync(object? arg)
    {
        if (!CanUnlockGroup(arg))
        {
            return;
        }

        if (arg is IList items)
        {
            SetBusy("Unlocking...");
            await Task.Run(() =>
            {
                var selectedGroups = items.Cast<DuplicateFile>().Select(x => x.Group).Distinct();
                var unlockableGroups = DuplicateFiles
                    .GroupBy(x => x.Group)
                    .Where(x => selectedGroups.Contains(x.Key))
                    .Where(x => !x.Any(y => y.IsDeleted));
                foreach (var item in unlockableGroups)
                {
                    item.ForEach(f =>
                    {
                        if (f.IsLocked)
                        {
                            f.IsLocked = false;
                        }
                    });
                }
            });

            SetBusy(false);
        }
    }

    private bool CanUnlockGroup(object? arg)
    {
        if (!IsBusy && arg is IList items)
        {
            var selectedGroups = items.Cast<DuplicateFile>().Select(x => x.Group).Distinct();
            var unlockableGroups = DuplicateFiles
                .GroupBy(x => x.Group)
                .Where(x => selectedGroups.Contains(x.Key))
                .Where(x => !x.Any(y => y.IsDeleted))
                .Where(x => x.Any(y => y.IsLocked));

            return unlockableGroups.Any();
        }

        return false;
    }

    [RelayCommand(CanExecute = nameof(CanMarkOrUnmarkAllFromThisDirectory))]
    private void MarkAllFromThisDirectory(object? arg)
    {
        if (!CanMarkOrUnmarkAllFromThisDirectory(arg))
        {
            return;
        }

        if (arg is IList items)
        {
            // Change the marking of locked files if all items in DuplicateFiles are locked. Otherwise, ignore locked
            // files.

            var dirsToMark = items.Cast<DuplicateFile>().Select(x => x.DirectoryName).Distinct();
            var canMarkLockedFiles = DuplicateFiles.All(x => x.IsLocked);
            foreach (var item in DuplicateFiles.Where(x =>
                !x.IsDeleted
                && (canMarkLockedFiles || (!canMarkLockedFiles && !x.IsLocked))
                && dirsToMark.Contains(x.DirectoryName)))
            {
                item.IsMarked = true;
            }
        }
    }

    [RelayCommand(CanExecute = nameof(CanMarkOrUnmarkAllFromThisDirectory))]
    private void UnmarkAllFromThisDirectory(object? arg)
    {
        if (!CanMarkOrUnmarkAllFromThisDirectory(arg))
        {
            return;
        }

        if (arg is IList items)
        {
            // Change the marking of locked files if all items in DuplicateFiles are locked. Otherwise, ignore locked
            // files.

            var dirsToUnmark = items.Cast<DuplicateFile>().Select(x => x.DirectoryName).Distinct();
            var canUnmarkLockedFiles = DuplicateFiles.All(x => x.IsLocked);
            foreach (var item in DuplicateFiles.Where(x =>
                !x.IsDeleted
                && (canUnmarkLockedFiles || (!canUnmarkLockedFiles && !x.IsLocked))
                && dirsToUnmark.Contains(x.DirectoryName)))
            {
                item.IsMarked = false;
            }
        }
    }

    private bool CanMarkOrUnmarkAllFromThisDirectory(object? arg)
    {
        if (!IsBusy && SelectedDuplicateFile != null)
        {
            return DuplicateFiles.All(x => x.IsLocked) || !SelectedDuplicateFile.IsLocked;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="arg">A collection representing the actioned item and the list of selected items.</param>
    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private void ApplyMarkingToSelectedSearchResults(object? arg)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        if (arg is IEnumerable<object> values)
        {
            var actionedItem = (DuplicateFile)values.ElementAt(0);
            var selectedItems = ((IList)values.ElementAt(1)).Cast<DuplicateFile>();
            if (selectedItems.Contains(actionedItem))
            {
                selectedItems
                    .Where(x => x != actionedItem && !x.IsDeleted && !x.IsLocked && x.IsMarked != actionedItem.IsMarked)
                    .ForEach(x => x.IsMarked = actionedItem.IsMarked);
            }
        }
    }

    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private void InvertMarkingOfSelectedSearchResult(object? arg)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        if (arg is IEnumerable<object> values)
        {
            var actionedItem = (DuplicateFile)values.ElementAt(0);
            if (!actionedItem.IsDeleted && !actionedItem.IsLocked)
            {
                actionedItem.IsMarked = !actionedItem.IsMarked;
                ApplyMarkingToSelectedSearchResultsCommand.Execute(arg);
            }
        }
    }

    /*private bool CanInvertMarkingOfSelectedSearchResult(object? arg)
    {
        if (!IsBusy)
        {
            if (arg is IEnumerable<object> values)
            {
                var actionedItem = (DuplicateFile)values.ElementAt(0);
                return !actionedItem.IsLocked;
            }
        }

        return false;
    }*/

    [RelayCommand(CanExecute = nameof(CanRefresh))]
    private async Task RefreshAsync(object? arg)
    {
        if (!CanRefresh(arg))
        {
            return;
        }

        SetBusy("Refreshing...");

        await Task.Run(() =>
        {
            Parallel.ForEach(DuplicateFiles, f => f.Refresh());
            UnmarkOrphanedFiles();
            UnlockDeletedGroups();
        });

        SetBusy(false);
    }

    private bool CanRefresh(object? arg)
    {
        return !IsBusy && DuplicateFiles.Any();
    }

    /// <summary>
    /// Remove all groups with 1 or less undeleted file.
    /// </summary>
    /// <param name="arg"></param>
    [RelayCommand(CanExecute = nameof(CanClean))]
    private async Task CleanAsync(object? arg)
    {
        if (!CanClean(arg))
        {
            return;
        }

        SetBusy("Cleaning...");
        await Task.Run(async () =>
        {
            var filesToRemove = new BlockingCollection<DuplicateFile>();
            Parallel.ForEach(DuplicateFiles.GroupBy(x => x.Group), group =>
            {
                if (group.Count(x => !x.IsDeleted) <= 1)
                {
                    foreach (var file in group.Where(x => !x.IsDeleted))
                    {
                        filesToRemove.Add(file);
                    }
                }
            });

            foreach (var file in DuplicateFiles.Where(x => x.IsDeleted))
            {
                filesToRemove.Add(file);
            }

            filesToRemove.CompleteAdding();

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                foreach (var file in filesToRemove)
                {
                    DuplicateFiles.Remove(file);
                }
            });

            UpdateGroupIds();
        });

        SetBusy(false);
    }

    private bool CanClean(object? arg)
    {
        return !IsBusy && DuplicateFiles.Any();
    }

    [RelayCommand(CanExecute = nameof(GetIfNotBusy))]
    private async Task FindWithinSearchResults(object? arg, CancellationToken ct)
    {
        if (!GetIfNotBusy(arg))
        {
            return;
        }

        var results = new List<DuplicateFile>();
        SetBusy("Working...", FindWithinSearchResultsCommand);
        try
        {
            await Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(FindWithinSearchResultsLookFor))
                {
                    results.AddRange(DuplicateFiles.Where(x => x.Name.Contains(FindWithinSearchResultsLookFor, StringComparison.OrdinalIgnoreCase)));
                }

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    FindWithinSearchResultsItems.Clear();

                    foreach (var item in results)
                    {
                        ct.ThrowIfCancellationRequested();

                        FindWithinSearchResultsItems.Add(item);
                    }

                    RaiseFindWithinResultsPerformedEvent(FindWithinResultsPerformed, new FindWithinSearchResultsSearchPerformedEventArgs(results.Count > 0));
                });
            });
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            SetBusy(false);
        }
    }

    [RelayCommand(CanExecute = nameof(CanClearFindWithinSearchResultsItems))]
    private void ClearFindWithinSearchResultsItems(object? arg)
    {
        if (!CanClearFindWithinSearchResultsItems(arg))
        {
            return;
        }

        FindWithinSearchResultsItems.Clear();
        FindWithinSearchResultsLookFor = null;
    }

    private bool CanClearFindWithinSearchResultsItems(object? arg)
    {
        return !IsBusy && (FindWithinSearchResultsItems.Any() || !string.IsNullOrEmpty(FindWithinSearchResultsLookFor));
    }

    [RelayCommand]
    private void ChangeTheme(object? arg)
    {
        if (arg is Theme theme)
        {
            switch (theme)
            {
                case Theme.Dark:
                    Application.Current!.RequestedThemeVariant = ThemeVariant.Dark;
                    Theme = Theme.Dark;

                    break;
                case Theme.Light:
                    Application.Current!.RequestedThemeVariant = ThemeVariant.Light;
                    Theme = Theme.Light;

                    break;
                default:
                    Application.Current!.RequestedThemeVariant = ThemeVariant.Default;
                    Theme = Theme.Auto;

                    break;
            }
        }
    }

    [RelayCommand]
    private async Task CloseAsync(object? arg)
    {
        if (IsBusy && OperationCanBeCanceled)
        {
            if (MessageBox != null && IsSearching)
            {
                var msgBox = await MessageBox.Invoke(new MessageBoxViewModel()
                {
                    Title = "Exit",
                    Message = "Search operation in progress.\n\nCancel and exit program?",
                    Icon = MessageBoxIcon.Question,
                    Buttons = MessageBoxButton.YesNo,
                    DefaultButton = MessageBoxDefaultButton.No
                });

                if (msgBox.DialogResult != true)
                {
                    return;
                }
            }

            CancelOperation(null);
        }

        Save();
        RaiseEvent(Closed);
    }

    [RelayCommand(CanExecute = nameof(CanCheckForUpdates))]
    private async Task CheckForUpdatesAsync(object? arg, CancellationToken ct)
    {
        if (!CanCheckForUpdates(arg))
        {
            return;
        }

        await CheckForUpdatesAsync(false, ct);
    }

    private bool CanCheckForUpdates(object? arg)
    {
        return true;
    }

    [RelayCommand]
    private void ShowAbout(object? arg)
    {
        var assm = Assembly.GetEntryAssembly();
        if (assm != null)
        {
            var name = assm.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            var version = assm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            var copyright = assm.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
            var arch = IntPtr.Size == 8 ? "x64" : "x86";
            MessageBox?.Invoke(new MessageBoxViewModel()
            {
                Title = "About",
                Message = $"{name} v{version} ({arch})\n\n{copyright}",
                Icon = MessageBoxIcon.AppIcon,
                CustomButton1Content = "_License",
                CustomButton1Action = new Action(() =>
                {
                    string licenseFile = "LICENSE";
                    if (File.Exists(licenseFile))
                    {
                        MessageBox?.Invoke(new MessageBoxViewModel()
                        {
                            Title = "License",
                            SecondaryMessage = File.ReadAllText(licenseFile),
                            SecondaryMessageWrapped = false
                        });
                    }
                    else
                    {
                        _fileService?.LaunchUrl(Constants.GplAddress);
                    }
                }),
                HyperlinkButtonContent = Constants.AppHomepage,
                HyperlinkButtonAction = new Action(() => _fileService?.LaunchUrl(Constants.AppHomepage))
            });
        }
    }

    private bool GetIfNotBusy(object? arg)
    {
        return !IsBusy;
    }

    #endregion // Command Implementations

    #region Public Methods

    public async Task ImportAsync(string? fileName, CancellationToken ct = default)
    {
        if (!string.IsNullOrEmpty(fileName))
        {
            SetBusy("Importing...", ImportCommand);
            try
            {
                await Task.Run(async () =>
                {
                    var serializer = new XmlSerializer(typeof(SerializableDuplicateFileList));
                    SerializableDuplicateFileList? files = null;
                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        files = (SerializableDuplicateFileList?)serializer.Deserialize(reader);
                    }

                    if (files != null)
                    {
                        // Marking criteria should be set to custom if any of the files were originally not deleted but
                        // now were found to be deleted.
                        var markingCriteriaChanged = false;
                        var hadCustomMarkingCriteria = (MarkingCriteria)files.MarkingCriteria == MarkingCriteria.Custom;

                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            DuplicateFiles.Clear();
                            foreach (var file in files.Files)
                            {
                                ct.ThrowIfCancellationRequested();

                                var dupeFile = new DuplicateFile(file, _fileService) { Group = file.Group, IsMarked = file.IsMarked };
                                if (!hadCustomMarkingCriteria && !file.IsDeleted && dupeFile.IsDeleted)
                                {
                                    markingCriteriaChanged = true;
                                }

                                DuplicateFiles.Add(dupeFile);
                            }

                            RaiseEvent(SearchPerformed);
                        });

                        if (!markingCriteriaChanged)
                        {
                            SelectedMarkingCriteria = (MarkingCriteria)files.MarkingCriteria;
                        }
                        else
                        {
                            SelectedMarkingCriteria = MarkingCriteria.Custom;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                {
                    MessageBox?.Invoke(new MessageBoxViewModel()
                    {
                        Title = "Export",
                        Message = "An error occurred while attempting to import data.",
                        SecondaryMessage = ex.Message,
                        Icon = MessageBoxIcon.Error
                    });
                }
            }
            finally
            {
                SetBusy(false);
            }

            await RefreshAsync(null);
        }
    }

    public void AddDirectoryForInclusion(IEnumerable<string> directories)
    {
        // This doesn't affect the directories list that's passed to this method.
        directories = directories.Select(x => x.TrimEnd(Path.DirectorySeparatorChar));

        foreach (var item in directories.Where(x => !IncludedDirectories.Any(y => string.Compare(y.FullName, x, true) == 0)))
        {
            var dir = new SearchDirectory(item, _fileService) { IncludeSubdirectories = IncludeSubdirectories, IsMarked = true };
            IncludedDirectories.Add(dir);
        }

        if (directories.Any())
        {
            var lastAddedDir = Path.GetDirectoryName(directories.First());
            if (!string.IsNullOrWhiteSpace(lastAddedDir))
            {
                _lastAddedDirectory = lastAddedDir;
            }
        }
    }

    public void AddDirectoryForExclusion(IEnumerable<string> directories)
    {
        // This doesn't affect the directories list that's passed to this method.
        directories = directories.Select(x => x.TrimEnd(Path.DirectorySeparatorChar));

        foreach (var item in directories.Where(x => !ExcludedDirectories.Any(y => string.Compare(y.FullName, x, true) == 0)))
        {
            var dir = new SearchDirectory(item, _fileService) { IncludeSubdirectories = ExcludeSubdirectories, IsMarked = true };
            ExcludedDirectories.Add(dir);
        }

        if (directories.Any())
        {
            var lastAddedDir = Path.GetDirectoryName(directories.First());
            if (!string.IsNullOrWhiteSpace(lastAddedDir))
            {
                _lastAddedDirectory = lastAddedDir;
            }
        }
    }

    #endregion // Public Methods

    #region Private Methods

    private void UpdateIsDirectoryExcluded()
    {
        foreach (var dir in IncludedDirectories)
        {
            dir.IsExcluded = IsDirectoryExcluded(dir);
        }
    }

    private bool IsDirectoryExcluded(SearchDirectory directory)
    {
        foreach (var excludedDir in ExcludedDirectories.Where(x => x.IsMarked))
        {
            if (string.Compare(directory.FullName, excludedDir.FullName, true) == 0)
            {
                return true;
            }

            if (excludedDir.IncludeSubdirectories)
            {
                if (directory.FullName.IsSameAsOrSubdirectoryOf(excludedDir.FullName))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private async Task<bool> GetIfShouldClearResults(string? msgBoxTitle = null)
    {
        if (MessageBox != null)
        {
            if (DuplicateFiles.Count > 0)
            {
                var msgBox = await MessageBox.Invoke(new MessageBoxViewModel()
                {
                    Title = msgBoxTitle,
                    Message = "Existing search results will be cleared.\n\nProceed?",
                    Icon = MessageBoxIcon.Question,
                    Buttons = MessageBoxButton.YesNo
                });

                if (msgBox.DialogResult != true)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private async Task MarkFilesKeepEarliestModifiedAsync(IEnumerable<DuplicateFile> files, CancellationToken ct = default)
    {
        await AutoMarkAsync(files, f => f.Modified, ct: ct);
    }

    private async Task MarkFilesKeepLatestModifiedAsync(IEnumerable<DuplicateFile> files, CancellationToken ct = default)
    {
        await AutoMarkAsync(files, f => f.Modified, true, ct);
    }

    private async Task MarkFilesKeepEarliestCreatedAsync(IEnumerable<DuplicateFile> files, CancellationToken ct = default)
    {
        await AutoMarkAsync(files, f => f.Created, ct: ct);
    }

    private async Task MarkFilesKeepLatestCreatedAsync(IEnumerable<DuplicateFile> files, CancellationToken ct = default)
    {
        await AutoMarkAsync(files, f => f.Created, true, ct);
    }

    private async Task MarkFilesKeepLargestLengthAsync(IEnumerable<DuplicateFile> files, CancellationToken ct = default)
    {
        await AutoMarkAsync(files, f => f.Length, true, ct);
    }

    private async Task MarkFilesKeepSmallestLengthAsync(IEnumerable<DuplicateFile> files, CancellationToken ct = default)
    {
        await AutoMarkAsync(files, f => f.Length, ct: ct);
    }

    private async Task MarkFilesKeepMoreLettersAsync(IEnumerable<DuplicateFile> files, CancellationToken ct = default)
    {
        await AutoMarkAsync(files, f => f.NameWithoutExtension.Count(char.IsLetter), true, ct);
    }

    private async Task MarkFilesKeepLessLettersAsync(IEnumerable<DuplicateFile> files, CancellationToken ct = default)
    {
        await AutoMarkAsync(files, f => f.NameWithoutExtension.Count(char.IsLetter), ct: ct);
    }

    private async Task AutoMarkAsync<T>(
        IEnumerable<DuplicateFile> files,
        Expression<Func<DuplicateFile, T>> orderBy,
        bool descending = false,
        CancellationToken ct = default)
    {
        var compiledOrderBy = orderBy.Compile();
        await Task.Run(() =>
        {
            // Can mark locked files if all specified files are locked. Otherwise, locked files are ignored.

            bool canMarkLockedFiles = files.All(x => x.IsLocked);
            Parallel.ForEach(
                files.GroupBy(f => f.Group).Where(x => canMarkLockedFiles || (!canMarkLockedFiles && !x.Any(y => y.IsLocked))),
                new ParallelOptions() { CancellationToken = ct },
                g =>
                {
                    // The first undeleted and all deleted files must be unmarked.

                    var markableFiles = g.Where(f => !f.IsDeleted);
                    var orderedMarkableFiles = descending ? markableFiles.OrderByDescending(compiledOrderBy) : markableFiles.OrderBy(compiledOrderBy);
                    var firstFile = orderedMarkableFiles.FirstOrDefault();
                    if (firstFile != null)
                    {
                        firstFile.IsMarked = false;
                        orderedMarkableFiles.Skip(1).ForEach(f => f.IsMarked = true);
                    }
                });
        });
    }

    /// <summary>
    /// Fixes the group numbers of items when they are added/removed from the list, e.g. if group 3 no longer exists
    /// and group numbers jump from 2 to 4, changes the group number of 4 to 3 to maintain chronology.
    /// </summary>
    private void UpdateGroupIds()
    {
        var newGroup = 1;
        foreach (var group in DuplicateFiles.GroupBy(x => x.Group))
        {
            foreach (var file in group)
            {
                if (file.Group != newGroup)
                {
                    file.Group = newGroup;
                }
            }

            newGroup++;
        }
    }

    /// <summary>
    /// Unmark files from groups which have 1 or less undeleted file.
    /// </summary>
    private void UnmarkOrphanedFiles()
    {
        Parallel.ForEach(
            DuplicateFiles
                .GroupBy(x => x.Group)
                .Where(g => g.Count(x => !x.IsDeleted) <= 1)
                .SelectMany(g => g)
                .Where(x => !x.IsDeleted && x.IsMarked),
            file =>
            {
                file.IsMarked = false;
            });
    }

    /// <summary>
    /// Unlock locked files from groups which have one or more deleted files.
    /// </summary>
    private void UnlockDeletedGroups()
    {
        var groupsWithDeletedFiles = DuplicateFiles
            .GroupBy(x => x.Group)
            .Where(x => x.Any(y => y.IsDeleted))
            .Where(x => x.Any(y => y.IsLocked))
            .Select(x => x.Key);
        Parallel.ForEach(DuplicateFiles, f =>
        {
            if (groupsWithDeletedFiles.Contains(f.Group) && f.IsLocked)
            {
                f.IsLocked = false;
            }
        });
    }

    private IEnumerable<string> BuildExtensionList(string extensions)
    {
        var result = new List<string>();
        var exts = extensions.Replace(" ", "").Split(';').Select(x => x);
        foreach (var ext in exts)
        {
            if (ext.Length < 3)
            {
                throw new InvalidOperationException();
            }

            result.Add(ext.Substring(1));
        }

        return result;
    }

    private void SetBusy(bool isBusy, string? primaryStatus, IAsyncRelayCommand? command)
    {
        if ((IsBusy && isBusy) || (!IsBusy && !isBusy))
        {
            return;
        }

        if (isBusy)
        {
            PrimaryStatus = primaryStatus ?? "";
            SecondaryStatus = "";
            TertiaryStatus = "";
            QuaternaryStatus = "";
            if (command != null)
            {
                _cancellableOperationCommand = command;
                OnPropertyChanged(nameof(OperationCanBeCanceled));
                Dispatcher.UIThread.Invoke(CancelOperationCommand.NotifyCanExecuteChanged);
            }
        }
        else
        {
            Progress = 0;
            PrimaryStatus = "Ready";
            SecondaryStatus = "";
            TertiaryStatus = "";
            QuaternaryStatus = "";
            if (_cancellableOperationCommand != null)
            {
                _cancellableOperationCommand = null;
                OnPropertyChanged(nameof(OperationCanBeCanceled));
                Dispatcher.UIThread.Invoke(CancelOperationCommand.NotifyCanExecuteChanged);
            }
        }

        IsBusy = isBusy;
    }

    private void SetBusy(bool isBusy)
    {
        SetBusy(isBusy, null, null);
    }

    private void SetBusy(IAsyncRelayCommand command)
    {
        SetBusy(true, null, command);
    }

    private void SetBusy(string primaryStatus, IAsyncRelayCommand? command = null)
    {
        SetBusy(true, primaryStatus, command);
    }

    private async Task CheckForUpdatesAsync(bool silent = false, CancellationToken ct = default)
    {
        if (MessageBox != null)
        {
            UpdateInfo updateInfo = default;
            try
            {
                updateInfo = await _updateService.GetUpdateInfoAsync();
            }
            catch (Exception ex)
            {
                if (!silent)
                {
                    await MessageBox.Invoke(new MessageBoxViewModel()
                    {
                        Title = "Update",
                        Message = $"An error occurred while attempting to check for updates.",
                        SecondaryMessage = ex.GetInnermostException()?.Message,
                        Icon = MessageBoxIcon.Error
                    });
                }

                return;
            }

            var currentVer = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (currentVer != null)
            {
                var updateable = updateInfo.IsNewerThan(currentVer);
                if (updateable)
                {
                    var msgBoxVM = new MessageBoxViewModel()
                    {
                        Title = "Update",
                        Message = $"An update has been released.\n\nNew version: {updateInfo.Version}\nCurrent version: {currentVer}\n",
                        Icon = MessageBoxIcon.Information,
                        Buttons = MessageBoxButton.OKCancel,
                        OKButtonContent = "_Download",
                        HyperlinkButtonContent = "Learn More",
                        HyperlinkButtonAction = new Action(() => _fileService?.LaunchUrl(updateInfo.UpdateInfoUrl))
                    };

                    if (silent)
                    {
                        msgBoxVM.CustomButton1Content = "Don't _Ask";
                        msgBoxVM.CustomButton1Action = new Action(() =>
                        {
                            AutoUpdateCheck = false;
                            msgBoxVM.Close();
                        });
                    }

                    var msgBox = await MessageBox.Invoke(msgBoxVM);
                    if (msgBox.DialogResult == true)
                    {
                        if (IsBusy || _updateService.IsAppPortable)
                        {
                            _fileService?.LaunchFile(updateInfo.FileUrl);
                        }
                        else
                        {
                            SetBusy(CheckForUpdatesCommand);
                            try
                            {
                                var updateFile = await _updateService.DownloadUpdateAsync(
                                    updateInfo,
                                    Path.GetTempPath(),
                                    new Progress<int>(p =>
                                    {
                                        PrimaryStatus = $"Downloading update... {p}%";
                                        Progress = p;
                                    }),
                                    ct);

                                if (updateFile != null)
                                {
                                    _fileService?.LaunchFile(updateFile.FullName);
                                    CloseCommand.Execute(null);
                                }
                                else
                                {
                                    _fileService?.LaunchUrl(updateInfo.UpdateInfoUrl);
                                }
                            }
                            catch (OperationCanceledException)
                            {

                            }
                            catch (Exception ex)
                            {
                                await MessageBox.Invoke(new MessageBoxViewModel()
                                {
                                    Title = "Update",
                                    Message = $"An error occurred while attempting download the update.",
                                    SecondaryMessage = ex.Message,
                                    Icon = MessageBoxIcon.Error
                                });
                            }
                            finally
                            {
                                SetBusy(false);
                            }
                        }
                    }
                }
                else
                {
                    if (!silent)
                    {
                        var msgBoxVM = new MessageBoxViewModel()
                        {
                            Title = "Update",
                            Message = "No new updates have been released.",
                            Icon = MessageBoxIcon.Information,
                            CheckBoxContent = "_Automatically check for updates",
                            CheckBoxChecked = AutoUpdateCheck
                        };

                        var msgBox = await MessageBox.Invoke(msgBoxVM);
                        if (msgBox.DialogResult == true)
                        {
                            AutoUpdateCheck = msgBox.CheckBoxChecked;
                        }
                    }
                }
            }
        }
    }

    private void Save()
    {
        if (_userData != null)
        {
            _userData.IncludedDirectories.Clear();
            foreach (var item in IncludedDirectories)
            {
                _userData.IncludedDirectories.Add(new SerializableSearchDirectory()
                {
                    FullName = item.FullName,
                    IsMarked = item.IsMarked
                });
            }

            _userData.ExcludedDirectories.Clear();
            foreach (var item in ExcludedDirectories)
            {
                _userData.ExcludedDirectories.Add(new SerializableSearchDirectory()
                {
                    FullName = item.FullName,
                    IsMarked = item.IsMarked
                });
            }

            _userData.SavedFileNamePatterns.Clear();
            _userData.SavedFileNamePatterns.AddRange(SavedFileNamePatterns);

            _userData.SavedIncludedExtensions.Clear();
            _userData.SavedIncludedExtensions.AddRange(SavedIncludedExtensions);

            _userData.SavedExcludedExtensions.Clear();
            _userData.SavedExcludedExtensions.AddRange(SavedExcludedExtensions);

            _userData.IncludeSubdirectories = IncludeSubdirectories;
            _userData.ExcludeSubdirectories = ExcludeSubdirectories;
            _userData.MatchSameFileName = MatchSameFileName;
            _userData.MatchSameContents = MatchSameContents;
            _userData.MatchSameType = MatchSameType;
            _userData.MatchSameSize = MatchSameSize;
            _userData.MatchAcrossDirectories = MatchAcrossDirectories;
            _userData.FileNamePattern = FileNamePattern;
            _userData.MinFileLength = MinimumFileLength;
            _userData.IncludedExtensions = IncludedExtensions;
            _userData.ExcludedExtensions = ExcludedExtensions;
            _userData.ExcludeSystemFiles = ExcludeSystemFiles;
            _userData.ExcludeHiddenFiles = ExcludeHiddenFiles;
            _userData.LastSelectedMarkingCriteria = (int)_lastValidMarkingCriteria;
            _userData.AdditionalOptionsExpanded = AdditionalOptionsExpanded;
            _userData.LastAddedDirectory = _lastAddedDirectory;
            _userData.ShowPreview = ShowPreview;
            _userData.PreviewPaneWidth = _oldPreviewPaneWidth;
            _userData.Theme = (int)Theme;
            _userData.AutoUpdateCheck = AutoUpdateCheck;
            _userData.DateCreateFrom = DateCreatedFrom;
            _userData.DateCreateTo = DateCreatedTo;
            _userData.DateModifiedFrom = DateModifiedFrom;
            _userData.DateModifiedTo = DateModifiedTo;

            var jsonString = JsonSerializer.Serialize(_userData, new JsonSerializerOptions() { WriteIndented = true });
            var userDataDir = Path.GetDirectoryName(_userDataFile);
            if (!Directory.Exists(userDataDir))
            {
                if (!string.IsNullOrEmpty(userDataDir))
                {
                    Directory.CreateDirectory(userDataDir);
                }
            }

            if (Directory.Exists(userDataDir))
            {
                File.WriteAllText(_userDataFile, jsonString);
            }
        }
    }

    #endregion // Private Methods
}
