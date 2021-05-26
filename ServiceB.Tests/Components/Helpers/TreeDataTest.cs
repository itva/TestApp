using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceB.Components.Helpers;

namespace ServiceB.Tests
{
    [TestClass]
    public class TreeDataTest
    {
        [TestMethod]
        public void TreeDataTest1()
        {
            var res=new TreeData();
            Assert.IsTrue(!res.RawData.Any());
        }

        [TestMethod]
        public void TreeDataTest2()
        {
            var res = new TreeData(new List<TreeRow>()
            {
                new TreeRow() {Id = 1, ParentId = null, Text = "item 1"},
                new TreeRow() {Id = 2, ParentId = null, Text = "item 2"},
                new TreeRow() {Id = 11, ParentId = 1, Text = "item 1-1"},
                new TreeRow() {Id = 111, ParentId = 11, Text = "item 1-1-1"},
                new TreeRow() {Id = 12, ParentId = 1, Text = "item 1-2"}
            });
            Assert.IsTrue(res.Roots.Any());
        }

        [TestMethod]
        public void NonUniqueIdsTest()
        {
            try
            {
                var res = new TreeData(new List<TreeRow>()
                {
                    new TreeRow() {Id = 1, ParentId = null, Text = "item 1"},
                    new TreeRow() {Id = 2, ParentId = null, Text = "item 2"},
                    new TreeRow() {Id = 2, ParentId = 1, Text = "item 1-1"}
                });
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.StartsWith("Неуникальные идентификаторы записи Id"));
            }
            
        }

        [TestMethod]
        public void OrphanRowTest()
        {
            try
            {
                var res = new TreeData(new List<TreeRow>()
                {
                    new TreeRow() {Id = 1, ParentId = null, Text = "item 1"},
                    new TreeRow() {Id = 2, ParentId = null, Text = "item 2"},
                    new TreeRow() {Id = 3, ParentId = 4, Text = "item 1-1"}
                });
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.StartsWith("Отсутствует родительская запись для ParentId"));
            }

        }

        [TestMethod]
        public void ToHtmlTest()
        {
            var data = new TreeData(new List<TreeRow>()
            {
                new TreeRow() {Id = 1, ParentId = null, Text = "item 1"},
                new TreeRow() {Id = 2, ParentId = null, Text = "item 2"},
                new TreeRow() {Id = 11, ParentId = 1, Text = "item 1-1"},
                new TreeRow() {Id = 111, ParentId = 11, Text = "item 1-1-1"},
                new TreeRow() {Id = 12, ParentId = 1, Text = "item 1-2"}
            });
            var res = "<qwe>"+data.ToHtml("tree")+"</qwe>";
            //проверяем на валидную структуру тегов
            try
            {
                var doc = XDocument.Parse(res);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }
    }
}
