<Query Kind="Statements" />

var input = File.ReadAllLines("Q:\\git\\advent\\day5_input.txt").Select(p => int.Parse(p)).ToArray();

int pos = 0;
int listSize = input.Length;
int stepsTaken = 0;

while (pos >= 0 && pos < listSize)
{
	var jmpSize = input[pos];
	if (jmpSize >= 3)
	{
		input[pos] -= 1;
	}
	else
	{
		input[pos] += 1;
	}
	pos += jmpSize;
	stepsTaken++;
}
Console.WriteLine($"Steps = {stepsTaken}");