<Query Kind="Program" />

void Main()
{
	//string text = "flqrgnkx";
	Stopwatch sw = new Stopwatch();
	sw.Start();
	string text = "vbqugkhl";
	var grid = new Grid();
	grid.Data = new bool[128][];
	for (int i = 0; i < 128; i++)
	{
		grid.Data[i] = GetRowBits(text, i);
	}
	int groupCount = CountGroups(grid);
	sw.Stop();
	Console.WriteLine("Groups: " + groupCount + " in ms: " + sw.ElapsedMilliseconds.ToString("0.0"));
}

class Grid
{
	public bool[][] Data;
	
	public bool IsSet(int pos)
	{
		return Data[pos / 128][pos % 128];
	}
	
	public IEnumerable<int> GetDirectNeighbours(int pos)
	{
		int x = pos / 128;
		int y = pos % 128;
		if (ExistsAndIsSet(x-1, y))
		{
			yield return pos - 128;
		}
		if (ExistsAndIsSet(x+1, y))
		{
			yield return pos + 128;
		}
		if (ExistsAndIsSet(x, y-1))
		{
			yield return pos - 1;
		}
		if (ExistsAndIsSet(x, y+1))
		{
			yield return pos + 1;
		}
	}

	private bool ExistsAndIsSet(int x, int y)
	{
		return x >= 0 && x < 128 && y >= 0 && y < 128 && Data[x][y]; 
	}
	
	public void ZeroAt(int pos)
	{
		Data[pos / 128][pos % 128] = false;
	}
}

int CountGroups(Grid grid)
{
	int groupCount = 0;
	for (int i = 0; i < 128 * 128; i++)
	{
		// is set? Begin examining new group.
		if (grid.IsSet(i))
		{
			groupCount++;
			ZeroAt(grid, i);
		}
	}
	
	return groupCount;
}

void ZeroAt(Grid g, int pos)
{
	var n = g.GetDirectNeighbours(pos);
	g.ZeroAt(pos);
	foreach (var neighbour in n)
	{
		ZeroAt(g, neighbour);
	}
}

bool[] GetRowBits(string text, int row)
{
	text = text + "-" + row;
	var input = text.ToCharArray().Select(p => (int)p).ToList();
	input.AddRange(new int[] { 17, 31, 73, 47, 23 });
	var buffer = new CircularBuffer(256);
	Tie(input, buffer);
	string hash = CalculateDenseHash(buffer);
	//Console.WriteLine($"Hash is {hash} of length {hash.Length}");
	return GetBits(hash);
}

void Tie(List<int> input, CircularBuffer buffer)
{
	int pos = 0;
	int skip = 0;
	for (int round = 0; round < 64; round++)
	{
		foreach (int len in input)
		{
			buffer.Reverse(pos, len);
			pos = (pos + len + skip) % buffer.ListLength;
			skip++;
		}
	}
}

string CalculateDenseHash(CircularBuffer buffer)
{
	string denseHash = "";
	for (int blockNumber = 0; blockNumber < 16; blockNumber++)
	{
		int blockResult = 0;
		for (int valueIndex = 0; valueIndex < 16; valueIndex++)
		{
			blockResult = blockResult ^ buffer.data[valueIndex + 16 * blockNumber];
		}
		denseHash += blockResult.ToString("X2");
	}
	return denseHash;
}

bool[] GetBits(string hash)
{
	var bytes = StringToByteArray(hash);
	bool[] data = new bool[bytes.Length * 8];
	int dataPos = 0;
	for (int byteIndex = 0; byteIndex < bytes.Length; byteIndex++)
	{
		for (int bitIndex = 0; bitIndex < 8; bitIndex++)
		{
			int ix = 7 - (bitIndex % 8);
			data[dataPos] = (bytes[byteIndex] & (0x1 << ix)) == (0x1 << ix);	 
			dataPos++;
		}
	}
	return data;
}

static byte[] StringToByteArray(String hex)
{
	int NumberChars = hex.Length;
	byte[] bytes = new byte[NumberChars / 2];
	for (int i = 0; i < NumberChars; i += 2)
		bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
	return bytes;
}

private class CircularBuffer
{
	public int ListLength;

	public int[] data;

	public CircularBuffer(int len)
	{
		ListLength = len;
		data = new int[ListLength];
		for (int i = 0; i < ListLength; i++)
		{
			data[i] = i;
		}
	}

	public void Reverse(int pos, int len)
	{
		if (len <= 1)
		{
			return;
		}

		for (int i = 0; i < len / 2; i++)
		{
			Swap(
				(pos + i) % ListLength,
				(pos + len - i - 1) % ListLength
				);
		}
	}

	private void Swap(int indexA, int indexB)
	{
		var value = data[indexA];
		data[indexA] = data[indexB];
		data[indexB] = value;
	}
}