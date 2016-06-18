using System;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class BulkInsertTests {
        [Test]
        public void T1() {
            // TODO: Удалять БД
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

                var tran = conn.BeginTransaction();
                {
                    var data = new Byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
                    var base64Photo = Convert.ToBase64String(data, 0, data.Length);

                    cmd = new SharpHsqlCommand("", conn);

                    for (var i = 0; i < 1000; i++) {
                        cmd.CommandText = "INSERT INTO \"clients\" (\"DoubleValue\", \"nombre\", \"photo\", \"created\") VALUES ('1.1', 'NOMBRE" + i.ToString() + "', '" + base64Photo + "', NOW() );";
                        res = cmd.ExecuteNonQuery();
                        Assert.AreEqual(1, res); // TODO: Или после цикла проверить количество записей? В чем смысл теста?
                    }
                }
                tran.Commit();
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
