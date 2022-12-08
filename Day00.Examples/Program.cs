using Day00;

Console.WriteLine("Hello, Examples");

Console.WriteLine("Casting Arrays of strings to Tuples");
(string a, string b, string c, string d) = new[] { "1", "2", "4", "5" };
Console.WriteLine($"a={a} b={b} c={c} d={d}");

var row = "1 ? 4 5";
(string e, string f, string g, string h) = row.Split(" ");
Console.WriteLine($"e={e} f={f} g={g} h={h}");

Console.WriteLine("Nodes");


