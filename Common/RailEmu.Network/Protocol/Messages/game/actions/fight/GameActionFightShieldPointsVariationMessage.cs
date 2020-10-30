
















// Generated on 10/13/2017 02:18:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RailEmu.Protocol.IO;
using RailEmu.Protocol.Types;

namespace RailEmu.Protocol.Messages
{

public class GameActionFightShieldPointsVariationMessage : AbstractGameActionMessage
{

public const uint Id = 6220;
public override uint MessageId
{
    get { return Id; }
}

public int targetId;
        public short delta;
        

public GameActionFightShieldPointsVariationMessage()
{
}

public GameActionFightShieldPointsVariationMessage(short actionId, int sourceId, int targetId, short delta)
         : base(actionId, sourceId)
        {
            this.targetId = targetId;
            this.delta = delta;
        }
        

public override void Serialize(IDataWriter writer)
{

base.Serialize(writer);
            writer.WriteInt(targetId);
            writer.WriteShort(delta);
            

}

public override void Deserialize(IDataReader reader)
{

base.Deserialize(reader);
            targetId = reader.ReadInt();
            delta = reader.ReadShort();
            

}


}


}
