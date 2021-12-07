using AoCTools;

const string inputFile = @"../../../../input01.txt";


Console.WriteLine("Day 01 - No Time for a Taxicab");
Console.WriteLine("Star 1");
Console.WriteLine();

var instructions = File.ReadAllText(inputFile).Split(", ");

Point2D position = (0, 0);
//Start facing north.
Point2D facing = (0, 1);

HashSet<Point2D> visitedLocations = new HashSet<Point2D>() { (0, 0) };

bool found = false;
Point2D hqPos = (0, 0);

foreach (string instruction in instructions)
{
    switch (instruction[0])
    {
        case 'R':
            facing = facing.Rotate(true);
            break;

        case 'L':
            facing = facing.Rotate(false);
            break;

        default:
            throw new ArgumentException($"Unexpected direction: {instruction[0]}");
    }

    int distance = int.Parse(instruction.Substring(1));

    for (int i = 1; i <= distance; i++)
    {
        position += facing;
        if (!found)
        {
            if (!visitedLocations.Add(position))
            {
                found = true;
                hqPos = position;
            }
        }
    }
}

Console.WriteLine($"Final distance: {Math.Abs(position.x) + Math.Abs(position.y)}");


Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

Console.WriteLine($"HQ Location: {Math.Abs(hqPos.x) + Math.Abs(hqPos.y)}");

Console.WriteLine();
Console.ReadKey();
