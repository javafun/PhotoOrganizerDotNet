using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MetadataExtractor.Formats.Exif;

namespace fileorganizer_dotnet.Processors
{
    public class JpgFileProcessor : FileProcessor
    {
        public override string SearchPattern => "*.jpg";

        public override DateTime? GetOriginalDate(IReadOnlyList<MetadataExtractor.Directory> metadataDict, string file)
        {
            var subIfdDirectory = metadataDict.OfType<ExifSubIfdDirectory>().FirstOrDefault();

            var dateStr = subIfdDirectory?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
            if (string.IsNullOrWhiteSpace(dateStr))
            {
                // try file creation date then
                var jpgFile = new FileInfo(file);
                return jpgFile.CreationTime;
            }
            var format = "yyyy:MM:dd HH:mm:ss";
            return DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);

        }

        public override string ToString()
        {
            return nameof(JpgFileProcessor);
        }
    }
}