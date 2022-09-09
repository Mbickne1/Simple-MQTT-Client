using Controller;

namespace Program {
    public class Program {
        static void Main(string[] args) {
            //Create and start the controller.
            MqttController controller = new MqttController();

            controller.Start();
            
            //Exit Simulation Whenever a Key is Pressed
            Console.ReadLine();
        }

    }
}