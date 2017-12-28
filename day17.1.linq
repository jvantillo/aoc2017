<Query Kind="Program" />

void Main()
{
	int input = 343;
	var buffer = new CircularBuffer();
	for (int i = 1; i <= 2017; i++)
	{
		buffer.Insert(input, i);
	}
	
	buffer.Values.Skip(Math.Max(0, buffer.CurrentPos - 10)).Take(15).ToArray().Dump();
}

public class CircularBuffer
{
	public CircularBuffer()
	{
		Values = new List<int>{ 0 };
	}
	
	public List<int> Values;
	
	public int CurrentPos = 0;
	
	public void Insert(int step, int value)
	{
		int pos = (CurrentPos + step) % Values.Count;
		pos++;
		Values.Insert(pos, value);
		CurrentPos = pos;
	}
}
