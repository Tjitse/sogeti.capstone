﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using Sogeti.Capstone.Data.Model;

namespace Sogeti.Capstone.Data.IntegrationTests
{
    [TestFixture]
    public class StatusDataIntegrationTests
    {
        private static readonly CapstoneContext Context =
            new CapstoneContext("Sogeti.Capstone.Data.Model.CapstoneContext");

        private Status _sampleStatus;

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
            _sampleStatus = new Status()
            {
                Title = "Sample Title"
            };
        }

        #endregion

        #region Tests

        [Test]
        public void Add_Status_With_Defaults()
        {
            //arrange
            var newStatus = new Status();

            //act
            Context.Statuses.Add(newStatus);
            Context.SaveChanges();

            //assert
            int rowCount = Context.Statuses.Count();
            rowCount.ShouldBeGreaterThan(0);
        }

        [Test]
        public void Should_Populate_ID()
        {
            // arrange
            var newStatus = new Status();

            // act
            Context.Statuses.Add(newStatus);
            Context.SaveChanges();

            // assert
            Context.Statuses.First().Id.ShouldBe(1);
        }

        [Test]
        public void Should_Not_Allow_Duplicate_ID()
        {
            // arrange
            Context.Statuses.Add(_sampleStatus);
            Context.SaveChanges();

            Status duplicateStatus = new Status
            {
                Id = 1,
                Title = "Unique Status"
            };

            // act
            Context.Statuses.Add(duplicateStatus);
            Context.SaveChanges();
            Status correctedDuplicateStatus = Context.Statuses.First(e => e.Title == "Unique Status");

            // assert
            correctedDuplicateStatus.Id.ShouldNotBe(1);
        }

        [Test]
        public void Delete_Status()
        {
            //arrange
            var newStatus = new Status();

            //act
            Context.Statuses.Add(newStatus);
            Context.SaveChanges();

            Context.Statuses.Remove(newStatus);
            Context.SaveChanges();

            //assert
            int rowCount = Context.Statuses.Count();
            rowCount.ShouldBe(0);
        }

        [Test]
        public void Update_Status_From_Defaults()
        {
            // arrange
            var newStatus = new Status()
            {
                Title = "Sample Status"
            };
            Context.Statuses.Add(newStatus);
            Context.SaveChanges();

            Status oldStatus = Context.Statuses.First(e => e.Title == "Sample Status");
            oldStatus.Title = "New Status";

            var oldId = oldStatus.Id;

            //act
            Context.Entry(oldStatus).State = EntityState.Modified;
            Context.SaveChanges();

            //assert
            Status modifiedStatus = Context.Statuses.Find(oldId);
            modifiedStatus.Title.ShouldBe("New Status");
        }

        [Test]
        public void Add_Status_With_Wrong_Input()
        {
            //assert
            Should.Throw<SqlException>(
                () =>
                {
                    Context.Database.ExecuteSqlCommand(
                    "INSERT INTO dbo.Status (Title) VALUES (#04/29/2045#)");
                });

        }

        #endregion
    }
}