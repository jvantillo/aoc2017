<Query Kind="Program" />

void Main()
{
	int input = 343;
	var buffer = new CircularBuffer();
	for (int i = 1; i <= 50000000; i++)
	{
		buffer.Insert(input, i);
	}
	
	//buffer.Values.Skip(Math.Max(0, buffer.CurrentPos - 10)).Take(15).ToArray().Dump();
	buffer.Dump();
}

public class CircularBuffer
{
	public CircularBuffer()
	{
	}
	
	public int CurrentPos = 0;
	public int ValueCount = 1;
	public int PosOfZero = 0;
	public int ValueAfterZero = 0;
	
	public void Insert(int step, int value)
	{
		int pos = (CurrentPos + step) % ValueCount;
		pos++;

		// Are we inserting before or after 0?
		if (pos <= PosOfZero)
		{
			PosOfZero++;
		} else if (pos - 1 == PosOfZero)
		{
			ValueAfterZero = value;
		}

		ValueCount++;
		CurrentPos = pos;
	}
}
