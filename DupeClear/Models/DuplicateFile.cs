// Copyright (C) 2024 Antik Mozib. All rights reserved.

using DupeClear.Models.Serializable;
using DupeClear.Native;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DupeClear.Models;

public class DuplicateFile : INotifyPropertyChanged
{
    private readonly IFileService? _fileService;

    public DateTime Created { get; }

    public string? DirectoryName => Path.GetDirectoryName(FullName);

    public Avalonia.Media.Imaging.Bitmap? FileIcon => _fileService?.GetFileIcon(FullName);

    public string FullName { get; }

    private int? _group;
    public int? Group
    {
        get => _group;
        set
        {
            if (_group != value)
            {
                _group = value;
                OnPropertyChanged();
            }
        }
    }

    public string? Hash { get; set; }

    public bool IsDeleted => !File.Exists(FullName);

    public bool IsHidden { get; }

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

    public bool IsSystemFile { get; }

    public long Length { get; }

    public DateTime Modified { get; }

    public string Name => Path.GetFileName(FullName);

    public string NameWithoutExtension
    {
        get
        {
            if (Name.Contains('.'))
            {
                return Name.Substring(0, Name.IndexOf('.'));
            }
            else
            {
                return Name;
            }
        }
    }

    private Match? _patternMatch;
    public Match? PatternMatch
    {
        get => _patternMatch;
        set
        {
            if (_patternMatch != value)
            {
                _patternMatch = value;
                if (value != null && value.Success && PatternMatchValue != value.Value)
                {
                    PatternMatchValue = value.Value;
                }
            }
        }
    }

    public string? PatternMatchValue { get; private set; }

    public string? Type => _fileService?.GetFileDescription(FullName);

    public event PropertyChangedEventHandler? PropertyChanged;

    public DuplicateFile(string fullName, IFileService? fileService = null)
    {
        _fileService = fileService;

        var fi = new FileInfo(fullName);
        Created = fi.CreationTime;
        FullName = fullName;
        IsHidden = fi.Attributes.HasFlag(FileAttributes.Hidden);
        IsSystemFile = fi.Attributes.HasFlag(FileAttributes.System);
        Length = fi.Length;
        Modified = fi.LastWriteTime;
    }

    public DuplicateFile(SerializableDuplicateFile serializable, IFileService? fileService = null)
    {
        _fileService = fileService;

        Created = serializable.Created;
        FullName = serializable.FullName ?? "";
        IsHidden = serializable.IsHidden;
        IsSystemFile = serializable.IsSystemFile;
        Length = serializable.Length;
        Modified = serializable.Modified;
    }

    public void Refresh()
    {
        if (IsDeleted && IsMarked)
        {
            IsMarked = false;
        }

        OnPropertyChanged(nameof(IsDeleted));
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
