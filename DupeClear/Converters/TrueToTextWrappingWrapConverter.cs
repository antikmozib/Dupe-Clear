// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DupeClear.Converters;
public class TrueToTextWrappingWrapConverter : IValueConverter
{
    public bool Inverted { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
        {
            return b == !Inverted ? TextWrapping.Wrap : TextWrapping.NoWrap;
        }

        return Inverted ? TextWrapping.Wrap : TextWrapping.NoWrap;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
