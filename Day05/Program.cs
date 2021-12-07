using System.Security.Cryptography;
using System.Text;

const string inputFile = @"../../../../input05.txt";

Console.WriteLine("Day 05 - How About a Nice Game of Chess?");
Console.WriteLine("Star 1");
Console.WriteLine();

string input = File.ReadAllText(inputFile);

long index = 0;

string password = "";
using MD5 md5Hash = MD5.Create();

    while (password.Length < 8)
{
    byte[] hashData = md5Hash.ComputeHash(Encoding.UTF8.GetBytes($"{input}{index++}"));

    if (hashData[0] == 0 && hashData[1] == 0 && hashData[2] < 0x10)
    {
        password += hashData[2].ToString("X2")[1];
    }
}

Console.WriteLine($"The password is: {password.ToLowerInvariant()}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

index = 0;
char[] password2 = new char[8];
HashSet<int> filledSlots = new HashSet<int>();

do
{
    byte[] hashData = md5Hash.ComputeHash(Encoding.UTF8.GetBytes($"{input}{index++}"));

    if (hashData[0] == 0 && hashData[1] == 0 && hashData[2] < 0x10)
    {
        int position = hashData[2] % 0x10;
        if (position < 8 && filledSlots.Add(position))
        {
            char character = hashData[3].ToString("X2")[0];
            password2[position] = character;
            filledSlots.Add(position);
        }
    }
}
while (filledSlots.Count < 8);

Console.WriteLine($"The password is: {new string(password2).ToLowerInvariant()}");

Console.WriteLine();
Console.ReadKey();
