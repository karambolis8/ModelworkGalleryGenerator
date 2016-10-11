using System.Collections.Generic;
using System.Linq;

namespace ModelworkGalleryGenerator.Statistics
{
    class ScaleCounterStatistic : IStatisticGenerator
    {
        public string StatisticName
        {
            get { return "ScaleCounter"; }
        }

        public IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries, string updateDate)
        {
            var list = new List<string>();

            list.Add("[size=150][b]Liczba modeli wg skali[/b][/size]");

            foreach (var group in galleryEntries
                .SelectMany(g => g.Scales)
                .GroupBy(e => e)
                .OrderByDescending(g => g.Count()))
            {
                var formatStr = group.Key == "nieznana" ? "[b]{0} - [/b]{1}" : "[b]1:{0} - [/b]{1}";
                list.Add(string.Format(formatStr, group.Key, group.Count()));
            }

            list.Add(string.Empty);
            list.Add(string.Format("[color=#008000]Stan na {0}[/color]", updateDate));

            return list;
        }
    }
}
