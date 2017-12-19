<Query Kind="Program" />

void Main()
{
	var input = "199,0,255,136,174,254,227,16,51,85,1,2,22,17,7,192".Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToArray();
	var buffer = new CircularBuffer(256);

	int pos = 0;
	int skip = 0;
	foreach (int len in input)
	{
		buffer.Reverse(pos, len);
		pos = (pos + len + skip) % buffer.ListLength;
		skip++;
	}

	Console.WriteLine($"First two numbers: {buffer.data[0]} and last: {buffer.data[1]}");
	Console.WriteLine($"Multiplieds: {buffer.data[0] * buffer.data[1]}");
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