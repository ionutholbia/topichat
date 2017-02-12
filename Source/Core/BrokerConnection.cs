using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Chatopia.Core
{
    public class BrokerConnection
    {
        const string MqttTopicPrefix = "message";
        const string BrokerUrl = "";

        readonly IMqttClient mqttClient;
        readonly string clientId;

        public BrokerConnection(IMqttClient mqttClient, string clientId)
        {
            this.mqttClient = mqttClient;
            this.clientId = clientId;

            this.mqttClient.MqttMsgPublishReceived += MqttMsgPublishReceived;

            this.mqttClient.Connect(clientId);

            this.mqttClient.Subscribe(new string[] { $"{MqttTopicPrefix}/{this.clientId}" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }

        public Action<Message> MessageReceived;

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
                throw new Exception($"Invalid mqtt topic {data}.");
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

            if (topicLevels.Count == 6)
            {
                return DecodeGroupMessage(topicLevels, Encoding.UTF8.GetString(eventArgs.Message, 0, eventArgs.Message.Length));
            }

            if (topicLevels.Count != 4)
            {
                throw new Exception($"Invalid mqtt message structure {eventArgs}.");
            }

            return new Message
            {
                Text = Encoding.UTF8.GetString(eventArgs.Message, 0, eventArgs.Message.Length),
                Topic = topicLevels[3],
                Receivers = new List<Contact> { new Contact { PhoneNumber = topicLevels[1] } },
                Sender = new Contact { PhoneNumber = topicLevels[2] }
            };
        }

        Message DecodeGroupMessage(List<string> topicLevels, string text)
        {
            var receivers = DecodeOtherReceivers(topicLevels[4]);
            receivers.Add(new Contact { PhoneNumber = topicLevels[1] });

            return new Message
            {
                Text = text,
                GroupId = topicLevels[2],
                Topic = topicLevels[3],
                Receivers = receivers,
                Sender = new Contact { PhoneNumber = topicLevels[5] }
            };
        }

        public async Task SendMessage(Message message)
        {
            if (message.Receivers.Count > 1)
            {
                var receiversStrings = "";
                foreach (var receiver in message.Receivers)
                {
                    receiversStrings += receiver.PhoneNumber + "-";
                }
                receiversStrings.Remove(receiversStrings.Length - 1);

                foreach (var receiver in message.Receivers)
                {
                    await Task.Run(() => this.mqttClient.Publish(
                        $"{MqttTopicPrefix}/{receiver.PhoneNumber}/{message.GroupId}/{message.Topic}/{receiversStrings}/{this.clientId}",
                        Encoding.UTF8.GetBytes(message.Text),
                        MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                        false));                   
                }
            }
            else
            {
                await Task.Run(() => this.mqttClient.Publish(
                    $"{MqttTopicPrefix}/{message.Receivers[0].PhoneNumber}/{this.clientId}/{message.Topic}",
                    Encoding.UTF8.GetBytes(message.Text),
                    MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
                    false));
            }                
        }
    }
}
