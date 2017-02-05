using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpHsql;

namespace SharpHSQL.UnitTests
{
    [TestFixture]
    class AccessTests
    {
        [Test]
        public void GetAccessRightFromStringTest()
        {
            Assert.AreEqual(AccessType.All, Access.GetRight("ALL"));
            Assert.AreEqual(AccessType.Select, Access.GetRight("SELECT"));
            Assert.AreEqual(AccessType.Update, Access.GetRight("UPDATE"));
            Assert.AreEqual(AccessType.Delete, Access.GetRight("DELETE"));
            Assert.AreEqual(AccessType.Insert, Access.GetRight("INSERT"));
        }

        [Test]
        public void GetAccessRightFromFlagTest()
        {
            Assert.AreEqual("ALL", Access.GetRight(AccessType.All));
            Assert.AreEqual(null, Access.GetRight(AccessType.None));
            Assert.AreEqual("SELECT", Access.GetRight(AccessType.Select));
            Assert.AreEqual("UPDATE", Access.GetRight(AccessType.Update));
            Assert.AreEqual("DELETE", Access.GetRight(AccessType.Delete));
            Assert.AreEqual("INSERT", Access.GetRight(AccessType.Insert));

            Assert.AreEqual("SELECT,INSERT", Access.GetRight(AccessType.Select | AccessType.Insert));
            Assert.AreEqual("SELECT,UPDATE", Access.GetRight(AccessType.Select | AccessType.Update));
            Assert.AreEqual("SELECT,DELETE", Access.GetRight(AccessType.Select | AccessType.Delete));

            Assert.AreEqual("SELECT,UPDATE", Access.GetRight(AccessType.Update | AccessType.Select));
            Assert.AreEqual("UPDATE,INSERT", Access.GetRight(AccessType.Update | AccessType.Insert));
            Assert.AreEqual("UPDATE,DELETE", Access.GetRight(AccessType.Update | AccessType.Delete));

            Assert.AreEqual("SELECT,DELETE", Access.GetRight(AccessType.Delete | AccessType.Select));
            Assert.AreEqual("DELETE,INSERT", Access.GetRight(AccessType.Delete | AccessType.Insert));
            Assert.AreEqual("UPDATE,DELETE", Access.GetRight(AccessType.Delete | AccessType.Update));

            Assert.AreEqual("SELECT,INSERT", Access.GetRight(AccessType.Insert | AccessType.Select));
            Assert.AreEqual("UPDATE,INSERT", Access.GetRight(AccessType.Insert | AccessType.Update));
            Assert.AreEqual("DELETE,INSERT", Access.GetRight(AccessType.Insert | AccessType.Delete));
        }

        [Test]
        public void CreateUserTest()
        {
            var access = new Access();
            var user = access.CreateUser("usr", "pwd", false);

            Assert.AreEqual(2, access.GetUsers().Count);
            Assert.AreEqual("usr", user.Name);
            Assert.AreEqual("pwd", user.Password);
            Assert.AreEqual(false, user.IsAdmin);
        }

        [Test]
        public void WhenCreateExistingUser_ShouldThrownException()
        {
            var access = new Access();
            access.CreateUser("usr", "pwd", false);

            try
            {
                access.CreateUser("usr", "pwd2", false);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Equals("S1000 User already exists: usr"))
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void DropUserTest()
        {
            var access = new Access();
            var user = access.CreateUser("usr", "pwd", true);

            access.DropUser("usr");

            Assert.AreEqual(null, access.GetUsers()[1]);
            Assert.AreEqual(null, user.Rights);
            Assert.AreEqual(false, user.IsAdmin);
        }

        [Test]
        public void WhenDropPublicUser_ShouldThrownException()
        {
            var access = new Access();

            try
            {
                access.DropUser("PUBLIC");
            }
            catch (Exception ex)
            {
                if (!ex.Message.Equals("S1000 Access is denied"))
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void WhenDropNotExistingUser_ShouldThrownException()
        {
            var access = new Access();

            try
            {
                access.DropUser("usr");
            }
            catch (Exception ex)
            {
                if (!ex.Message.Equals("S1000 User not found: usr"))
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void GetUserTest()
        {
            var access = new Access();
            var user = access.CreateUser("usr", "pwd", true);

            var resultUser = access.GetUser(user.Name, user.Password);

            Assert.AreEqual(user.Name, resultUser.Name);
            Assert.AreEqual(user.Password, resultUser.Password);
            Assert.AreEqual(user.IsAdmin, resultUser.IsAdmin);
        }

        [Test]
        public void WhenGetPublicUser_ShouldThrownException()
        {
            var access = new Access();

            try
            {
                access.GetUser("PUBLIC", "");
            }
            catch (Exception ex)
            {
                if (!ex.Message.Equals("S1000 Access is denied"))
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void WhenGetUserAndWrongName_ShouldThrownException()
        {
            var access = new Access();

            try
            {
                access.GetUser("usr", "");
            }
            catch (Exception ex)
            {
                if (!ex.Message.Equals("S1000 User not found: usr"))
                {
                    Assert.Fail(ex.Message);
                }
            }
        }

        [Test]
        public void WhenGetUserAndWrongPassword_ShouldThrownException()
        {
            var access = new Access();
            access.CreateUser("usr", "pwd", true);

            try
            {
                access.GetUser("usr", "qwerty");
            }
            catch (Exception ex)
            {
                if (!ex.Message.Equals("S1000 Access is denied"))
                {
                    Assert.Fail(ex.Message);
                }
            }
        }
    }
}
