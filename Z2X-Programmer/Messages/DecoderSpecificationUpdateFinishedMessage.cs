using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2XProgrammer.Messages
{
    public class DecoderSpecificationUpdateFinishedMessage : ValueChangedMessage<bool>
    {
        public DecoderSpecificationUpdateFinishedMessage (bool value) : base(value)
        {

        }
    }
}
