using System.Collections.Generic;
using System.Linq;

namespace ModelworkGalleryGenerator.Statistics
{
    class ScaleFilterStatistic : IStatisticGenerator
    {
        private string _scale;

        public string StatisticName
        {
            get { return "ScaleStatistics_" + _scale; }
        }

        public ScaleFilterStatistic(string scale)
        {
            this._scale = scale;
        }

        public IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries)
        {
            var scaleEnties = galleryEntries
                .Where(e => e.Scales.Contains(this._scale))
                .OrderBy(e => e.Model);
            
            var list = new List<string>();

            if (scaleEnties.Count() > 500)
            {
                var stat = scaleEnties
                    .GroupBy(e => e.Title.Trim().ToLower()[0])
                    .OrderBy(s => s.Key);

                foreach (var s in stat)
                {
                    list.Add(string.Format("{0} - {1}", s.Key, s.Count()));
                }
            }

            var formatStr = this._scale == "nieznana" ? "[size=150][b]Skala {0}[/b][/size]" : "[size=150][b]Skala 1:{0}[/b][/size]";
            list.Add(string.Format(formatStr, this._scale));

            foreach (var entry in scaleEnties)
            {
                list.Add(string.Format("[url={0}][b]{1}[/b] {2} ({3})[/url]", entry.URL, entry.Model, entry.Author, string.Join(", ", entry.Producers)));
            }

            return list;
        }
    }
}
