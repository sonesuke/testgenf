using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testgen
{
    public class TestCase
    {
        private Variable result;

        public TestCase(Variable result)
        {
            // TODO: Complete member initialization
            this.result = result;
        }
        public int Count { 
            get
            {
                return result.Factors.Count();
            }
        }

        public Dictionary<string, string> this[int i]
        {
            get
            {
                var d = new Dictionary<string,string>();
                int j = 0;
                foreach (var n in result.FactorName)
                {
                    d[n] = result.Factors[i][j];
                    j++;
                }

                return d;
            }
        }
    }
}
