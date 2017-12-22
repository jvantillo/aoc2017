<Query Kind="Program" />


void Main()
{
	var layers = ReadInput();
	Simulation sim = new Simulation(layers);
	sim.Run();
	//sim.Dump();
	Console.WriteLine($"Severity: {sim.Catches.Select(l => sim.Layers[l]).Select(l => l.Depth * l.Range).Sum()}");
}

List<Layer> ReadInput()
{
	List<Layer> layers = new List<Layer>();
	var instructions = File.ReadAllLines("Q:\\git\\advent\\day13_input.txt");
	foreach (var instruction in instructions)
	{
		var split = instruction.Split(':');
		layers.Add(new Layer{
			Depth = int.Parse(split[0].Trim()),
			Range = int.Parse(split[1].Trim()),
		});
	}
	return layers;
}

public class Layer
{
	public int Depth { get; set; }
	public int Range { get; set; }
	public int ScanPosition { get; set; }
	private int scanDirection = 1;
	
	public void MoveScanPosition()
	{
		if (Range == 1)
		{
			return;
		}
		
		ScanPosition += scanDirection;
		if (ScanPosition == 0 || ScanPosition == Range - 1)
		{
			scanDirection = -1 * scanDirection;
		}
	}
}

public class Simulation
{
	public Simulation(List<Layer> layers)
	{
		Layers = layers.ToDictionary(l => l.Depth);
		MaxDepth = layers.Max(l => l.Depth);
        CurrentDepth = -1;
		Catches = new List<int>();
	}
	
	public Dictionary<int, Layer> Layers { get; private set; }
	public int CurrentDepth { get; private set; }
	public int MaxDepth { get; private set; }
	public List<int> Catches { get; set; }
	
	public void Run()
	{
		while (CurrentDepth < MaxDepth)
		{
			Step();
            Move();
			//Layers[6].Dump();
		}
	}
	
	private void Step()
	{
		CurrentDepth++;
		Layer layer;
		if (Layers.TryGetValue(CurrentDepth, out layer))
		{
			if (layer.ScanPosition == 0)
			{
				RegisterCatch(layer.Depth);
			}
		}
	}
	
	private void Move()
	{
		foreach (var layer in Layers.Values)
		{
			layer.MoveScanPosition();
		}
	}
	
	private void RegisterCatch(int depth)
	{
		Catches.Add(depth);
	}
}

