namespace NetMq_Test
{
    internal class Program
    {
        public static CancellationTokenSource cts = new CancellationTokenSource();
               
        static async Task Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Server server = new();
                    Task serverTask = Task.Run(() => server.ServerTranceiver(cts.Token), cts.Token);
                    await serverTask;
                }
                else
                {
                    Client client = new();
                    Task clientTask = Task.Run(() => client.ClientTranceiver(cts.Token), cts.Token);
                    await clientTask;
                }
            }
            finally
            {
                cts.Dispose();
            }       
        }
    }
}
