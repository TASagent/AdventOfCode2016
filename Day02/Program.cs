using AoCTools;

const string inputFile = @"../../../../input02.txt";


Console.WriteLine("Day 02 - Bathroom Security");
Console.WriteLine("Star 1");
Console.WriteLine();


Dictionary<Point2D, int> keypad = new Dictionary<Point2D, int>();

for (int x = 0; x < 3; x++)
{
    for (int y = 0; y < 3; y++)
    {
        keypad.Add((x, y), 1 + x + 3 * y);
    }
}


string[] instructions = File.ReadAllLines(inputFile);

//Start at 5
Point2D position = (1, 1);

List<int> combo = new List<int>();

foreach (string line in instructions)
{
    foreach (char c in line)
    {
        switch (c)
        {
            case 'U':
                if (keypad.ContainsKey(position - Point2D.YAxis))
                {
                    position -= Point2D.YAxis;
                }
                break;

            case 'D':
                if (keypad.ContainsKey(position + Point2D.YAxis))
                {
                    position += Point2D.YAxis;
                }
                break;

            case 'L':
                if (keypad.ContainsKey(position - Point2D.XAxis))
                {
                    position -= Point2D.XAxis;
                }
                break;

            case 'R':
                if (keypad.ContainsKey(position + Point2D.XAxis))
                {
                    position += Point2D.XAxis;
                }
                break;

            default:
                throw new Exception($"Unexpected char: {c}");
        }
    }

    combo.Add(keypad[position]);
}


Console.WriteLine($"Combo: {string.Join("", combo.Select(i => i.ToString()))}");


Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

char[,] keypad2Setup =
{
    {'\0', '\0', '1', '\0', '\0' },
    {'\0', '2', '3', '4', '\0' },
    {'5', '6', '7', '8', '9' },
    {'\0', 'A', 'B', 'C', '\0' },
    {'\0', '\0', 'D', '\0', '\0' }
};

Dictionary<Point2D, char> keypad2 = new Dictionary<Point2D, char>();

for (int x = 0; x < 5; x++)
{
    for (int y = 0; y < 5; y++)
    {
        if (keypad2Setup[y, x] != '\0')
        {
            keypad2.Add((x, y), keypad2Setup[y, x]);
        }
    }
}


List<char> combo2 = new List<char>();

position = (0, 2);

foreach (string line in instructions)
{
    foreach (char c in line)
    {
        switch (c)
        {
            case 'U':
                if (keypad2.ContainsKey(position - Point2D.YAxis))
                {
                    position -= Point2D.YAxis;
                }
                break;

            case 'D':
                if (keypad2.ContainsKey(position + Point2D.YAxis))
                {
                    position += Point2D.YAxis;
                }
                break;

            case 'L':
                if (keypad2.ContainsKey(position - Point2D.XAxis))
                {
                    position -= Point2D.XAxis;
                }
                break;

            case 'R':
                if (keypad2.ContainsKey(position + Point2D.XAxis))
                {
                    position += Point2D.XAxis;
                }
                break;

            default:
                throw new Exception($"Unexpected char: {c}");
        }
    }

    combo2.Add(keypad2[position]);
}

Console.WriteLine($"Combo 2: {string.Join("", combo2)}");

Console.WriteLine();
Console.ReadKey();
