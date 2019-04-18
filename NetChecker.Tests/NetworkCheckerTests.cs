using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using Xunit;

namespace NetChecker.Tests
{
    public class GraphHelperTests
    {
        private const string _testFileName = "test.csv";
        private readonly NetworkChecker _networkChecker = new NetworkChecker();

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] { new List<(string, string)> (), true },

                new object[] { new List<(string, string)> {
                    ("Node1", "Node2")
                }, true },

                new object[] { new List<(string, string)> {
                    ("Node1", "Node2"),
                    ("Node2", "Node3")
                }, true },

                new object[] { new List<(string, string)> {
                    ("Node1", "Node2"),
                    ("Node2", "Node3"),
                    ("Node3", "Node1")
                }, true },

                new object[] { new List<(string, string)> {
                    ("Node1", "Node2"),
                    ("Node1", "Node3")
                }, true },

                new object[] { new List<(string, string)> {
                    ("Node1", "Node2"),
                    ("Node1", "Node3"),
                    ("Node4", "Node5"),
                    ("Node4", "Node6"),
                    ("Node1", "Node4")
                }, true },

                new object[] { new List<(string, string)> {
                    ("Node1", "Node2"),
                    ("Node1", "Node3"),
                    ("Node4", "Node5"),
                    ("Node4", "Node6"),
                    ("Node4", "Node1")
                }, true },

                new object[] { new List<(string, string)> {
                    ("Node1", "Node2"),
                    ("Node2", "Node3"),
                    ("Node3", "Node1"),
                    ("Node5", "Node4")
                }, false },
            
                new object[] { new List<(string, string)> {
                    ("Node1", "Node2"),
                    ("Node3", "Node4"),
                    ("Node5", "Node6")
                }, false },

                new object[] { new List<(string, string)> {
                    ("Node1", "Node2"),
                    ("Node2", null),
                }, true },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void NetworkCheckerListTest(List<(string, string)> source, bool expected)
        {
            var result = _networkChecker.CheckList(source);
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public void NetworkCheckerTest(List<(string, string)> source, bool expected)
        {
            try
            {
                using (var file = File.CreateText(_testFileName))
                {
                    file.WriteLine("Col1;Col2");
                    foreach (var (col1, col2) in source)
                    {
                        file.WriteLine($"{col1};{col2}");
                    }
                }

                var result = _networkChecker.Check(_testFileName);
                Assert.Equal(expected, result);
            }
            finally
            {
                File.Delete(_testFileName);
            }
        }

        [Fact]
        public void NetworkCheckerTest_ArgumentNull()
        {
            Assert.Throws<ArgumentNullException>(() => _networkChecker.CheckList(null));
        }
    }
}
