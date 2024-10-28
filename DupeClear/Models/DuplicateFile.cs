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

    public DateTime? Created { get; }

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

    public bool IsDeleted => File.Exists(FullName) == false;

    public bool? IsHidden { get; }

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

    public bool? IsSystemFile { get; }

    public long? Length { get; }

    public DateTime? Modified { get; }

    public string Name => Path.GetFileName(FullName);

    public string NameWithoutExtension
    {
        get
        {
            var ext = Path.GetExtension(FullName);
            if (string.IsNullOrEmpty(ext))
            {
                return Name;
            }
            else
            {
                return Name.Substring(0, Name.Length - ext.Length);
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
        : this(
              fullName,
              null,
              null,
              null,
              null,
              null,
              fileService)
    { }

    public DuplicateFile(SerializableDuplicateFile serializable, IFileService? fileService = null)
        : this(
              serializable.FullName,
              serializable.Created,
              serializable.IsHidden,
              serializable.IsSystemFile,
              serializable.Length,
              serializable.Modified,
              fileService)
    { }

    private DuplicateFile(
        string? fullName,
        DateTime? created,
        bool? isHidden,
        bool? isSystemFile,
        long? length,
        DateTime? modified,
        IFileService? fileService)
    {
        if (string.IsNullOrEmpty(fullName))
        {
            throw new InvalidOperationException(nameof(fullName) + " cannot be null or empty.");
        }
        else
        {
            _fileService = fileService;

            FullName = fullName;
            if (File.Exists(fullName))
            {
                var fi = new FileInfo(fullName);
                Created = fi.CreationTime;
                IsHidden = fi.Attributes.HasFlag(FileAttributes.Hidden);
                IsSystemFile = fi.Attributes.HasFlag(FileAttributes.System);
                Length = fi.Length;
                Modified = fi.LastWriteTime;
            }
            else
            {
                Created = created;
                IsHidden = isHidden;
                IsSystemFile = isSystemFile;
                Length = length;
                Modified = modified;
            }
        }
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
