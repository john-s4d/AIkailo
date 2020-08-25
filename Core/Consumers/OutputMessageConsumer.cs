using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AIkailo.Messaging;
using AIkailo.Model;
using MassTransit;
using IConvertible = AIkailo.Model.IConvertible<System.IConvertible>;

namespace AIkailo.Core
{
    internal class OutputMessageConsumer : IMessageConsumer<OutputMessage>
    {

        public Task Consume(ConsumeContext<OutputMessage> context)
        {
            Console.WriteLine("OutputMessageConsumer.Consume(OutputMessage)");

            // Disassemble a Scene into its structured data and extract the target if possible
            string target = string.Empty;
            List<Tuple<IConvertible, IConvertible>> data = new List<Tuple<IConvertible, IConvertible>>();

            foreach (Association a in context.Message.Scene)
            {
                Scene s = (Scene)a.Child;
                Concept cParamName = s[0].Child;
                Concept cParamValue = s[1].Child;

                if (cParamName.Equals(Constants.TARGET_GUID))
                {
                    target = cParamValue.Definition.ToString();
                }
                else
                {
                    data.Add(new Tuple<IConvertible, IConvertible>(cParamName.Definition, cParamValue.Definition));
                }
            }

            // Send the message to an external target
            AIkailo.MessageService.Send(target, new ExternalMessage(data));
            return null;
        }
    }
}
