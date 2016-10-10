using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelworkGalleryGenerator.Statistics
{
    class ScaleFilterStatistic : IStatisticGenerator
    {
        private int _scale;

        public string StatisticName
        {
            get { return "ScaleStatistics_" + _scale; }
        }

        public ScaleFilterStatistic(int scale)
        {
            this._scale = scale;
        }

        public IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries)
        {
            var list = new List<string>();

            list.Add(string.Format("[size=150][b]Skala 1:{0}[/b][/size]", this._scale));

            foreach(var entry in galleryEntries
                .Where(e => e.Scales.Contains(this._scale))
                .OrderBy(e => e.Model))
            {
                list.Add(string.Format("[url={0}][b]{1}[/b] {2} ({3})[/url]", entry.URL, entry.Model, entry.Author, string.Join(", ", entry.Producers)));
            }

            return list;
        }
    }
}
