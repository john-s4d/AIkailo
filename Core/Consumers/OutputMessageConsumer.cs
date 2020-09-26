using System;
using System.Threading.Tasks;
using AIkailo.Messaging;
using AIkailo.Model.Common;
using AIkailo.Model.Internal;
using MassTransit;

namespace AIkailo.Core
{
    internal class OutputMessageConsumer : IMessageConsumer<OutputMessage>
    {
        public async Task Consume(ConsumeContext<OutputMessage> context)
        {
            Console.WriteLine("OutputMessageConsumer.Consume(OutputMessage)");

            /*
            // Disassemble a Scene into its structured data and extract the target if possible
            string target = string.Empty;
            DataPackage data = new DataPackage();

            foreach (Association a in context.Message.Scene)
            {
                AIkailo.DataService.
                Scene s = (Scene)a.ChildId;
                Concept cParamName = s[0].Child;
                Concept cParamValue = s[1].Child;

                if (cParamName.Equals(Constants.TARGET_GUID))
                {
                    target = cParamValue.Definition.ToString();
                }
                else
                {
                    data.Add(new PrimPair(cParamName.Definition, cParamValue.Definition));
                }
            }
            */
            
            // Send the message to an external target
            string target = "Interaction.Output";
            DataPackage data = new DataPackage();
            data.Add("output", "bar");
                        
            ISendEndpoint endpoint = await context.GetSendEndpoint(new Uri($"rabbitmq://localhost/{target}"));
            await endpoint.Send(new ExternalMessage(data));

            //context.Send<ExternalMessage>()            
        }
    }
}
