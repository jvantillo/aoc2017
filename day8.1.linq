<Query Kind="Program" />

void Main()
{
	var rawInput = File.ReadAllLines("Q:\\git\\advent\\day8_input.txt").ToArray();
	
	Processor processor = new Processor();
	var instructions = rawInput.Select(p => processor.CreateInstruction(p)).ToArray();
	foreach (var instruction in instructions)
	{
		processor.Execute(instruction);
	}
	processor.DumpHighest();
}

class Processor
{
	private Dictionary<string, int> registers;
	private Dictionary<string, Operation> operations;
	private Dictionary<string, Condition> conditions;
	private int maxEver;
	// Sample: c inc -20 if c == 10
	private static Regex reInstruction = new Regex(@"
		^
		(?<opRegister>[a-z]+)
		\s
		(?<op>[a-z]+)
		\s
		(?<opArgument>[0-9\-]+)
		\s
		if
		\s
		(?<condRegister>[a-z]+)
		\s
		(?<cond>[^\s]+)
		\s
		(?<condArgument>[0-9\-]+)	
	", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

	public Processor()
	{
		registers = new Dictionary<string, int>();
		operations = new Dictionary<string, UserQuery.Processor.Operation>
		{
			{ "inc", Plus },
			{ "dec", Minus },
		};
		conditions = new Dictionary<string, UserQuery.Processor.Condition>
		{
			{ "==", EqualTo },
			{ "!=", NotEqualTo },
			{ ">", GreaterThan },
			{ ">=", GreaterThanOrEqualTo },
			{ "<", LessThan },
			{ "<=", LessThanOrEqualTo },
		};
		maxEver = 0;
	}

	public void DumpHighest()
	{
		Console.WriteLine("Highest value is " + registers.Values.Max() + " and highest ever was " + maxEver);
	}

	public void Execute(Instruction instruction)
	{
		if (instruction.Condition.Invoke(GetRegister(instruction.ConditionRegister), instruction.ConditionArgument))
		{
			var val = instruction.Operation.Invoke(GetRegister(instruction.OpRegister), instruction.OpArgument);
			registers[instruction.OpRegister] = val;
			if (val > maxEver)
			{
				maxEver = val;
			}
		}
	}
	
	private int GetRegister(string register)
	{
		int value;
		if (registers.TryGetValue(register, out value))
		{
			return value;
		}
		return 0;
	}

	public Instruction CreateInstruction(string line)
	{
		var m = reInstruction.Match(line);
		if (!m.Success)
		{
			throw new ApplicationException("Unmatched: " + line);
		}
		
		return new Instruction
		{
			Condition = conditions[m.Groups["cond"].Value],
			ConditionRegister = m.Groups["condRegister"].Value,
			ConditionArgument = int.Parse(m.Groups["condArgument"].Value),
			Operation = operations[m.Groups["op"].Value],
			OpRegister = m.Groups["opRegister"].Value,
			OpArgument = int.Parse(m.Groups["opArgument"].Value)
		};
	}

	private int Plus(int registerValue, int conditionValue)
	{
		return registerValue + conditionValue;
	}

	private int Minus(int registerValue, int conditionValue)
	{
		return registerValue - conditionValue;
	}

	private bool EqualTo(int registerValue, int conditionValue)
	{
		return registerValue == conditionValue;
	}

	private bool NotEqualTo(int registerValue, int conditionValue)
	{
		return registerValue != conditionValue;
	}

	private bool GreaterThan(int registerValue, int conditionValue)
	{
		return registerValue > conditionValue;
	}

	private bool GreaterThanOrEqualTo(int registerValue, int conditionValue)
	{
		return registerValue >= conditionValue;
	}

	private bool LessThan(int registerValue, int conditionValue)
	{
		return registerValue < conditionValue;
	}

	private bool LessThanOrEqualTo(int registerValue, int conditionValue)
	{
		return registerValue <= conditionValue;
	}
	
	public class Instruction
	{
		public Operation Operation { get; set; }
		public Condition Condition { get; set; }
		public string OpRegister { get; set; }
		public string ConditionRegister { get; set; }
		public int OpArgument { get; set; }
		public int ConditionArgument { get; set; }
	}
	
	public delegate int Operation(int registerValue, int conditionValue);
	
	public delegate bool Condition(int registerValue, int conditionValue);
}