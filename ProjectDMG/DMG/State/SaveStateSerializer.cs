using System.IO;
using ProtoBuf;

namespace ProjectDMG.DMG.State
{
    internal static class SaveStateSerializer
    {
        public static void Serialize<T>(string filePath, T obj) where T : class
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                Serializer.Serialize(fs, obj);
            }
        }

        public static T Deserialize<T>(string filePath) where T : class
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                return Serializer.Deserialize<T>(fs);
            }
        }
    }
}
