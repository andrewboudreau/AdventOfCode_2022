using Day00;
using Day00.Solvers;

using static Day00.ReadInputs;

var treefs = new TreeFs();
Read()
    .SolveWith(solveWith: treefs)
    .ToConsole("\r\nDay 07 Solution");

public class TreeFs : Solution
{
    private int partOne;
    private (int Size, string Name) partTwo = (int.MaxValue, string.Empty);

    public TreeFs()
    {
        RootFolder = new Folder("/", default);
        CurrentFolder = RootFolder;
        Folders = new HashSet<Folder>() { RootFolder };
    }

    public Folder CurrentFolder { get; private set; }
    
    public Folder RootFolder { get; private set; }
    
    public HashSet<Folder> Folders { get; private set; }

    private void NavigateToRootFolder()
        => CurrentFolder = RootFolder;

    private void NavigateToSubFolder(string name)
        => CurrentFolder = CurrentFolder.Folders.FirstOrDefault(x => x.Name == name)
            ?? throw new InvalidOperationException($"'{CurrentFolder.Name}' doesn't have child folder '{name}'.");

    private void NavigateToParentFolder()
        => CurrentFolder = CurrentFolder.Parent
            ?? throw new InvalidOperationException($"{CurrentFolder.Name} doesn't have a parent folder.");

    public override ISolve Load(string? data)
    {
        if (data is null)
        {
            throw new InvalidOperationException("null value in load");
        }

        if (data == "$ ls")
        {
            return this;
        }

        if (data.StartsWith("$ cd"))
        {
            var name = data.Split(' ')[2];
            switch (name)
            {
                case "/":
                    NavigateToRootFolder();
                    //Console.WriteLine("Navigate to Root Folder");
                    return this;

                case "..":
                    NavigateToParentFolder();
                    //Console.WriteLine("Navigate to Parent Folder");
                    return this;

                default:
                    NavigateToSubFolder(name);
                    //Console.WriteLine($"Navigate to Folder '{name}'");
                    return this;
            }
        }

        (string Parameter, string Name) = data.Split(' ');
        if (Parameter == "dir")
        {
            var newFolder = CurrentFolder.AddFolder(Name);
            Folders.Add(newFolder);
            //Console.WriteLine($"Add folder '{Name}' to '{CurrentFolder.Name}'.");
            return this;
        }
        else if (int.TryParse(Parameter, out var size))
        {
            CurrentFolder.AddFile(size, Name);
            //Console.WriteLine($"Add File '{Name}' to '{CurrentFolder.Name}'.");
            return this;
        }

        throw new InvalidOperationException($"Cound not parse row '{data}'.");
    }

    public override ISolve Solve()
    {
        var threshold = 100_000;
        var updateSize = 30_000_000;
        var maxCapcity = 70_000_000;
        var used = RootFolder.Size();
        var free = maxCapcity - used;
        var requiredSpace = updateSize - free;

        Console.WriteLine($"using {used:#,000} of {maxCapcity:#,000}, there is {free:#,000} free");
        Console.WriteLine($"requires {requiredSpace:#,000} more space for update to run");

        foreach (var folder in Folders)
        {
            var size = folder.Size();
            if (size <= threshold)
            {
                partOne += size;
            }

            if (size >= requiredSpace)
            {
                if (size < partTwo.Size)
                {
                    partTwo = (size, folder.Name);
                }
            }
        }

        return this;
    }

    public override string ToString()
    {
        return $"Part one sum is {partOne}.\r\nPart two should delete {partTwo.Name} for {partTwo.Size} space.";
    }

    public record File(int Size, string Name);

    public class Folder
    {
        public Folder(string name, Folder? parent)
        {
            Name = name;
            Files = new();
            Folders = new();
            Parent = parent;
        }

        public string Name { get; private set; }

        public Folder? Parent { get; private set; }

        public List<File> Files { get; private set; }

        public List<Folder> Folders { get; private set; }

        public Folder AddFolder(string name)
        {
            var folder = new Folder(name, this);
            Folders.Add(folder);
            return folder;
        }

        public void AddFile(int size, string name)
            => Files.Add(new File(size, name));

        public int Size()
            => Files.Sum(x => x.Size) + Folders.Sum(x => x.Size());

        public void List(int depth = 0)
        {
            static string Pad(int depth) => "".PadLeft(depth * 2, ' ');

            foreach (var file in Files)
            {
                Console.WriteLine($"{Pad(depth)} - {file.Size} {file.Name}");
            }

            foreach (var dir in Folders)
            {
                Console.WriteLine($"{Pad(depth)} - dir {dir.Name}");
                dir.List(++depth);
            }
        }
    }
}