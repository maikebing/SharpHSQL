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
    class ColumnTypesTests : BaseQueryTest {
        [Test]
        public void ObjectColumntTypeTest() {
            TestQuery(connection => {
                var myData = new Hashtable {
                    {"1", "ONE"}, 
                    {"2", "TWO"}, 
                    {"3", "TREE"}, 
                    {"4", "FOUR"}, 
                    {"5", "FIVE"}
                };

                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "DELETE FROM \"data\" WHERE \"id\" = 1;";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO \"data\" (\"id\", \"MyObject\") VALUES( @id, @MyObject);";
                cmd.Parameters.Add(new SharpHsqlParameter("@id", DbType.Int32, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, 1));
                cmd.Parameters.Add(new SharpHsqlParameter("@MyObject", DbType.Object, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, myData));
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT \"data\".\"id\", \"data\".\"MyObject\" FROM \"data\";";
                var reader = cmd.ExecuteReader();

                reader.Read();
                Assert.AreEqual(1, reader.GetInt32(0));

                var readData = (Hashtable)reader.GetValue(1); ;
                foreach (DictionaryEntry entry in readData) {
                    Assert.AreEqual(myData[entry.Key], entry.Value);
                }

                reader.Close();
            });
        }
    }
}
