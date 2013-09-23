using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testgen;

namespace TestTestgen
{
    [TestClass]
    public class TestTestgen
    {
        [TestMethod]
        public void TestCreate()
        {
            Evaluator e = new Evaluator();
        }

        [TestMethod]
        public void TestEvaluate()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; A;");
        }

        [TestMethod]
        public void TestTestCaseA()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; A;");
            Assert.AreEqual(c.Count, 3);
            Assert.AreEqual(c[0]["A"], "a1");
            Assert.AreEqual(c[1]["A"], "a2");
            Assert.AreEqual(c[2]["A"], "a3");
        }

        [TestMethod]
        public void TestTestCaseB()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; B: b1, b2; B;");
            Assert.AreEqual(c.Count, 2);
            Assert.AreEqual(c[0]["B"], "b1");
            Assert.AreEqual(c[1]["B"], "b2");
        }

        [TestMethod]
        public void TestTestCaseAPlusB()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; B: b1, b2; A+B;");
            Assert.AreEqual(c.Count, 3);
            Assert.AreEqual(c[0]["A"], "a1");
            Assert.AreEqual(c[0]["B"], "b1");
            Assert.AreEqual(c[1]["A"], "a2");
            Assert.AreEqual(c[1]["B"], "b2");
            Assert.AreEqual(c[2]["A"], "a3");
            Assert.AreEqual(c[2]["B"], "b1");
        }

        [TestMethod]
        public void TestTestCaseBPlusA()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; B: b1, b2; B+A;");
            Assert.AreEqual(c.Count, 3);
            Assert.AreEqual(c[0]["A"], "a1");
            Assert.AreEqual(c[0]["B"], "b1");
            Assert.AreEqual(c[1]["A"], "a2");
            Assert.AreEqual(c[1]["B"], "b2");
            Assert.AreEqual(c[2]["A"], "a3");
            Assert.AreEqual(c[2]["B"], "b1");
        }

        [TestMethod]
        public void TestTestCaseAAllB()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; B: b1, b2; A*B;");
            Assert.AreEqual(c.Count, 6);
            Assert.AreEqual(c[0]["A"], "a1");
            Assert.AreEqual(c[0]["B"], "b1");
            Assert.AreEqual(c[1]["A"], "a1");
            Assert.AreEqual(c[1]["B"], "b2");
            Assert.AreEqual(c[2]["A"], "a2");
            Assert.AreEqual(c[2]["B"], "b1");
            Assert.AreEqual(c[3]["A"], "a2");
            Assert.AreEqual(c[3]["B"], "b2");
            Assert.AreEqual(c[4]["A"], "a3");
            Assert.AreEqual(c[4]["B"], "b1");
            Assert.AreEqual(c[5]["A"], "a3");
            Assert.AreEqual(c[5]["B"], "b2");
        }

        [TestMethod]
        public void TestTestCaseAMeltB()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; B: b1, b2; A<-B;");
            Assert.AreEqual(c.Count, 3);
            Assert.AreEqual(c[0]["A"], "a1");
            Assert.AreEqual(c[0]["B"], "b1");
            Assert.AreEqual(c[1]["A"], "a2");
            Assert.AreEqual(c[1]["B"], "b2");
            Assert.AreEqual(c[2]["A"], "a3");
            Assert.AreEqual(c[2]["B"], "b1");
        }

        [TestMethod]
        public void TestTestCaseBMeltA()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; B: b1, b2; B<-A;");
            Assert.AreEqual(c.Count, 2);
            Assert.AreEqual(c[0]["A"], "a1");
            Assert.AreEqual(c[0]["B"], "b1");
            Assert.AreEqual(c[1]["A"], "a2");
            Assert.AreEqual(c[1]["B"], "b2");
        }

        [TestMethod]
        public void TestTestCaseMultiTerm()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; B: b1, b2; C: c1, c2; C*B+A;");
            Assert.AreEqual(c.Count, 4);
            Assert.AreEqual(c[0]["A"], "a1");
            Assert.AreEqual(c[0]["B"], "b1");
            Assert.AreEqual(c[0]["C"], "c1");

            Assert.AreEqual(c[1]["A"], "a2");
            Assert.AreEqual(c[1]["B"], "b2");
            Assert.AreEqual(c[1]["C"], "c1");

            Assert.AreEqual(c[2]["A"], "a3");
            Assert.AreEqual(c[2]["B"], "b1");
            Assert.AreEqual(c[2]["C"], "c2");

            Assert.AreEqual(c[3]["A"], "a1");
            Assert.AreEqual(c[3]["B"], "b2");
            Assert.AreEqual(c[3]["C"], "c2");
        }

        [TestMethod]
        public void TestTestCaseMultiTermPa()
        {
            Evaluator e = new Evaluator();
            TestCase c = e.Evaluate("A: a1, a2, a3; B: b1, b2; C: c1, c2; C*(B<-A);");
            Assert.AreEqual(c.Count, 4);
            Assert.AreEqual(c[0]["A"], "a1");
            Assert.AreEqual(c[0]["B"], "b1");
            Assert.AreEqual(c[0]["C"], "c1");

            Assert.AreEqual(c[1]["A"], "a2");
            Assert.AreEqual(c[1]["B"], "b2");
            Assert.AreEqual(c[1]["C"], "c1");

            Assert.AreEqual(c[2]["A"], "a1");
            Assert.AreEqual(c[2]["B"], "b1");
            Assert.AreEqual(c[2]["C"], "c2");

            Assert.AreEqual(c[3]["A"], "a2");
            Assert.AreEqual(c[3]["B"], "b2");
            Assert.AreEqual(c[3]["C"], "c2");
        }
    }

}
