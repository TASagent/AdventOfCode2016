const string inputFile = @"../../../../input25.txt";

Console.WriteLine("Day 25 - Four-Dimensional Adventure");
Console.WriteLine("Star 1");
Console.WriteLine();

Console.WriteLine("See the input file for analysis:");
Console.WriteLine();

foreach (string line in File.ReadLines(inputFile))
{
    Console.WriteLine(line);
}

Console.WriteLine();
Console.ReadKey();
