using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace Weather_TelegramBot
{
    class Program
    {
        static void Main()
        {
            //Метод для запука бота
            BotMenagement.BotStart();
        }

    }
}

