using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace com.organo.x4ever.Models
{
    public class MessageModel
    {
        private List<Message> Messages { get; set; }

        public void Add(Message message)
        {
            Messages.Add(message);
        }

        public void Add(Page from, Page to, MessageType messageType, string messageText)
        {
            this.Add(new Message() { From = from, To = to, MessageType = messageType, MessageText = messageText });
        }

        public List<Message> Get()
        {
            return Messages.ToList();
        }

        public List<Message> GetByFrom(Page from)
        {
            return Messages.Where(m => m.From == from).ToList();
        }

        public List<Message> GetByFrom(Page from, MessageType messageType)
        {
            return Messages.Where(m => m.From == from && m.MessageType == messageType).ToList();
        }

        public List<Message> GetByTo(Page to)
        {
            return Messages.Where(m => m.To == to).ToList();
        }

        public List<Message> GetByTo(Page to, MessageType messageType)
        {
            return Messages.Where(m => m.To == to && m.MessageType == messageType).ToList();
        }

        public List<Message> Get(Page from, Page to)
        {
            return Messages.Where(m => m.From == from && m.To == to).ToList();
        }

        public List<Message> Get(Page from, Page to, MessageType messageType)
        {
            return Messages.Where(m => m.From == from && m.To == to && m.MessageType == messageType).ToList();
        }

        public List<Message> Get(MessageType messageType)
        {
            return Messages.Where(m => m.MessageType == messageType).ToList();
        }

        public void Clear()
        {
            Messages.Clear();
        }

        public List<Message> Remove(Message message)
        {
            Messages.Remove(message);
            return this.Get();
        }
    }

    public class Message
    {
        public Page From { get; set; }
        public Page To { get; set; }
        public MessageType MessageType { get; set; }
        public string MessageText { get; set; }
    }

    public enum MessageType
    {
        SUCCESS,
        FAILED,
        WARNING
    }
}