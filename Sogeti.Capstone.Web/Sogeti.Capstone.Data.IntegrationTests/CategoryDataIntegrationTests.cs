﻿using System.Data.Entity;
using System.Linq;
using System.Data.SqlClient;
using NUnit.Framework;
using Shouldly;
using Sogeti.Capstone.Data.Model;

namespace Sogeti.Capstone.Data.IntegrationTests
{
    [TestFixture]
    public class CategoryDataIntegrationTests
    {
        private static readonly CapstoneContext Context =
            new CapstoneContext("Sogeti.Capstone.Data.Model.CapstoneContext");

        #region SetUp

        [TestFixtureSetUp]
        public void Init()
        {
            IntegrationTestDatabaseInitalizer.AssemblyInit(Context);
        }

        [SetUp]
        public void TestInit()
        {
            Context.RemoveAllDbSetDataDatabase();
        }

        #endregion

        #region Tests

        [Test]
        public void Add_Category_With_Defaults()
        {
            //arrange
            var newCategory = new Category();

            //act
            Context.Category.Add(newCategory);
            Context.SaveChanges();

            //assert
            int rowCount = Context.Category.Count();
            rowCount.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Delete_Category()
        {
            //arrange
            var newCategory = new Category();

            //act
            Context.Category.Add(newCategory);
            Context.SaveChanges();

            Context.Category.Remove(newCategory);
            Context.SaveChanges();

            //assert
            int rowCount = Context.Category.Count();
            rowCount.ShouldBe(0);
        }

        [Test]
        public void Should_Populate_ID()
        {
            // arrange
            var newCategory = new Category();

            // act
            Context.Category.Add(newCategory);
            Context.SaveChanges();

            // assert
            Context.Category.First().Id.ShouldBe(1);
        }

        [Test]
        public void Add_Category_With_Wrong_Input()
        {
            //assert
            Should.Throw<SqlException>(
                () =>
                {
                    Context.Database.ExecuteSqlCommand(
                    "INSERT INTO dbo.Categories (Title) VALUES (#4/20/2020#)");
                });
        }

        #endregion
    }
}