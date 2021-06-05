using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetadataExtractor.Formats.QuickTime;

namespace fileorganizer_dotnet.Processors
{
    public class Mp4FileProcessor : FileProcessor
    {
        public override string SearchPattern => "*.mp4";

        public override DateTime? GetOriginalDate(IReadOnlyList<MetadataExtractor.Directory> metadataDict, string file)
        {
            var quickMovDirectory = metadataDict.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();
            var dateStr = quickMovDirectory?.GetDescription(QuickTimeMovieHeaderDirectory.TagCreated);

            if (string.IsNullOrWhiteSpace(dateStr))
            {
                return null;
            }

            var format = "ddd. MMM dd HH:mm:ss yyyy";

            return DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return nameof(Mp4FileProcessor);
        }
    }
}