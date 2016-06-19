using System;
using System.Data;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class SelectTests : BaseQueryTest {
        [Test]
        public void SimpleSelect() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT * FROM \"books\"";
                var reader = cmd.ExecuteReader();

                var booksCount = 0;
                while (reader.Read()) {
                    booksCount += 1;
                }

                Assert.AreEqual(4, booksCount);
            });
        }

        [Test]
        public void SelectWithOrder() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT * FROM \"books\" ORDER BY \"value\"";
                var reader = cmd.ExecuteReader();

                var expectedOrder = new[] { 4, 1, 3, 2 };

                var rowIndex = 0;
                while (reader.Read()) {
                    Assert.AreEqual(expectedOrder[rowIndex], reader.GetInt32(0));
                    rowIndex += 1;
                }

                reader.Close();
            });
        }

        [Test]
        public void WhereClauseTest() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT COUNT(*) as CNT, SUM(\"value\") FROM \"books\" WHERE \"author\" = 'Andy'";
                var reader = cmd.ExecuteReader();
                reader.Read();

                Assert.AreEqual(2, reader.GetInt32(0));
                Assert.AreEqual(81.15, reader.GetDecimal(1));
                reader.Close();
            });
        }

        [Test]
        public void GroupByClauseTest() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT \"author\", SUM(\"value\") FROM \"books\" GROUP BY \"author\";";
                var reader = cmd.ExecuteReader();

                reader.Read();
                Assert.AreEqual("Andy", reader.GetString(0));
                Assert.AreEqual(81.15, reader.GetDecimal(1));

                reader.Read();
                Assert.AreEqual("Any", reader.GetString(0));
                Assert.AreEqual(34.0, reader.GetDecimal(1));

                Assert.False(reader.Read());

                reader.Close();
            });
        }

        protected override void PrepareDatabase(SharpHsqlConnection connection) {
            base.PrepareDatabase(connection);

            var cmd = new SharpHsqlCommand("", connection);
            cmd.CommandText = "DROP TABLE IF EXIST \"books\";CREATE TABLE \"books\" (\"id\" INT NOT NULL PRIMARY KEY, \"name\" char, \"author\" char, \"qty\" int, \"value\" numeric);";
            cmd.ExecuteNonQuery();

            var tran = connection.BeginTransaction();
            {
                cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "INSERT INTO \"books\" VALUES (1, 'Book000', 'Any', 1, '23.5');";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO \"books\" VALUES (2, 'Book001', 'Andy', 2, '43.9');";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO \"books\" VALUES (3, 'Book002', 'Andy', 3, '37.25');";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO \"books\" VALUES (4, 'Book003', 'Any', 1, '10.5');";
                cmd.ExecuteNonQuery();
            }
            tran.Commit();
        }
    }
}
