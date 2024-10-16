// Copyright (C) 2024 Antik Mozib. All rights reserved.

using DupeClear.Native;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace DupeClear.Models;

public class SearchDirectory : INotifyPropertyChanged
{
    private readonly IFileService? _fileService;

    public string Name { get; }

    public string FullName { get; }

    private bool _includeSubdirectories;
    public bool IncludeSubdirectories
    {
        get => _includeSubdirectories;
        set
        {
            if (_includeSubdirectories != value)
            {
                _includeSubdirectories = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isMarked;
    public bool IsMarked
    {
        get => _isMarked;
        set
        {
            if (_isMarked != value)
            {
                _isMarked = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _isExcluded;
    public bool IsExcluded
    {
        get => _isExcluded;
        set
        {
            if (_isExcluded != value)
            {
                _isExcluded = value;
                OnPropertyChanged();
            }
        }
    }

    public Avalonia.Media.Imaging.Bitmap? FolderIcon => _fileService?.GetFolderIcon(FullName);

    public SearchDirectory(string fullName, IFileService? fileService = null)
    {
        _fileService = fileService;

        Name = new DirectoryInfo(fullName).Name;
        FullName = fullName;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
