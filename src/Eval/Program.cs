using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testgen;

namespace Eval
{
    class Program
    {
        static Dictionary<string, int> symbolTable = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            List<Parser.Exp> e = Testgen.Parser.testParse(
                "d1 + d2 + d3"
                ).ToList();
            foreach (var a in e)
            {
                int i = Parse(a);

            }

            System.Console.ReadLine();

        }

        static int Parse(Testgen.Parser.Exp val)
        {
            var idval = val as Testgen.Parser.Exp.IDVal;
            if (idval != null)
            {
                return Parse(idval);
            }
            var opval = val as Testgen.Parser.Exp.Op;
            if (opval != null)
            {
                return Parse(opval);
            }
            return 12;
        }

        static int Parse(Testgen.Parser.Exp.IDVal val)
        {
            return symbolTable[val.Item];
        }

        static int Parse(Testgen.Parser.Exp.Op val)
        {
            switch (val.Item1)
            {
                case ":":
                case "+":
                    return EvaluatePlus(val);
                default:
                    return 0;
            }
        }

        static int EvaluatePlus(Testgen.Parser.Exp.Op val)
        {
            return Parse(val.Item2) + Parse(val.Item3);
            //return Parse(val.Item2) + Parse(val.Item3);

        }
    }
}
