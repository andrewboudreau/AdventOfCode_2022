namespace Day00
{
    public interface ISolve
    {
        ISolve Load(string? data);

        ISolve Solve();
    }

    public static class ISolverExtensions
    {
        public static TSolver SolveWith<TSolver>(this IEnumerable<string?> source, TSolver? solver = default)
            where TSolver : ISolve, new()
        {
            solver ??= new TSolver();
            foreach (var row in source)
            {
                solver.Load(row);
            }

            solver.Solve();
            return solver;
        }
    }
}
