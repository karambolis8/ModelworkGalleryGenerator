using System.Linq;
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

            var scales = lines
                .SelectMany(l => l.Scales)
                .GroupBy(l => l);

            var generators = scales
                .Select(g => new ScaleFilterStatistic(g.Key))
                .ToList<IStatisticGenerator>();

            var authorsWithScale = scales
                .Select(g => new TopTenAuthorsStatistic(g.Key))
                .ToList<IStatisticGenerator>();
            generators.AddRange(authorsWithScale);

            var producersWithScale = scales
                .Select(g => new TopTenProducersStatistic(g.Key))
                .ToList<IStatisticGenerator>();
            generators.AddRange(producersWithScale);

            generators.Add(new ScaleCounterStatistic());
            generators.Add(new TopTenAuthorsStatistic());
            generators.Add(new TopTenProducersStatistic());

            StatisticsWriter.WriteStatistics(@"C:\modelwork_galerie", lines, generators);
        }
    }
}
