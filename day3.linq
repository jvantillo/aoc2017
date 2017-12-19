<Query Kind="Statements" />

int input = 289326;
//int input = 1024;

int gridSize = 1;
while (Math.Pow(gridSize, 2) < input)
{
	gridSize += 2;
}
Console.WriteLine("gridSize = " + gridSize);

int number = (int)Math.Pow(gridSize - 2, 2) + 1;
int ring = gridSize / 2;
int x = ring;
int y = ring - 1;
int[][] direction = new [] { new []{ 0, -1 }, new []{ -1, 0 }, new []{ 0, 1 }, new []{ 1, 0 } };
int directionId = 0;

while (number != input)
{
	number += 1;
	x += direction[directionId][0];
	y += direction[directionId][1];
	bool isCorner = (x == ring || x == -ring) && (y == ring || y == -ring);
	if (isCorner)
	{
		directionId++;
	}
}

var distance = Math.Abs(x) + Math.Abs(y);

Console.WriteLine($"{input} is in grid {gridSize} on ring {ring} on x/y {x}/{y} having a distance of {distance}");