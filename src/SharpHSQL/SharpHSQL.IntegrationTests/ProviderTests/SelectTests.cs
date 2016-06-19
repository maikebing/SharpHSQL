using System;
using System.Data;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class SelectTests : BaseQueryTest {
        [Test]
        [Ignore("Not correct")]
        public void T2() {
            TestQuery((connection) => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT * FROM \"books\"";
                var reader = cmd.ExecuteReader();

                var booksCount = 0;
                while (reader.Read()) {
                    booksCount += 1;
                    //Console.WriteLine("id={0}book={1},\tauthor={2},\tqty={3},\tvalue={4}", reader.GetInt32(0),
                    //    reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDecimal(4));
                }

                Assert.AreEqual(3, booksCount);
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T3() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT * FROM \"books\" ORDER BY \"value\"";
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    Console.WriteLine("id={0}book={1},\tauthor={2},\tqty={3},\tvalue={4}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDecimal(4));
                }

                Console.WriteLine();

                reader.Close();

                Console.WriteLine();
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T4() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT COUNT(*) as CNT, SUM(\"value\") FROM \"books\" WHERE \"author\" = 'Andy'";
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    Console.WriteLine("count={0},\tvalue={1}", reader.GetInt32(0), reader.GetDecimal(1));
                }

                Console.WriteLine();

                reader.Close();
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T5() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT \"name\", \"author\", SUM(\"value\") FROM \"books\" WHERE \"author\" = 'Andy' GROUP BY \"name\", \"author\";";
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    Console.WriteLine("name={0},\tauthor={1},\tvalue={2}", reader.GetString(0), reader.GetString(1), reader.GetDecimal(2));
                }

                Console.WriteLine();

                reader.Close();
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T6() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT \"name\", SUM(\"value\") FROM \"books\" WHERE \"author\" = 'Andy' GROUP BY \"name\";";
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    Console.WriteLine("name={0},\tvalue={1}", reader.GetString(0), reader.GetDecimal(1));
                }

                Console.WriteLine();

                reader.Close();
                Assert.Pass();
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
                cmd.CommandText = "INSERT INTO \"books\" VALUES (2, 'Book001', 'Andy2', 2, '43.9');";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO \"books\" VALUES (3, 'Book002', 'Andy', 3, '37.25');";
                cmd.ExecuteNonQuery();
            }
            tran.Commit();
        }
    }
}
