using System.Collections.Generic;
using System.Linq;
using DbContext;
using IDbContext.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbContextTestProject
{
    [TestClass]
    public class TestFileContextUnitTest1
    {
        [TestMethod]
        public void UploadSubdivisionsFromFileAsyncTestMethod1()
        {
            var dbf = new TestFileContext();
            var rslt = dbf.UploadSubdivisionsFromFileAsync(@"c:\1\1.json").Result;
            Assert.IsTrue(rslt!=null);
        }

        [TestMethod]
        public void SaveSubDivisionsIntoFileAsyncTestMethod1()
        {

            var dbc = new TestDbContext();
            var res = dbc.GetSubdivisionsAsync().Result;

            var dbf = new TestFileContext();
            var rslt=dbf.SaveSubDivisionsIntoFileAsync(res.ToList(), @"c:\1\1.json").Result;
            Assert.IsTrue(rslt==0);
        }
    }
}
