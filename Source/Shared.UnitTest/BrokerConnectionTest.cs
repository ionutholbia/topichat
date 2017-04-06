using System.Text;
using NUnit.Framework;
using uPLibrary.Networking.M2Mqtt;
using Moq;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Topichat.Core.UnitTest
{
    [TestFixture]
    public class BrokerConnectionTest
    {
        BrokerConnection brokerConnection;
        Mock<MqttClient> mqttClientMock;

        [SetUp]
        public void SetUp()
        {
            this.mqttClientMock = new Mock<MqttClient>();

            this.brokerConnection = new BrokerConnection("0040744360800");
        }

        [Test]
        public void OneToOne_MessageReceived()
        {
            var message = new Message();
            this.brokerConnection.MessageReceived += (obj) =>
            {
                message = obj;
            };

            this.mqttClientMock.Raise(mock => mock.MqttMsgPublishReceived += (sender, e) => { }, 
                                      null, 
                                      new MqttMsgPublishEventArgs(
                                          "message/0040744360800/0040740660810/VacantaLaMunte",
                                          Encoding.UTF8.GetBytes("Salut"),
                                          false,
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                          false));

            Assert.AreEqual("Salut", message.Text);
            Assert.AreEqual("VacantaLaMunte", message.Topic);
            Assert.AreEqual("0040740660810", message.Sender.PhoneNumber);
            Assert.AreEqual(1, message.Receivers.Count);
            Assert.AreEqual("0040744360800", message.Receivers[0].PhoneNumber);
        }

        [Test]
        public void OneToOne_InvalidMessage()
        {
            var message = new Message();
            this.brokerConnection.MessageReceived += (obj) =>
            {
                message = obj;
            };

            this.mqttClientMock.Raise(mock => mock.MqttMsgPublishReceived += (sender, e) => { },
                                      null,
                                      new MqttMsgPublishEventArgs(
                                          "message/0040744360800/",
                                          Encoding.UTF8.GetBytes("Salut"),
                                          false,
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                          false));

            Assert.IsNull(message.Text);
            Assert.IsNull(message.Topic);
            Assert.IsNull(message.Sender);
            Assert.IsNull(message.Receivers);
        }

        [Test]
        public void MessageReceived_One2One()
        {
        	var message = new Message();
        	this.brokerConnection.MessageReceived += (obj) =>
        	{
        		message = obj;
        	};

        	this.mqttClientMock.Raise(mock => mock.MqttMsgPublishReceived += (sender, e) => { },
        							  null,
        							  new MqttMsgPublishEventArgs(
        								  "message/0040744360800/14c9ed4e-3371-4533-91bf-af63e0e3a88a/VacantaLaMunte/111/999",
        								  Encoding.UTF8.GetBytes("Salut"),
        								  false,
        								  MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
        								  false));

        	Assert.AreEqual("Salut", message.Text);
        	Assert.AreEqual("VacantaLaMunte", message.Topic);
        	Assert.AreEqual("999", message.Sender.PhoneNumber);
        	Assert.AreEqual(1, message.Receivers.Count);
        	Assert.AreEqual("111", message.Receivers[0].PhoneNumber);
        	Assert.AreEqual("0040744360800", message.Receivers[4].PhoneNumber);
        }

        [Test]
        public void MessageReceived_Group()
        {
            var message = new Message();
            this.brokerConnection.MessageReceived += (obj) =>
            {
                message = obj;
            };

            this.mqttClientMock.Raise(mock => mock.MqttMsgPublishReceived += (sender, e) => { },
                                      null,
                                      new MqttMsgPublishEventArgs(
                                          "message/0040744360800/14c9ed4e-3371-4533-91bf-af63e0e3a88a/VacantaLaMunte/111-222-333-444/999",
                                          Encoding.UTF8.GetBytes("Salut"),
                                          false,
                                          MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                                          false));

            Assert.AreEqual("Salut", message.Text);
            Assert.AreEqual("VacantaLaMunte", message.Topic);
            Assert.AreEqual("999", message.Sender.PhoneNumber);
            Assert.AreEqual(5, message.Receivers.Count);
            Assert.AreEqual("111", message.Receivers[0].PhoneNumber);
            Assert.AreEqual("222", message.Receivers[1].PhoneNumber);
            Assert.AreEqual("333", message.Receivers[2].PhoneNumber);
            Assert.AreEqual("444", message.Receivers[3].PhoneNumber);
            Assert.AreEqual("0040744360800", message.Receivers[4].PhoneNumber);
        }
    }
}
