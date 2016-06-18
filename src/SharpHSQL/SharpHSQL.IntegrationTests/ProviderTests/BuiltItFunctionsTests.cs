using System;
using System.Data.Hsql;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    [TestFixture]
    class BuiltItFunctionsTests : BaseQueryTest {
        [Test]
        [Ignore("Not correct")]
        public void T1() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT MAX(\"clients\".\"id\") FROM \"clients\";";
                object result = cmd.ExecuteScalar();
                if (result != null) {
                    var res = (int)result;
                    Console.WriteLine("MAX=" + res);
                }
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T2() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT SUM(\"clients\".\"id\") FROM \"clients\";";
                var result = cmd.ExecuteScalar();
                if (result != null) {
                    var res = (int)result;
                    Console.WriteLine("SUM=" + res);
                }
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T3() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT COUNT(\"clients\".\"id\") FROM \"clients\";";
                var result = cmd.ExecuteScalar();
                if (result != null) {
                    var res = (int)result;
                    Console.WriteLine("COUNT=" + res);
                }
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T4() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "SELECT AVG(\"clients\".\"id\") FROM \"clients\";";
                var result = cmd.ExecuteScalar();
                if (result != null) {
                    var res = (int)result;
                    Console.WriteLine("AVG=" + res);
                }
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T5() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CALL ABS(-33.5632);";
                var result = cmd.ExecuteScalar();
                if (result != null) {
                    var abs = (Double)result;
                    Console.WriteLine("ABS=" + abs);
                }
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T6() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CALL USER();";
                string user = (string)cmd.ExecuteScalar();
                Console.WriteLine("USER=" + user);
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T7() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CALL SQRT(3);";
                Double sqrt = (Double)cmd.ExecuteScalar();
                Console.WriteLine("SQRT=" + sqrt);
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T8() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CALL SUBSTRING('0123456', 3, 2);";
                string subs = (String)cmd.ExecuteScalar();
                Console.WriteLine("SUBSTRING=" + subs);
                Assert.Pass();
            });
        }

        [Test]
        [Ignore("Not correct")]
        public void T9() {
            TestQuery(connection => {
                var cmd = new SharpHsqlCommand("", connection);
                cmd.CommandText = "CALL ASCII('A');";
                int ascii = (int)cmd.ExecuteScalar();
                Console.WriteLine("ASCII=" + ascii);
                Assert.Pass();
            });
        }
    }
}
