// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace DupeClear.Converters;

public class TrueToStrikethroughConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isDeleted)
        {
            return isDeleted ? TextDecorations.Strikethrough : null;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
