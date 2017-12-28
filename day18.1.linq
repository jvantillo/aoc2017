<Query Kind="Program" />

void Main()
{
	var input = ReadInput();
	//input.Dump();
	
	Processor proc = new Processor();
	long instructionPointer = 0;
	while (instructionPointer >= 0 && instructionPointer < input.Length)
	{
		var instruction = input[instructionPointer];
		switch (instruction.Command)
		{
			case "snd":
			{
				proc.Snd(instruction.X);
				instructionPointer++;
				break;
				}
			case "set":
				{
					proc.Set(instruction.X, instruction.Y);
					instructionPointer++;
					break;
				}
			case "add":
				{
					proc.Add(instruction.X, instruction.Y);
					instructionPointer++;
					break;
				}
			case "mul":
				{
					proc.Mul(instruction.X, instruction.Y);
					instructionPointer++;
					break;
				}
			case "mod":
				{
					proc.Mod(instruction.X, instruction.Y);
					instructionPointer++;
					break;
				}
			case "rcv":
				{
					long? data = proc.Rcv(instruction.X);
					if (data.HasValue)
					{
						Console.WriteLine("RCV value: " + data.Value);
						return;
					}
					instructionPointer++;
					break;
				}
			case "jgz":
				{
					long? offset = proc.Jgz(instruction.X, instruction.Y);
					if (offset.HasValue)
					{
						instructionPointer += offset.Value;
					}
					else
					{
						instructionPointer++;
					}
					break;
				}
			default:
				throw new ApplicationException("Unknown instruction " + instruction.Command);
		}
	}
}

public class Processor
{
	public Processor()
	{
		registers = new Dictionary<string, long>();
	}
	
	public Dictionary<string, long> registers;
	
	public long LastPlayedSound;
	
	public void Snd(string X)
	{
		long val = Resolve(X);
		LastPlayedSound = val;
	}
	
	private void EnsureRegister(string register)
	{
		if (!registers.ContainsKey(register))
		{
			registers[register] = 0L;
		}
	}
	
	public void Set(string X, string Y)
	{
		EnsureRegister(X);
		registers[X] = Resolve(Y);
	}

	public void Add(string X, string Y)
	{
		EnsureRegister(X);
		registers[X] += Resolve(Y);
	}

	public void Mul(string X, string Y)
	{
		EnsureRegister(X);
		registers[X] *= Resolve(Y);
	}

	public void Mod(string X, string Y)
	{
		EnsureRegister(X);
		registers[X] = registers[X] % Resolve(Y);
	}

	public long? Rcv(string X)
	{
		EnsureRegister(X);
		if (registers[X] != 0L)
		{
			return LastPlayedSound;
		}
		return null;
	}
	
	public long? Jgz(string X, string Y)
	{
		EnsureRegister(X);
		if (registers[X] > 0)
		{
			return Resolve(Y);
		}
		return null;
	}

	private long Resolve(string item)
	{
		long val;
		if (long.TryParse(item, out val))
		{
			return val;
		}
		return GetRegisterValue(item);
	}
	
	public long GetRegisterValue(string register)
	{
		long val;
		if (registers.TryGetValue(register, out val))
		{
			return val;
		}
		return registers[register] = 0;
	}
}

struct Instruction
{
	public string Command { get; set; }
	public string X { get; set; }
	public string Y { get; set; }
}

Instruction[] ReadInput()
{
	var instructions = File.ReadAllLines("Q:\\git\\advent\\day18_input.txt");
	Regex reInstruct = new Regex(@"
	^
	(?<command>[a-z]{3})
	\s
	(?<X>[a-z0-9]+)
	(
		\s
		(?<Y>[a-z0-9\-]+)
	)?
	", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
	
	List<Instruction> list = new List<UserQuery.Instruction>();
	foreach (var instruction in instructions)
	{
		Match m = reInstruct.Match(instruction);
		string command = m.Groups["command"].Value;
		string x = m.Groups["X"].Value;
		string y = String.Empty;
		if (m.Groups["Y"].Success)
		{
			y = m.Groups["Y"].Value;
		}

		list.Add(new Instruction{
			Command = command,
			X = x,
			Y = y,
		});
	}
	return list.ToArray();
}
