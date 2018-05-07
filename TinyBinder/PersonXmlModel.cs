using System.Xml.Serialization;

namespace TinyBinderExample
{
    [XmlRoot("template")]
    public class PersonXmlModel
    {
        [XmlElement("details")]
        public string Details { get; set; }
    }
}
