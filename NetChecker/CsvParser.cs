using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetChecker
{
    public class CsvParser
    {
        private readonly bool _hasHeaderRecord;
        private readonly string _delimiter;
        private readonly ClassMap _classMap;

        public CsvParser(bool hasHeaderRecord = true, string delimiter = ";", ClassMap classMap = null)
        {
            _hasHeaderRecord = hasHeaderRecord;
            _delimiter = delimiter;
            _classMap = classMap ?? new TupleMap();
        }

        public List<(string, string)> Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Specified file does not exists {path}");
            }
            
            using (var sr = new StreamReader(path, false))
            {
                using (var csv = new CsvReader(sr))
                {
                    csv.Configuration.HasHeaderRecord = _hasHeaderRecord;
                    csv.Configuration.RegisterClassMap(_classMap);
                    csv.Configuration.Delimiter = _delimiter;
                    return csv.GetRecords<(string, string)>().ToList();
                }
            }
        }
    }
}
