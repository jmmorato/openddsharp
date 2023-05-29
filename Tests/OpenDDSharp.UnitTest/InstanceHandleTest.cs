/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 - 2021 Jose Morato

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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;

namespace OpenDDSharp.UnitTest
{
    /// <summary>
    /// <see cref="InstanceHandle"/> unit test class.
    /// </summary>
    [TestClass]
    public class InstanceHandleTest
    {
        #region Constants
        private const string TEST_CATEGORY = "InstanceHandle";
        #endregion

        #region Test Methods
        /// <summary>
        /// Test <see cref="InstanceHandle"/> equality.
        /// </summary>
        [TestMethod]
        [TestCategory(TEST_CATEGORY)]
        public void TestEquality()
        {
            // Initialize
            InstanceHandle handle = 1;
            InstanceHandle equalHandle = 1;

            // Test with null parameter
            bool result = handle.Equals(null);
            Assert.IsFalse(result);

            // Test with other object type
            result = handle.Equals("string");
            Assert.IsFalse(result);

            // Test true
            Assert.IsTrue(handle.Equals(equalHandle));
            Assert.IsTrue(handle.Equals(1));
            Assert.IsTrue(handle == equalHandle);
            Assert.IsTrue(handle != InstanceHandle.HandleNil);

            // Test false
            Assert.IsFalse(handle.Equals(InstanceHandle.HandleNil));
            Assert.IsFalse(handle.Equals(0));
            Assert.IsFalse(handle != equalHandle);
            Assert.IsFalse(handle == InstanceHandle.HandleNil);

            // Test GetHashCode
            Assert.AreEqual(1, handle.GetHashCode());
        }
        #endregion
    }
}