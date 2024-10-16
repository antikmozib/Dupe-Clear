// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia.Data.Converters;
using DupeClear.Models.MessageBox;
using System;
using System.Globalization;

namespace DupeClear.Converters;

public class MessageBoxIconToValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Question:
                    return "\uf059";

                case MessageBoxIcon.Warning:
                    return "\uf071";

                case MessageBoxIcon.Error:
                    return "\uf057";

                default:
                    return "\uf05a";
            }
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
