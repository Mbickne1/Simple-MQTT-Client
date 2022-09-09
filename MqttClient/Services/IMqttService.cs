using MQTTnet.Client;

namespace Controller.Interface.Service {
    interface IMqttService {

        void CreateClient();
        void  CreateClientOptions(string clientId, string server, int port);

        void AttachAsyncEvents();

        Task Run();

        Task OnConnectedToServer(MqttClientConnectedEventArgs Args);
        Task OnDisconnectFromServer(MqttClientDisconnectedEventArgs Args);
    }
}