<Query Kind="Program" />

void Main()
{
	long a = 289;
	long b = 629;
	int matchCount= 0;
	
	for (int i = 0; i < 40000000; i++)
	{
		a = (a * 16807) % 2147483647;
		b = (b * 48271) % 2147483647;
		if ((a & 0xFFFF) == (b & 0xFFFF))
		{
			matchCount++;
		}
		//Console.WriteLine($"{a.ToString().PadLeft(20, ' ')} {b.ToString().PadLeft(20, ' ')}");
	}
	matchCount.Dump();
}

// Define other methods and classes here