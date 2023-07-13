using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCUBA_FINAL.Models
{
    public class MSG_Model
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string MessageReceived { get; set; }
        public string MessageSend { get; set; }
        public DateTime Date { get; set; }
    }
}
