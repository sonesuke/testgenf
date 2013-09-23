using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testgen;
using Microsoft.FSharp.Collections;

namespace Testgen
{
    using Declarations = Dictionary<string, List<string>>;


      
    public class Evaluator
    {
         public TestCase Evaluate(string src)
        {
            var ast = Parser.ParseText(src);
            return Execute(ast.ToList());
        }

        private TestCase Execute(List<Parser.Exp> ast)
        {
            Declarations declaration = new Declarations();
            Variable result = null;
            foreach (var exp in ast)
            {
                result = Parse(exp, declaration);
            }
            return new TestCase(result);
        }

        private Variable Parse(Parser.Exp exp, Declarations declaration)
        {
            var idExp = exp as Testgen.Parser.Exp.IDVal;
            if (idExp != null)
            {
                return Parse(idExp, declaration);
            }

            var declExp = exp as Testgen.Parser.Exp.Decl;
            if (declExp != null)
            {
                return Parse(declExp, declaration);
            }

            var opExp = exp as Testgen.Parser.Exp.Op;
            if (opExp != null)
            {
                return Parse(opExp, declaration);
            }
            return null;
        }

        private Variable Parse(Parser.Exp.Decl declExp, Declarations declaration)
        {
            string factorName = declExp.Item1;
            List<string> factors = declExp.Item2.ToList();
            declaration[factorName] = factors;
            return null;
        }

        private Variable Parse(Parser.Exp.IDVal idExp, Declarations declaration)
        {
            string factorName = idExp.Item;
            Variable v = new Variable();
            v.FactorName.Add(factorName);

            var fs = declaration[factorName];
            foreach (var f in fs)
            {
                var fac = new List<string>();
                fac.Add(f);
                v.Factors.Add(fac);
            }
            return v;
        }


        private Variable Parse(Parser.Exp.Op opExp, Declarations declaration)
        {
            string op = opExp.Item1;
            Variable v1 = Parse(opExp.Item2, declaration);
            Variable v2 = Parse(opExp.Item3, declaration);
            switch (op)
            {
                case "+":
                    return ExecutePlus(v1, v2);
                case "*":
                    return ExecuteAll(v1, v2);
                case "<-":
                    return ExecuteMelt(v1, v2);
                default:
                    System.Diagnostics.Debug.Assert(false);
                    return null;
            }
        }

        private Variable ExecuteMelt(Variable v1, Variable v2)
        {
            Variable v = new Variable();
            v.FactorName = v1.FactorName.Concat(v2.FactorName).ToList();

            int v1max = v1.Factors.Count();
            int v2max = v2.Factors.Count();
            int vmax = v1max;

            for (int i = 0; i < vmax; ++i)
            {
                var v1l = v1.Factors[i % v1max];
                var v2l = v2.Factors[i % v2max];
                var vl = v1l.Concat(v2l).ToList();
                v.Factors.Add(vl);
            }

            return v;
        }

        private Variable ExecuteAll(Variable v1, Variable v2)
        {
            Variable v = new Variable();
            v.FactorName = v1.FactorName.Concat(v2.FactorName).ToList();

            int v1max = v1.Factors.Count();
            int v2max = v2.Factors.Count();

            for (int i = 0; i < v1max; ++i)
            {
                var v1l = v1.Factors[i];
                for (var j = 0; j < v2max; ++j)
                {
                    var v2l = v2.Factors[j];
                    var vl = v1l.Concat(v2l).ToList();
                    v.Factors.Add(vl);
                }
            }

            return v;
        }

        private Variable ExecutePlus(Variable v1, Variable v2)
        {
            Variable v = new Variable();
            v.FactorName = v1.FactorName.Concat(v2.FactorName).ToList();

            int v1max = v1.Factors.Count();
            int v2max = v2.Factors.Count();
            int vmax = Math.Max(v1max, v2max);

            for (int i = 0; i < vmax; ++i)
            {
                var v1l = v1.Factors[i % v1max];
                var v2l = v2.Factors[i % v2max];
                var vl = v1l.Concat(v2l).ToList();
                v.Factors.Add(vl);
            }

            return v;
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Testgen;

//namespace Eval
//{
//    class Program
//    {
//        static Dictionary<string, int> symbolTable = new Dictionary<string, int>();

//        static void Main(string[] args)
//        {
//            List<Parser.Exp> e = Testgen.Parser.ParseText(
//                "d1 + d2 + d3"
//                ).ToList();
//            foreach (var a in e)
//            {
//                int i = Parse(a);

//            }

//            System.Console.ReadLine();

//        }

//        static int Parse(Testgen.Parser.Exp val)
//        {
//            var idval = val as Testgen.Parser.Exp.IDVal;
//            if (idval != null)
//            {
//                return Parse(idval);
//            }
//            var opval = val as Testgen.Parser.Exp.Op;
//            if (opval != null)
//            {
//                return Parse(opval);
//            }
//            return 12;
//        }

//        static int Parse(Testgen.Parser.Exp.IDVal val)
//        {
//            return symbolTable[val.Item];
//        }

//        static int Parse(Testgen.Parser.Exp.Op val)
//        {
//            switch (val.Item1)
//            {
//                case ":":
//                case "+":
//                    return EvaluatePlus(val);
//                default:
//                    return 0;
//            }
//        }

//        static int EvaluatePlus(Testgen.Parser.Exp.Op val)
//        {
//            return Parse(val.Item2) + Parse(val.Item3);
//            //return Parse(val.Item2) + Parse(val.Item3);

//        }
//    }
//}
