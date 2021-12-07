using AoCTools;

const string inputFile = @"../../../../input24.txt";

Console.WriteLine("Day 24 - Air Duct Spelunking");
Console.WriteLine("Star 1");
Console.WriteLine();


//There are 7 points of interest.
//Can be Visited in any order
//Possible orderings: 7!
//5040 possible orderings.

//It is feasible to just brute-force test all orderings.
string[] lines = File.ReadAllLines(inputFile);

Point2D bounds = (lines[0].Length, lines.Length);

bool[,] grid = new bool[bounds.x, bounds.y];
int[,] distance = new int[bounds.x, bounds.y];

Point2D start = (0, 0);

List<Point2D> pointsOfInterest = new List<Point2D>();

for (int y = 0; y < bounds.y; y++)
{
    for (int x = 0; x < bounds.x; x++)
    {
        char c = lines[y][x];
        if (c == '.')
        {
            grid[x, y] = true;
        }
        else if (c == '#')
        {
            grid[x, y] = false;
        }
        else if (c == '0')
        {
            grid[x, y] = true;
            start = (x, y);
        }
        else
        {
            grid[x, y] = true;
            pointsOfInterest.Add((x, y));
        }
    }
}

pointsOfInterest.Insert(0, start);

//If the optimal path from point A to point B crosses point C, I don't need to bother
//including that permutation.

//Start by calculating the distance between each pair of points of interest

Dictionary<Point2D, int> distanceMap = new Dictionary<Point2D, int>();

for (int a = 0; a < pointsOfInterest.Count - 1; a++)
{
    for (int b = a + 1; b < pointsOfInterest.Count; b++)
    {
        int steps = CalculateSteps(pointsOfInterest[a], pointsOfInterest[b], grid, distance);
        distanceMap.Add((a, b), steps);
        distanceMap.Add((b, a), steps);
    }
}

List<int> destinationList = new List<int>();

int shortestPath = ShortestPath(0, Enumerable.Range(1, pointsOfInterest.Count - 1), distanceMap);

Console.WriteLine($"Shortest Path: {shortestPath}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//What is the shortest path to visit every marked space on the map and return to the origin?

int shortestPath2 = ShortestPath2(0, Enumerable.Range(1, pointsOfInterest.Count - 1), distanceMap);

Console.WriteLine($"Shortest Path2: {shortestPath2}");


Console.WriteLine();
Console.ReadKey();


static int ShortestPath(
    int start,
    IEnumerable<int> remaining,
    Dictionary<Point2D, int> distanceMap)
{
    int remainingCount = remaining.Count();
    if (remainingCount == 1)
    {
        return distanceMap[(start, remaining.First())];
    }

    int min = int.MaxValue;
    for (int i = 0; i < remainingCount; i++)
    {
        int value = remaining.ElementAt(i);
        int newPath = distanceMap[(start, value)] + ShortestPath(
            start: remaining.ElementAt(i),
            remaining: remaining.Where((x, j) => j != i),
            distanceMap: distanceMap);
        min = Math.Min(min, newPath);
    }

    return min;
}

static int ShortestPath2(
    int start,
    IEnumerable<int> remaining,
    Dictionary<Point2D, int> distanceMap)
{
    int remainingCount = remaining.Count();
    if (remainingCount == 1)
    {
        int final = remaining.First();
        return distanceMap[(start, final)] + distanceMap[(final, 0)];
    }

    int min = int.MaxValue;
    for (int i = 0; i < remainingCount; i++)
    {
        int value = remaining.ElementAt(i);
        int newPath = distanceMap[(start, value)] + ShortestPath2(
            start: remaining.ElementAt(i),
            remaining: remaining.Where((x, j) => j != i),
            distanceMap: distanceMap);
        min = Math.Min(min, newPath);
    }

    return min;
}


int CalculateSteps(
    in Point2D start,
    in Point2D end,
    bool[,] grid,
    int[,] distance)
{
    //Reset
    for (int y = 0; y < bounds.y; y++)
    {
        for (int x = 0; x < bounds.x; x++)
        {
            distance[x, y] = int.MaxValue;
        }
    }

    Point2D tempEnd = end;

    PriorityQueue<Point2D> pendingPoints = new PriorityQueue<Point2D>((Point2D point) => (tempEnd - point).TaxiCabLength);
    pendingPoints.Enqueue(start);
    distance[start.x, start.y] = 0;

    while (pendingPoints.Count > 0)
    {
        Point2D nextPoint = pendingPoints.Dequeue();
        int steps = distance[nextPoint.x, nextPoint.y] + 1;

        if (steps > distance[end.x, end.y])
        {
            continue;
        }


        foreach (Point2D adj in GetAdjacentBoundedPoints(nextPoint))
        {
            if (grid[adj.x, adj.y] && distance[adj.x, adj.y] > steps)
            {
                distance[adj.x, adj.y] = steps;
                pendingPoints.EnqueueOrUpdate(adj);
            }
        }
    }

    return distance[end.x, end.y];
}


IEnumerable<Point2D> GetAdjacentBoundedPoints(Point2D point)
{
    if (point.x > 0)
    {
        yield return point - Point2D.XAxis;
    }

    if (point.x < bounds.x - 1)
    {
        yield return point + Point2D.XAxis;
    }

    if (point.y > 0)
    {
        yield return point - Point2D.YAxis;
    }

    if (point.y < bounds.y - 1)
    {
        yield return point + Point2D.YAxis;
    }
}