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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.OpenDDS.RTPS;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="RtpsDiscovery"/> unit test.
    /// </summary>
    [TestClass]
    public class RtpsDiscoveryTest
    {
        #region Constants
        private const string RTPS_DISCOVERY = "RtpsDiscoveryTest";
        private const string TEST_CATEGORY = "RtpsDiscovery";
        #endregion

        #region Test Method
        /// <summary>
        /// Test the properties default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestDefaultValues()
        {
            RtpsDiscovery disc = new RtpsDiscovery(RTPS_DISCOVERY);
            Assert.IsNotNull(disc.ResendPeriod);
            Assert.AreEqual(30, disc.ResendPeriod.Seconds);
            Assert.AreEqual(0, disc.ResendPeriod.MicroSeconds);
            Assert.AreEqual(7400, disc.PB);
            Assert.AreEqual(250, disc.DG);
            Assert.AreEqual(2, disc.PG);
            Assert.AreEqual(0, disc.D0);
            Assert.AreEqual(10, disc.D1);
            Assert.AreEqual(2, disc.DX);
            Assert.AreEqual(true, disc.SedpMulticast);
            Assert.AreEqual("0.0.0.0:0", disc.SedpLocalAddress);
            Assert.AreEqual("0.0.0.0:0", disc.SpdpLocalAddress);
            Assert.IsNotNull(disc.SpdpSendAddrs);
            Assert.AreEqual(0, disc.SpdpSendAddrs.Count());
            Assert.AreEqual("239.255.0.1:0", disc.DefaultMulticastGroup);
            Assert.AreEqual(1, disc.Ttl);
            Assert.AreEqual(string.Empty, disc.MulticastInterface);
            Assert.AreEqual(string.Empty, disc.GuidInterface);
            Assert.IsFalse(string.IsNullOrWhiteSpace(disc.Key));
        }

        /// <summary>
        /// Test the properties non-default values after calling the constructor.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNonDefaultValues()
        {
            RtpsDiscovery disc = new RtpsDiscovery(RTPS_DISCOVERY)
            {
                ResendPeriod = new TimeValue
                {
                    Seconds = 1,
                    MicroSeconds = 500000,
                },
                PB = 8400,
                DG = 500,
                PG = 4,
                D0 = 10,
                D1 = 20,
                DX = 4,
                Ttl = 2,
                SedpMulticast = false,
                SedpLocalAddress = "127.0.0.1",
                SpdpLocalAddress = "127.0.0.1",
                DefaultMulticastGroup = "239.255.42.1",
                MulticastInterface = "eth0",
                GuidInterface = "eth0",
            };

            Assert.IsNotNull(disc.ResendPeriod);
            Assert.AreEqual(1, disc.ResendPeriod.Seconds);
            Assert.AreEqual(500000, disc.ResendPeriod.MicroSeconds);
            Assert.AreEqual(8400, disc.PB);
            Assert.AreEqual(500, disc.DG);
            Assert.AreEqual(4, disc.PG);
            Assert.AreEqual(10, disc.D0);
            Assert.AreEqual(20, disc.D1);
            Assert.AreEqual(4, disc.DX);
            Assert.AreEqual(false, disc.SedpMulticast);
            Assert.AreEqual("127.0.0.1:0", disc.SedpLocalAddress);
            Assert.AreEqual("127.0.0.1:0", disc.SpdpLocalAddress);
            Assert.AreEqual("239.255.42.1:0", disc.DefaultMulticastGroup);
            Assert.AreEqual(2, disc.Ttl);
            Assert.AreEqual("eth0", disc.MulticastInterface);
            Assert.AreEqual("eth0", disc.GuidInterface);
            Assert.IsFalse(string.IsNullOrWhiteSpace(disc.Key));
        }

        /// <summary>
        /// Test the null or empty guards.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestNullOrEmptyName()
        {
            bool exception = false;
            try
            {
                _ = new RtpsDiscovery(null);
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
                _ = new RtpsDiscovery(string.Empty);
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
                _ = new RtpsDiscovery("   ");
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