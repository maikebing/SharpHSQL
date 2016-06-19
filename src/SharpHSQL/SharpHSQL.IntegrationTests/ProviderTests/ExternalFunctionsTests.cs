using System;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class ExternalFunctionsTests : BaseQueryTest {
        [Test]
        public void T1() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CREATE ALIAS CALCRATE FOR \"SharpHSQL.IntegrationTests,SharpHSQL.IntegrationTests.Simple.calcrate\";";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CALL CALCRATE(100, 21);";
                var rate = (Decimal)cmd.ExecuteScalar();
                Assert.AreEqual(121, rate);
            });
        }

        [Test]
        public void ShowParametersExternalFunction() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CREATE ALIAS CALCRATE FOR \"SharpHSQL.IntegrationTests,SharpHSQL.IntegrationTests.Simple.calcrate\";";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SHOW PARAMETERS CALCRATE;";
                var reader = cmd.ExecuteReader();

                reader.Read();
                Assert.AreEqual("CALCRATE", reader.GetString(0));
                Assert.AreEqual("RETURN_VALUE", reader.GetString(1));
                Assert.AreEqual("DECIMAL", reader.GetString(2));
                Assert.AreEqual(0, reader.GetInt32(3));

                reader.Read();
                Assert.AreEqual("CALCRATE", reader.GetString(0));
                Assert.AreEqual("amount", reader.GetString(1));
                Assert.AreEqual("DECIMAL", reader.GetString(2));
                Assert.AreEqual(1, reader.GetInt32(3));

                reader.Read();
                Assert.AreEqual("CALCRATE", reader.GetString(0));
                Assert.AreEqual("percent", reader.GetString(1));
                Assert.AreEqual("DECIMAL", reader.GetString(2));
                Assert.AreEqual(2, reader.GetInt32(3));

                Assert.False(reader.Read());

                reader.Close();
            });
        }
    }
}
