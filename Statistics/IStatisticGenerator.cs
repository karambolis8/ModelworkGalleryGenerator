using System.Collections.Generic;

namespace ModelworkGalleryGenerator.Statistics
{
    interface IStatisticGenerator
    {
        string StatisticName { get; }

        IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries, string updateDate);
    }
}
