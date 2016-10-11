using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Configuration;
using System;

namespace ModelworkGalleryGenerator.Statistics
{
    static class StatisticsWriter
    {
        private static int minimumSeparatePostSize = Int32.Parse(ConfigurationManager.AppSettings["MinimumSeparatePostSize"]);

        public static void WriteStatistics(string outputDir, IList<GalleryEntry> galleryEntries, IEnumerable<IStatisticGenerator> generators, string updateDate)
        {
            var scaleFilterGenerators = generators.Where(g => g is ScaleFilterStatistic);
            var otherGenerators = generators.Where(g => !(g is ScaleFilterStatistic));

            foreach (var generator in otherGenerators)
            {
                var statisticRows = generator.GenerateStatisticsRows(galleryEntries);
                statisticRows.Add(string.Empty);
                statisticRows.Add(string.Format("[color=#008000]Stan na {0}[/color]", updateDate));
                File.WriteAllLines(Path.Combine(outputDir, generator.StatisticName) + ".txt", statisticRows.ToArray());
            }
            
            IList<string> singleFile = new List<string>();

            foreach (var generator in scaleFilterGenerators)
            {
                var statisticRows = generator.GenerateStatisticsRows(galleryEntries);

                if(statisticRows.Count < minimumSeparatePostSize)
                {
                    foreach (var row in statisticRows)
                        singleFile.Add(row);
                    singleFile.Add(string.Empty);
                }
                else
                {
                    statisticRows.Add(string.Empty);
                    statisticRows.Add(string.Format("[color=#008000]Stan na {0}[/color]", updateDate));
                    File.WriteAllLines(Path.Combine(outputDir, generator.StatisticName) + ".txt", statisticRows.ToArray());
                }
            }

            if(singleFile.Any())
            {
                singleFile.Add(string.Format("[color=#008000]Stan na {0}[/color]", updateDate));
                File.WriteAllLines(Path.Combine(outputDir, "ScaleStatistics_SingleFile") + ".txt", singleFile.ToArray());
            }
        }
    }
}
