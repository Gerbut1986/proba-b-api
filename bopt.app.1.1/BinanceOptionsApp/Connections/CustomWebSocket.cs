namespace BinanceOptionsApp.Connections
{
    using Helpers;
    using System;
    using WebSocketSharp;

    public class CustomWebSocket
    {
        private WebSocket ws1, ws2;

        public void Connect1(string dataSend)
        {
            // IP адреса і порт першого сервера
            string server1IP = "95.69.216.125"; //IP_адреса_сервера_1
            int server1Port = 1234; // Порт першого сервера

            // IP адреса і порт другого сервера
            string server2IP = "194.146.38.45"; //IP_адреса_сервера_2
            int server2Port = 5678; // Порт другого сервера

            // Створення об'єкту WebSocket для підключення до першого сервера
            using (ws1 = new WebSocket($"ws://{server1IP}:{server1Port}/"))
            {
                // Обробник події відкриття з'єднання з першим сервером
                ws1.OnOpen += (sender, e) =>
                {
                    // Console.WriteLine("Підключено до першого сервера!");
                    // Тут можна відправляти дані до першого сервера, якщо потрібно

                    ws1.Send("Привіт я перший сервер!");// Відправка повідомлення до першого сервера
                };

                // Обробник події отримання повідомлення від першого сервера
                ws1.OnMessage += (sender, e) =>
                {
                    // Console.WriteLine($"Повідомлення від першого сервера: {e.Data}");
                    var receiveMessage = e.Data;// Обробка отриманого повідомлення
                };

                // Підключення до першого сервера
                ws1.Connect();             
            }
            // Створення об'єкту WebSocket для підключення до другого сервера
            using (ws2 = new WebSocket($"ws://{server2IP}:{server2Port}/"))
            {
                // Обробник події відкриття з'єднання з другим сервером
                ws2.OnOpen += (sender, e) =>
                {
                    //Console.WriteLine("Підключено до другого сервера!");
                    // Тут можна відправляти дані до другого сервера, якщо потрібно
                };

                // Обробник події отримання повідомлення від другого сервера
                ws2.OnMessage += (sender, e) =>
                {
                    string data = e.Data;
                    // new _().W;
                    //  Console.WriteLine($"Повідомлення від другого сервера: {e.Data}");
                    // Обробка отриманого повідомлення
                };

                // Підключення до другого сервера
                ws2.Connect();

                // Тут можна реалізувати логіку обміну даними між серверами через веб-сокети

                // Console.ReadLine(); // Зупинка програми, щоб підключення залишилися відкритими
            }
        }
    }
}
