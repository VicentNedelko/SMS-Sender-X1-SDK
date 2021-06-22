using LogicModule.Nodes.Helpers;
using LogicModule.ObjectModel;
using LogicModule.ObjectModel.TypeSystem;
using System.Threading.Tasks;

namespace LogicNodesSDK.Logic.Nodes
{
  public class Node : LogicNodeBase
  {
        private readonly ITypeService typeService;
        public Node(INodeContext context)
            : base(context)
        {
            context.ThrowIfNull("context");

            this.typeService = context.GetService<ITypeService>();
            this.Send = typeService.CreateBool(PortTypes.Binary, "Send", false);
            this.Phone = typeService.CreateString(PortTypes.String, "Phone", "1016666");
            this.Message = typeService.CreateString(PortTypes.String, "Message", "Empty");
            this.Status = typeService.CreateString(PortTypes.String, "SMS Status");
        }

        [Input(IsInput = true, IsRequired = true, DisplayOrder = 1)]
        public BoolValueObject Send { get; private set; }

        [Input(IsInput = false, IsRequired = true, DisplayOrder = 2)]
        public StringValueObject Phone { get; private set; }

        [Input(IsInput = false, IsRequired = true, DisplayOrder = 3)]
        public StringValueObject Message { get; private set; }

        [Output(IsRequired = true)]
        public StringValueObject Status { get; set; }

        public override async void Execute()
        {
            if (Send.HasValue == true)
            {
                SmsManager smsManager = new SmsManager();
                Status.BlockGraph();
                var getSmsStatus = await smsManager.SendAsync(Message, Phone);
                Status.Value = getSmsStatus.Dlr_status;
            }
        }
    }
}
