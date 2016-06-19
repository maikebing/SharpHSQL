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
        public void DataAdapterFill_ShouldFillAllRows() {
            TestQuery(connection => {
                using (var cmd = new SharpHsqlCommand("", connection)) {
                    cmd.CommandText =
                        "SELECT \"clients\".\"id\", \"clients\".\"DoubleValue\", \"clients\".\"nombre\" FROM \"clients\" WHERE \"clients\".\"id\" = 5;";

                    using (var adapter = new SharpHsqlDataAdapter(cmd)) {
                        var ds = new DataSet();
                        var res = adapter.Fill(ds);

                        Assert.AreEqual(1, res);
                        Assert.AreEqual(1, ds.Tables[0].Rows.Count);
                        Assert.Pass();
                    }
                }
            });
        }
    }
}
