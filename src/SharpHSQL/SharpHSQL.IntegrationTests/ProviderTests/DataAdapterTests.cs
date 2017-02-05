using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Hsql;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class DataAdapterTests : BaseQueryTest {
        [Test]
        public void DataAdapterFill_ShouldFillAllRows() {
            var dbPrototype = new DataSet("mytest");
            dbPrototype.Tables.Add(GenerateTableClients());

            TestQuery(dbPrototype, connection => {
                var commandText = "SELECT id, DoubleValue, nombre FROM clients WHERE id = 5;";
                using (var cmd = new SharpHsqlCommand(commandText, connection))
                using (var adapter = new SharpHsqlDataAdapter(cmd)) {
                    var ds = new DataSet();
                    var res = adapter.Fill(ds);

                    // 3. Assertion checking
                    Assert.AreEqual(1, res);
                    Assert.AreEqual(1, ds.Tables[0].Rows.Count);
                    Assert.Pass();
                }
            });           
        }
    }
}
