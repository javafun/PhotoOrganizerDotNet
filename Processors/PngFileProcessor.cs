using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetadataExtractor.Formats.FileSystem;

namespace fileorganizer_dotnet.Processors
{
    public class PngFileProcessor : FileProcessor
    {
        public override string SearchPattern => "*.png";

        public override DateTime? GetOriginalDate(IReadOnlyList<MetadataExtractor.Directory> metadataDict, string file)
        {
            var subIfdDirectory = metadataDict.OfType<FileMetadataDirectory>().FirstOrDefault();

            var dateStr = subIfdDirectory?.GetDescription(FileMetadataDirectory.TagFileModifiedDate);
            if (string.IsNullOrWhiteSpace(dateStr))
            {
                return null;
            }
            var format = "ddd. MMM dd HH:mm:ss zzz yyyy";
            return DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return nameof(PngFileProcessor);
        }
    }
}