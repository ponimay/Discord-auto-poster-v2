using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot_discord
{
    internal class Config
    {
        public string channel_id { get; set; }
        public string header { get; set; }
        public string cooldown { get; set; }
        public string payload { get; set; }
        public string gif_path { get; set; }
        public bool debugging_enabled { get; set; }
        public string telegram_bot_token { get; set; }
        public string telegram_chat_id { get; set; }
    }
}
