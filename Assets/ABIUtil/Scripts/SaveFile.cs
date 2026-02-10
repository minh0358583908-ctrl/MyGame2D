using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace BaseGame
{
    public static class SaveFile
    {
        private static string BasePath;

        public static void SetBasePath(string path)
        {
            BasePath = path;
        }

        public static T Load<T>()
        {
            try
            {
                var savePathJson = JoinString(BasePath, typeof(T).Name, ".json");
                if (!File.Exists(savePathJson))
                {
                    var savePath = JoinString(BasePath, typeof(T).Name, ".dat");
                    if (!File.Exists(savePath)) return default;
                    var formatter = new BinaryFormatter();
                    var fileStream = File.Open(savePath, FileMode.Open);
                    var obj = formatter.Deserialize(fileStream);
                    fileStream.Close();
                    return (T)obj;
                }
                var jsonString = File.ReadAllText(savePathJson);
                return JsonUtility.FromJson<T>(jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }

        public static void Save<T>(object obj)
        {
            var savePath = JoinString(BasePath, typeof(T).Name, ".json");
            File.WriteAllText(savePath, JsonUtility.ToJson(obj));
        }

        private static string JoinString(params object[] objs)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int length = objs.Length;
            for (int index = 0; index < length; ++index)
                stringBuilder.Append(objs[index].ToString());
            return stringBuilder.ToString();
        }
    }
}