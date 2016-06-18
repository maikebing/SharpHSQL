using System;
using System.Data;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class SelectTests : BaseQueryTest {
        [Test]
        [Ignore("Not correct")]
        public void T1() {
            TestQuery((connection) => {
                var cmd = new SharpHsqlCommand("", connection);

                cmd.CommandText = "SELECT \"clients\".\"id\", \"clients\".\"DoubleValue\", \"clients\".\"nombre\",  \"clients\".\"photo\", \"clients\".\"created\" FROM \"clients\" ORDER BY \"clients\".\"id\" ";
                IDataReader reader = cmd.ExecuteReader();

                byte[] photo = null;

                while (reader.Read()) {
                    var len = reader.GetBytes(3, 0, null, 0, 0);
                    photo = new byte[len];
                    reader.GetBytes(3, 0, photo, 0, (int)len);
                    Console.WriteLine("id={0}, doubleValue={1}, nombre={2}, photo={3}, created={4}", reader.GetInt32(0), reader.GetDouble(1), reader.GetString(2), photo.Length, reader.GetDateTime(4).ToString("yyyy.MM.dd hh:mm:ss.fffffff"));
                }

                reader.Close();
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T2() {
            TestQuery((connection) => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT * FROM \"books\"";
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    Console.WriteLine("id={0}book={1},\tauthor={2},\tqty={3},\tvalue={4}", reader.GetInt32(0),
                        reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetDecimal(4));
                }

                Console.WriteLine();

                reader.Close();
                Assert.Pass();
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
    }
}
