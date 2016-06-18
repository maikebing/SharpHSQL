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
    class DataAdapterTests : BaseQueryTest {
        [Test]
        [Ignore("Not correct")]
        public void T1() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT \"clients\".\"id\", \"clients\".\"DoubleValue\", \"clients\".\"nombre\" FROM \"clients\" WHERE \"clients\".\"id\" = 5;";

                SharpHsqlDataAdapter adapter = new SharpHsqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                var res = adapter.Fill(ds);
                adapter = null;

                Console.WriteLine();
                Console.WriteLine("DataSet.Fill: " + ds.Tables[0].Rows.Count);
                Assert.Pass();
            });
        }
    }
}
