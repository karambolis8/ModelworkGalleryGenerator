using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelworkGalleryGenerator.Statistics
{
    class TopTenAuthorsStatistic : IStatisticGenerator
    {
        private int? _scale;

        public TopTenAuthorsStatistic()
        {
            this._scale = null;
        }

        public TopTenAuthorsStatistic(int scale)
        {
            this._scale = scale;
        }

        public string StatisticName
        {
            get
            {
                if(this._scale.HasValue)
                    return string.Format("Top10AuthorsFor{0}", this._scale.Value);
                return "Top10Authors";
            }
        }

        public IList<string> GenerateStatisticsRows(IList<GalleryEntry> galleryEntries)
        {
            var list = new List<string>();

            if(this._scale.HasValue)
                list.Add(string.Format("[size=150][b]Top 10 modelarzy wg. ilości galerii dla skali 1:{0}[/b][/size]", this._scale.Value));
            else
                list.Add("[size=150][b]Top 10 modelarzy wg. ilości galerii[/b][/size]");

            Func<GalleryEntry, bool> optionalScaleFilter;
            if (this._scale.HasValue)
                optionalScaleFilter = g => g.Scale == this._scale.Value;
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

            return list;
        }
    }
}
