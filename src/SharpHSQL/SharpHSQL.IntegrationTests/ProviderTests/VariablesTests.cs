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
            TestQuery(connection => {
                PrepareVariables(connection);
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT @MyVar;";
                var myVar = (String)cmd.ExecuteScalar();

                Assert.AreEqual("Andy", myVar);
            });
        }

        [Test]
        public void UsingVariableInQuery() {
            TestQuery(connection => {
                PrepareVariables(connection);
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT \"name\", \"author\", SUM(\"value\") FROM \"books\" WHERE \"author\" = @MyVar GROUP BY \"name\", \"author\";";
                var reader = cmd.ExecuteReader();
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
            TestQuery(connection => {
                PrepareVariables(connection);

                var data = new Byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
                var base64Photo = Convert.ToBase64String(data, 0, data.Length);

                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "INSERT INTO \"clients\" (\"DoubleValue\", \"nombre\", \"photo\", \"created\") VALUES ('1.1', @MyVar, '" + base64Photo + "', NOW() );";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DECLARE @MyId INT;SET @MyId = IDENTITY();";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT @MyId;";
                var myid = (Int32)cmd.ExecuteScalar();
                Assert.AreEqual(11, myid);
            });
        }

        [Test]
        public void AssignVaribleInQuery() {
            TestQuery(connection => {
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

        protected override void PrepareDatabase(SharpHsqlConnection connection)
        {
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

        private void PrepareVariables(SharpHsqlConnection connection) {
            var cmd = new SharpHsqlCommand("", connection);
            cmd.CommandText = "DECLARE @MyVar CHAR;SET @MyVar = 'Andy';";
            cmd.ExecuteNonQuery();
        }
    }
}
