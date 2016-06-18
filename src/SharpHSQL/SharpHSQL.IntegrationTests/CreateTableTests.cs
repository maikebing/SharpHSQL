using System;
using System.Collections.Generic;
using System.Data.Hsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests {
    [TestFixture]
    class CreateTableTests {
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

                    cmd.CommandText =
                        "DROP TABLE IF EXIST \"books\";CREATE TABLE \"books\" (\"id\" INT NOT NULL PRIMARY KEY, \"name\" char, \"author\" char, \"qty\" int, \"value\" numeric);";
                    res = cmd.ExecuteNonQuery();
                    Assert.AreEqual(0, res);

                    cmd.CommandText = "INSERT INTO \"books\" VALUES (1, 'Book000', 'Any', 1, '23.5');";
                    res = cmd.ExecuteNonQuery();
                    Assert.AreEqual(1, res);

                    cmd.CommandText = "INSERT INTO \"books\" VALUES (2, 'Book001', 'Andy', 2, '43.9');";
                    res = cmd.ExecuteNonQuery();
                    Assert.AreEqual(1, res);

                    cmd.CommandText = "INSERT INTO \"books\" VALUES (3, 'Book002', 'Andy', 3, '37.25');";
                    res = cmd.ExecuteNonQuery();
                    Assert.AreEqual(1, res);
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
