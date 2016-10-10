using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

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

                    line = RemoveUnallowedChars(line);

                    var values = line.Split(this._columnSeparator);

                    if (values.Length < 8)
                        throw new Exception("invalid row format");

                    var scaleStr = values[6];
                    var scales = ParseScale(scaleStr);

                    var producerStr = values[7];
                    var producers = ParseProducers(producerStr);

                    var entry = new GalleryEntry() 
                    { 
                        LastPostDate = DateTime.Parse(values[0]),
                        CreateData = DateTime.Parse(values[1]),
                        URL = values[2],
                        Title = values[3],
                        Author = values[4],
                        Model = values[5],
                        Scales = scales,
                        Producers = producers
                    };

                    list.Add(entry);
                }
            }

            return list;
        }

        private int[] ParseScale(string scaleStr)
        {
            var scales = scaleStr.Split('+');
            var result = new List<int>(scales.Length);

            foreach (var s in scales)
            {
                var scale = s.Trim();
                var index = scale.IndexOfAny(new[] { ',', '.', ':', '/' });
                if (index > 0)
                    scale = scale.Substring(index + 1);
                result.Add(Int32.Parse(scale));
            }

            return result.ToArray();
        }

        private string[] ParseProducers(string producerStr)
        {
            if (string.IsNullOrEmpty(producerStr) || producerStr.Contains("?"))
                return new[] {"brak"};

            var producers = producerStr.Split('+');
            var result = new List<string>(producers.Length);
            result.AddRange(producers.Select(p => p.Trim()));
            return result.ToArray();
        }

        private string RemoveUnallowedChars(string lineOrg)
        {
            var sb = new StringBuilder(lineOrg);
            sb.Replace("&gt;", string.Empty);
            sb.Replace("&lt;", string.Empty);
            sb.Replace("&amp;", string.Empty);
            sb.Replace("&quot;", string.Empty);
            sb.Replace(";)", string.Empty);
            return sb.ToString();
        }
    }
}
