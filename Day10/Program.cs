using static Day00.ReadInputs;

(int Register, List<int> Cycles) start = (1, new List<int>());

var computer = Read()
    .Aggregate(start, (computer, instruction) =>
    {
        ArgumentNullException.ThrowIfNull(instruction);

        computer.Cycles.Add(computer.Register);
        if (instruction[0] == 'a')
        {
            computer.Cycles.Add(computer.Register);
            computer.Register += int.Parse(instruction.Split(" ")[1]);
        }
        return computer;
    });

foreach (var scanline in computer.Cycles.Chunk(40))
{
    foreach (var pixel in scanline.Select((register, index) => (register, index)))
    {
        bool on = 
            pixel.register - 1 <= pixel.index && 
            pixel.index <= pixel.register + 1;

        Console.Write(on ? "█" : " ");
    }

    Console.WriteLine();
}
