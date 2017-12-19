<Query Kind="Program" />

void Main()
{
	var rawInput = File.ReadAllLines("Q:\\git\\advent\\day7_input.txt").ToArray();
	var nodes = ParseInput(rawInput);
	//nodes.Dump();
	FirstPartSolution(nodes.ToList());
}

void FirstPartSolution(List<Node> nodes)
{
	while (nodes.Count > 1)
	{
		nodes.RemoveAll(p => p.ChildNames.Count() == 0);
		foreach (var node in nodes)
		{
			node.ChildNames = node.ChildNames.Where(p => nodes.Any(n => n.Name == p)).ToArray();
		}
	}
	
	Console.WriteLine("Remaining node is " + nodes[0].Name);
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
