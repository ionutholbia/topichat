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
        readonly string clientId;

        public BrokerConnection(string clientId)
        {
            this.mqttClient = new MqttClient(BrokerUrl);
            this.clientId = clientId;

            this.mqttClient.MqttMsgPublishReceived += MqttMsgPublishReceived;
            this.mqttClient.Connect(clientId);
            if (this.mqttClient.IsConnected)
            {
                this.mqttClient.Subscribe(new string[] { $"{MqttTopicPrefix}/{this.clientId}/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
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
                    $"{MqttTopicPrefix}/{receiver.PhoneNumber}/{message.ConversationId}/{message.Topic}/{receiversStrings}/{this.clientId}",
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
                receivers.Add(new Contact { PhoneNumber = phoneNumber });
            }

            return receivers;
        }

        Message DecodeMessage(MqttMsgPublishEventArgs eventArgs)
        {
            var topicLevels = SplitString(eventArgs.Topic, '/');
            return DecodeGroupMessage(topicLevels, Encoding.UTF8.GetString(eventArgs.Message, 0, eventArgs.Message.Length));
        }

        Message DecodeGroupMessage(List<string> topicLevels, string text)
        {
            var receivers = DecodeOtherReceivers(topicLevels[4]);

            return new Message
            {
                Text = text,
                ConversationId = topicLevels[2],
                Topic = topicLevels[3],
                Receivers = receivers,
                Sender = new Contact { PhoneNumber = topicLevels[5] }
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
