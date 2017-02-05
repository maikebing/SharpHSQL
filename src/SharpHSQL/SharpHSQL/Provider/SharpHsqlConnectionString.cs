#region License

/*
 * SharpHsqlConnectionString.cs
 *
 * Copyright (c) 2004, Andres G Vettori
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * Redistributions of source code must retain the above copyright notice, this
 * list of conditions and the following disclaimer.
 *
 * Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation
 * and/or other materials provided with the distribution.
 *
 * Neither the name of the HSQL Development Group nor the names of its
 * contributors may be used to endorse or promote products derived from this
 * software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * This package is based on HypersonicSQL, originally developed by Thomas Mueller.
 *
 * C# SharpHsql ADO.NET Provider by Andrés G Vettori.
 * http://workspaces.gotdotnet.com/sharphsql
 */

#endregion

using System;
using System.Globalization;

namespace System.Data.Hsql
{
    /// <summary>
    /// Helper static class for building SharpHsql connection strings.
    /// </summary>
    internal sealed class SharpHsqlConnectionString
    {
        private readonly string _connectionString;

        /// <summary>
        /// Class used internally for comparisons.
        /// </summary>
        private static readonly CompareInfo InvariantComparer;

        internal const string initialCatalogKey = "initial catalog";
        internal const string databaseKey = "database";
        internal const string userIdKey = "user id";
        internal const string uidKey = "uid";
        internal const string pwdKey = "pwd";
        internal const string passwordKey = "password";

        /// <summary>
        /// Database name.
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// User password.
        /// </summary>
        public string UserPassword { get; private set; }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static SharpHsqlConnectionString()
        {
            InvariantComparer = CultureInfo.InvariantCulture.CompareInfo;
        }

        /// <summary>
        /// Creates a new <see cref="SharpHsqlConnectionString"/> object
        /// using a connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        internal SharpHsqlConnectionString(string connectionString)
        {
            _connectionString = string.Empty;
            Database = string.Empty;
            UserName = string.Empty;
            UserPassword = string.Empty;

            if (string.IsNullOrEmpty(connectionString) || connectionString.Trim().Length == 0)
                throw new ArgumentNullException(nameof(connectionString));

            var pairs = connectionString.Split(';');

            if (pairs.Length < 3)
                throw new ArgumentException("The connection string is invalid.", nameof(connectionString));

            for (var i = 0; i < pairs.Length; i++)
            {
                if (pairs[i].Trim() == string.Empty)
                    continue;

                var pair = pairs[i].Split('=');

                if (pair.Length != 2)
                    throw new ArgumentException("The connection string has an invalid parameter.", nameof(connectionString));

                var key = pair[0].ToLower().Trim();
                var value = pair[1].ToLower().Trim();

                if (InvariantComparer.Compare(key, initialCatalogKey) == 0 || InvariantComparer.Compare(key, databaseKey) == 0)
                {
                    Database = value;
                }

                if (InvariantComparer.Compare(key, userIdKey) == 0 || InvariantComparer.Compare(key, uidKey) == 0)
                {
                    UserName = value;
                }

                if (InvariantComparer.Compare(key, passwordKey) == 0 || InvariantComparer.Compare(key, pwdKey) == 0)
                {
                    UserPassword = value;
                }
            }

            if (Database == string.Empty)
                throw new ArgumentException("Database parameter is invalid in connection string.", nameof(connectionString));

            if (UserName == string.Empty)
                throw new ArgumentException("UserName parameter is invalid in connection string.", nameof(connectionString));

            _connectionString = connectionString;
        }

        public override string ToString()
        {
            return _connectionString;
        }
    }
}