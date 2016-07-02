using System;
using System.Data;
using System.Data.Hsql;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SharpHSQL.IntegrationTests.ProviderTests {
    class BaseQueryTest {
        protected void TestQuery(DataSet dbPrototype, Action<SharpHsqlConnection> test) {
            var connection = GenerateTestDatabase(dbPrototype);
            try {
                connection.Open();
                test(connection);
            }
            catch (SharpHsqlException ex) {
                Assert.Fail(ex.Message);
            }
            finally {
                connection.Close();
            }
        }

        protected SharpHsqlConnection GenerateTestDatabase(DataSet dbPrototype) {
            var tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), ".tests", Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            var connection = new SharpHsqlConnection(String.Format("Initial Catalog={0}/{1};User Id=sa;Pwd=;", tempDirectory, dbPrototype.DataSetName));
            try {
                connection.Open();
                foreach (var table in dbPrototype.Tables.Cast<DataTable>()) {
                    var createTableQueryBuilder = new StringBuilder();
                    createTableQueryBuilder.AppendFormat("DROP TABLE IF EXIST \"{0}\";", table.TableName);
                    createTableQueryBuilder.AppendFormat("CREATE TABLE \"{0}\" (", table.TableName);

                    var counter = 0;
                    foreach (var column in table.Columns.Cast<DataColumn>()) {
                        var columnDescription = "\"" + column.ColumnName + "\" " + GetDbType(column.DataType);

                        if (!column.AllowDBNull)
                            columnDescription += " NOT NULL";

                        if (table.PrimaryKey.Contains(column))
                            columnDescription += " PRIMARY KEY";

                        createTableQueryBuilder.Append(columnDescription);
                        if (counter < table.Columns.Count - 1)
                            createTableQueryBuilder.Append(",");

                        counter += 1;
                    }

                    createTableQueryBuilder.Append(");");

                    var cmd = new SharpHsqlCommand(createTableQueryBuilder.ToString(), connection);
                    cmd.ExecuteNonQuery();

                    var tran = connection.BeginTransaction();
                    {
                        foreach (var row in table.Rows.Cast<DataRow>()) {
                            var rowInsertQuery = "INSERT INTO " + table.TableName + "(";
                            var columnIndex = 0;
                            foreach (var column in table.Columns.Cast<DataColumn>()) {
                                rowInsertQuery += "\"" + column.ColumnName + "\"";
                                if (columnIndex < table.Columns.Count - 1)
                                    rowInsertQuery += ",";

                                columnIndex += 1;
                            }

                            rowInsertQuery += ") VALUES (";
                            columnIndex = 0;
                            foreach (var column in table.Columns.Cast<DataColumn>()) {
                                rowInsertQuery += FormatValue(column.DataType, row[column]);
                                if (columnIndex < table.Columns.Count - 1)
                                    rowInsertQuery += ",";

                                columnIndex += 1;
                            }

                            rowInsertQuery += ");";

                            var insertCommand = new SharpHsqlCommand(rowInsertQuery, connection);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                }

                return connection;
            }
            finally {
                connection.Close();
            }
        }

        protected DataTable GenerateTableData() {
            var table = new DataTable("data");
            var idColumn = table.Columns.Add("id", typeof(Int32));
            idColumn.AllowDBNull = false;
            table.Columns.Add("MyObject", typeof(Object));
            table.PrimaryKey = new[] { idColumn };
            return table;
        }

        protected DataTable GenerateTableClients() {
            var table = new DataTable("clients");
            var idColumn = table.Columns.Add("id", typeof(Int32));
            idColumn.AllowDBNull = false;
            table.Columns.Add("DoubleValue", typeof(Double));
            table.Columns.Add("nombre", typeof(String));
            table.Columns.Add("photo", typeof(Byte[]));
            table.Columns.Add("created", typeof(DateTime));
            table.PrimaryKey = new[] { idColumn };

            for (var i = 1; i <= 10; i++) {
                table.Rows.Add(i, 1.1, "NOMBRE", new Byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 }, DateTime.Now);
            }

            return table;
        }

        protected DataTable GenerateTableBooks() {
            var table = new DataTable("books");
            var idColumn = table.Columns.Add("id", typeof(Int32));
            idColumn.AllowDBNull = false;
            table.Columns.Add("name", typeof(String));
            table.Columns.Add("author", typeof(String));
            table.Columns.Add("qty", typeof(Int32));
            table.Columns.Add("value", typeof(Decimal));
            table.PrimaryKey = new[] { idColumn };

            table.Rows.Add(1, "Book000", "Any", 1, 23.5m);
            table.Rows.Add(2, "Book001", "Andy", 2, 43.9m);
            table.Rows.Add(3, "Book002", "Andy", 3, 37.25m);
            table.Rows.Add(4, "Book003", "Any", 1, 10.5m);

            return table;
        }

        private String GetDbType(Type type) {
            if (type == typeof(Int32))
                return "int";

            if (type == typeof(Double))
                return "double";

            if (type == typeof(Decimal))
                return "numeric";

            if (type == typeof(String))
                return "char";

            if (type == typeof(Byte[]))
                return "varbinary";

            if (type == typeof(DateTime))
                return "date";

            if (type.Name == "Object")
                return "object";

            throw new InvalidOperationException("Type '" + type.Name + "' not supported");
        }

        private String FormatValue(Type type, Object value) {
            if (type == typeof(Int32))
                return value.ToString();

            if (type == typeof(Double))
                return ((Double) value).ToString(NumberFormatInfo.InvariantInfo);

            if (type == typeof(Decimal))
                return ((Decimal)value).ToString(NumberFormatInfo.InvariantInfo);

            if (type == typeof(String))
                return String.Format("'{0}'", value);

            if (type == typeof(Byte[])) {
                var v = (Byte[])value;
                var base64Value = Convert.ToBase64String(v, 0, v.Length);
                return String.Format("'{0}'", base64Value);
            }

            if (type == typeof(DateTime))
                return String.Format("'{0}'", ((DateTime)value).ToString("yyyy-MM-dd"));

            if (type.Name == "Object")
                return "object";

            throw new InvalidOperationException("Type '" + type.Name + "' not supported");
        }
    }
}
