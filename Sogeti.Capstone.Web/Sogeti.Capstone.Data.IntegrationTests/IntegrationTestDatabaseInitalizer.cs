﻿using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using Sogeti.Capstone.Data.Model;
using Sogeti.Capstone.Data.Model.Migrations;

namespace Sogeti.Capstone.Data.IntegrationTests
{
    public static class IntegrationTestDatabaseInitalizer
    {
        public static void AssemblyInit(CapstoneContext context)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ""));
            Database.SetInitializer(new DropCreateDatabaseAlways<CapstoneContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CapstoneContext, Configuration>());

            Console.WriteLine("Assembly setup: creating LocalDb database");
            Trace.WriteLine(String.Format("Location of localdb : {0}", AppDomain.CurrentDomain.GetData("DataDirectory")));

            context.Database.Delete();
            context.Database.Initialize(true);
        }
    }
}