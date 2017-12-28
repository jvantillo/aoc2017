<Query Kind="Program" />

void Main()
{
	long a = 289;
	long b = 629;
	int matchCount= 0;
	
	for (int i = 0; i < 5000000; i++)
	{
		do
		{
			a = (a * 16807) % 2147483647;
		} while (a % 4 != 0);
		do
		{
			b = (b * 48271) % 2147483647;
		} while (b % 8 != 0);
		if ((a & 0xFFFF) == (b & 0xFFFF))
		{
			matchCount++;
		}
		//Console.WriteLine($"{a.ToString().PadLeft(20, ' ')} {b.ToString().PadLeft(20, ' ')}");
	}
	matchCount.Dump();
}

// Define other methods and classes here
