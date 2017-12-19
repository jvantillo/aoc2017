<Query Kind="Program" />

void Main()
{
	var rawInput = File.ReadAllLines("Q:\\git\\advent\\day7_input.txt").ToArray();
	var nodes = ParseInput(rawInput);
	//nodes.Dump();
	string rootName = FindRootName(nodes.ToList());
	SecondSolution(ParseInput(rawInput), rootName);
}

void SecondSolution(Node[] nodes, string rootName)
{
	var root = nodes.Single(p => p.Name == rootName);
	GetTotalWeightOfNode(nodes, root);
}

int GetTotalWeightOfNode(Node[] nodes, Node node)
{
	if (node.Children.Length == 0)
	{
		return node.Weight;
	}
	
	// Examine children of node, determine their weight
	var weights = node.Children.Select(p => GetTotalWeightOfNode(nodes, nodes[p])).ToArray();
		
	// If all children are equal in weight, nothing to do
	var countsPerWeight = (from weight in weights
						  group weight by weight into g
						  select new { Weight = g.Key, Items = g, Count = g.Count() })
						  .OrderBy(p => p.Count)
						  .ToArray();
	if (countsPerWeight.Count() == 1)
	{
		return node.Weight + weights.Sum();
	}
	
	if (countsPerWeight.Count() > 2)
	{
		throw new ApplicationException("My logic has failed here.");
	}
	
	// Select the one that's different from the rest
	var problematicNodeWeight = countsPerWeight[0].Items.Single();
	string problematicNodeName = node.ChildNames[Array.IndexOf(weights, problematicNodeWeight)];
	Node problematicNode = nodes.Single(p => p.Name == problematicNodeName);
	int correction = countsPerWeight[1].Weight - problematicNodeWeight;
	Console.WriteLine($"{problematicNode.Name} has weight {problematicNode.Weight} and should be {problematicNode.Weight + correction}");
	Process.GetCurrentProcess().Kill();		// ugly :P
	return -1;
}

string FindRootName(List<Node> nodes)
{
	while (nodes.Count > 1)
	{
		nodes.RemoveAll(p => p.ChildNames.Count() == 0);
		foreach (var node in nodes)
		{
			node.ChildNames = node.ChildNames.Where(p => nodes.Any(n => n.Name == p)).ToArray();
		}
	}
	
	Console.WriteLine("Root node is " + nodes[0].Name);
	return nodes[0].Name;
}


Regex reLine = new Regex(@"(?<name>[a-z]+)
	\s 
	\( (?<weight>\d+) \) 
	( \s \-\> \s ( (?<child>[a-z]+) (\,\s)?  )+   )?",
	RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Compiled);

Node[] ParseInput(string[] input)
{
	List<Node> nodes = new List<Node>();
	foreach (var line in input)
	{
		Match m = reLine.Match(line);
		if (!m.Success)
		{
			throw new ApplicationException("Failed match on: " + line);
		}
		
		string name = m.Groups["name"].Value;
		int weight = int.Parse(m.Groups["weight"].Value);
		List<string> children = new List<string>();
		
		var childGroup = m.Groups["child"];
		if (childGroup.Success)
		{
			foreach (Capture cap in childGroup.Captures)
			{
				children.Add(cap.Value);
			}
		}
		
		nodes.Add(new Node{
			Index = nodes.Count(),
			Name = name,
			Weight = weight,
			ChildNames = children.ToArray(),
		});
	}
	
	// Repair child indexes
	foreach (Node node in nodes)
	{
		node.Children = node.ChildNames.Select(p => nodes.Single(z => z.Name == p).Index).ToArray();
	}
	
	return nodes.ToArray();
}

class Node
{
	public int Index { get; set; }
	public string Name { get; set; }
	public int Weight { get; set; }
	public int[] Children { get; set; }
	public string[] ChildNames { get; set; }

	public override string ToString()
	{
		return $"{Name} ({Weight}) -> { String.Join(", ", ChildNames) }";
	}
}
