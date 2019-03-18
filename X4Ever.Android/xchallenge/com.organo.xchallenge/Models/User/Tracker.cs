using System;

namespace com.organo.xchallenge.Models.User
{
    public class Tracker
    {
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public string AttributeLabel { get; set; }
        public DateTime ModifyDate { get; set; }
        public string MediaLink { get; set; }
        public string RevisionNumber { get; set; }
    }
}