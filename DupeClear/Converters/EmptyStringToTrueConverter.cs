// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace DupeClear.Converters;

public class EmptyStringToTrueConverter : IValueConverter
{
    public bool Inverted { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null)
        {
            return Inverted ? !string.IsNullOrWhiteSpace(value.ToString()) : string.IsNullOrWhiteSpace(value.ToString());
        }

        return !Inverted;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
