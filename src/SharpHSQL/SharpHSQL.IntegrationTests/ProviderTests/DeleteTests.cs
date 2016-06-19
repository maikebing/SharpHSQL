using System;
using System.Data;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class DeleteTests : BaseQueryTest {
        [Test]
        public void Delete_ShouldDeleteRecord() {
            var dbPrototype = new DataSet("mytest");
            var clientsTable = GenerateTableClients();
            dbPrototype.Tables.Add(clientsTable);

            TestQuery(dbPrototype, connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "DELETE FROM \"clients\" WHERE \"clients\".\"id\" = 6;";
                var res = cmd.ExecuteNonQuery();
                Assert.AreEqual(1, res);
            });
        }
    }
}
