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
        public void DeclareVariableTest() {
            var dbPrototype = new DataSet("mytest");

            TestQuery(dbPrototype, connection => {
                PrepareVariables(connection);
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT @MyVar;";
                var myVar = (String)cmd.ExecuteScalar();

                Assert.AreEqual("Andy", myVar);
            });
        }

        [Test]
        public void UsingVariableInQuery() {
            var dbPrototype = new DataSet("mytest");
            dbPrototype.Tables.Add(GenerateTableBooks());

            TestQuery(dbPrototype, connection => {
                PrepareVariables(connection);
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT \"name\", \"author\", SUM(\"value\") FROM \"books\" WHERE \"author\" = @MyVar GROUP BY \"name\", \"author\";";
                var reader = cmd.ExecuteReader();

                reader.Read();
                Assert.AreEqual("Book001", reader.GetString(0));
                Assert.AreEqual("Andy", reader.GetString(1));
                Assert.AreEqual(43.9, reader.GetDecimal(2));

                reader.Read();
                Assert.AreEqual("Book002", reader.GetString(0));
                Assert.AreEqual("Andy", reader.GetString(1));
                Assert.AreEqual(37.25, reader.GetDecimal(2));

                Assert.False(reader.Read());
                reader.Close();
            });
        }

        [Test]
        public void UsingVariableInInsertStatement() {
            var dbPrototype = new DataSet("mytest");
            dbPrototype.Tables.Add(GenerateTableClients());

            TestQuery(dbPrototype, connection => {
                PrepareVariables(connection);

                var data = new Byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
                var base64Photo = Convert.ToBase64String(data, 0, data.Length);

                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "INSERT INTO \"clients\" (\"id\", \"DoubleValue\", \"nombre\", \"photo\", \"created\") VALUES (100, '1.1', @MyVar, '" + base64Photo + "', NOW() );";
                var res = cmd.ExecuteNonQuery();
                Assert.AreEqual(1, res);
            });
        }

        [Test]
        public void AssignVaribleInQuery() {
            var dbPrototype = new DataSet("mytest");
            dbPrototype.Tables.Add(GenerateTableClients());

            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "DECLARE @MyId INTEGER;";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SET @MyId = SELECT MAX(\"clients\".\"id\") + 1 FROM \"clients\";";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT @MyId;";
                var myid = (Int32)cmd.ExecuteScalar();
                Assert.AreEqual(11, myid);
            });
        }

        private void PrepareVariables(SharpHsqlConnection connection) {
            var cmd = new SharpHsqlCommand("", connection);
            cmd.CommandText = "DECLARE @MyVar CHAR;SET @MyVar = 'Andy';";
            cmd.ExecuteNonQuery();
        }
    }
}
