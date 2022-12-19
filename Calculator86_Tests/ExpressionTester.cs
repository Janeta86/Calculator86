


using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator86_Tests
{

    [TestClass]
    public class ExpressionTester
    {
        [TestMethod]
        public void Parser_TestSum_7()
        {
            var parser = new MathExpression("3+4");
            Assert.AreEqual(3+4, parser.Parse());
        }
        [TestMethod]
        public void Parser_TestOrder_6()
        {
            var parser = new MathExpression("2+2*2");
            Assert.AreEqual(2+2*2, parser.Parse());
        }
        [TestMethod]
        public void Parser_TestDiv_2dot5()
        {
            var parser = new MathExpression("5/2");
            Assert.AreEqual(5.0m/2.0m, parser.Parse());
        }

        [TestMethod]
        public void Parser_TestSum_Minus0d1()
        {
            var parser = new MathExpression("-5.3+5.2");
            Assert.AreEqual(-5.3m + 5.2m, parser.Parse());
        }
        [TestMethod]
        public void Parser_TestDifference_3()
        {
            var parser = new MathExpression("12---9");
            Assert.AreEqual(3, parser.Parse());
        }
        [TestMethod]
        public void Parser_TestMul()
        {
            var parser = new MathExpression("0.7*98");
            Assert.AreEqual(0.7m* 98m, parser.Parse());
        }

        [TestMethod]
        public void Parser_TestBig()
        {
            var parser = new MathExpression("2.6*-(93.9312+(0-1))*2*31/99");
            Assert.AreEqual(
                2.6m * -(93.9312m + (0m - 1m)) * 2m * 31m / 99m,
                parser.Parse()
                );

        }


    }
}