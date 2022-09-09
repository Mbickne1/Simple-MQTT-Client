using Controller.Service;
using System.Text.Json;

namespace Controller {
    public class MqttController  {

        private MqttService mqttService;

        public MqttController() {
            mqttService = default!;
        }

        //Starts an MqttClient that Simulates Being Both A Publisher and A Subscriber
        public async void Start() {
            mqttService = new MqttService();
            mqttService.CreateClient();
            mqttService.CreateClientOptions("mbickne1", "localhost", 5004);

            mqttService.AttachAsyncEvents();
            
            await mqttService.Run();
            await mqttService.SubscribeToTopic("/topicA");

            while(true) {
                  string json = JsonSerializer.Serialize(new { message = "Message From The Publisher", sent = DateTime.UtcNow });
                  await mqttService.PublishMessage("/topicA", json);

                  await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}