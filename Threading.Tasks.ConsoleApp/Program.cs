using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Threading.Tasks.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");

            CallSeveralTimes();

            Console.ReadKey();
        }

        #region Performance testing
        public static async void CallSeveralTimes()
        {
            var time = new Stopwatch();
            
            while (true)
            {
                //time.Start();
                //await GetValueTaskAsync();
                //time.Stop();

                //Console.WriteLine($"ElapsedTicks using {nameof(GetValueTaskAsync)}: {time.ElapsedTicks}");
                //time.Reset();

                time.Start();
                await GetTaskAsync();
                time.Stop();

                Console.WriteLine($"ElapsedTicks using {nameof(GetTaskAsync)}: {time.ElapsedTicks}");
                time.Reset();

                Console.WriteLine("======================================");
                await Task.Delay(1000);
            }
        }

        public static async ValueTask<int> GetValueTaskAsync()
        {
            await Task.Delay(0);
            return 10;
        }

        public static async Task<int> GetTaskAsync()
        {
            await Task.Delay(0);
            return 10;
        }
        #endregion
    }

    #region Implementation flexibility - Using ValueTask
    class WithTask : IWithTask
    {
        public Task<int> DoSomeThing()
        {
            //must return a Task.Run or Task.FromResult
            //return Task.Run(() => { return 1; });
            return Task.FromResult(1);
        }
    }

    interface IWithTask
    {
        Task<int> DoSomeThing();
    }

    class WithValueTask : IWithValueTask
    {
        //Synchronous implementation
        public ValueTask<int> DoSomeThing()
        {
            return new ValueTask<int>(1);
        }

        //Asynchronous implementation
        //public async ValueTask<int> DoSomeThing()
        //{
        //    await Task.Delay(100);
        //    return 1;
        //}
    }

    interface IWithValueTask
    {
        ValueTask<int> DoSomeThing();
    }
    #endregion
}
