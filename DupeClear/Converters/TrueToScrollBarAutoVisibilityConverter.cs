// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear.Converters;
public class TrueToScrollBarAutoVisibilityConverter : IValueConverter
{
    public bool Inverted { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b == !Inverted ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;
        }

        return Inverted ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
