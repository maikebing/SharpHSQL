using System;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class CreateTableTests {
        [Test]
        public void CreateTable_ShouldSuccessed() {
            var conn = new SharpHsqlConnection("Initial Catalog=mytest;User Id=sa;Pwd=;");
            try {
                conn.Open();

                var cmd = new SharpHsqlCommand("", conn);

                cmd.CommandText = "DROP TABLE IF EXIST \"data\";CREATE TABLE \"data\" (\"id\" int NOT NULL PRIMARY KEY, \"MyObject\" OBJECT);";
                var res = cmd.ExecuteNonQuery();
                Assert.AreEqual(0, res);

                cmd.CommandText = "DROP TABLE IF EXIST \"clients\";CREATE TABLE \"clients\" (\"id\" int NOT NULL IDENTITY PRIMARY KEY, \"DoubleValue\" double, \"nombre\" char, \"photo\" varbinary, \"created\" date );";
                res = cmd.ExecuteNonQuery();
                Assert.AreEqual(0, res);
            }
            catch (SharpHsqlException ex) {
                Assert.Fail(ex.Message);
            }
            finally {
                conn.Close();
            }
        }
    }
}
