using System;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace uPLibrary.Networking.M2Mqtt
{
#if BROKER
    #region Constants ...

        // thread names
        private const string RECEIVE_THREAD_NAME = "ReceiveThread";
        private const string RECEIVE_EVENT_THREAD_NAME = "DispatchEventThread";
        private const string PROCESS_INFLIGHT_THREAD_NAME = "ProcessInflightThread";
        private const string KEEP_ALIVE_THREAD = "KeepAliveThread";

    #endregion
#endif

    /// <summary>
    /// Delagate that defines event handler for PUBLISH message received
    /// </summary>
    public delegate void MqttMsgPublishEventHandler(object sender, MqttMsgPublishEventArgs e);

    /// <summary>
    /// Delegate that defines event handler for published message
    /// </summary>
    public delegate void MqttMsgPublishedEventHandler(object sender, MqttMsgPublishedEventArgs e);

    /// <summary>
    /// Delagate that defines event handler for subscribed topic
    /// </summary>
    public delegate void MqttMsgSubscribedEventHandler(object sender, MqttMsgSubscribedEventArgs e);

    /// <summary>
    /// Delagate that defines event handler for unsubscribed topic
    /// </summary>
    public delegate void MqttMsgUnsubscribedEventHandler(object sender, MqttMsgUnsubscribedEventArgs e);

#if BROKER
        /// <summary>
        /// Delagate that defines event handler for SUBSCRIBE message received
        /// </summary>
        public delegate void MqttMsgSubscribeEventHandler(object sender, MqttMsgSubscribeEventArgs e);

        /// <summary>
        /// Delagate that defines event handler for UNSUBSCRIBE message received
        /// </summary>
        public delegate void MqttMsgUnsubscribeEventHandler(object sender, MqttMsgUnsubscribeEventArgs e);

        /// <summary>
        /// Delagate that defines event handler for CONNECT message received
        /// </summary>
        public delegate void MqttMsgConnectEventHandler(object sender, MqttMsgConnectEventArgs e);

        /// <summary>
        /// Delegate that defines event handler for client disconnection (DISCONNECT message or not)
        /// </summary>
        public delegate void MqttMsgDisconnectEventHandler(object sender, EventArgs e);
#endif

    /// <summary>
    /// Delegate that defines event handler for cliet/peer disconnection
    /// </summary>
    public delegate void ConnectionClosedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// MQTT protocol version
    /// </summary>
    public enum MqttProtocolVersion
    {
        Version_3_1 = MqttMsgConnect.PROTOCOL_VERSION_V3_1,
        Version_3_1_1 = MqttMsgConnect.PROTOCOL_VERSION_V3_1_1
    }

    public interface IMqttClient
    {
        // event for PUBLISH message received
        event MqttMsgPublishEventHandler MqttMsgPublishReceived;
        // event for published message
        event MqttMsgPublishedEventHandler MqttMsgPublished;
        // event for subscribed topic
        event MqttMsgSubscribedEventHandler MqttMsgSubscribed;
        // event for unsubscribed topic
        event MqttMsgUnsubscribedEventHandler MqttMsgUnsubscribed;
        /// <summary>
        /// Connection status between client and broker
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Client identifier
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Clean session flag
        /// </summary>
        bool CleanSession { get; }

        /// <summary>
        /// Will flag
        /// </summary>
        bool WillFlag { get; }

        /// <summary>
        /// Will QOS level
        /// </summary>
        byte WillQosLevel { get; }

        /// <summary>
        /// Will topic
        /// </summary>
        string WillTopic { get; }

        /// <summary>
        /// Will message
        /// </summary>
        string WillMessage { get; }

        /// <summary>
        /// MQTT protocol version
        /// </summary>
        MqttProtocolVersion ProtocolVersion { get; set; }

        /// <summary>
        /// Connect to broker
        /// </summary>
        /// <param name="clientId">Client identifier</param>
        /// <returns>Return code of CONNACK message from broker</returns>
        byte Connect(string clientId);

        /// <summary>
        /// Connect to broker
        /// </summary>
        /// <param name="clientId">Client identifier</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Return code of CONNACK message from broker</returns>
        byte Connect(string clientId,
                     string username,
                     string password);

        /// <summary>
        /// Connect to broker
        /// </summary>
        /// <param name="clientId">Client identifier</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="cleanSession">Clean sessione flag</param>
        /// <param name="keepAlivePeriod">Keep alive period</param>
        /// <returns>Return code of CONNACK message from broker</returns>
        byte Connect(string clientId,
            string username,
            string password,
            bool cleanSession,
            ushort keepAlivePeriod);

        /// <summary>
        /// Connect to broker
        /// </summary>
        /// <param name="clientId">Client identifier</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="willRetain">Will retain flag</param>
        /// <param name="willQosLevel">Will QOS level</param>
        /// <param name="willFlag">Will flag</param>
        /// <param name="willTopic">Will topic</param>
        /// <param name="willMessage">Will message</param>
        /// <param name="cleanSession">Clean sessione flag</param>
        /// <param name="keepAlivePeriod">Keep alive period</param>
        /// <returns>Return code of CONNACK message from broker</returns>
        byte Connect(string clientId,
            string username,
            string password,
            bool willRetain,
            byte willQosLevel,
            bool willFlag,
            string willTopic,
            string willMessage,
            bool cleanSession,
            ushort keepAlivePeriod);

        /// <summary>
        /// Disconnect from broker
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Subscribe for message topics
        /// </summary>
        /// <param name="topics">List of topics to subscribe</param>
        /// <param name="qosLevels">QOS levels related to topics</param>
        /// <returns>Message Id related to SUBSCRIBE message</returns>
        ushort Subscribe(string[] topics, byte[] qosLevels);

        /// <summary>
        /// Unsubscribe for message topics
        /// </summary>
        /// <param name="topics">List of topics to unsubscribe</param>
        /// <returns>Message Id in UNSUBACK message from broker</returns>
        ushort Unsubscribe(string[] topics);

        /// <summary>
        /// Publish a message asynchronously (QoS Level 0 and not retained)
        /// </summary>
        /// <param name="topic">Message topic</param>
        /// <param name="message">Message data (payload)</param>
        /// <returns>Message Id related to PUBLISH message</returns>
        ushort Publish(string topic, byte[] message);

        /// <summary>
        /// Publish a message asynchronously
        /// </summary>
        /// <param name="topic">Message topic</param>
        /// <param name="message">Message data (payload)</param>
        /// <param name="qosLevel">QoS Level</param>
        /// <param name="retain">Retain flag</param>
        /// <returns>Message Id related to PUBLISH message</returns>
        ushort Publish(string topic, byte[] message, byte qosLevel, bool retain);
    }
}
