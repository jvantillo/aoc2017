<Query Kind="Program" />

void Main()
{
	//string text = "flqrgnkx";
	string text = "vbqugkhl";
	int sum = 0;
	for (int i = 0; i < 128; i++)
	{
		sum += CountRow(text, i);
	}
	Console.WriteLine("Occupied: " + sum);
}

int CountRow(string text, int row)
{
	text = text + "-" + row;
	var input = text.ToCharArray().Select(p => (int)p).ToList();
	input.AddRange(new int[] { 17, 31, 73, 47, 23 });
	var buffer = new CircularBuffer(256);
	Tie(input, buffer);
	string hash = CalculateDenseHash(buffer);
	//Console.WriteLine($"Hash is {hash} of length {hash.Length}");
	if (row < 8)
	{
		Visualise(hash, 8);
	}
	return CountBits(hash);
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

int CountBits(string hash)
{
	int setCount = 0;
	var bytes = StringToByteArray(hash);
	for (int byteIndex = 0; byteIndex < bytes.Length; byteIndex++)
	{
		for (int bitIndex = 0; bitIndex < 8; bitIndex++)
		{
			if ( (bytes[byteIndex] & (0x1 << bitIndex)) == (0x1 << bitIndex) )
			{
				setCount++;
			}
		}
	}
	return setCount;
}

void Visualise(string hash, int max)
{
	// convert hash to bits
	StringBuilder sb = new StringBuilder();
	var bytes = StringToByteArray(hash);
	for (int i = 0; i < max; i++)
	{
		int byteIndex = i / 8;
		int bitIndex = 7 - (i % 8);

		bool isSet = (bytes[byteIndex] & (0x1 << bitIndex)) == (0x1 << bitIndex);
		//Console.WriteLine($"D: {byteIndex} / {bitIndex} / {bytes[byteIndex]} / {(0x1 << bitIndex)} / {(bytes[byteIndex] & (0x1 << bitIndex))}");
		sb.Append(isSet? "#" : ".");
	}
	Console.WriteLine(sb.ToString());
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