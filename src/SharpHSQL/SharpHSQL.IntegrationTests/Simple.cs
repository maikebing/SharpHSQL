using System;

namespace SharpHSQL.IntegrationTests {
    /// <summary>
    /// Summary description for Simple.
    /// </summary>
    public class Simple {
        public Simple() {
        }

        public Decimal calcrate(Decimal amount, Decimal percent) {
            return amount + (amount * percent / 100);
        }

        public static Double tan(Double value) {
            return Math.Tan(value);
        }
    }
}
