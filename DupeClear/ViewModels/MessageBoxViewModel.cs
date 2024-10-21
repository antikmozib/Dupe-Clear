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

    public bool IsIconVisible => Icon != MessageBoxIcon.None;

    public MessageBoxButton Buttons { get; set; } = MessageBoxButton.OK;

    public string OKButtonContent { get; set; } = "_OK";

    public string CancelButtonContent { get; set; } = "_Cancel";

    public MessageBoxDefaultButton DefaultButton { get; set; } = MessageBoxDefaultButton.OK;

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

    [RelayCommand]
    private void OKButton(object? arg)
    {
        Close(true);
    }

    [RelayCommand]
    private void CancelButton(object? arg)
    {
        Close(false);
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

    protected void RaiseEvent(EventHandler? handler)
    {
        handler?.Invoke(this, new EventArgs());
    }
}
