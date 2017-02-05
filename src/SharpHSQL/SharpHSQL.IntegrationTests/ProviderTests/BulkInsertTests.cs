using System;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class BulkInsertTests {
        [Test]
        public void BulInsert_ShouldSuccessed() {
            var conn = new SharpHsqlConnection("Initial Catalog=mytest;User Id=sa;Pwd=;");
            try {
                conn.Open();
                var commandText = "DROP TABLE IF EXIST \"clients\";CREATE TABLE \"clients\" (\"id\" int NOT NULL IDENTITY PRIMARY KEY, \"DoubleValue\" double, \"nombre\" char, \"photo\" varbinary, \"created\" date );";
                var cmd = new SharpHsqlCommand(commandText, conn);
                cmd.ExecuteNonQuery();

                var tran = conn.BeginTransaction();
                {
                    var data = new Byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
                    var base64Photo = Convert.ToBase64String(data, 0, data.Length);

                    var insertCommand = new SharpHsqlCommand("", conn);
                    for (var i = 0; i < 1000; i++) {
                        insertCommand.CommandText = "INSERT INTO \"clients\" (\"DoubleValue\", \"nombre\", \"photo\", \"created\") VALUES (1.1, 'NOMBRE" + i.ToString() + "', '" + base64Photo + "', NOW() );";
                        insertCommand.ExecuteNonQuery();
                    }
                }
                tran.Commit();

                Assert.Pass();
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
