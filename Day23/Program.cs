using AoCTools;

const string inputFile = @"../../../../input23.txt";

Console.WriteLine("Day 23 - Safe Cracking");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);

Context context = new Context(lines.Select(ParseLine));
context.SetValue("a", 7);
context.Execute();

Console.WriteLine($"Value in reg A: {context.GetValue("a")}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

context = new Context(lines.Select(ParseLine));
context.SetValue("a", 12);
context.Execute();

Console.WriteLine($"Value in reg A: {context.GetValue("a")}");

Console.WriteLine();
Console.ReadKey();


static Instruction ParseLine(string line) => line[0] switch
{
    'c' => new CopyInstr(line),
    'i' or 'd' => new InplaceInstr(line),
    'j' => new JNZInstr(line),
    't' => new ToggleInstr(line),
    _ => throw new Exception(),
};

class Context
{
    private readonly Dictionary<string, int> registers = new Dictionary<string, int>();
    private readonly List<Instruction> instructions;
    private int instr = 0;

    public Context(IEnumerable<Instruction> instructions)
    {
        this.instructions = new List<Instruction>(instructions);
    }

    public void Execute()
    {
        while (instr >= 0 && instr < instructions.Count)
        {
            instr += instructions[instr].Execute(this);
        }
    }

    public void Toggle(int offset)
    {
        int toggledInstr = instr + offset;

        if (toggledInstr >= 0 && toggledInstr < instructions.Count)
        {
            instructions[toggledInstr] = instructions[toggledInstr].Toggle();
        }
    }

    public int GetValue(string register) => registers.GetValueOrDefault(register);
    public int SetValue(string register, int value) => registers[register] = value;
}

abstract class Reference
{
    public abstract int GetValue(Context context);

    public static Reference Parse(string segment)
    {
        if (segment.Length == 1 && char.IsLetter(segment[0]))
        {
            return new RegisterReference(segment);
        }

        return new ValueReference(int.Parse(segment));
    }
}

class RegisterReference : Reference
{
    public readonly string register;

    public RegisterReference(string register) => this.register = register;
    public override int GetValue(Context context) => context.GetValue(register);
}

class ValueReference : Reference
{
    public readonly int value;

    public ValueReference(int value) => this.value = value;
    public override int GetValue(Context context) => value;
}

abstract class Instruction
{
    public abstract Instruction Toggle();
    public abstract int Execute(Context context);
}

class CopyInstr : Instruction
{
    private readonly Reference value;
    private readonly Reference register;

    public CopyInstr(string line)
    {
        //Parse
        string[] splitLine = line.Split(' ');
        value = Reference.Parse(splitLine[1]);
        register = Reference.Parse(splitLine[2]);
    }

    public CopyInstr(Reference value, Reference register)
    {
        this.value = value;
        this.register = register;
    }

    public override Instruction Toggle() => new JNZInstr(value, register);

    public override int Execute(Context context)
    {
        if (register is RegisterReference regRef)
        {
            context.SetValue(regRef.register, value.GetValue(context));
        }

        return 1;
    }
}

class InplaceInstr : Instruction
{
    private readonly bool increment;
    private readonly string register;

    public InplaceInstr(string line)
    {
        //Parse
        increment = (line[0] == 'i');
        register = line.Substring(4);
    }

    public InplaceInstr(bool increment, string register)
    {
        this.increment = increment;
        this.register = register;
    }

    public override int Execute(Context context)
    {
        if (increment)
        {
            context.SetValue(register, context.GetValue(register) + 1);
        }
        else
        {
            context.SetValue(register, context.GetValue(register) - 1);
        }

        return 1;
    }

    public override Instruction Toggle() => new InplaceInstr(!increment, register);
}

class ToggleInstr : Instruction
{
    private readonly string register;

    public ToggleInstr(string line)
    {
        //Parse
        register = line.Substring(4);
    }

    public override int Execute(Context context)
    {
        context.Toggle(context.GetValue(register));
        return 1;
    }

    public override Instruction Toggle() => new InplaceInstr(register);
}

class JNZInstr : Instruction
{
    private readonly Reference value;
    private readonly Reference jump;

    public JNZInstr(string line)
    {
        //Parse
        string[] splitLine = line.Split(' ');
        value = Reference.Parse(splitLine[1]);
        jump = Reference.Parse(splitLine[2]);
    }

    public JNZInstr(Reference value, Reference jump)
    {
        this.value = value;
        this.jump = jump;
    }

    public override int Execute(Context context)
    {
        if (value.GetValue(context) != 0)
        {
            return jump.GetValue(context);
        }

        return 1;
    }

    public override Instruction Toggle() => new CopyInstr(value, jump);
}
