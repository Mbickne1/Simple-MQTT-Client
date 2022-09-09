using Controller.Interface.Service;
using MQTTnet.Client;
using MQTTnet;
using System.Text;

namespace Controller.Service {
    public class MqttService : IMqttService {
        private MqttClientOptionsBuilder Builder;
        private IMqttClient Client;
        private MqttClientOptions Options;

        public MqttService() {
            Client = default!;
            Options = default!;
            Builder = default!;
        }

        public void CreateClient() {
            Client = new MqttFactory().CreateMqttClient();
        }

        public void CreateClientOptions(string ClientId, string Server, int Port) {
            Builder = new MqttClientOptionsBuilder()
                .WithClientId(ClientId)
                .WithTcpServer(Server, Port);

           Options = Builder.Build();
        }

        public void AttachAsyncEvents() {
            Client.ConnectedAsync += async e => await OnConnectedToServer(e);
            Client.DisconnectedAsync += async e => await OnDisconnectFromServer(e);
            Client.ApplicationMessageReceivedAsync += async msg => await ApplicationMessageReceieved(msg);
        }

        public async Task Run() {
            await Client.ConnectAsync(Options);
        }

        public Task OnConnectedToServer(MqttClientConnectedEventArgs Args) {
            Console.WriteLine("### PUBLISHER CONNECTION ESTABLISHED ###");

            return Task.CompletedTask;
        }
        public Task OnDisconnectFromServer(MqttClientDisconnectedEventArgs Args) {
            Console.WriteLine("### PUBLISHER DISCONNECTED FROM SERVER ###");

            return Task.CompletedTask;
        }
        
        public async Task PublishMessage(string Topic, string Payload) {
            MqttApplicationMessageBuilder applicationMessageBuilder = new MqttApplicationMessageBuilder()
                                    .WithTopic(Topic)
                                    .WithPayload(Payload);

            await Client.PublishAsync(applicationMessageBuilder.Build());
        }

        public async Task SubscribeToTopic(string Topic) {
            MqttClientSubscribeOptionsBuilder subscribeOptionsBuilder = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(Topic);

            MqttClientSubscribeOptions subscribeOptions = subscribeOptionsBuilder.Build();
            await Client.SubscribeAsync(
                subscribeOptions
            );
        }

          public Task ApplicationMessageReceieved(MqttApplicationMessageReceivedEventArgs Msg) {
            if(Msg.ClientId != null) {
                var payloadText = Encoding.UTF8.GetString(
                    Msg?.ApplicationMessage?.Payload ?? Array.Empty<byte>());

                Console.WriteLine($"Received msg: {payloadText} ClientId: {Msg.ClientId}");
            }

            return Task.CompletedTask;
        }
    }
}