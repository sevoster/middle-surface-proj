using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MidSurfaceNameSpace.IO;
using MidSurfaceNameSpace.Primitive;
using System.Diagnostics;

namespace MidSurfaceNameSpace.UnitTests
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void TestImportMethodCorrect()
        {
            FigureParser parser = new FigureParser();
            string pathToTestFile = Environment.CurrentDirectory + @"\test1.xml";

            //Try to parse test xml
            IFigure figure = parser.ParseFile(pathToTestFile);

            //Check that we got figure
            Assert.AreNotEqual(null, figure);
        }
    }
}
