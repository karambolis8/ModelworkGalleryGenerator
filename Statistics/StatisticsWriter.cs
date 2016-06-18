using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ModelworkGalleryGenerator.Statistics
{
    static class StatisticsWriter
    {
        public static void WriteStatistics(string outputDir, IList<GalleryEntry> galleryEntries, IEnumerable<IStatisticGenerator> generators)
        {
            foreach (var generator in generators)
            {
                var statisticRows = generator.GenerateStatisticsRows(galleryEntries);
                File.WriteAllLines(Path.Combine(outputDir, generator.StatisticName) + ".txt", statisticRows.ToArray());
            }
        }
    }
}
