// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DupeClear.Helpers;
using DupeClear.Models.Events;
using DupeClear.Models.MessageBox;
using DupeClear.Native;
using DupeClear.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DupeClear.Views;

public partial class MainView : UserControl
{
    private readonly IWindowService? _windowService;
    private MainViewModel? _viewModel;
    private bool _viewModelClosed;

    public MainView()
    {
        InitializeComponent();

        IncludedDirectoriesListBox.AddHandler(DragDrop.DragOverEvent, IncludedDirectoriesListBox_DragOver);
        IncludedDirectoriesListBox.AddHandler(DragDrop.DropEvent, IncludedDirectoriesListBox_Drop);

        ExcludedDirectoriesListBox.AddHandler(DragDrop.DragOverEvent, ExcludedDirectoriesListBox_DragOver);
        ExcludedDirectoriesListBox.AddHandler(DragDrop.DropEvent, ExcludedDirectoriesListBox_Drop);

        AppIconImage.AddHandler(PointerReleasedEvent, AppIconImage_PreviewPointerReleased, RoutingStrategies.Tunnel);
        AppTitleTextBlock.AddHandler(PointerReleasedEvent, AppTitleTextBlock_PreviewPointerReleased, RoutingStrategies.Tunnel);
    }

    public MainView(IWindowService windowService) : this()
    {
        _windowService = windowService;
    }

