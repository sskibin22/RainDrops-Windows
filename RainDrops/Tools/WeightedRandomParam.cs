using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Tools
{
    internal class WeightedRandomParam
    {
        public Action Func { get; }
        public double Ratio { get; }

        public WeightedRandomParam(Action func, double ratio)
        {
            Func = func;
            Ratio = ratio;
        }
    }
}
