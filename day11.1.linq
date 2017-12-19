<Query Kind="Program" />

void Main()
{
	var instructions = File.ReadAllText("Q:\\git\\advent\\day11_input.txt")
	.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
	
	int x = 0, y = 0;		// Coordinates on “odd-q” vertical layout
	int maxDistance = 0;
	foreach (var instruction in instructions)
	{
		switch (instruction.ToLower())
		{
			case "n":
				y--;
				break;
			case "s":
				y++;
				break;
			case "nw":
				if (x % 2 == 0)
				{
					y--;
				}
				x--;
				break;
			case "ne":
				if (x % 2 == 0)
				{
					y--;
				}
				x++;
				break;
			case "sw":
				x--;
				if (x % 2 == 0)
				{
					y++;
				}
				break;
			case "se":
				x++;
				if (x % 2 == 0)
				{
					y++;
				}
				break;
			default:
				throw new ApplicationException("Unknown instruction " + instruction);
		}
		maxDistance = Math.Max(maxDistance, ds(x, y));
	}

	Console.WriteLine($"After run, x = {x} and y {y}");
	
	// Now find out how many steps we need to take to go from [0, 0] to the coordinates found
	
	int distance = ds(x, y);
	Console.WriteLine("Distance: " + distance);
	Console.WriteLine("Max distance: " + maxDistance);
}

void cube_to_oddq(int x, int y, int z, out int col, out int row)
{
	col = x;
	row = z + (x - (x & 1)) / 2;
}

void oddq_to_cube(int col, int row, out int x, out int y, out int z)
{
	x = col;
	z = row - (col - (col & 1)) / 2;
	y = -x - z;
}

int ds(int x, int y)
{
	int ax, ay, az, bx, by, bz;
	oddq_to_cube(0, 0, out ax, out ay, out az);
	oddq_to_cube(x, y, out bx, out by, out bz);
	return cube_distance(ax, ay, az, bx, by, bz);
}

int cube_distance(int ax, int ay, int az, int bx, int by, int bz)
{
	return Math.Max(Math.Max(Math.Abs(ax - bx), Math.Abs(ay - by)), Math.Abs(az - bz));
}

int hex_distance(int aq, int ar, int bq, int br)
{
	return (Math.Abs(aq - bq)
		  + Math.Abs(aq + ar - bq - br)
		  + Math.Abs(ar - br)) / 2;
}