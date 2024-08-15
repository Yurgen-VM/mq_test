using NetMQ;
using NetMQ.Sockets;
using System.Text;

namespace NetMq_Test
{
    public class Server
    {
        private RouterSocket _routerSocket = new RouterSocket();        
        public string _urlServerBind;

        public Server()
        {
            _urlServerBind = "tcp://*:1234";
        }

        public async Task ServerTranceiver(CancellationToken cancellationToken)
        {            
            try
            {
                _routerSocket.Bind(_urlServerBind);
                Console.WriteLine("Сервер ожидает сообщений");

                while (!cancellationToken.IsCancellationRequested)
                {
                    var msg = await Task.Run(() => _routerSocket.ReceiveMultipartMessage(), cancellationToken);

                    string msgStr = Encoding.UTF8.GetString(msg.Last.Buffer); // Ковертируем массив байт в строку
                    
                    Console.WriteLine($"Получено сообщение: {msgStr}"); 
                    var responseMessage = new NetMQMessage();
                    responseMessage.Append(msg.First); // Добавялем в ответное сообщение идентификатор клиента
                    responseMessage.Append("Confirm"); // Добавялем в ответное сообщение текст сообщения
                    _routerSocket.SendMultipartMessage(responseMessage);
                }
            }
            finally 
            {
                _routerSocket.Dispose();
                Console.WriteLine("Сервер завершил работу ");
            }
        }
    }



}
