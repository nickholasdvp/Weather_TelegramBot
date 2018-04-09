using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Weather_TelegramBot
{
    public class WeatherService
    {
        public static string GetWeatherMeteoservice(string town)
        {
            //Резервируем выходную переменную
            StringBuilder output = new StringBuilder();
            //Получение ссылки на XML документ в зависимости от города
            string urlPath = SelectTown(town, "meteoservice");
            if (urlPath == "0")
            {
                return "Такого города нет! Выберите город из списка:\n" +
                    "/msk - Москва\n/spb - Санкт-Петербург\n/orel - Орел\n/prm - Пермь\n/krd - Краснодар";
            }

            //Открытие xml документа
            XDocument xmlDoc = XDocument.Load(urlPath);
            if (xmlDoc != null)
            {
                //Получение сведений о городе из документа XML
                string townIndex = xmlDoc.Element("MMWEATHER").Element("REPORT").Element("TOWN").Attribute("index").Value;
                output.AppendLine("Погода " + IndexToTown(townIndex));

                //Заготовка для считывания показаний у данного города
                var weatherInfo = xmlDoc.Element("MMWEATHER").Element("REPORT").Element("TOWN").Elements("FORECAST").ToArray();

                //Вывод температуры и давления
                output.AppendLine($"Температура: {weatherInfo[0].Element("TEMPERATURE").Attribute("min").Value}..." +
                    $"{weatherInfo[0].Element("TEMPERATURE").Attribute("max").Value}{(char)0176}C");
                output.AppendLine($"Давление: {weatherInfo[0].Element("PRESSURE").Attribute("min").Value}..." +
                    $"{weatherInfo[0].Element("PRESSURE").Attribute("max").Value} мм.рт.ст.");
                output.AppendLine();
                output.AppendLine("Погода предоставлена сервисом\nwww.meteoservice.ru");
            }

            //Вывод
            return output.ToString();
        }

        private static string SelectTown(string town, string service)
        {
            //Резервируем переменную под ссылку
            string urlPath = string.Empty;
            //Выбор метеосервиса
            if (service == "meteoservice")
            {
                //Выбор города
                switch (town.ToLower())
                {
                    case "/msk":
                        urlPath = "https://xml.meteoservice.ru/export/gismeteo/point/37.xml";
                        break;
                    case "/spb":
                        urlPath = "https://xml.meteoservice.ru/export/gismeteo/point/69.xml";
                        break;
                    case "/orel":
                        urlPath = "https://xml.meteoservice.ru/export/gismeteo/point/115.xml";
                        break;
                    case "/prm":
                        urlPath = "https://xml.meteoservice.ru/export/gismeteo/point/59.xml";
                        break;
                    case "/krd":
                        urlPath = "https://xml.meteoservice.ru/export/gismeteo/point/199.xml";
                        break;
                    default:
                        urlPath = "0";
                        break;
                }
            }

            //Возвращаем ссылку на сервис
            return urlPath;
        }

        //Метод для соответствия между index'ом и городом в Meteoservece
        private static string IndexToTown(string index)
        {
            switch (index)
            {
                case "37":
                    return "в Москве";
                case "69":
                    return "в Санкт-Петербурге";
                case "115":
                    return "в Орле";
                case "59":
                    return "в Перми";
                case "199":
                    return "в Краснодаре";
            }
            return "Ошибка считывания города";
        }
    }
}
