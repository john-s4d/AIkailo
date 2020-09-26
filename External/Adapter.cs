using System;
using AIkailo.Messaging;
using AIkailo.Model.Internal;
using AIkailo.Model.Common;

namespace AIkailo.External
{
    public class Adapter
    {
        private MessageService _messageService;

        private Adapter() { }

        public Adapter(string host)
        {
            _messageService = new MessageService(host);
        }

        public void Start()
        {
            _messageService.Start();
        }

        public void Stop()
        {
            _messageService.Stop();
        }

        public Input CreateInputSource(string name)
        {
            Input result = new Input(name);
            result.InputEvent += OnInputEvent;
            return result;
        }

        public void CreateOutputTarget(string name, Action<DataPackage> callback)
        {
            Output result = new Output(name);
            _messageService.RegisterConsumer(result.Name, result.Consumer);
            result.OutputEvent += callback;
        }

        private void OnInputEvent(string source, DataPackage data)
        {
            _messageService.Publish(new InputMessage(source, data));
        }
    }
}