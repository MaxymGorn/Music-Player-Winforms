using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Maxs_Gorn
{
    class DatManage
    {
        public void WriteJson<T>(T obj, string path) where T : class
        {

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(fs, obj);
            }

        }


        public void SerializeXML<T>(T users, string path) where T : class
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, users);
            }
        }
        public T DeserializeXML<T>(string SelectedPath) where T : class
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(SelectedPath, FileMode.OpenOrCreate))
            {
                return (T)xml.Deserialize(fs);
            }

        }

        public T DeserializeBinary<T>(string SelectedPath) where T:class
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fs = new FileStream(SelectedPath, FileMode.Open, FileAccess.Read);
            return (T)binaryFormatter.Deserialize(fs);
        }

        public T DeserializeJson<T>(string SelectedPath) where T : class
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (FileStream fs = new FileStream(SelectedPath, FileMode.OpenOrCreate))
            {
                return (T)serializer.ReadObject(fs);
            }

        }

        public string DeserializeTxt(string SelectedPath)
        {
            StreamReader reader = new StreamReader(SelectedPath);
            return reader.ReadToEnd();

        }
        public void WriteBinary<T>(T obj, string path) where T : class
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            bf.Serialize(fs, obj);
            fs.Close();
        }
    }
}
