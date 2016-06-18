using System;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class DeleteTests : BaseQueryTest {
        [Test]
        [Ignore("Not correct")]
        public void T1() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "DELETE FROM \"clients\" WHERE \"clients\".\"id\" = 6;";
                var res = cmd.ExecuteNonQuery();

                Console.WriteLine();
                Assert.Pass();
            });
        }
    }
}
