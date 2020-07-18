using AIkailo.External;

namespace AIkailo.Modules.Interaction
{
    public class InteractionActionMessage : IActionMessage
    {
        public string Data { get; set; }
        //public List<Tuple<IConvertible, IConvertible>> Data { get; set; } = new List<Tuple<IConvertible, IConvertible>>();
    }
}
