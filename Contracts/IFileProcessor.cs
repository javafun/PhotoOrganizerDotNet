using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using fileorganizer_dotnet.Processors;

namespace fileorganizer_dotnet.Contracts
{
    public interface IFileProcessor
    {
        string OutputDirectory { get; }
        string PhotoDirectory { get; }
        string ArchiveDirectory { get; }
        string CurrentDirectory { get; }
        string SearchPattern { get; }
        Result<FileProcessor.FileProcessResult> ProcessFiles();
        DateTime? GetOriginalDate(IReadOnlyList<MetadataExtractor.Directory> metadataDict, string file);
        void RelocateFile(DateTime date, string file);
        void ArchiveFile(string file);
    }
}