    private void UserControl_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is MainViewModel mainViewModel)
        {
            if (_viewModel != null)
            {
                _viewModel.SearchPerformed -= ViewModel_SearchPerformed;
                _viewModel.FindWithinResultsPerformed -= ViewModel_FindWithinResultsPerformed;
                _viewModel.Closed -= ViewModel_Closed;
            }

            _viewModel = mainViewModel;
            _viewModel.AsyncDirectoryPicker = PickDirectoryAsync;
            _viewModel.AsyncFileSaver = SaveFileAsync;
            _viewModel.AsyncFilePicker = OpenFileAsync;
            _viewModel.MessageBox = ShowMessageBoxAsync;
            _viewModel.SearchPerformed += ViewModel_SearchPerformed;
            _viewModel.FindWithinResultsPerformed += ViewModel_FindWithinResultsPerformed;
            _viewModel.Closed += ViewModel_Closed;
        }
    }

    private void UserControl_Loaded(object? sender, RoutedEventArgs e)
    {
        if (Parent is Window parentWindow)
        {
            parentWindow.PropertyChanged += ParentWindow_PropertyChanged;
            parentWindow.Closing += ParentWindow_Closing;
        }
    }

    private void ParentWindow_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (OperatingSystem.IsWindows())
        {
            if (e.Property.Name == nameof(Window.WindowState))
            {
                if (sender is Window window)
                {
                    if (window.WindowState == WindowState.Maximized)
                    {
                        MainGrid.Margin = new Thickness(8);
                    }
                    else
                    {
                        MainGrid.Margin = new Thickness(0);
                    }
                }
            }
        }
    }

    private void ParentWindow_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (_viewModel != null && !_viewModelClosed)
        {
            _viewModel.CloseCommand.Execute(null);
            e.Cancel = true;
        }
    }

    private void AppIconImage_PreviewPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is Control c)
        {
            ShowWindowContextMenu(c);
        }
    }

    private void AppTitleTextBlock_PreviewPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is Control c)
        {
            var point = e.GetCurrentPoint(c);
            if (point.Properties.PointerUpdateKind == PointerUpdateKind.RightButtonReleased)
            {
                ShowWindowContextMenu(c);
            }
        }
    }

    private void IncludedDirectoriesListBox_DoubleTapped(object? sender, TappedEventArgs e)
    {
        CheckListBoxItemWhenDoubleClicked(sender, e);
    }

    private void IncludedDirectoriesListBox_Drop(object? sender, DragEventArgs e)
    {
        var files = e.Data.GetFiles();
        if (files != null)
        {
            _viewModel?.AddDirectoryForInclusion(files.Select(x => x.Path.LocalPath).Where(x => Directory.Exists(x)));
        }
    }

    private void IncludedDirectoriesListBox_DragOver(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Files) && e.Data.GetFiles()!.Any(x => Directory.Exists(x.Path.LocalPath)))
        {
            e.DragEffects = DragDropEffects.Copy;
        }
        else
        {
            e.DragEffects = DragDropEffects.None;
        }
    }

    private void ExcludedDirectoriesListBox_DoubleTapped(object? sender, TappedEventArgs e)
    {
        CheckListBoxItemWhenDoubleClicked(sender, e);
    }

    private void ExcludedDirectoriesListBox_Drop(object? sender, DragEventArgs e)
    {
        var files = e.Data.GetFiles();
        if (files != null)
        {
            _viewModel?.AddDirectoryForExclusion(files.Select(x => x.Path.LocalPath).Where(x => Directory.Exists(x)));
        }
    }

    private void ExcludedDirectoriesListBox_DragOver(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Files) && e.Data.GetFiles()!.Any(x => Directory.Exists(x.Path.LocalPath)))
        {
            e.DragEffects = DragDropEffects.Copy;
        }
        else
        {
            e.DragEffects = DragDropEffects.None;
        }
    }

    private void IncludedDirectoryCheckBox_Click(object? sender, RoutedEventArgs e)
    {
        SelectParentListBoxItemWhenItemInputClicked(sender);
    }

    private void ExcludedDirectoryCheckBox_Click(object? sender, RoutedEventArgs e)
    {
        SelectParentListBoxItemWhenItemInputClicked(sender);
    }

    private void ResultsGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is DataGrid dg)
        {
            dg.ScrollIntoView(dg.SelectedItem, null);
        }
    }

    private void SearchResultCheckBox_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is CheckBox cb)
        {
            var gvRow = cb.ParentOfType<DataGridRow>();
            if (gvRow != null && gvRow.IsSelected == false)
            {
                var gv = gvRow.ParentOfType<DataGrid>();
                if (gv != null)
                {
                    gv.SelectedItems.Clear();
                    gvRow.IsSelected = true;
                }
            }
        }
    }

    private void SelectAllMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        ResultsGrid.SelectAll();
    }

    private void ViewModel_SearchPerformed(object? sender, EventArgs e)
    {
        MainTabControl.SelectedIndex = 2;
    }

    private void ViewModel_FindWithinResultsPerformed(object? sender, FindWithinSearchResultsSearchPerformedEventArgs e)
    {
        if (e.MatchFound)
        {
            FindWithSearchResultsDataGrid.SelectedIndex = 0;
            FindWithSearchResultsDataGrid.Focus();
        }
    }

    private void ViewModel_Closed(object? sender, EventArgs e)
    {
        if (Parent is Window parentWindow)
        {
            _viewModelClosed = true;
            Dispatcher.UIThread.InvokeAsync(parentWindow.Close);
        }
    }

    private async Task<IEnumerable<string>> PickDirectoryAsync(string startingDirectory)
    {
        List<string> result = [];
        var tl = TopLevel.GetTopLevel(this);
        if (tl != null)
        {
            var folders = await tl.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                AllowMultiple = true,
                SuggestedStartLocation = await tl.StorageProvider.TryGetFolderFromPathAsync(startingDirectory)
            });

            folders.ForEach(x => result.Add(x.Path.LocalPath));
        }

        return result;
    }

    private async Task<string?> SaveFileAsync(string? title, string? fileName)
    {
        var tl = TopLevel.GetTopLevel(this);
        if (tl != null)
        {
            var opts = new FilePickerSaveOptions
            {
                Title = title,
                SuggestedFileName = fileName,
                FileTypeChoices = new List<FilePickerFileType>()
                    {
                        new FilePickerFileType("Dupe Clear Search Result")
                        {
                            Patterns = new[] { $"*{Constants.SearchResultsFileExtension}" }
                        }
                    }
            };

            var file = await tl.StorageProvider.SaveFilePickerAsync(opts);

            if (file != null)
            {
                return file.Path.LocalPath;
            }
        }

        return null;
    }

    private async Task<string?> OpenFileAsync(string? title = null)
    {
        var tl = TopLevel.GetTopLevel(this);
        if (tl != null)
        {
            var opts = new FilePickerOpenOptions
            {
                Title = title,
                FileTypeFilter = new List<FilePickerFileType>()
                    {
                        new FilePickerFileType("Dupe Clear Search Results")
                        {
                            Patterns = new[] { $"*{Constants.SearchResultsFileExtension}" }
                        }
                    }
            };

            var file = await tl.StorageProvider.OpenFilePickerAsync(opts);

            if (file != null)
            {
                return file.FirstOrDefault()?.Path.LocalPath;
            }
        }

        return null;
    }

    private async Task<MessageBoxResult> ShowMessageBoxAsync(MessageBoxViewModel viewModel)
    {
        return await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var tl = TopLevel.GetTopLevel(this)!;
            MessageBoxWindow? window;
            if (_windowService == null)
            {
                window = new MessageBoxWindow();
            }
            else
            {
                window = new MessageBoxWindow(_windowService);
            }

            window.DataContext = viewModel;
            await window.ShowDialog((Window)tl);

            return viewModel.Result;
        });
    }

    private void SelectParentListBoxItemWhenItemInputClicked(object? sender, bool deselectOthers = false)
    {
        if (sender is CheckBox cb)
        {
            var lbItem = cb.ParentOfType<ListBoxItem>();

            // If item is not selected, select the item and deselect all other selected items. If item is selected,
            // deselect all other selected items if the parameter asks for it.
            if (lbItem != null && (!lbItem.IsSelected || deselectOthers))
            {
                var lb = cb.ParentOfType<ListBox>();
                if (lb != null)
                {
                    lb.SelectedItems?.Clear();
                    lbItem.IsSelected = true;
                }
            }
        }
    }

    private void CheckListBoxItemWhenDoubleClicked(object? sender, TappedEventArgs e)
    {
        if (sender is ListBox lb)
        {
            var lbItems = lb.ChildrenOfType<ListBoxItem>();
            foreach (var lbItem in lbItems)
            {
                if (lbItem.IsPointerOver)
                {
                    var cb = lbItem.ChildrenOfType<CheckBox>().FirstOrDefault();
                    if (cb != null && cb.IsEnabled && !cb.IsPointerOver)
                    {
                        cb.IsChecked = !cb.IsChecked;
                    }
                }
            }
        }
    }

    private void ShowWindowContextMenu(Control relativeTo)
    {
        var tl = TopLevel.GetTopLevel(this);
        if (tl != null)
        {
            var hWnd = tl.TryGetPlatformHandle();
            if (hWnd != null)
            {
                var x = 0;
                var y = 0;
                if (Parent is Window parentWindow)
                {
                    var pt = relativeTo.GetRelativePosition(parentWindow);
                    if (pt != null)
                    {
                        x = (int)pt.Value.X;
                        y = (int)pt.Value.Y;
                    }
                }

                _windowService?.ShowContextMenu(hWnd.Handle, x, y + (int)relativeTo.Bounds.Height);
            }
        }
    }
}
