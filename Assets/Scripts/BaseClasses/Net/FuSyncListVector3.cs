using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.BaseClasses.Net
{
    public class SyncListVector3 : SyncList<Vector3>
    {
        protected override void SerializeItem(NetworkWriter writer, Vector3 item)
        {
            writer.Write(item);
        }

        protected override Vector3 DeserializeItem(NetworkReader reader)
        {
            return reader.ReadVector3();
        }

        public static SyncListVector3 ReadInstance(NetworkReader reader)
        {
            ushort count = reader.ReadUInt16();
            SyncListVector3 result = new SyncListVector3();
            for (ushort i = 0; i < count; i++)
            {
                result.Add(reader.ReadVector3());
            }
            return result;
        }

        public static void WriteInstance(NetworkWriter writer, SyncListVector3 items)
        {
            writer.Write((ushort)items.Count);
            foreach (var item in items)
            {
                writer.Write(item);
            }
        }


    }
}