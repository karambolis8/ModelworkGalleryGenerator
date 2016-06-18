using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelworkGalleryGenerator.Statistics
{
    interface IStatisticGenerator
    {
        string StatisticName { get; }

        IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries);
    }
}
