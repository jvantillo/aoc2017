<Query Kind="Statements" />

string input = "5	1	10	0	1	7	13	14	3	12	8	10	7	12	0	6";
//string input = "0 2 7 0";
int[] bankSizes = input.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToArray();
int bankCount = bankSizes.Length;

Dictionary<string, int> hashes = new Dictionary<string, int>();

int cycleCount = 0;
string checksum = CalculateChecksum();
while (!hashes.ContainsKey(checksum))
{
	hashes[checksum] = cycleCount;
	cycleCount++;
	Redistribute();
	checksum = CalculateChecksum();
}

Console.WriteLine("Steps: " + cycleCount + " at " + (cycleCount - hashes[checksum]));

string CalculateChecksum()
{
	return string.Join("!", bankSizes.Select(p => p.ToString()).ToArray());
}

void Redistribute()
{
	// Get biggest
	int biggestSize = bankSizes[0];
	int biggestSizeIndex = 0;
	for (int i = 1; i < bankCount; i++)
	{
		if (bankSizes[i] > biggestSize)
		{
			biggestSize = bankSizes[i];
			biggestSizeIndex = i;
		}
	}

	bankSizes[biggestSizeIndex] = 0;
	
	int remainder = biggestSize;
	int pos = (biggestSizeIndex + 1) % bankCount;
	while (remainder > 0)
	{
		bankSizes[pos] += 1;
		pos = (pos + 1) % bankCount;
		remainder--;
	}
}