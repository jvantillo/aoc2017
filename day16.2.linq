<Query Kind="Program" />

void Main()
{
	int[] positions = new int[16];	// program positions
	int[] programs = new int[16];	// indexes with programs
	
	// allocate
	for (int i = 0; i < 16; i++)
	{
		positions[i] = programs[i] = i;
	}

	string[] input = ReadInput();
	Dictionary<string, int> history = new Dictionary<string, int>();
	for (int loopCount = 0; loopCount < 1000000000; loopCount++)
	{
		foreach (var instruction in input)
		{
			switch (instruction[0])
			{
				case 's':
					{
						int spinSize = int.Parse(instruction.Substring(1));

						for (int i = 0; i < 16; i++)
						{
							positions[i] = (positions[i] + spinSize) % 16;
							programs[positions[i]] = i;
						}

						break;
					}
				case 'x':
					{
						string[] tmp = instruction.Substring(1).Split('/');
						int a = int.Parse(tmp[0]);
						int b = int.Parse(tmp[1]);

						// find which programs are at the given positions
						int pA = programs[a];
						int pB = programs[b];

						// re-assign program positions
						positions[pA] = b;
						positions[pB] = a;

						// swap contents at indexes
						programs[a] = pB;
						programs[b] = pA;

						break;
					}
				case 'p':
					{
						int pA = ProgramToNumber(instruction[1]);
						int pB = ProgramToNumber(instruction[3]);

						// find positions of these programs
						int a = positions[pA];
						int b = positions[pB];

						// re-assign program positions
						positions[pA] = b;
						positions[pB] = a;

						// swap contents at indexes
						programs[a] = pB;
						programs[b] = pA;

						break;
					}
				default:
					throw new ApplicationException("Unknown instruction " + instruction);
			}

			// Sanity check
			/*if (positions.Distinct().Count() != 16 || programs.Distinct().Count() != 16)
			{
				Console.WriteLine("Corruption after " + instruction);
				return;
			}*/
		} // foreach


		// calculate checksum and add to history
		StringBuilder s = new StringBuilder();
		foreach (var pr in programs)
		{
			s.Append(NumberToProgram(pr));
		}
		string checksum = s.ToString();

		if (history.ContainsKey(checksum))
		{
			var pos = history[checksum];
			
			// say pos = 60 and we hit 0 in cache. Meaning every 60 gens it will repeat (0, 60, 120), after position (0 + 1)
			var solPos = (1000000000 - (pos + 1)) % (loopCount - pos);
			var solution = history.Single(p => p.Value == solPos);
			Console.WriteLine(solution.ToString());
			return;
		}
		history[checksum] = loopCount;
	}

	// No loop found:
	
	// collect output
	StringBuilder sb = new StringBuilder();
	foreach (var pr in programs)
	{
		sb.Append(NumberToProgram(pr));
	}
	Console.WriteLine(sb.ToString());
	//positions.Dump();
	//programs.Dump();
}

int ProgramToNumber(char c)
{
	return (int)c - (int)'a';
}

char NumberToProgram(int i)
{
	return (char) (i + (int)'a');
}

string[] ReadInput()
{
	var instructions = File.ReadAllText("Q:\\git\\advent\\day16_input.txt");
	return instructions.Split(',');
}
