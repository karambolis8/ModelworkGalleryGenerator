using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelworkGalleryGenerator.Statistics
{
    class TopTenAuthorsStatistic : IStatisticGenerator
    {
        private string _scale;

        public TopTenAuthorsStatistic()
        {
            this._scale = null;
        }

        public TopTenAuthorsStatistic(string scale)
        {
            this._scale = scale;
        }

        public string StatisticName
        {
            get
            {
                if(this._scale != null)
                    return string.Format("Top10AuthorsFor_{0}", this._scale);
                return "Top10Authors";
            }
        }

        public IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries, string updateDate)
        {
            var list = new List<string>();

            if (this._scale != null)
            {
                var formatStr = this._scale == "nieznana"
                    ? "[size=150][b]Top 10 modelarzy wg. ilości galerii dla skali {0}[/b][/size]"
                    : "[size=150][b]Top 10 modelarzy wg. ilości galerii dla skali 1:{0}[/b][/size]";

                list.Add(string.Format(formatStr, this._scale));
            }
            else
            {
                list.Add("[size=150][b]Top 10 modelarzy wg. ilości galerii[/b][/size]");
            }

            Func<GalleryEntry, bool> optionalScaleFilter;
            if (this._scale != null)
                optionalScaleFilter = g => g.Scales.Contains(this._scale);
            else
                optionalScaleFilter = _ => true;

            foreach (var group in galleryEntries
                .Where(g => optionalScaleFilter(g))
                .GroupBy(e => e.Author)
                .OrderByDescending(g => g.Count())
                .Take(10))
            {
                list.Add(string.Format("[b]{0} - [/b]{1}", group.Key, group.Count()));
            }

            list.Add(string.Empty);
            list.Add(string.Format("[color=#008000]Stan na {0}[/color]", updateDate));

            return list;
        }
    }
}
