using System;
using System.Threading.Tasks;

namespace com.organo.xchallenge.Globals
{
    public class Timer : IDisposable
    {
        private string Name { get; set; }

        public delegate void TimerCallback(object state);

        public async Task TimerAsync(string name, Action callback, int dueTimeMilliseconds)
        {
            Name = name;
            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(dueTimeMilliseconds);
                    callback();
                    await TimerAsync(Name, callback, dueTimeMilliseconds);
                }
            });
        }

        public void Dispose()
        {
        }
    }
}