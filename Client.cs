using NetMQ;
using NetMQ.Sockets;
using System.Text;

namespace NetMq_Test
{
    public class Client
    {
        string url = "tcp://127.0.0.1:1234";

        public async Task ClientTranceiver(CancellationToken cancellationToken)
        {
            using (DealerSocket dealerSocket = new DealerSocket())
            {
                try
                {
                    dealerSocket.Connect(url); // Подключаемся к серверу
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Введите сообщение");
                        string msg = Console.ReadLine()!;
                        if (msg == null)
                        {
                            Console.WriteLine("Вы не ввели сообщение");
                            continue;
                        }

                        byte[] msgByte = Encoding.UTF8.GetBytes(msg);
                        //string msgStr = Encoding.UTF8.GetString(msgByte);

                        await Task.Run(() => dealerSocket.SendFrame(msgByte), cancellationToken);
                        Console.WriteLine($"Сообщение: {msg} отправлено на {url}");

                        var msgReplace = await Task.Run(() => dealerSocket.ReceiveMultipartMessage(), cancellationToken);
                        Console.WriteLine($"Получено сообщение {msgReplace.Last.ConvertToString()}");
                    }
                }
                catch(Exception ex) 
                {
                    Console.WriteLine($"Ошибка в работе клиента! {ex.Message}");
                }
                finally
                {
                    Console.WriteLine("Клиент завершил работу");
                }
            }            
        }
    }
}
