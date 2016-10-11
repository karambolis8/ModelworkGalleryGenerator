﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelworkGalleryGenerator.Statistics
{
    class TopTenProducersStatistic : IStatisticGenerator
    {
        private string _scale;

        public TopTenProducersStatistic()
        {
            this._scale = null;
        }

        public TopTenProducersStatistic(string scale)
        {
            this._scale = scale;
        }

        public string StatisticName
        {
            get
            {
                if(this._scale != null)
                    return string.Format("Top10ProducersFor{0}", this._scale);
                return "Top10Producers";
            }
        }

        public IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries)
        {
            var list = new List<string>();

            if(this._scale != null)
                list.Add(string.Format("[size=150][b]Top 10 producentów wg. ilości galerii dla skali 1:{0}[/b][/size]", this._scale));
            else
                list.Add("[size=150][b]Top 10 producentów wg. ilości galerii[/b][/size]");

            Func<GalleryEntry, bool> optionalScaleFilter;
            if (this._scale != null)
                optionalScaleFilter = g => g.Scales.Contains(this._scale);
            else
                optionalScaleFilter = _ => true;

            var col = galleryEntries
                .Where(g => optionalScaleFilter(g))
                .SelectMany(g => g.Producers)
                .GroupBy(e => e)
                .OrderByDescending(g => g.Count())
                .Take(10);

            foreach (var group in col)
            {
                list.Add(string.Format("[b]{0} - [/b]{1}", group.Key, group.Count()));
            }

            return list;
        }
    }
}
