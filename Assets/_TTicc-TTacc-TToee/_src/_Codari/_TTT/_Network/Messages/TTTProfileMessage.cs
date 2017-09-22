using UnityEngine.Networking;

namespace Codari.TTT.Network
{
    public sealed class TTTProfileMessage : MessageBase
    {
        public string json;

        public override void Deserialize(NetworkReader reader)
        {
            json = reader.ReadString();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(json);
        }
    }
}
