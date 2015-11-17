using TestApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestApp.Model;

namespace TestApp {
    public class ControllerLoadSave {

        //object lockSave = new object();
        //object lockCP = new object();

        public static ControllerLoadSave Instance { get; } = new ControllerLoadSave();
        private ControllerLoadSave() { }

        public void Save(Diagram diagram, string path) {
            Task.Run(() => SerializeToFile(diagram, path));
        }
        private void SerializeToFile(Diagram diagram, string path) {
            using (FileStream stream = File.Create(path)) {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                serializer.Serialize(stream, diagram);
            }
        }

        public Task<Diagram> load(string path) {
            return Task.Run(() => DeserializeFromFile(path));
        }
        private Diagram DeserializeFromFile(string path) {
            using (FileStream stream = File.OpenRead(path)) {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                return serializer.Deserialize(stream) as Diagram;
            }
        }

        public Task<string> ToString(Diagram diagram) {
            return Task.Run(() => SerializeToString(diagram));
        }
        private string SerializeToString(Diagram diagram) {
            StringBuilder stringBuilder = new StringBuilder();
            using (TextWriter stream = new StringWriter(stringBuilder)) {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                serializer.Serialize(stream, diagram);
                return stringBuilder.ToString();
            }
        }

        public Task<Diagram> FromString(string str) {
            return Task.Run(() => DeserializeFromString(str));
        }
        private Diagram DeserializeFromString(string str) {
            using (TextReader stream = new StringReader(str)) {
                XmlSerializer serializer = new XmlSerializer(typeof(Diagram));
                Diagram diagram = serializer.Deserialize(stream) as Diagram;
                return diagram;
            }
        }
    }
}