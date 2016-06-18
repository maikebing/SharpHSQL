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
        [Ignore("Not correct")]
        public void T1() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT \"clients\".\"id\", \"clients\".\"DoubleValue\", \"clients\".\"nombre\" FROM \"clients\" WHERE \"clients\".\"id\" = 5;";
                cmd.CommandText = "SHOW DATABASES;";
                var reader = cmd.ExecuteReader();

                for (int i = 0; i < reader.FieldCount; i++) {
                    Console.Write(reader.GetName(i));
                    Console.Write("\t");
                }
                Console.Write(Environment.NewLine);

                while (reader.Read()) {
                    for (int i = 0; i < reader.FieldCount; i++) {
                        Console.Write(reader.GetValue(i).ToString());
                        Console.Write("\t");
                        Console.Write(Environment.NewLine);
                    }
                }

                Console.WriteLine();
                reader.Close();
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T2() {
            // Dataset Fill for SHOW DATABASES
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                var adapter = new SharpHsqlDataAdapter(cmd);
                var ds = new DataSet();
                var res = adapter.Fill(ds);
                adapter = null;

                Console.WriteLine();
                Console.WriteLine("DATABASES: " + ds.Tables[0].Rows.Count);

                Console.WriteLine();

                cmd.CommandText = "SHOW TABLES;";
                var reader = cmd.ExecuteReader();

                for (int i = 0; i < reader.FieldCount; i++) {
                    Console.Write(reader.GetName(i));
                    Console.Write("\t");
                }
                Console.Write(Environment.NewLine);

                while (reader.Read()) {
                    for (int i = 0; i < reader.FieldCount; i++) {
                        Console.Write(reader.GetValue(i).ToString());
                        Console.Write("\t");
                        Console.Write(Environment.NewLine);
                    }
                }

                Console.WriteLine();
                reader.Close();
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T3() {
            // Dataset Fill for SHOW TABLES
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                var adapter = new SharpHsqlDataAdapter(cmd);
                var ds = new DataSet();
                var res = adapter.Fill(ds);
                adapter = null;

                Console.WriteLine();
                Console.WriteLine("TABLES: " + ds.Tables[0].Rows.Count);

                Hashtable myData = new Hashtable();
                myData.Add("1", "ONE");
                myData.Add("2", "TWO");
                myData.Add("3", "TREE");
                myData.Add("4", "FOUR");
                myData.Add("5", "FIVE");

                cmd.Parameters.Clear();
                cmd.CommandText = "DELETE FROM \"data\" WHERE \"id\" = 1;";
                res = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                cmd.CommandText = "INSERT INTO \"data\" (\"id\", \"MyObject\") VALUES( @id, @MyObject);";
                cmd.Parameters.Add(new SharpHsqlParameter("@id", DbType.Int32, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, 1));
                cmd.Parameters.Add(new SharpHsqlParameter("@MyObject", DbType.Object, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, myData));
                res = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();


                cmd.CommandText = "SELECT \"data\".\"id\", \"data\".\"MyObject\" FROM \"data\";";
                var reader = cmd.ExecuteReader();
                Console.Write(Environment.NewLine);

                int myId = 0;
                Hashtable readData = null;
                while (reader.Read()) {
                    myId = reader.GetInt32(0);
                    readData = (Hashtable)reader.GetValue(1);
                }

                foreach (DictionaryEntry entry in readData) {
                    Console.WriteLine(String.Format("Key: {0}, Value: {1}", entry.Key.ToString(), entry.Value.ToString()));
                }


                Console.WriteLine();
                reader.Close();
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T4() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SHOW ALIAS;";
                var reader = cmd.ExecuteReader();

                Console.Write(Environment.NewLine);

                while (reader.Read()) {
                    Console.WriteLine("ALIAS {0} FOR {1}", reader.GetString(0), reader.GetString(1));
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
                cmd.CommandText = "SHOW COLUMNS \"clients\";";
                var reader = cmd.ExecuteReader();

                Console.Write(Environment.NewLine);

                while (reader.Read()) {
                    Console.WriteLine("TABLE: {0}, COLUMN: {1},\n\t NATIVE TYPE: {2},\t DB TYPE: {3},\n\t POSITION: {4},\t NULLABLE: {5},\t IDENTITY: {6}", reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetValue(3), reader.GetInt32(4), reader.GetBoolean(5), reader.GetBoolean(6));
                }

                Console.WriteLine();
                reader.Close();
                Assert.Pass();
            });
        }
    }
}
