using System;
using System.Collections.Generic;
using System.Text;

namespace SCUBA_FINAL.Models
{
    public class ChatModel
    {
        public string Message { get; set; }
        public int ColumnPosition { get; set; }
        public string HorizontalPostion { get; set; }
        public string MessageColor { get; set; }
        public DateTime Date { get; set; }
    }
}
