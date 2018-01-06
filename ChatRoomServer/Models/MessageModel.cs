using System.ComponentModel;
using System.Xml.Linq;

namespace ChatRoomServer.Models
{
    public class MessageModel : AbstractXmlSerializable, INotifyPropertyChanged
    {
        public string Content { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        //public DateTime TimeSent { get; set; }
        //public Contact Sender { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public override bool FromXml(XElement xmlElement)
        {
            if (!Valid(xmlElement))
            {
                return false;
            }
            this.From = xmlElement.Element("From").Value;
            this.To = xmlElement.Element("To").Value;
            this.Content = xmlElement.Element("Content").Value;
            return true;
        }

        public override XElement ToXml()
        {
            XElement serialized = new XElement(GetStringType(),
                new XElement("Type", GetStringType()),
                new XElement("From", From),
                new XElement("To", To),
                new XElement("Content", Content));
            return serialized;
        }
    }
}
