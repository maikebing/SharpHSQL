using System;
using System.Data;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class SelectTests : BaseQueryTest {
        [Test]
        public void SimpleSelect() {
            var dbPrototype = new DataSet("mytest");
            var booksTable = GenerateTableBooks();
            dbPrototype.Tables.Add(booksTable);

            TestQuery(dbPrototype, connection => {
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
            var dbPrototype = new DataSet("mytest");
            var booksTable = GenerateTableBooks();
            dbPrototype.Tables.Add(booksTable);

            TestQuery(dbPrototype, connection => {
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
            var dbPrototype = new DataSet("mytest");
            var booksTable = GenerateTableBooks();
            dbPrototype.Tables.Add(booksTable);

            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT COUNT(*) as CNT, SUM(\"value\") FROM \"books\" WHERE \"author\" = 'Andy'";
                var reader = cmd.ExecuteReader();
                reader.Read();

                Assert.AreEqual(2, reader.GetInt32(0));
                Assert.AreEqual(81.15m, reader.GetDecimal(1));
                reader.Close();
            });
        }

        [Test]
        public void GroupByClauseTest() {
            var dbPrototype = new DataSet("mytest");
            var booksTable = GenerateTableBooks();
            dbPrototype.Tables.Add(booksTable);

            TestQuery(dbPrototype, connection => {
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
    }
}
