const string inputFile = @"../../../../input09.txt";

Console.WriteLine("Day 09 - Explosives in Cyberspace");
Console.WriteLine("Star 1");
Console.WriteLine();

string line = File.ReadAllText(inputFile);

Console.WriteLine($"Decompressed Line Length: {Expand(line)}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

Console.WriteLine($"Decompressed Line Length: {Expand2(line)}");

Console.WriteLine();
Console.ReadKey();


static long Expand(string input)
{
    long count = 0;
    int index = 0;

    while (index < input.Length)
    {
        if (input[index] == '(')
        {
            int endParen = input.IndexOf(')', index);
            (int l, int r) = ReadRep(input.Substring(index + 1, endParen - index - 1));

            index = endParen + 1;

            string repeatedSeq = input.Substring(index, l);

            count += r * l;
            index += l;
        }
        else
        {
            count++;
            index++;
        }
    }

    return count;
}

static long Expand2(string input)
{
    long count = 0;
    int index = 0;

    while (index < input.Length)
    {
        if (input[index] == '(')
        {
            int endParen = input.IndexOf(')', index);
            (int l, int r) = ReadRep(input.Substring(index + 1, endParen - index - 1));

            index = endParen + 1;

            count += r * Expand2(input.Substring(index, l));
            index += l;
        }
        else
        {
            count++;
            index++;
        }
    }

    return count;
}

static (int l, int r) ReadRep(string segment)
{
    int indexOfX = segment.IndexOf('x');
    return (int.Parse(segment[..indexOfX]), int.Parse(segment[(indexOfX + 1)..]));
}