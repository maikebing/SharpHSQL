using System;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class DeleteTests : BaseQueryTest {
        [Test]
        public void Delete_ShouldDeleteRecord() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "DELETE FROM \"clients\" WHERE \"clients\".\"id\" = 6;";
                var res = cmd.ExecuteNonQuery();
                Assert.AreEqual(1, res);
            });
        }
    }
}
