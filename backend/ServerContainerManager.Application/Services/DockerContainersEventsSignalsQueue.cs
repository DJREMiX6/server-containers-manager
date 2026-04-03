using System.Threading.Channels;

namespace ServerContainerManager.Application.Services
{
    internal class DockerContainersEventsSignalsQueue
    {
        private readonly Channel<bool> _signalsQueue = Channel.CreateUnbounded<bool>(new () 
        {
            SingleReader = true, 
            SingleWriter = false
        });

        public ValueTask WriteAsync(bool value, CancellationToken cancellationToken) 
        {
            return _signalsQueue.Writer.WriteAsync(value, cancellationToken);
        }

        public bool TryWrite(bool value)
        {
            return _signalsQueue.Writer.TryWrite(value);
        }

        public ValueTask<bool> ReadAsync(CancellationToken cancellationToken)
        {
            return _signalsQueue.Reader.ReadAsync(cancellationToken);
        }

        public ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken)
        {
            return _signalsQueue.Reader.WaitToReadAsync(cancellationToken);
        }

        public bool TryRead(out bool item)
        {
            return _signalsQueue.Reader.TryRead(out item);
        }
    }
}
