using System;
using System.Threading;
using Xamarin.Forms;

namespace com.organo.x4ever.Extended.Utilities
{
    public class TimerWrapper
    {
        private readonly TimeSpan _timespan;
        private readonly Action _callback;
        private readonly bool _shouldRepeat;

        private CancellationTokenSource _cancellation;

        public TimerWrapper(TimeSpan timespan, bool shouldRepeat, Action callback)
        {
            _timespan = timespan;
            _callback = callback;
            _shouldRepeat = shouldRepeat;
            _cancellation = new CancellationTokenSource();
        }

        public void Start()
        {
            CancellationTokenSource cts = _cancellation; // safe copy
            Device.StartTimer(_timespan, () =>
            {
                if (cts.IsCancellationRequested || !_shouldRepeat) return false;

                _callback.Invoke();

                if (_shouldRepeat)
                {
                    return true; // or true for periodic behavior
                }
                else
                {
                    return false;
                }
            });
        }

        public void Stop()
        {
            Interlocked.Exchange(ref _cancellation, new CancellationTokenSource()).Cancel();
        }
    }
}