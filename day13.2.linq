<Query Kind="Program" />

void Main()
{
	int delay = 0;
	var layers = ReadInput();
	Simulation sim = new Simulation(layers);
	bool doTry = true;
	while (doTry)
	{
		sim.Reset(delay);
//		if (delay == 10)
//		{
//			sim.Dump();
//			var res = sim.Run();
//			res.Dump();
//			sim.Dump();
//			return;
//		}
		doTry = sim.Run();
		delay++;
		if (delay % 50000 == 0)
		{
			Console.WriteLine("@" + delay);
		}
	}
	Console.WriteLine("Solution @ " + (delay - 1));
}

Layer[] ReadInput()
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
	return layers.OrderBy(p => p.Depth).ToArray();
}

public class Layer
{
	public int Depth { get; set; }
	public int Range { get; set; }
	public int ScanPosition { get; set; }
	private int scanDirection;

	public void Reset()
	{
		scanDirection = 1;
		ScanPosition = 0;
	}
	
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
	
	public void MoveScanPosition(int amount)
	{
		// Reduce amount by roundtrip size
		int roundtripSize = 2 * (Range - 1);
		amount = amount % roundtripSize;
		for (int i = 0; i < amount; i++)
		{
			MoveScanPosition();
		}
	}
}

public class Simulation
{
	public Simulation(Layer[] layers)
	{
		Layers = layers;
		MaxDepth = layers.Max(l => l.Depth);
		DepthIndex = new Dictionary<int, int>();
		for (int i = 0; i < layers.Length; i++)
		{
			DepthIndex[layers[i].Depth] = i;
		}
	}
	
	public Dictionary<int, int> DepthIndex;
	public readonly Layer[] Layers;
	public int CurrentDepth { get; private set; }
	public int MaxDepth { get; private set; }
	public int Delay { get; private set; }
	
	public void Reset(int delay)
	{
		Delay = delay;
		CurrentDepth = -1;
		foreach (var layer in Layers)
		{
			layer.Reset();
		}
	}
	
	public bool Run()
	{
		foreach (var layer in Layers)
		{
			layer.MoveScanPosition(Delay);
		}

		bool hasCatch = false;
		while (!hasCatch && CurrentDepth < MaxDepth)
		{
			hasCatch = Step();
            Move();
		}
		return hasCatch;
	}
	
	private bool Step()
	{
		CurrentDepth++;
		int index;
		if (DepthIndex.TryGetValue(CurrentDepth, out index))
		{
			Layer layer = Layers[index];
			if (layer.ScanPosition == 0)
			{
				return true;
			}
		}
		return false;
	}
	
	private void Move()
	{
		foreach (var layer in Layers)
		{
			layer.MoveScanPosition();
		}
	}
}