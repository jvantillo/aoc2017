<Query Kind="Program" />

void Main()
{
	int width, height;
	var input = ReadInput(out width, out height);

	// x & y of starting pos
	int y = 0;  // first line
	int x = input[0].IndexOf('|');
	int direction = 2;      // (0 = up, 1 = right, 2 = down, 3 = left)

	string seen = "";
	bool hasEnded = false;
	int stepCount =1;	// account for first char
	while (!hasEnded)
	{
		// advance in direction
		switch (direction)
		{
			case 0:
				y--;
				break;
			case 1:
				x++;
				break;
			case 2:
				y++;
				break;
			case 3:
				x--;
				break;
		}

		// determine new direction
		char charAtPos = input[y][x];
		stepCount++;
		//Console.WriteLine(charAtPos);
		bool isNode = charAtPos >= 'A' && charAtPos <= 'Z';
		if (isNode)
		{
			seen += charAtPos;
		}
		else
		{
			switch (charAtPos)
			{
				case '|':
				case '-':
					{
						break;
					}
				case '+':
					{
						// any direction except current
						int newDirection = -1;
						if (direction != 2 && y > 0 && input[y-1][x] != ' ')
						{
							newDirection = 0;
						}
						else if (direction != 3 && x < (width-1) && input[y][x + 1] != ' ')
						{
							newDirection = 1;
						}
						else if (direction != 0 && y < (height -1) && input[y + 1][x] != ' ')
						{
							newDirection = 2;
						}
						else if (direction != 1 && x > 0 && input[y][x-1] != ' ')
						{
							newDirection = 3;
						}
						
						// end of line?
						if (newDirection == -1)
						{
							hasEnded = true;
							Console.WriteLine("Line end at +");
						}
						direction = newDirection;
						break;
					}
				default:
					{
						stepCount--;
						Console.WriteLine("End at unknown char");
						hasEnded = true;
						break;
					}
			}

		}
	}
	Console.WriteLine("Seen:  " + seen);
	Console.WriteLine("Steps: " + stepCount);
}

string[] ReadInput(out int width, out int height)
{
	var lines = File.ReadAllLines("Q:\\git\\advent\\day19_input.txt");
	width = lines.Max(l => l.Length);
	height = lines.Count();
	return lines;
}
