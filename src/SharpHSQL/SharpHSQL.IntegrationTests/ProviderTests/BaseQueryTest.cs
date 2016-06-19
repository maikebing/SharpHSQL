using System;
using System.Data.Hsql;
using System.IO;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    class BaseQueryTest {
        protected void TestQuery(Action<SharpHsqlConnection> test)
        {
            var tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), ".tests", Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            var conn = new SharpHsqlConnection("Initial Catalog=" + tempDirectory + "/mytest;User Id=sa;Pwd=;");
            try {
                conn.Open();
                PrepareDatabase(conn);
                test(conn);
            }
            catch (SharpHsqlException ex) {
                Assert.Fail(ex.Message);
            }
            finally {
                conn.Close();
            }
        }

        protected virtual void PrepareDatabase(SharpHsqlConnection connection) {
            var cmd = new SharpHsqlCommand("", connection);

            cmd.CommandText = "DROP TABLE IF EXIST \"data\";CREATE TABLE \"data\" (\"id\" int NOT NULL PRIMARY KEY, \"MyObject\" OBJECT);";
            var res = cmd.ExecuteNonQuery();
            Assert.AreEqual(0, res);

            cmd.CommandText = "DROP TABLE IF EXIST \"clients\";CREATE TABLE \"clients\" (\"id\" int NOT NULL IDENTITY PRIMARY KEY, \"DoubleValue\" double, \"nombre\" char, \"photo\" varbinary, \"created\" date );";
            res = cmd.ExecuteNonQuery();
            Assert.AreEqual(0, res);

            var tran = connection.BeginTransaction();
            {
                var data = new Byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
                var base64Photo = Convert.ToBase64String(data, 0, data.Length);

                cmd = new SharpHsqlCommand("", connection);

                for (var i = 0; i < 10; i++) {
                    cmd.CommandText = String.Format(
                        "INSERT INTO clients (DoubleValue, nombre, photo, created) VALUES ('1.1', 'NOMBRE{0}', '{1}', NOW() );",
                        i,
                        base64Photo);
                    res = cmd.ExecuteNonQuery();
                    Assert.AreEqual(1, res);

                    cmd.CommandText = "CALL IDENTITY();";
                    var id = (Int32)cmd.ExecuteScalar();
                    Assert.AreEqual(i + 1, id);
                }
            }
            tran.Commit();
        }
    }
}
