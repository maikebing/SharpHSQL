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
    class ParameterizedQueryTests : BaseQueryTest {
        [Test]
        [Ignore("Not correct query")]
        public void ParameterizedQuery_ShouldSuccessful() {
            var dbPrototype = new DataSet("mytest");
            dbPrototype.Tables.Add(GenerateTableClients());

            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                var dt = DateTime.Now;
                var photo = new Byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
                cmd.CommandText = "INSERT INTO \"clients\" (\"DoubleValue\", \"nombre\", \"photo\", \"created\") VALUES (@DoubleValue, @nombre, @photo, @date );SET @Id = IDENTITY();";
                cmd.Parameters.Add(new SharpHsqlParameter("@Id", DbType.Int32, 0, ParameterDirection.Output, false, 0, 0, null, DataRowVersion.Current, null));
                cmd.Parameters.Add(new SharpHsqlParameter("@DoubleValue", DbType.Double, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, 1.1));
                cmd.Parameters.Add(new SharpHsqlParameter("@nombre", DbType.String, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, "Andrйs"));
                cmd.Parameters.Add(new SharpHsqlParameter("@photo", DbType.Binary, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, photo));
                cmd.Parameters.Add(new SharpHsqlParameter("@date", DbType.DateTime, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, dt));
                cmd.ExecuteNonQuery();

                var p = (SharpHsqlParameter)cmd.Parameters["@Id"];
                var myid = (Int32)p.Value;
                Assert.AreEqual(11, myid);

                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT \"clients\".\"created\" FROM \"clients\" WHERE \"clients\".\"id\" = " + myid + ";";

                var reader = cmd.ExecuteReader();
                reader.Read();
                Assert.AreEqual(dt, reader.GetDateTime(0));
                reader.Close();
            });
        }
    }
}
