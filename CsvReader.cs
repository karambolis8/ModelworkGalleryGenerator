using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ModelworkGalleryGenerator
{
    class CsvReader
    {
        private string _fileName;

        private char _columnSeparator;

        public CsvReader(string fileName, char columnSeparator)
        {
            this._fileName = fileName;
            this._columnSeparator = columnSeparator;
        }

        public IList<GalleryEntry> ReadLines(bool skipFirstRow)
        {
            var list = new List<GalleryEntry>();

            using(var reader = new StreamReader(File.OpenRead(this._fileName)))
            {
                if (skipFirstRow)
                    reader.ReadLine();
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(this._columnSeparator);

                    if (values.Length < 8)
                        throw new Exception("invalid row format");

                    var scaleStr = values[6];
                    var index = scaleStr.IndexOfAny(new []{',', '.', ':', '/'});
                    if (index > 0)
                        scaleStr = scaleStr.Substring(index + 1);
                    var scale = Int32.Parse(scaleStr);

                    var entry = new GalleryEntry() 
                    { 
                        LastPostDate = DateTime.Parse(values[0]),
                        CreateData = DateTime.Parse(values[1]),
                        URL = values[2],
                        Title = values[3],
                        Author = values[4],
                        Model = values[5],
                        Scale = scale,
                        Producer = values[7]
                    };

                    list.Add(entry);
                }
            }

            return list;
        }
    }
}
