using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelworkGalleryGenerator.Statistics
{
    class TopTenProducersStatistic : IStatisticGenerator
    {
        private int? _scale;

        public TopTenProducersStatistic()
        {
            this._scale = null;
        }

        public TopTenProducersStatistic(int scale)
        {
            this._scale = scale;
        }

        public string StatisticName
        {
            get
            {
                if(this._scale.HasValue)
                    return string.Format("Top10ProducersFor{0}", this._scale.Value);
                return "Top10Producers";
            }
        }

        public IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries)
        {
            var list = new List<string>();

            if(this._scale.HasValue)
                list.Add(string.Format("[size=150][b]Top 10 producentów wg. ilości galerii dla skali 1:{0}[/b][/size]", this._scale.Value));
            else
                list.Add("[size=150][b]Top 10 producentów wg. ilości galerii[/b][/size]");

            Func<GalleryEntry, bool> optionalScaleFilter;
            if (this._scale.HasValue)
                optionalScaleFilter = g => g.Scale == this._scale.Value;
            else
                optionalScaleFilter = _ => true;

            foreach (var group in galleryEntries
                .Where(g => optionalScaleFilter(g))
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
