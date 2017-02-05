using System;
using System.Collections.Generic;
using System.Data.Hsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharpHSQL.UnitTests.ProviderTests
{
    [TestFixture]
    class SharpHsqlConnectionStringTests
    {
        [Test]
        public void Constructor_NoPairOfValues_ThrowArgumentException()
        {
            var connectionString = "myDb";
            try
            {
                var testConnectionString = new SharpHsqlConnectionString(connectionString);
                Assert.Fail("Expected exception of type ArgumentException.");
            }
            catch (ArgumentException aex)
            {
                Assert.IsTrue(aex.Message.StartsWith("The connection string is invalid."));
                Assert.AreEqual("connectionString", aex.ParamName);
            }
        }

        [Test]
        public void Constructor_InsufficientPairOfValues_ThrowArgumentException()
        {
            var connectionString = "myDb=rt;dd;";
            try
            {
                var testConnectionString = new SharpHsqlConnectionString(connectionString);
                Assert.Fail("Expected exception of type ArgumentException.");
            }
            catch (ArgumentException aex)
            {
                Assert.IsTrue(aex.Message.StartsWith("The connection string has an invalid parameter."));
                Assert.AreEqual("connectionString", aex.ParamName);
            }
        }

        [Test]
        public void Constructor_PasswordPairMissing_SetEmptyPassword() {
            var connectionString = "database=mydb;user id=sa;";
            var testConnectionString = new SharpHsqlConnectionString(connectionString);

            Assert.AreEqual("mydb", testConnectionString.Database);
            Assert.AreEqual("sa", testConnectionString.UserName);
            Assert.AreEqual(string.Empty, testConnectionString.UserPassword);
        }

        [Test]
        [TestCase("initial catalog", "user id", "password")]
        [TestCase("database", "uid", "pwd")]
        [TestCase("Initial Catalog", "User Id", "Password")]
        [TestCase("Database", "Uid", "Pwd")]
        public void Constructor_PairsHasCorrectKeys_FillDataIgnoringCase(string dbKey, string userKey, string passwordKey)
        {
            var connectionString = $"{dbKey}=mydb;{userKey}=sa;{passwordKey}=dfg";
            var testConnectionString = new SharpHsqlConnectionString(connectionString);

            Assert.AreEqual("mydb", testConnectionString.Database);
            Assert.AreEqual("sa", testConnectionString.UserName);
            Assert.AreEqual("dfg", testConnectionString.UserPassword);
        }

        [Test]
        public void Constructor_PairsHasCorrectValues_FillDataIgnoringCase() {
            var connectionString = $"Database=MyDatabase;User Id=John;Password=FrGnT";
            var testConnectionString = new SharpHsqlConnectionString(connectionString);

            Assert.AreEqual("mydatabase", testConnectionString.Database);
            Assert.AreEqual("john", testConnectionString.UserName);
            Assert.AreEqual("frgnt", testConnectionString.UserPassword);
        }

        [Test]
        public void Constructor_DatabaseNameIsEmpty_ThrowArgumentException()
        {
            var connectionString = "database=;uid=sa;password=";
            try
            {
                var testConnectionString = new SharpHsqlConnectionString(connectionString);
                Assert.Fail("Expected exception of type ArgumentException.");
            }
            catch (ArgumentException aex)
            {
                Assert.IsTrue(aex.Message.StartsWith("Database parameter is invalid in connection string."));
                Assert.AreEqual("connectionString", aex.ParamName);
            }
        }

        [Test]
        public void Constructor_UserNameIsEmpty_ThrowArgumentException()
        {
            var connectionString = "database=myDb;uid=;password=";
            try
            {
                var testConnectionString = new SharpHsqlConnectionString(connectionString);
                Assert.Fail("Expected exception of type ArgumentException.");
            }
            catch (ArgumentException aex)
            {
                Assert.IsTrue(aex.Message.StartsWith("UserName parameter is invalid in connection string."));
                Assert.AreEqual("connectionString", aex.ParamName);
            }
        }
    }
}