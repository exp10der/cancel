using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace cancel
{
    internal class Program
    {
        private static void Main() => MainAsync().Wait();

        public static async Task MainAsync()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5));
            var token = cts.Token;
            Task task = null;

            try
            {
                //task = Task.Run(() =>Foo(token));
                task = Task.Run(() => Foo(token), token);


                await task; // Без авейта эксепшен не прокинется нету контекста
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine(task?.Status);
                Console.WriteLine(e);
                Debugger.Break();
            }
            catch (Exception e)
            {
                Console.WriteLine(task?.Status);
                Console.WriteLine(e);
                throw;
            }

            Console.ReadKey();
        }

        private static async Task Foo(CancellationToken ct)
        {
            for (var i = 0; i < 10; i++)
            {
                ct.ThrowIfCancellationRequested();
                Console.WriteLine(i);
                await Task.Delay(1000);
            }
        }
    }
}