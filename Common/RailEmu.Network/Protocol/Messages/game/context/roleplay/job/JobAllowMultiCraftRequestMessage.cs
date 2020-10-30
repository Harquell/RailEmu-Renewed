
















// Generated on 10/13/2017 02:18:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RailEmu.Protocol.IO;
using RailEmu.Protocol.Types;

namespace RailEmu.Protocol.Messages
{

public class JobAllowMultiCraftRequestMessage : Message
{

public const uint Id = 5748;
public override uint MessageId
{
    get { return Id; }
}

public bool enabled;
        

public JobAllowMultiCraftRequestMessage()
{
}

public JobAllowMultiCraftRequestMessage(bool enabled)
        {
            this.enabled = enabled;
        }
        

public override void Serialize(IDataWriter writer)
{

writer.WriteBoolean(enabled);
            

}

public override void Deserialize(IDataReader reader)
{

enabled = reader.ReadBoolean();
            

}


}


}
