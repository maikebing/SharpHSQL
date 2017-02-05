using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("SharpHsql")]
[assembly: AssemblyDescription("C# port of Java HSQLDB")]
#if DEBUG 
[assembly: AssemblyConfiguration("DEBUG")]
[assembly: InternalsVisibleTo("SharpHSQL.UnitTests, PublicKey=" + 
"0024000004800000940000000602000000240000525341310004000001000100a3ef1d985c3014" +
"94fde2e243a145b04b579f3ec8ec97006a8900c46a1cb35bd1d01bb9f8c5161cf9042116ce71e0" +
"6e98c00c476b62c4394ae9f155dc2f51a41568a4bd0fe8218a27a0040c98c75833d76fac828c64" +
"fbe53bd179a1b1233906b78cb77d5b253907bc528c372bcb8b280b42f432a4e9907c39a681c67a" +
"2716a0d1")]

#else
[assembly: AssemblyConfiguration("RELEASE")]
#endif
[assembly: AssemblyCompany("NTS")]
[assembly: AssemblyProduct("SharpHsql")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("All trademarks are property of their respective holders.")]
[assembly: AssemblyCulture("")]		

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Revision
//      Build Number
//
// You can specify all the value or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]

//
// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified - the assembly cannot be signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. 
//   (*) If the key file and a key name attributes are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP - that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the file is installed into the CSP and used.
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
//
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

