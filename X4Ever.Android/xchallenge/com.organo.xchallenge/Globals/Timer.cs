using System;
using System.Threading.Tasks;
using com.organo.xchallenge.Extensions;

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
                    await Task.Delay(TimeSpan.FromMilliseconds(dueTimeMilliseconds));
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