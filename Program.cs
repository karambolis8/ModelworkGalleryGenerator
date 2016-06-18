using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelworkGalleryGenerator.Statistics;

namespace ModelworkGalleryGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = args[0];

            var reader = new CsvReader(inputFile, ';');
            var lines = reader.ReadLines(true);

            // dodatkowe statystyki - top 10 modelarzy i producentów w podziale na skale

            var generators = lines
                .GroupBy(l => l.Scale)
                .Select(g => new ScaleFilterStatistic(g.Key))
                .ToList<IStatisticGenerator>();

            generators.Add(new ScaleCounterStatistic());
            generators.Add(new TopTenAuthorsStatistic());
            generators.Add(new TopTenProducersStatistic());

            StatisticsWriter.WriteStatistics(@"C:\modelwork_galerie", lines, generators);
        }
    }
}
