// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using DupeClear.Native;
using DupeClear.ViewModels;
using System.Threading.Tasks;

namespace DupeClear;

public partial class MessageBoxWindow : Window
{
    private MessageBoxViewModel? _viewModel;

    private readonly IWindowService? _windowService;

    public MessageBoxWindow()
    {
        InitializeComponent();
    }

    public MessageBoxWindow(IWindowService windowService) : this()
    {
        _windowService = windowService;
    }

    private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_windowService != null)
        {
            var hWnd = TryGetPlatformHandle();
            if (hWnd != null)
            {
                _windowService.HideIcon(hWnd.Handle);
                _windowService.HideMaxMinButtons(hWnd.Handle);
            }
        }
    }

    private void Window_DataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is MessageBoxViewModel vm)
        {
            if (_viewModel != null)
            {
                _viewModel.Closed -= ViewModel_Closed;
            }

            _viewModel = vm;
            vm.AsyncClipboardCopier = CopyToClipboard;
            vm.Closed += ViewModel_Closed;
        }
    }

    private void ViewModel_Closed(object? sender, System.EventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(Close);
    }

    private async Task CopyToClipboard(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var clipboard = GetTopLevel(this)?.Clipboard;
        var dataObject = new DataObject();
        dataObject.Set(DataFormats.Text, text);
        if (clipboard != null)
        {
            await clipboard.SetDataObjectAsync(dataObject);
        }
    }
}
