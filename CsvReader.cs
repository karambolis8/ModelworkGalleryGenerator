using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Text;

namespace ModelworkGalleryGenerator
{
    class CsvReader
    {
        private static string[] _scaleDividers = ConfigurationManager.AppSettings["AllowedScaleDividers"].Split('|');
        private static string[] _noProducerStrings = ConfigurationManager.AppSettings["NoProducersStrings"].Split('|');
        private static string[] _scratchStrings = ConfigurationManager.AppSettings["ScratchStrings"].Split('|');
        private static string[] _unallowedMarkup = ConfigurationManager.AppSettings["UnallowedMarkup"].Split('|');
        private static string[] _unknownScaleStrings = ConfigurationManager.AppSettings["UnknownScaleStrings"].Split('|');

        private readonly string _fileName;

        private readonly char _columnSeparator;

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

                    string[] scales;
                    try
                    {
                        var scaleStr = values[6];
                        scales = ParseScale(scaleStr);
                    }
                    catch (Exception e)
                    {
                        throw;
                    }

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

        private string[] ParseScale(string scaleStr)
        {
            if (string.IsNullOrEmpty(scaleStr) || _unknownScaleStrings.Contains(scaleStr.ToLower()))
                return new[] { "nieznana" };

            var scales = scaleStr.Split('+');
            var result = new List<string>(scales.Length);

            foreach (var s in scales)
            {
                var scale = s.Trim();
                var index = IndexOfAny(scale, _scaleDividers);
                if (index > 0)
                    scale = scale.Substring(index + 1);
                result.Add(scale);
            }

            return result.ToArray();
        }

        private string[] ParseProducers(string producerStr)
        {
            if (string.IsNullOrEmpty(producerStr) || _noProducerStrings.Contains(producerStr.ToLower()))
                return new[] {"brak"};

            if (_scratchStrings.Any(s => producerStr.ToLower().Contains(s)))
                return new[] { "scratch" };

            var producers = producerStr.Split('+');
            var result = new List<string>(producers.Length);
            result.AddRange(producers.Select(p => p.Trim()));
            return result.ToArray();
        }

        private string RemoveUnallowedChars(string lineOrg)
        {
            var sb = new StringBuilder(lineOrg);
            foreach(var markup in _unallowedMarkup)
                sb.Replace(markup, string.Empty);
            return sb.ToString();
        }

        public int IndexOfAny(string test, string[] values)
        {
            int first = -1;
            foreach (string item in values)
            {
                int i = test.IndexOf(item);
                if (i >= 0)
                {
                    if (first > 0)
                    {
                        if (i < first)
                        {
                            first = i;
                        }
                    }
                    else {
                        first = i;
                    }
                }
            }
            return first;
        }
    }
}
