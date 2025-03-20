// Copyright (C) 2017-2025 Antik Mozib. All rights reserved.

using DupeClear.Helpers;
using DupeClear.Native;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DupeClear.Models.Finder;

public class FinderService
{
    private static async Task<IEnumerable<SearchDirectory>> ListDirectoriesAsync(
        string path,
        bool includeSubdirectories,
        bool excludeSystemDirectories,
        bool excludeHiddenHiddenDirectories,
        CancellationToken ct = default)
    {
        return await Task.Run(async () =>
        {
            List<SearchDirectory> result = [];
            if (Directory.Exists(path))
            {
                var di = new DirectoryInfo(path);
                if (excludeSystemDirectories && di.Attributes.HasFlag(FileAttributes.System))
                {
                    return result;
                }

                if (excludeHiddenHiddenDirectories && di.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    return result;
                }

                foreach (var item in di.GetDirectories())
                {
                    ct.ThrowIfCancellationRequested();

                    result.Add(new SearchDirectory(item.FullName));
                    if (includeSubdirectories)
                    {
                        foreach (var subdir in await ListDirectoriesAsync(item.FullName, includeSubdirectories, excludeSystemDirectories, excludeHiddenHiddenDirectories, ct))
                        {
                            result.Add(subdir);
                        }
                    }
                }
            }

            return result;
        });
    }

