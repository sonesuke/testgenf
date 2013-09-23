using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testgen
{
    using FactorName = List<string>;
    using Factors = List<List<string>>;

    public class Variable
    {
        public Variable()
        {
            FactorName = new FactorName();
            Factors = new Factors();
        }
        public FactorName FactorName { get; set; }
        public Factors Factors { get; set; }
    }
}
