using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDDSharp.DDS;
using OpenDDSharp.OpenDDS.DCPS;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class DomainParticipantTest
    {
        [TestMethod]
        [TestCategory("DomainParticipant")]
        public void TestDomainId()
        {
            DomainParticipantFactory dpf = ParticipantService.Instance.GetDomainParticipantFactory();
            DomainParticipant participant = dpf.CreateParticipant(42);

            Assert.IsNotNull(participant);
            Assert.AreEqual(42, participant.DomainId);

            dpf.DeleteParticipant(participant);
            ParticipantService.Instance.Shutdown();
        }
    }
}