    private static async Task<IEnumerable<DuplicateFile>> ListFilesAsync(
        string path,
        IEnumerable<string>? includedExtensions,
        IEnumerable<string>? excludedExtensions,
        bool excludeSystemFiles,
        bool excludeHiddenFiles,
        long minLength,
        IFileService? fileService = null,
        CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            List<DuplicateFile> result = [];
            var di = new DirectoryInfo(path);
            foreach (var item in di.GetFiles())
            {
                ct.ThrowIfCancellationRequested();

                if (item.Length == 0 || item.Length < minLength)
                {
                    continue;
                }

                if (includedExtensions != null)
                {
                    if (!includedExtensions.Any(x => string.Compare(item.Extension, x, true) == 0))
                    {
                        continue;
                    }
                }

                if (excludedExtensions != null)
                {
                    if (excludedExtensions.Any(x => string.Compare(item.Extension, x, true) == 0))
                    {
                        continue;
                    }
                }

                var df = new DuplicateFile(item.FullName, fileService);
                if (excludeSystemFiles && df.IsSystemFile == true)
                {
                    continue;
                }

                if (excludeHiddenFiles && df.IsHidden == true)
                {
                    continue;
                }

                result.Add(df);
            }

            return result;
        });
    }

    private static string GetFileHash(string path)
    {
        Stream stream;
        if (JpegPatcher.IsJpeg(path))
        {
            var memStream = new MemoryStream();
            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            stream = JpegPatcher.PatchAwayExif(fileStream, memStream);
        }
        else
        {
            stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        stream.Position = 0;
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(stream);
        stream.Dispose();

        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    public static async Task<FinderResult> FindAsync(
        IEnumerable<SearchDirectory> includedDirectories,
        IEnumerable<SearchDirectory> excludedDirectories,
        FinderOption options,
        string? fileNamePattern,
        long minLength,
        IEnumerable<string>? includedExtensions,
        IEnumerable<string>? excludedExtensions,
        DateTime? dateCreatedFrom,
        DateTime? dateCreatedTo,
        DateTime? dateModifiedFrom,
        DateTime? dateModifiedTo,
        IFileService? fileService = null,
        IProgress<FinderProgress>? progressReporter = null,
        CancellationToken ct = default)
    {
        FinderResult result = new();
        List<SearchDirectory> includedDirs = [];

        // List all included directories, including possible duplicates.
        foreach (var dir in includedDirectories)
        {
            ct.ThrowIfCancellationRequested();

            includedDirs.Add(dir);
            if (dir.IncludeSubdirectories)
            {
                try
                {
                    includedDirs.AddRange(await ListDirectoriesAsync(
                        dir.FullName,
                        true,
                        options.HasOption(FinderOption.ExcludeSystemFiles),
                        options.HasOption(FinderOption.ExcludeHiddenFiles),
                        ct));
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    result.Errors.TryAdd(dir.FullName, ex.Message);
                }
            }
        }

        // Build list of excluded directories.
        List<SearchDirectory> excludedDirs = [];
        foreach (var dir in excludedDirectories)
        {
            ct.ThrowIfCancellationRequested();

            excludedDirs.Add(dir);
            if (dir.IncludeSubdirectories)
            {
                try
                {
                    excludedDirs.AddRange(await ListDirectoriesAsync(dir.FullName, true, false, false, ct));
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch
                {

                }
            }
        }

        foreach (var dir in excludedDirectories.Where(x => includedDirs.Any(y => string.Compare(y.FullName, x.FullName, true) == 0)))
        {
            result.ExcludedDirectories.Add(dir);
        }

        // Final list of included directories, removing duplicates and excluded directories.
        includedDirs = includedDirs
            .DistinctBy(x => x.FullName)
            .Where(x => !excludedDirs.Any(y => string.Compare(y.FullName, x.FullName, true) == 0))
            .ToList();

        // Build list of files to search.
        List<DuplicateFile> targetFiles = [];
        foreach (var dir in includedDirs)
        {
            ct.ThrowIfCancellationRequested();

            try
            {
                targetFiles.AddRange(await ListFilesAsync(
                    dir.FullName,
                    includedExtensions,
                    excludedExtensions,
                    options.HasOption(FinderOption.ExcludeSystemFiles),
                    options.HasOption(FinderOption.ExcludeHiddenFiles),
                    minLength,
                    fileService,
                    ct));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                result.Errors.TryAdd(dir.FullName, ex.Message);
            }
        }

        var group = 1;
        long duplicateLength = 0;
        var progressCount = 0;
        long progressLength = 0;
        List<DuplicateFile> sourceFiles = [];
        sourceFiles.AddRange(targetFiles);
        if (dateCreatedFrom.HasValue || dateCreatedTo.HasValue || dateModifiedFrom.HasValue || dateModifiedTo.HasValue)
        {
            if (dateCreatedFrom.HasValue)
            {
                sourceFiles = sourceFiles.Except(sourceFiles.Where(x => x.Created < dateCreatedFrom)).ToList();
            }

            if (dateCreatedTo.HasValue)
            {
                sourceFiles = sourceFiles.Except(sourceFiles.Where(x => x.Created > dateCreatedTo)).ToList();
            }

            if (dateModifiedFrom.HasValue)
            {
                sourceFiles = sourceFiles.Except(sourceFiles.Where(x => x.Modified < dateModifiedFrom)).ToList();
            }

            if (dateModifiedTo.HasValue)
            {
                sourceFiles = sourceFiles.Except(sourceFiles.Where(x => x.Modified < dateModifiedTo)).ToList();
            }
        }

        var totalCount = sourceFiles.Count;
        var totalLength = sourceFiles.Where(x => x.Length.HasValue).Sum(x => x.Length!.Value);
        foreach (var file1 in sourceFiles)
        {
            if (ct.IsCancellationRequested)
            {
                break;
            }

            var stopwatch = Stopwatch.StartNew();

            progressReporter?.Report(new FinderProgress()
            {
                ProgressCount = progressCount,
                ProgressLength = progressLength,
                TotalCount = totalCount,
                TotalLength = totalLength,
                CurrentFileName = file1.FullName,
                DuplicateCount = result.DuplicateCount,
                DuplicateLength = duplicateLength,
            });

            if (file1.Group.HasValue)
            {
                continue;
            }

            progressCount++;
            if (file1.Length.HasValue)
            {
                progressLength += file1.Length.Value;
            }

            if (!string.IsNullOrWhiteSpace(fileNamePattern) && file1.PatternMatch == null)
            {
                file1.PatternMatch = Regex.Match(file1.NameWithoutExtension, fileNamePattern, RegexOptions.IgnoreCase);
            }

            var removeFromTarget = new List<DuplicateFile> { file1 };
            foreach (var file2 in targetFiles)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                if (file2 == file1)
                {
                    continue;
                }

                // Type
                if (options.HasOption(FinderOption.SameType))
                {
                    if (file2.Type != file1.Type)
                    {
                        continue;
                    }
                }

                // Size
                if (options.HasOption(FinderOption.SameSize))
                {
                    if (file2.Length != file1.Length)
                    {
                        continue;
                    }
                }

                // File name or file name pattern
                if (options.HasOption(FinderOption.SameFileName))
                {
                    // File name pattern
                    if (!string.IsNullOrWhiteSpace(fileNamePattern))
                    {
                        if (!string.IsNullOrWhiteSpace(file1.PatternMatchValue))
                        {
                            if (file2.PatternMatch == null)
                            {
                                file2.PatternMatch = Regex.Match(file2.NameWithoutExtension, fileNamePattern, RegexOptions.IgnoreCase);
                            }

                            if (!string.IsNullOrWhiteSpace(file2.PatternMatchValue))
                            {
                                if (string.Compare(file2.PatternMatchValue, file1.PatternMatchValue, true) != 0)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (string.Compare(file2.NameWithoutExtension, file1.NameWithoutExtension, true) != 0)
                    {
                        continue;
                    }
                }

                // Across directories
                if (!options.HasOption(FinderOption.AcrossDirectories))
                {
                    if (file2.DirectoryName != file1.DirectoryName)
                    {
                        continue;
                    }
                }

                // Contents
                if (options.HasOption(FinderOption.SameContents))
                {
                    if (string.IsNullOrWhiteSpace(file1.Hash))
                    {
                        try
                        {
                            file1.Hash = GetFileHash(file1.FullName);
                        }
                        catch (Exception ex)
                        {
                            result.Errors.TryAdd(file1.FullName, ex.Message);
                            break;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(file2.Hash))
                    {
                        try
                        {
                            file2.Hash = GetFileHash(file2.FullName);
                        }
                        catch (Exception ex)
                        {
                            result.Errors.TryAdd(file2.FullName, ex.Message);
                            continue;
                        }
                    }

                    if (file2.Hash != file1.Hash)
                    {
                        continue;
                    }
                }

                // Found a duplicate.

                file2.Group = group;
                result.Files.Add(file2);
                removeFromTarget.Add(file2);
                if (!file1.Group.HasValue)
                {
                    file1.Group = group;
                }

                if (sourceFiles.Contains(file2))
                {
                    progressCount++;
                    if (file2.Length.HasValue)
                    {
                        progressLength += file2.Length.Value;
                    }
                }

                result.DuplicateCount++;
                if (file2.Length.HasValue)
                {
                    duplicateLength += file2.Length.Value;
                }
            }

            if (file1.Group.HasValue)
            {
                result.Files.Add(file1);

                group++;
            }

            removeFromTarget.ForEach(x => targetFiles.Remove(x));

            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 1000)
            {
                Log.Debug($"Matching \"{file1.FullName}\" took {stopwatch.ElapsedMilliseconds}ms");
            }
        }

        return result;
    }

    public static async Task<FinderResult> DeleteFilesAsync(
        IEnumerable<DuplicateFile> files,
        IFileService fileService,
        IProgress<FinderProgress>? progressReporter = null,
        CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            var result = new FinderResult();
            var total = files.Count();
            var progress = 0;
            long actionedFileLength = 0;
            foreach (var file in files)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                progress++;
                progressReporter?.Report(new FinderProgress()
                {
                    ProgressCount = progress,
                    TotalCount = total,
                    CurrentFileName = file.FullName,
                    DuplicateCount = result.Files.Count,
                    DuplicateLength = actionedFileLength,
                });

                try
                {
                    if (File.Exists(file.FullName))
                    {
                        fileService.MoveToRecycleBin(file.FullName);
                    }

                    if (file.Length.HasValue)
                    {
                        actionedFileLength += file.Length.Value;
                    }

                    result.Files.Add(file);
                }
                catch (Exception ex)
                {
                    result.Errors.TryAdd(file.FullName, ex.Message);
                }
            }

            return result;
        });
    }
}
