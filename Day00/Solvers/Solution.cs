using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day00.Solvers
{
    public abstract class Solution : ISolve, ILoadInput
    {
        public abstract ISolve Load(string? data);

        public abstract ISolve Solve();

        ISolve ILoadInput.Load(string? data) => Load(data);

        ISolve ISolve.Solve() => Solve();
    }
}
