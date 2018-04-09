using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Weather_TelegramBot
{
    public class BotMenagement
    {
        public static void BotStart()
        {
            #region TelegramBotSettings
            //пометка сообщения как прочитанного
            int updateId = 0;
            //идентификатор пользователя отправившего сообщение
            string messageFromId = string.Empty;
            //Сообщение от пользователя
            string messageText = string.Empty;
            //Сообщение которое отправляется пользователю
            StringBuilder outMessageText = new StringBuilder();
            //Имя пользователя
            string firstName = string.Empty;
            string token = "578126800:AAEIUZNIdHzfLOMweOKP5f9VNjDZt88ByCw";
            #endregion

            #region WebSettings
            WebClient webClient = new WebClient();
            //Формирование стартового Url
            string startUrl = $"https://api.telegram.org/bot{token}";
            #endregion

            #region Obrabotka
            //Организация бесконечного цикла запросов
            while (true)
            {
                //обнуление outMessageText
                outMessageText = new StringBuilder();

                //Получение данных
                string url = $"{startUrl}/getUpdates?offset={updateId + 1}";
                string responce = webClient.DownloadString(url);

                //Считывание файла getUpdates (json)
                var messages = JObject.Parse(responce)["result"].ToArray();

                foreach (var currentMessage in messages)
                {
                    //Получение теущего updateId
                    updateId = Convert.ToInt32(currentMessage["update_id"]);
                    try
                    {
                        //считываение показаний пользователя отправившего сообщение
                        firstName = currentMessage["message"]["from"]["first_name"].ToString();
                        messageFromId = currentMessage["message"]["from"]["id"].ToString();
                        messageText = currentMessage["message"]["text"].ToString();

                        //Формирование служебного сообщения, отображаемого в консоли
                        Console.WriteLine($"{firstName} {messageFromId} {messageText}");

                        //Формирование выходного сообщения
                        outMessageText.Append(WeatherService.GetWeatherMeteoservice(messageText));

                        //Формирование ссылки для отправки сообщения пользователю
                        url = $"{startUrl}/sendMessage?chat_id={messageFromId}&text={outMessageText.ToString()}";
                        webClient.DownloadString(url);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Thread.Sleep(1000);
            }
            #endregion


        }
    }
}