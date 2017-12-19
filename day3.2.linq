<Query Kind="Statements" />



var vals = new Dictionary<string, int>();
vals["0-0"] = 1;

int ring = 3;
int x = 1, y = 0;

var directions = new int[][] { new [] { 0, -1 }, new [] { -1, 0 }, new [] { 0, 1 }, new [] { 1, 0 } };
var currentDirection = 0;

var lastVal = 1;
while (lastVal <= 289326)
{
	// calculate number at position
	string currentPos = $"{x}-{y}";
	int myVal = 0;
	myVal += Lookup(x - 1, y - 1);
	myVal += Lookup(x, y - 1);
	myVal += Lookup(x + 1, y - 1);
	myVal += Lookup(x - 1, y);
	myVal += Lookup(x + 1, y);
	myVal += Lookup(x - 1, y + 1);
	myVal += Lookup(x, y + 1);
	myVal += Lookup(x + 1, y + 1);
	vals[currentPos] = myVal;
	lastVal = myVal;

	// Ensure direction
	if (Math.Abs(x) == ring / 2 && Math.Abs(y) == ring / 2)
	{
		currentDirection++;
	}

	// end of ring?
	if (currentDirection == directions.Length)
	{
		ring += 2;
		x += 1;
		currentDirection = 0;
	}
	else
	{
		// Move to next cell
		x += directions[currentDirection][0];
		y += directions[currentDirection][1];
	}
}

Console.WriteLine($"Found {lastVal} at {x}, {y}");

int Lookup(int a, int b)
{
	string p = $"{a}-{b}";
	int v;
	if (vals.TryGetValue(p, out v))
	{
		return v;
	}
	return 0;
}