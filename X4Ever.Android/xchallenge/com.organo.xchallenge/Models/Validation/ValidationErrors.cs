using System.Collections.Generic;
using System.Text;

namespace com.organo.xchallenge.Models.Validation
{
    public class ValidationError
    {
        public string MessageText { get; set; }
    }

    public class ValidationErrors
    {
        public ValidationErrors()
        {
            MessageList = new List<ValidationError>();
        }

        private List<ValidationError> MessageList { get; set; }

        public void Add(string message)
        {
            MessageList.Add(new ValidationError() { MessageText = message });
        }

        public int Count()
        {
            return MessageList.Count;
        }

        public bool Exists()
        {
            return MessageList.Count > 0;
        }

        public List<ValidationError> Get()
        {
            return MessageList;
        }

        public string Show()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var error in MessageList)
            {
                if (sb.ToString().Trim().Length > 0)
                {
                    sb.Append(";");
                }
                sb.Append(error.MessageText);
            }
            return sb.ToString();
        }

        public string Show(string splitter = "\n")
        {
            StringBuilder sb = new StringBuilder();
            foreach (var error in MessageList)
            {
                if (sb.ToString().Trim().Length > 0)
                {
                    sb.Append(splitter);
                }
                sb.Append(error.MessageText);
            }
            return sb.ToString();
        }
    }
}