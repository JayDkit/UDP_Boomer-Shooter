using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace GD.Serialization
{
    public sealed class Serializer
    {
        public static void Save(string name, object obj)
        {
            var dataContractSerializer = new DataContractSerializer(obj.GetType()); //TODO - add check on Type to ensure its serializable
            var xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "\t";
            var xmlWriter = XmlWriter.Create(name, xmlSettings);

            dataContractSerializer.WriteObject(xmlWriter, obj);
            xmlWriter.Close();
        }

        public static object Load(string name, Type type)
        {
            var fStream = new FileStream(name, FileMode.Open);
            var textReader = XmlDictionaryReader.CreateTextReader(fStream, new XmlDictionaryReaderQuotas());
            var objSerializer = new DataContractSerializer(type); //TODO - add check on Type to ensure its serializable

            object deserializedObject = objSerializer.ReadObject(textReader, true);
            textReader.Close();
            fStream.Close();
            return deserializedObject;
        }
    }
}