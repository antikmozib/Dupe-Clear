// Copyright (C) 2024 Antik Mozib. All rights reserved.

using Avalonia;
using Avalonia.VisualTree;
using DupeClear.Models.Finder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DupeClear.Helpers;

public static class Extensions
{
    public static FinderOption AddOption(this FinderOption option, FinderOption flag)
    {
        if (!option.HasFlag(flag))
        {
            option |= flag;
        }

        return option;
    }

    public static FinderOption RemoveOption(this FinderOption option, FinderOption flag)
    {
        if (option.HasFlag(flag))
        {
            option &= ~flag;
        }

        return option;
    }

    public static bool HasOption(this FinderOption option, FinderOption flag) => option.HasFlag(flag);

    public static string ConvertLengthToString(this long length)
    {
        double lengthDbl = length;

        if (length > 1000 * 1000 * 1000)
        {
            return $"{lengthDbl / (1024 * 1024 * 1024):N3} GB";
        }
        else if (length > 1000 * 1000)
        {
            return $"{lengthDbl / (1024 * 1024):N2} MB";
        }
        else if (length > 1000)
        {
            return $"{lengthDbl / 1024:N0} KB";
        }
        else
        {
            return $"{length:N0} B";
        }
    }

    public static string ConvertMillisecondsToString(this double milliseconds)
    {
        var ts = TimeSpan.FromMilliseconds(milliseconds);
        var hr = ts.Hours;
        var min = ts.Minutes;
        var sec = ts.Seconds;

        if (hr > 0)
        {
            return $"{hr}h {min}m";
        }
        else if (min > 0)
        {
            return $"{min}m";
        }
        else
        {
            return $"{sec}s";
        }
    }

    public static string ConvertMillisecondsToString(this long milliseconds) => ConvertMillisecondsToString(Convert.ToDouble(milliseconds));

    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }

    public static T? ParentOfType<T>(this Visual? visual) where T : Visual
    {
        if (visual == null)
        {
            return null;
        }

        var parent = visual.GetVisualParent();

        if (parent == null)
        {
            return null;
        }
        else if (parent.GetType() == typeof(T) || parent.GetType().IsSubclassOf(typeof(T)))
        {
            return (T)parent;
        }
        else
        {
            return ParentOfType<T>(parent);
        }
    }

    public static IEnumerable<T> ChildrenOfType<T>(this Visual? visual) where T : Visual
    {
        List<T> result = [];

        if (visual != null)
        {
            foreach (var item in visual.GetVisualChildren())
            {
                if (item != null)
                {
                    if (item.GetType() == typeof(T) || item.GetType().IsSubclassOf(typeof(T)))
                    {
                        result.Add((T)item);
                    }

                    if (item.GetVisualChildren().Any())
                    {
                        result.AddRange(ChildrenOfType<T>(item));
                    }
                }
            }
        }

        return result;
    }

    public static Point? GetRelativePosition(this Visual? visual, Visual? relativeTo)
    {
        if (visual == null || relativeTo == null)
        {
            return null;
        }

        var x = visual?.Bounds.X;
        var y = visual?.Bounds.Y;
        var current = visual;

        while (true)
        {
            var parent = current.ParentOfType<Visual>();

            x += parent?.Bounds.X;
            y += parent?.Bounds.Y;

            if (parent == null || parent?.GetType() == relativeTo?.GetType())
            {
                break;
            }

            current = parent;
        }

        if (x != null && y != null)
        {
            return new Point(x.Value, y.Value);
        }
        else
        {
            return null;
        }
    }

    public static Exception? GetInnermostException(this Exception exception)
    {
        Exception? ie = exception;
        if (ie == null)
        {
            return null;
        }
        else if (ie.InnerException == null)
        {
            return ie;
        }
        else
        {
            return ie.InnerException.GetInnermostException();
        }
    }

    public static bool IsSubdirectoryOf(this string current, string compareAgainst)
    {
        var dir = Path.GetFullPath(current);
        var parentDir = Path.GetFullPath(compareAgainst).TrimEnd(Path.DirectorySeparatorChar).Append(Path.DirectorySeparatorChar);

        return dir.StartsWith(new string(parentDir.ToArray()), StringComparison.OrdinalIgnoreCase);
    }
}
