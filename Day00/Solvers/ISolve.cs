namespace Day00
{
    public interface ISolve
    {   
        ISolve Solve();
    }

    public interface ILoadInput
    {
        ISolve Load(string? line);
    }

    public static class ISolverExtensions
    {
        public static TSolver SolveWith<TSolver>(this IEnumerable<string?> source, TSolver? solveWith = default)
            where TSolver : ISolve, ILoadInput, new()
                => SolveWith(source, (solver, row) => (TSolver)solver.Load(row), solveWith);

        public static TSolver SolveWith<TSolver>(this IEnumerable<string?> source, Func<TSolver, string?, TSolver> prepareWith, TSolver? solveWith = default) 
            where TSolver : ISolve, new()
        {
            solveWith ??= new TSolver();
            foreach (var row in source)
            {
                solveWith = prepareWith(solveWith, row);
            }

            solveWith.Solve();
            return solveWith;
        }
    }
}
