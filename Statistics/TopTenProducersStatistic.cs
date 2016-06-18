using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelworkGalleryGenerator.Statistics
{
    class TopTenProducersStatistic : IStatisticGenerator
    {
        public string StatisticName
        {
            get { return "Top10Producers"; }
        }

        public IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries)
        {
            var list = new List<string>();

            list.Add("[size=150][b]Top 10 producentów wg. ilości galerii[/b][/size]");

            foreach (var group in galleryEntries
                .GroupBy(e => e.Producer)
                .OrderByDescending(g => g.Count())
                .Take(10))
            {
                list.Add(string.Format("[b]{0} - [/b]{1}", group.Key, group.Count()));
            }

            return list;
        }
    }
}
