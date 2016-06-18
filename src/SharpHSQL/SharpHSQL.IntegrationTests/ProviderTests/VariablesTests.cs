using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Hsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class VariablesTests : BaseQueryTest {
        [Test]
        [Ignore("Not correct")]
        public void T1() {
            TestQuery(connection => {
                PrepareVariables(connection);
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT @MyVar;";
                string var = (string)cmd.ExecuteScalar();
                Console.WriteLine("@MyVar=" + var);

                Console.WriteLine();
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T2() {
            TestQuery(connection => {
                PrepareVariables(connection);
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT \"name\", \"author\", SUM(\"value\") FROM \"books\" WHERE \"author\" = @MyVar GROUP BY \"name\", \"author\";";
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
        public void T3() {
            TestQuery(connection => {
                PrepareVariables(connection);

                var data = new Byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
                var base64Photo = Convert.ToBase64String(data, 0, data.Length);

                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "INSERT INTO \"clients\" (\"DoubleValue\", \"nombre\", \"photo\", \"created\") VALUES (1.1, @MyVar, '" + base64Photo + "', NOW() );";
                var res = cmd.ExecuteNonQuery();
                cmd.CommandText = "DECLARE @MyId INT;SET @MyId = IDENTITY();";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT @MyId;";
                int myid = (int)cmd.ExecuteScalar();
                Console.WriteLine("Inserted id={0}", myid);

                Console.WriteLine();
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T4() {
            TestQuery(connection => {
                PrepareVariables(connection);
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SET @MyId = SELECT MAX(\"clients\".\"id\") + 1 FROM \"clients\";";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT @MyId;";
                var myid = (int)cmd.ExecuteScalar();
                Console.WriteLine("Next id={0}", myid);

                Console.WriteLine();
                Assert.Pass();
            });
        }

        private void PrepareVariables(SharpHsqlConnection connection) {
            var cmd = new SharpHsqlCommand("", connection);
            cmd.CommandText = "DECLARE @MyVar CHAR;SET @MyVar = 'Andy';";
            cmd.ExecuteNonQuery();
        }
    }
}
