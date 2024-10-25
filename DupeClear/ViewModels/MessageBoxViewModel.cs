// Copyright (C) 2024 Antik Mozib. All rights reserved.

using CommunityToolkit.Mvvm.Input;
using DupeClear.Models.MessageBox;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace DupeClear.ViewModels;

public partial class MessageBoxViewModel : ViewModelBase
{
    private readonly string? _appTitle;

    private string? _primaryButtonContent;

    public Func<string?, Task>? AsyncClipboardCopier { get; set; } = null;

    private string? _title;
    public string? Title
    {
        get
        {
            if (string.IsNullOrEmpty(_title))
            {
                return _appTitle;
            }

            return _title;
        }
        set
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged();
            }
        }
    }

    public string? Header { get; set; }

    public string? Message { get; set; }

    public string? SecondaryMessage { get; set; }

    public bool SecondaryMessageWrapped { get; set; } = true;

    public MessageBoxIcon Icon { get; set; } = MessageBoxIcon.None;

    public MessageBoxButton Buttons { get; set; } = MessageBoxButton.OK;

    public string? OKButtonContent
    {
        get => GetPrimaryButtonContent();
        set
        {
            if (_primaryButtonContent != value)
            {
                _primaryButtonContent = value;
                OnPropertyChanged();
            }
        }
    }

    public string? YesButtonContent
    {
        get => GetPrimaryButtonContent();
        set
        {
            if (_primaryButtonContent != value)
            {
                _primaryButtonContent = value;
                OnPropertyChanged();
            }
        }
    }

    /*private string? _yesButtonContent;
    public string? YesButtonContent
    {
        get
        {
            if (string.IsNullOrEmpty(_yesButtonContent))
            {
                if (Buttons == MessageBoxButton.OK || Buttons == MessageBoxButton.OKCancel)
                {
                    YesButtonContent = "_OK";
                }
                else
                {
                    YesButtonContent = "_Yes";
                }
            }

            return _yesButtonContent;
        }
        set
        {
            if (_yesButtonContent != value)
            {
                _yesButtonContent = value;
                OnPropertyChanged();
            }
        }
    }*/

    public string NoButtonContent { get; set; } = "_No";

    public string CancelButtonContent { get; set; } = "_Cancel";

    public MessageBoxDefaultButton DefaultButton { get; set; } = MessageBoxDefaultButton.Auto;

    public string? HyperlinkButtonContent { get; set; }

    public Action? HyperlinkButtonAction { get; set; }

    public string? CustomButton1Content { get; set; }

    public Action? CustomButton1Action { get; set; }

    public string? CustomButton2Content { get; set; }

    public Action? CustomButton2Action { get; set; }

    public string? CheckBoxContent { get; set; }

    public bool CheckBoxChecked { get; set; }

    public MessageBoxResult Result { get; } = new MessageBoxResult();

    public event EventHandler? Closed;

    public MessageBoxViewModel()
    {
        var assm = Assembly.GetEntryAssembly();
        if (assm != null)
        {
            var appTitle = assm.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            _appTitle = appTitle;
        }
    }

    public void Close(bool? dialogResult = null)
    {
        Result.CheckBoxChecked = CheckBoxChecked;
        Result.DialogResult = dialogResult;
        RaiseEvent(Closed);
    }

    [RelayCommand]
    private async Task CopyToClipboardAsync(object? arg)
    {
        if (AsyncClipboardCopier != null)
        {
            await AsyncClipboardCopier.Invoke(SecondaryMessage);
        }
    }

    /// <summary>
    /// The Yes or OK button command.
    /// </summary>
    /// <param name="arg"></param>
    [RelayCommand]
    private void PrimaryButton(object? arg)
    {
        Close(true);
    }

    [RelayCommand]
    private void NoButton(object? arg)
    {
        Close(false);
    }

    [RelayCommand]
    private void CancelButton(object? arg)
    {
        bool? dialogResult = false;
        if (Buttons == MessageBoxButton.YesNoCancel)
        {
            dialogResult = null;
        }

        Close(dialogResult);
    }

    [RelayCommand]
    private void HyperlinkButton(object? arg)
    {
        HyperlinkButtonAction?.Invoke();
    }

    [RelayCommand]
    private void CustomButton1(object? arg)
    {
        CustomButton1Action?.Invoke();
    }

    [RelayCommand]
    private void CustomButton2(object? arg)
    {
        CustomButton2Action?.Invoke();
    }

    private string GetPrimaryButtonContent()
    {
        if (string.IsNullOrEmpty(_primaryButtonContent))
        {
            _primaryButtonContent = Buttons == MessageBoxButton.OK || Buttons == MessageBoxButton.OKCancel ? "_OK" : "_Yes";
        }

        return _primaryButtonContent;
    }

    protected void RaiseEvent(EventHandler? handler)
    {
        handler?.Invoke(this, new EventArgs());
    }
}
