using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Topichat.Core;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Topichat.Shared
{
    public sealed class BrokerConnection : IBrokerConnection, IDisposable
    {
        const string BrokerUrl = "ec2-54-212-229-1.us-west-2.compute.amazonaws.com";

        const string MqttTopicPrefix = "message";

        readonly MqttClient mqttClient;
        readonly ContactManager contactManager;

        public BrokerConnection(ContactManager contactManager)
        {
            this.contactManager = contactManager;
            this.mqttClient = new MqttClient(BrokerUrl);

            this.mqttClient.MqttMsgPublishReceived += MqttMsgPublishReceived;
            this.mqttClient.Connect(contactManager.Me.PhoneNumber);
            if (this.mqttClient.IsConnected)
            {
                this.mqttClient.Subscribe(new string[] { $"{MqttTopicPrefix}/{contactManager.Me.PhoneNumber}/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            }
        }

        public Action<Message> MessageReceived { get; set;}

        public async Task SendMessage(Message message)
        {
            var receiversStrings = "";
            foreach (var receiver in message.Receivers)
            {
                receiversStrings += receiver.PhoneNumber + "-";
            }
            receiversStrings = receiversStrings.Remove(receiversStrings.Length - 1);

            foreach (var receiver in message.Receivers)
            {
                await Task.Run(() => this.mqttClient.Publish(
                    $"{MqttTopicPrefix}/{receiver.PhoneNumber}/{message.TopicId}/{message.Topic}/{receiversStrings}/{this.contactManager.Me.PhoneNumber}",
                    Encoding.UTF8.GetBytes(message.Text),
                    MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                    false));
            }
        }

        void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs eventArgs)
        {
            try
            {
                var message = DecodeMessage(eventArgs);
                MessageReceived?.Invoke(message);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to decode message. Exception: {ex} Message: {eventArgs}");

            }
        }

        List<string> SplitString(string data, char separator)
        {
            var substrings = new List<string>();

            var startIndex = 0;
            var indexSeparator = data.IndexOf(separator);
            if (indexSeparator == -1)
            {
                return new List<string> { data };
            }

            while (indexSeparator != -1)
            {
                substrings.Add(data.Substring(startIndex, indexSeparator - startIndex));
                startIndex = indexSeparator + 1;
                indexSeparator = data.IndexOf(separator, startIndex + 1);
            }
            substrings.Add(data.Substring(startIndex, data.Length - startIndex));

            return substrings;
        }

        List<Contact> DecodeOtherReceivers(string data)
        {
            var receiversPhoneNumber = SplitString(data, '-');

            var receivers = new List<Contact>();
            foreach (var phoneNumber in receiversPhoneNumber)
            {
                receivers.Add(this.contactManager.FindContact(phoneNumber));
            }

            return receivers;
        }

        Message DecodeMessage(MqttMsgPublishEventArgs eventArgs)
        {
            var topicLevels = SplitString(eventArgs.Topic, '/');

			return new Message
			{
				Text = Encoding.UTF8.GetString(eventArgs.Message, 0, eventArgs.Message.Length),
				TopicId = topicLevels[2],
				Topic = topicLevels[3],
				Receivers = DecodeOtherReceivers(topicLevels[4]),
                Sender = this.contactManager.FindContact(topicLevels[5]),
				TimeStamp = DateTime.Now
			};
		}

        public void Dispose()
        {
            if (this.mqttClient != null)
            {
                this.mqttClient.Disconnect();
            }
        }
    }
}
