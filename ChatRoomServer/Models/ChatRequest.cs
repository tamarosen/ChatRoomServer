using System.Xml.Linq;

namespace ChatRoomServer.Models
{
    class ChatRequest : AbstractXmlSerializable
    {
        public string From { get; set; }
        public string To { get; set; }

        public override bool FromXml(XElement xmlElement)
        {
            if (!Valid(xmlElement))
            {
                return false;
            }
            this.From = xmlElement.Element("From").Value;
            this.To = xmlElement.Element("To").Value;
            return true;
        }

        public override XElement ToXml()
        {
            XElement serialized = new XElement(GetStringType(),
                new XElement("Type", GetStringType()),
                new XElement("From", From),
                new XElement("To", To));
            return serialized;
        }
    }
}
