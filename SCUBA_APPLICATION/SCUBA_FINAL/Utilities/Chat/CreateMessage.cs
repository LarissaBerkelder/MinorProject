using SCUBA_FINAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCUBA_FINAL.Utilities.Chat
{
    public class CreateMessage
    {
        public ChatModel Message;

        public ChatModel CreateMessageSend(string message)
        {
            Message = new ChatModel
            {
                Message = message,
                ColumnPosition = 1,
                HorizontalPostion = "end",
                MessageColor = "#FFFFFF",
                Date = DateTime.Now

            };

            return Message;
        }

        public ChatModel CreateMessageReceived(string message)
        {
            Message = new ChatModel
            {
                Message = message,
                ColumnPosition = 0,
                HorizontalPostion = "start",
                MessageColor = "#BABABA",
                Date = DateTime.Now

            };

            return Message;
        }

    }
}
