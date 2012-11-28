using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShakrLabs.Choice.Web.Models;

namespace ShakrLabs.Choice.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetCategoryList()
        {
            List<Category> items = new List<Category> ( Category.GetAll());

            Assert.IsTrue(items.Count > 0);
        }
    }
}
