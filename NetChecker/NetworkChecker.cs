using System;
using System.Collections.Generic;
using System.Linq;

namespace NetChecker
{
    public class NetworkChecker
    {
        private readonly CsvParser _parser;
        public NetworkChecker()
        {
            _parser = new CsvParser();
        }
        
        public bool Check(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            
            var nodeList = _parser.Parse(path);
            return CheckList(nodeList);
        }
        
        private static bool CheckList(IReadOnlyCollection<(string, string)> list)
        {
            if (list.Count < 2)
            {
                // no computer or only one connected pair = no unreachable segments
                return true; 
            }

            var nodes = CreateNetwork(list);

            var reachable = new HashSet<string>();
            MarkReachable(nodes.First(), reachable);

            return nodes.Count == reachable.Count;
        }

        private static IReadOnlyCollection<Node> CreateNetwork(IEnumerable<(string, string)> list)
        {
            var nodes = new Dictionary<string, Node>();
            foreach (var (sourceNode, destNode) in list)
            {
                nodes.TryAdd(sourceNode, new Node(sourceNode));
                nodes.TryAdd(destNode, new Node(destNode));

                nodes[sourceNode].AddChild(nodes[destNode]);
                nodes[destNode].AddChild(nodes[sourceNode]);
            }

            return nodes.Values;
        }

        private static void MarkReachable(Node node, ISet<string> visited)
        {
            if (!visited.Add(node.Id) || node.Nodes.Count == 0)
            {
                return;
            }

            foreach (var child in node.Nodes)
            {
                MarkReachable(child, visited);
            }
        }

        private class Node
        {
            public string Id { get; }

            public ICollection<Node> Nodes { get; }

            public Node(string id)
            {
                Id = id;
                Nodes = new HashSet<Node>();
            }

            public void AddChild(Node child) => Nodes.Add(child);

            public override int GetHashCode() => Id.GetHashCode();
        }
    }
}
