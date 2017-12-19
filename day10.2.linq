<Query Kind="Program" />

void Main()
{
	string text = "199,0,255,136,174,254,227,16,51,85,1,2,22,17,7,192";
	//string text = "1,2,3";
	var input = text.ToCharArray().Select(p => (int)p).ToList();
	input.AddRange(new int[] {17, 31, 73, 47, 23});
	var buffer = new CircularBuffer(256);

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

	Console.WriteLine($"First two numbers: {buffer.data[0]} and {buffer.data[1]}");
	Console.WriteLine($"Multiplied: {buffer.data[0] * buffer.data[1]}");
	
	string denseHash = "";
	for (int blockNumber = 0; blockNumber < 16; blockNumber++)
	{
		int blockResult = 0;
		for (int valueIndex = 0; valueIndex < 16; valueIndex++)
		{
			blockResult= blockResult ^ buffer.data[valueIndex + 16 * blockNumber];
		}
		denseHash += blockResult.ToString("X2");
	}
	
	Console.WriteLine("Dense hash: " + denseHash.ToLower());
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