// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DupeClear.Converters;

public class MultiCommandParameterConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values;
    }
}
