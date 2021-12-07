const string inputFile = @"../../../../input15.txt";

Console.WriteLine("Day 15 - Timing is Everything");
Console.WriteLine("Star 1");
Console.WriteLine();


//Input File:
//Disc #1 has 13 positions; at time=0, it is at position 11.
//Disc #2 has 5 positions; at time=0, it is at position 0.
//Disc #3 has 17 positions; at time=0, it is at position 11.
//Disc #4 has 3 positions; at time=0, it is at position 0.
//Disc #5 has 7 positions; at time=0, it is at position 2.
//Disc #6 has 19 positions; at time=0, it is at position 17.

//All have a prime number of positions, so the periodicity is the product.
// 19*17*13*7*5*3 = 440895

//For each disc, P = (x0 + t)%N


string[] lines = File.ReadAllLines(inputFile);

List<Disc> discs = lines.Select(x => new Disc(x)).ToList();

long t = 0;
long periodicity = 1;


foreach (Disc disc in discs)
{
    //Advance by steps of periodicity to the next interaction
    while (disc.AdjustedPositionAt(t) != 0)
    {
        t += periodicity;
    }

    periodicity *= disc.positions;
}

Console.WriteLine($"First strike time: {t}");


Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

discs.Add(new Disc(discs.Count + 1, 11, 0));

t = 0;
periodicity = 1;

foreach (Disc disc in discs)
{
    //Advance by steps of periodicity to the next interaction
    while (disc.AdjustedPositionAt(t) != 0)
    {
        t += periodicity;
    }

    periodicity *= disc.positions;
}

Console.WriteLine($"First strike time: {t}");

Console.WriteLine();
Console.ReadKey();

readonly struct Disc
{
    public readonly long discNumber;
    public readonly long positions;
    public readonly long initialPosition;

    public Disc(long discNumber, long positions, long initialPosition)
    {
        this.discNumber = discNumber;
        this.positions = positions;
        this.initialPosition = initialPosition;
    }

    public Disc(string line)
    {
        string[] splitLine = line.Split(new char[] { ' ', '#', '.' }, StringSplitOptions.RemoveEmptyEntries);

        discNumber = long.Parse(splitLine[1]);
        positions = long.Parse(splitLine[3]);
        initialPosition = long.Parse(splitLine[11]);
    }

    public long PositionAt(long t) => (initialPosition + t) % positions;

    public long AdjustedPositionAt(long t) => PositionAt(t + discNumber);
}