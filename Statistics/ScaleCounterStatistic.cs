using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelworkGalleryGenerator.Statistics
{
    class ScaleCounterStatistic : IStatisticGenerator
    {
        public string StatisticName
        {
            get { return "ScaleCounter"; }
        }

        public IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries)
        {
            var list = new List<string>();

            list.Add("[size=150][b]Liczba modeli wg skali[/b][/size]");

            foreach (var group in galleryEntries
                .GroupBy(e => e.Scale)
                .OrderByDescending(g => g.Count()))
            {
                list.Add(string.Format("[b]1:{0} - [/b]{1}", group.Key, group.Count()));
            }

            return list;
        }
    }
}
