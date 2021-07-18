using System;
using System.Collections.Generic;
using System.IO;
using CSharpFunctionalExtensions;
using fileorganizer_dotnet.Contracts;
using MetadataExtractor;

namespace fileorganizer_dotnet.Processors
{
 public abstract class FileProcessor : IFileProcessor
 {
        public virtual string OutputDirectory => Path.Combine(CurrentDirectory, "output");

        public virtual string PhotoDirectory => Path.Combine(CurrentDirectory, "photos");

        public virtual string ArchiveDirectory => Path.Combine(CurrentDirectory, OutputDirectory, "archive");

        public virtual string CurrentDirectory => System.IO.Directory.GetCurrentDirectory();


        public class FileProcessResult
        {
            public string ProcessorType { get; set; }
            public int TotalFiles { get; set; }

            public override string ToString()
            {
                return $"Processor type: {ProcessorType} - The number of files {TotalFiles} ";
            }
        }

        public abstract string SearchPattern { get; }


        public virtual Result<FileProcessResult> ProcessFiles()
        {
            FileProcessResult fileProcessResult = new FileProcessResult();
            var files = System.IO.Directory.GetFiles(PhotoDirectory, SearchPattern, new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive });
            fileProcessResult.TotalFiles = files.Length;
            fileProcessResult.ProcessorType = this.ToString();
            foreach (var f in files)
            {
                try
                {
                    var directories = ImageMetadataReader.ReadMetadata(f);
                    // foreach (var directory in directories)
                    //     foreach (var tag in directory.Tags)
                    //         Console.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");
                    var originalDate = GetOriginalDate(directories, f);
                    if (!originalDate.HasValue)
                    {
                        Console.WriteLine($"{f} file missed exif original date, and will be moving to archive.");
                        ArchiveFile(f);
                    }
                    else
                    {
                        RelocateFile(originalDate.Value, f);
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                    ArchiveFile(f);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return Result.Success<FileProcessResult>(fileProcessResult);
        }

        public abstract DateTime? GetOriginalDate(IReadOnlyList<MetadataExtractor.Directory> metadataDict, string file);
        public virtual void RelocateFile(DateTime date, string file)
        {
            string dateFolder = Path.Combine(CurrentDirectory, OutputDirectory, date.Year.ToString("00"), date.Month.ToString("00"), date.Day.ToString("00"));

            if (!System.IO.Directory.Exists(dateFolder))
            {
                System.IO.Directory.CreateDirectory(dateFolder);
            }
            var fileName = Path.GetFileName(file);
            File.Move(file, Path.Combine(dateFolder, fileName));
        }

        public virtual void ArchiveFile(string file)
        {
            if (!System.IO.Directory.Exists(ArchiveDirectory))
            {
                System.IO.Directory.CreateDirectory(ArchiveDirectory);
            }

            File.Move(file, Path.Combine(ArchiveDirectory, Path.GetFileName(file)));
            Console.WriteLine($"{file} has been moved to archive");
        }


    }
}