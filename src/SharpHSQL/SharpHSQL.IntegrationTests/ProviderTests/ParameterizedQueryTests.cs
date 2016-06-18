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
        [Ignore("Not correct")]
        public void T1() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                DateTime dt = DateTime.Now;
                byte[] photo = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };
                cmd.CommandText = "INSERT INTO \"clients\" (\"DoubleValue\", \"nombre\", \"photo\", \"created\") VALUES (@DoubleValue, @nombre, @photo, @date );SET @Id = IDENTITY();";
                cmd.Parameters.Add(new SharpHsqlParameter("@Id", DbType.Int32, 0, ParameterDirection.Output, false, 0, 0, null, DataRowVersion.Current, null));
                cmd.Parameters.Add(new SharpHsqlParameter("@DoubleValue", DbType.Double, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, 1.1));
                cmd.Parameters.Add(new SharpHsqlParameter("@nombre", DbType.String, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, "Andrйs"));
                cmd.Parameters.Add(new SharpHsqlParameter("@photo", DbType.Binary, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, photo));
                cmd.Parameters.Add(new SharpHsqlParameter("@date", DbType.DateTime, 0, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, dt));
                var res = cmd.ExecuteNonQuery();
                SharpHsqlParameter p = (SharpHsqlParameter)cmd.Parameters["@Id"];
                var myid = (int)p.Value;
                Console.WriteLine("Inserted id={0}", myid);
                Console.WriteLine();

                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT \"clients\".\"created\" FROM \"clients\" WHERE \"clients\".\"id\" = " + myid + ";";
                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    Console.WriteLine(String.Format("Dates are equal: {0}.", dt.Equals(reader.GetDateTime(0))));
                }

                Console.WriteLine();
                reader.Close();
                Assert.Pass();
            });
        }
    }
}
