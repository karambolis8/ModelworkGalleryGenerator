using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelworkGalleryGenerator
{
    class GalleryEntry
    {
        public DateTime LastPostDate { get; set; }
        public DateTime CreateData { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Model { get; set; }
        public int[] Scales { get; set; }
        public string[] Producers { get; set; }
    }
}
