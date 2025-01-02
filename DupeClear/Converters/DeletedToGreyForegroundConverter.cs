// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace DupeClear.Converters;

public class DeletedToGreyForegroundConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool deleted)
        {
            object? fgBrush = null;
            if (deleted)
            {
                Application.Current?.TryGetResource("AppSearchResultsDeletedForegroundBrush", Application.Current.ActualThemeVariant, out fgBrush);
                if (fgBrush != null)
                {
                    return (Brush)fgBrush;
                }
            }
            else
            {
                Application.Current?.TryGetResource("AppSearchResultsForegroundBrush", Application.Current.ActualThemeVariant, out fgBrush);
                if (fgBrush != null)
                {
                    return (Brush)fgBrush;
                }
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
