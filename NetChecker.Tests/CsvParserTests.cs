using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace NetChecker.Tests
{
    public class CsvParserTests
    {
        private const string _testFileName = "test.csv";
        private readonly CsvParser _parser = new CsvParser();

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { new List<(string, string)> {
                    ("Node1", "Node2"),
                    ("Node2", "Node3")
                }},
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void ParseTest(List<(string, string)> data)
        {
            try
            {
                using (var file = File.CreateText(_testFileName))
                {
                    file.WriteLine("Col1;Col2");
                    foreach (var (col1, col2) in data)
                    {
                        file.WriteLine($"{col1};{col2}");
                    }
                }

                var result = _parser.Parse(_testFileName);
                Assert.NotNull(result);
            }
            finally
            {
                File.Delete(_testFileName);
            }
        }

        [Fact]
        public void CsvParser_ErrorsTest()
        {
            Assert.Throws<ArgumentNullException>(() => _parser.Parse(null));
            Assert.Throws<ArgumentException>(() => _parser.Parse("fake\\path.csv"));
        }


    }
}
