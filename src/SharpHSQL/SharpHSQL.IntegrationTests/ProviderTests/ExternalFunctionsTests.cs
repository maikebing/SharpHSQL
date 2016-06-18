using System;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class ExternalFunctionsTests : BaseQueryTest {
        [Test]
        [Ignore("Not correct")]
        public void T1() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CREATE ALIAS CALCRATE FOR \"ExternalFunction,ExternalFunction.Simple.calcrate\";";
                var res = cmd.ExecuteNonQuery();
                cmd.CommandText = "CALL CALCRATE(100, 21);";
                Decimal rate = (Decimal)cmd.ExecuteScalar();
                Console.WriteLine("CALCRATE=" + rate);

                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T2() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CREATE ALIAS EXTTAN FOR \"ExternalFunction,ExternalFunction.Simple.tan\";";
                var res = cmd.ExecuteNonQuery();
                cmd.CommandText = "CALL EXTTAN(23.456);";
                Double tan = (Double)cmd.ExecuteScalar();
                Console.WriteLine("EXTTAN=" + tan);
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T5() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CREATE ALIAS CALCRATE FOR \"ExternalFunction,ExternalFunction.Simple.calcrate\";";
                var res = cmd.ExecuteNonQuery();

                cmd.CommandText = "SHOW PARAMETERS CALCRATE;";
                var reader = cmd.ExecuteReader();

                Console.Write(Environment.NewLine);

                while (reader.Read()) {
                    Console.WriteLine("ALIAS: {0}, PARAM: {1},\t TYPE {2},\t POSITION: {3}", reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                }

                Console.WriteLine();
                reader.Close();
                Assert.Pass();
            });
        }

    }
}
