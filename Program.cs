using System.Linq;
using ModelworkGalleryGenerator.Statistics;
using System.Configuration;

namespace ModelworkGalleryGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = args[0];
            var updateDate = args[1];

            var reader = new CsvReader(inputFile, ';');
            var lines = reader.ReadLines(true);

            var scales = lines
                .SelectMany(l => l.Scales)
                .GroupBy(l => l);

            var generators = scales
                .Select(g => new ScaleFilterStatistic(g.Key))
                .ToList<IStatisticGenerator>();

            var authorsWithScale = scales
                .Where(s => s.Count() >= 10)
                .Select(g => new TopTenAuthorsStatistic(g.Key))
                .ToList<IStatisticGenerator>();
            generators.AddRange(authorsWithScale);

            var producersWithScale = scales
                .Where(s => s.Count() >= 10)
                .Select(g => new TopTenProducersStatistic(g.Key))
                .ToList<IStatisticGenerator>();
            generators.AddRange(producersWithScale);

            generators.Add(new ScaleCounterStatistic());
            generators.Add(new TopTenAuthorsStatistic());
            generators.Add(new TopTenProducersStatistic());

            StatisticsWriter.WriteStatistics(ConfigurationManager.AppSettings["OutputDir"], lines, generators, updateDate);
        }
    }
}
