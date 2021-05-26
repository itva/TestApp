using System.Collections.Generic;
using System.Linq;
using DbContext;
using IDbContext.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbContextTestProject
{
    [TestClass]
    public class TestDbContextUnitTest1
    {
        [TestMethod]
        public void GetSubdivisionsAsyncTestMethod1()
        {
            var dbc = new TestDbContext();
            var res= dbc.GetSubdivisionsAsync().Result;
            Assert.IsTrue(res!=null);
        }

        [TestMethod]
        public void SaveSubDivisionsInDbAsyncTestMethod1()
        {
            var dbc = new TestDbContext();
            var rslt=dbc.SaveSubDivisionsInDbAsync(new List<Subdivision>()
            {
                new Subdivision() {Id = 1, ParentId = null, Name = "������������� 1"},
                new Subdivision() {Id = 2, ParentId = null, Name = "������������� 2"},
                new Subdivision() {Id = 11, ParentId = 1, Name = "������������� 11"},
                new Subdivision() {Id = 12, ParentId = 1, Name = "������������� 12"},
                new Subdivision() {Id = 21, ParentId = 2, Name = "������������� 21"},
                new Subdivision() {Id = 211, ParentId = 21, Name = "������������� 211"},
                new Subdivision() {Id = 212, ParentId = 21, Name = "������������� 212"},
                new Subdivision() {Id = 2111, ParentId = 211, Name = "������������� 2111"}
            }).Result;
            Assert.IsTrue(rslt==0);
        }
    }
}
