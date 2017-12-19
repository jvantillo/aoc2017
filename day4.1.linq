<Query Kind="Statements" />

var input = File.ReadAllLines("Q:\\git\\advent\\day4_input.txt");
int goodLineCount = input.Count(CheckLine);
Console.WriteLine("Good " + goodLineCount);

bool CheckLine(string line)
{
	var words = line.Split(' ');
	List<string> seen = new List<string>();
	foreach (var word in words)
	{
		var sorted = AnagramLookup(word);
		if (seen.Contains(word) || seen.Contains(sorted))
		{
			return false;
		}
		seen.Add(word);
		seen.Add(sorted);
	}
	return true;
}

string AnagramLookup(string s)
{
	char[] charArray = s.ToCharArray();
	charArray = charArray.OrderBy(a => a).ToArray();
	return new string(charArray);
}

//class Node
//{
//	public Node(string text)
//	{
//		Text = text;
//		Value = text[0];
//		Remainder = text.Length > 1 ? text.Substring(1) : String.Empty;
//	}
//	
//	public char Value { get; private set; }
//	
//	public string Remainder { get; private set; }
//	
//	public string Text { get; set; }
//}