// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DupeClear.Native;
using DupeClear.ViewModels;
using DupeClear.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DupeClear;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        var collection = new ServiceCollection();
        if (OperatingSystem.IsWindows())
        {
            collection.AddSingleton<IWindowService, Native.Windows.WindowService>();
            collection.AddSingleton<IFileService, Native.Windows.FileService>();
        }
        else if (OperatingSystem.IsLinux())
        {
            collection.AddSingleton<IFileService, Native.Linux.FileService>();
        }

        collection.AddTransient<MainView>();
        collection.AddTransient<MainViewModel>();

        var services = collection.BuildServiceProvider();
        var mainView = services.GetRequiredService<MainView>();
        var mainViewModel = services.GetRequiredService<MainViewModel>();
        mainView.DataContext = mainViewModel;

        DupeClear.Resources.Resources.Culture = new System.Globalization.CultureInfo("en");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow();
            mainWindow.Content = mainView;
            desktop.MainWindow = mainWindow;

            if (desktop.Args != null && desktop.Args.Length > 0)
            {
                Task.Run(async () => await mainViewModel.ImportAsync(desktop.Args.FirstOrDefault()));
            }
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = mainView;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
