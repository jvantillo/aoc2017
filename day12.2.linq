<Query Kind="Program" />

void Main()
{
	var nodes = ReadInput();
	var remainingNodes = ReadInput();
	var groupCount = 0;
	
	while (remainingNodes.Count() > 0)
	{
		var participant = remainingNodes[0];
		var grp = IdentifyGroup(participant.GroupNumber, nodes);
		remainingNodes.RemoveAll(p => grp.Any(v => v == p.GroupNumber));
		groupCount++;
	}
	
	Console.WriteLine($"There are {groupCount} groups");
}

List<int> IdentifyGroup(int participant, List<Node> nodes)
{
	List<Node> toBeVisited = nodes.Where(p => p.GroupNumber == participant).ToList();
	List<int> visited = new List<int>();

	while (toBeVisited.Any())
	{
		var node = toBeVisited[0];
		toBeVisited.RemoveAt(0);

		if (!visited.Any(p => p == node.GroupNumber))
		{
			visited.Add(node.GroupNumber);
			foreach (var connection in node.Connected)
			{
				toBeVisited.Add(nodes.Single(p => p.GroupNumber == connection));
			}
		}
	}
	
	return visited;
}

List<Node> ReadInput()
{
	var instructions = File.ReadAllLines("Q:\\git\\advent\\day12_input.txt");

	Regex reLine = new Regex(@"^
		(?<group>\d+)
		\s
		\<\-\>
		\s
		((?<connected>\d+) (\, \s)?)+
	", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

	List<Node> nodes = new List<Node>();
	foreach (var instruction in instructions)
	{
		Match m = reLine.Match(instruction);
		if (!m.Success)
		{
			throw new ApplicationException("Failed match on: " + instruction);
		}
		var groupNumber = int.Parse(m.Groups["group"].Value);
		List<int> connected = new List<int>();
		foreach (Capture cap in m.Groups["connected"].Captures)
		{
			connected.Add(int.Parse(cap.Value));
		}
		nodes.Add(new Node
		{
			GroupNumber = groupNumber,
			Connected = connected,
		});
	}
	return nodes;
}


struct Node
{
	public int GroupNumber { get; set; }
	public List<int> Connected { get; set; }
}