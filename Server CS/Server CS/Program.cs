using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Server_CS
{
    public class Program
    {
        /// <summary>
        ///     Глобальный объект в котором хранятся логины и пароли.
        /// </summary>
        public static List<RegData> RegDatas = new List<RegData>();
        /// <summary>
        ///     Глобальный объект в котором хранится вся история сообщений пользователей.
        /// </summary>
        public static List<Message> Messages = new List<Message>();
        /// <summary>
        ///     Словарь Пользователей (В сети/Не в сети).
        /// </summary>
        public static List<string> OnlineUsers = new List<string>();
        /// <summary>
        ///     Словарь пользователей и времени последнего сигнала онлайн
        /// </summary>
        public static Dictionary<string, DateTime> OnlineUsersTimeout = new Dictionary<string, DateTime>();
        private static string Url = "http://localhost:5000";
        /// <summary>
        ///     Точка входа программы.
        /// </summary>
        /// <param name="args">Входящие аргументы, которые передаются через интерпретатор командной строки (cmd.exe)</param>
        public static void Main(string[] args)
        {
            JsonWorker.Load();
            string IP;
            string port;
            ///Количество аргументов, переданных через интерпретатор командной строки (cmd.exe)
            if (args.Length > 0)
            {
                IP = args[0];
                port = args[1];
            }
            else
            {
                Console.Write("Enter IP(or press enter or default):");
                IP = Console.ReadLine();
                if (!string.IsNullOrEmpty(IP))
                {
                    Console.Write("Enter port:");
                    port = Console.ReadLine();
                    Url = $"http://{IP}:{port}";
                }
            }
            CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        ///     Создание веб-сервера для обработки запросов.
        /// </summary>
        /// <param name="args">Входящие аргументы, которые передаются через интерпретатор командной строки (cmd.exe)</param>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                ///Лямбда выражение. Слева от лямбда-оператора (=>) определяется список параметров (объект webBuilder класса WebHostBuilder),
                ///а справа блок выражений (методы класса WebHostBuilder), использующий эти параметры.
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls(Url);
                });
        }
    }
}