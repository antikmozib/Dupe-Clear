// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DupeClear.Converters;

public class AllTrueToTrueConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        return values.All(x =>
        {
            if (x is bool boolVal)
            {
                return boolVal == true;
            }

            return false;
        });
    }
}
