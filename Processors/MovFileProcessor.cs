using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetadataExtractor.Formats.QuickTime;

namespace fileorganizer_dotnet.Processors
{
    public class MovFileProcessor : FileProcessor
    {
        public override string SearchPattern => "*.mov";

        public override DateTime? GetOriginalDate(IReadOnlyList<MetadataExtractor.Directory> metadataDict, string file)
        {
            var quickMovDirectory = metadataDict.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();
            var dateStr = quickMovDirectory?.GetDescription(QuickTimeMetadataHeaderDirectory.TagCreationDate);

            if (string.IsNullOrWhiteSpace(dateStr))
            {
                return null;
            }

            var format = "ddd. MMM dd HH:mm:ss zzz yyyy";

            return DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return nameof(MovFileProcessor);
        }
    }
}