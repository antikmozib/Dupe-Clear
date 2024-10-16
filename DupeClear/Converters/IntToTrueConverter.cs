// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace DupeClear.Converters;

public class IntToTrueConverter : IValueConverter
{
    public bool Inverted { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && parameter != null)
        {
            var current = (int)value;
            var compareAgainst = (int)parameter;

            return Inverted ? current != compareAgainst : current == compareAgainst;
        }

        return !Inverted;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
