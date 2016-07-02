using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Hsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class MetadataQueriesTests : BaseQueryTest {
        [Test]
        public void ShowDatabases_ShouldReturnCurrentDatabaseName() {
            var dbPrototype = new DataSet("mytest");
            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SHOW DATABASES;";
                var reader = cmd.ExecuteReader();
                Assert.AreEqual(1, reader.FieldCount);
                Assert.AreEqual("DATABASE", reader.GetName(0));
                reader.Read();
                Assert.AreEqual("MYTEST", reader.GetString(0));
                reader.Close();
            });
        }

        [Test(Description = "Dataset Fill for SHOW DATABASES")]
        public void ShowDatabases_ShouldCorrectFillDataAdapter() {
            var dbPrototype = new DataSet("mytest");
            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SHOW DATABASES;";
                var adapter = new SharpHsqlDataAdapter(cmd);
                var ds = new DataSet();
                var res = adapter.Fill(ds);

                Assert.AreEqual(1, res);
                Assert.AreEqual(1, ds.Tables[0].Rows.Count);
            });
        }

        [Test]
        public void ShowTables_ShouldReturnTablesList() {
            var dbPrototype = new DataSet("mytest");
            dbPrototype.Tables.Add(GenerateTableData());
            dbPrototype.Tables.Add(GenerateTableClients());

            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SHOW TABLES;";
                var reader = cmd.ExecuteReader();

                Assert.AreEqual(1, reader.FieldCount);
                Assert.AreEqual("TABLE", reader.GetName(0));

                var expectedTables = new[] { "data", "clients" };
                var tablesCount = 0;
                while (reader.Read()) {
                    Assert.AreEqual(expectedTables[tablesCount], reader.GetString(0).ToLowerInvariant());
                    tablesCount += 1;
                }

                Assert.AreEqual(2, tablesCount);
                reader.Close();
            });
        }

        [Test(Description = "Dataset Fill for SHOW TABLES")]
        public void ShowTables_ShouldCorrectFillDataAdapter() {
            var dbPrototype = new DataSet("mytest");
            dbPrototype.Tables.Add(GenerateTableClients());
            dbPrototype.Tables.Add(GenerateTableData());

            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SHOW TABLES;";
                var adapter = new SharpHsqlDataAdapter(cmd);
                var ds = new DataSet();
                var res = adapter.Fill(ds);

                Assert.AreEqual(2, res);
                Assert.AreEqual(2, ds.Tables[0].Rows.Count);
            });
        }

        [Test]
        public void ShowAlias_ShouldReturnAliases() {
            var dbPrototype = new DataSet("mytest");
            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SHOW ALIAS;";
                var reader = cmd.ExecuteReader();

                var aliasesCount = 0;
                while (reader.Read()) {
                    aliasesCount += 1;
                }

                reader.Close();
                Assert.AreEqual(66, aliasesCount); // TODO: Unreliable test
            });
        }

        [Test]
        public void ShowColumns_ShouldReturnColumnsList() {
            var dbPrototype = new DataSet("mytest");
            dbPrototype.Tables.Add(GenerateTableData());

            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SHOW COLUMNS data;";
                var reader = cmd.ExecuteReader();

                Console.Write(Environment.NewLine);

                // column id
                reader.Read();
                Assert.AreEqual("data", reader.GetString(0).ToLowerInvariant());
                Assert.AreEqual("id", reader.GetString(1).ToLowerInvariant());
                Assert.AreEqual("INTEGER", reader.GetString(2));
                Assert.AreEqual(DbType.Int32, (DbType)reader.GetValue(3));
                Assert.AreEqual(0, reader.GetInt32(4));
                Assert.AreEqual(false, reader.GetBoolean(5));
                Assert.AreEqual(false, reader.GetBoolean(6));

                // column MyObject
                reader.Read();
                Assert.AreEqual("data", reader.GetString(0).ToLowerInvariant());
                Assert.AreEqual("myobject", reader.GetString(1).ToLowerInvariant());
                Assert.AreEqual("OBJECT", reader.GetString(2));
                Assert.AreEqual(DbType.Object, (DbType)reader.GetValue(3));
                Assert.AreEqual(1, reader.GetInt32(4));
                Assert.AreEqual(true, reader.GetBoolean(5));
                Assert.AreEqual(false, reader.GetBoolean(6));

                // No more columns
                Assert.IsFalse(reader.Read());
                reader.Close();
            });
        }
    }
}
