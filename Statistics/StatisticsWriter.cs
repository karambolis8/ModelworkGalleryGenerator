using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ModelworkGalleryGenerator.Statistics
{
    static class StatisticsWriter
    {
        public static void WriteStatistics(string outputDir, IList<GalleryEntry> galleryEntries, IEnumerable<IStatisticGenerator> generators, string updateDate)
        {
            foreach (var generator in generators)
            {
                var statisticRows = generator.GenerateStatisticsRows(galleryEntries, updateDate);
                File.WriteAllLines(Path.Combine(outputDir, generator.StatisticName) + ".txt", statisticRows.ToArray());
            }
        }
    }
}
