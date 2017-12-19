<Query Kind="Program" />

void Main()
{
	Reader reader = new Reader();
	reader.ReadAll();
}

public class Reader
{
	public int totalScore = 0;
	
	private bool ignoreNext = false;
	
	private bool inGarbageMode = false;
	
	private string input;
	
	private int inputLength;
	
	private int garbageChars;
	
	Stack<int> groupScores;
	
	public Reader()
	{
		input = File.ReadAllText("Q:\\git\\advent\\day9_input.txt").Trim();
		inputLength = input.Length;
		groupScores = new Stack<int>();
	}
	
	public void ReadAll()
	{
		for (int i = 0; i < inputLength; i++)
		{
			Read(i);
		}
		
		Console.WriteLine("Total score " + totalScore + " with garbase count: " + garbageChars);
	}
	
	public void Read(int pos)
	{
		if (ignoreNext)
		{
			ignoreNext = false;
			return;
		}
		
		char c = input[pos];
		
		if (inGarbageMode)
		{
			if (c == '>')
			{
				inGarbageMode = false;
				return;
			}
			else if (c == '!')
			{
				ignoreNext = true;
				return;
			}
			garbageChars++;
			return;
		}
		
		switch (c)
		{
			case '!':
				ignoreNext = true;
				return;
			case '{':
				StartGroup();
				return;
			case '}':
				TerminateGroup();
				return;
			case '<':
				inGarbageMode = true;
				return;
			default:
				// ignore, 'contents'
				break;
		}
	}
	
	private void StartGroup()
	{
		int existingGroupCount = groupScores.Count();
		if (existingGroupCount == 0)
		{
			groupScores.Push(1);
		}
		else
		{
			groupScores.Push(groupScores.Peek() + 1);
		}
	}
	
	private void TerminateGroup()
	{
		int closingGroupScore = groupScores.Pop();
		totalScore += closingGroupScore;
	}
}


