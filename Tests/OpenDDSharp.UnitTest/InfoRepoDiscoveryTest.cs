/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2022 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="InfoRepoDiscovery"/> unit test class.
    /// </summary>
    [TestClass]
    public class InfoRepoDiscoveryTest
    {
        #region Constants
        private const string INFOREPO_DISCOVERY = "InfoRepoTest";
        private const string TEST_CATEGORY = "InfoRepoDiscovery";
        #endregion

        #region Test Method
        /// <summary>
        /// Test <see cref="InfoRepoDiscovery"/> constructor default values.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDefaultValues()
        {
            var infoRepo = new InfoRepoDiscovery(INFOREPO_DISCOVERY, "file://repo.ior");
            Assert.AreEqual(INFOREPO_DISCOVERY, infoRepo.Key);
            Assert.AreEqual(string.Empty, infoRepo.BitTransportIp);
            Assert.AreEqual(0, infoRepo.BitTransportPort);
        }

        /// <summary>
        /// Test <see cref="InfoRepoDiscovery"/> constructor non default values.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            var infoRepo = new InfoRepoDiscovery(INFOREPO_DISCOVERY, "file://repo.ior")
            {
                BitTransportIp = "127.0.0.1",
                BitTransportPort = 42,
            };

            Assert.AreEqual(INFOREPO_DISCOVERY, infoRepo.Key);
            Assert.AreEqual("127.0.0.1", infoRepo.BitTransportIp);
            Assert.AreEqual(42, infoRepo.BitTransportPort);
        }

        /// <summary>
        /// Test <see cref="InfoRepoDiscovery"/> constructor guards for null or empty parameters.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNullOrEmptyParameters()
        {
            var exception = false;
            try
            {
                _ = new InfoRepoDiscovery(null, "file://repo.ior");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                _ = new InfoRepoDiscovery(string.Empty, "file://repo.ior");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                _ = new InfoRepoDiscovery("   ", "file://repo.ior");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                _ = new InfoRepoDiscovery(INFOREPO_DISCOVERY, null);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                _ = new InfoRepoDiscovery(INFOREPO_DISCOVERY, string.Empty);
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);

            exception = false;
            try
            {
                _ = new InfoRepoDiscovery(INFOREPO_DISCOVERY, "   ");
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
            Assert.IsTrue(exception);
        }
        #endregion
    }
}