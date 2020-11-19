using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Server_CS
{
    internal class JsonWorker
    {
        /// <summary>
        ///     Файл для сохранения сообщений и регистрационных данных (логины и пароли) в виде json массива.
        /// </summary>
        private const string MessagesPath = @"messages.json";
        private const string RegDataPath = @"regData.json";

        /// <summary>
        ///     Метот класса, который сохраняет сообщения в файл.
        /// </summary>
        /// <param name="messages">Массив сообщений</param>
        public static async void Save(List<Message> messages)
        {
            var NumberOfRetries = 3;
            var DelayOnRetry = 1000;
            for (var i = 1; i <= NumberOfRetries; ++i)
                try
                {
                    ///await запускает асинхронную задачу инициализации новый экземпляр класса StreamWriter для указанного файла 
                    ///с помощью кодировки по умолчанию и размера буфера.
                    await using var streamWriter = new StreamWriter(MessagesPath);
                    ///await запускает асинхронную задачу WriteAsync.
                    await streamWriter.WriteAsync(JsonConvert.SerializeObject(messages));
                    break;
                }
                catch (IOException e) when (i <= NumberOfRetries)
                {
                    Thread.Sleep(DelayOnRetry);
                }
        }
        /// <summary>
        ///     Функция сохранения регистрационных данных в файл.
        /// </summary>
        /// <param name="regDatas">Массив связок логин/пароль.</param>
        public static async void Save(List<RegData> regDatas)
        {
            await using var streamWriter = new StreamWriter(RegDataPath);
            await streamWriter.WriteAsync(JsonConvert.SerializeObject(regDatas));
        }
        /// <summary>
        ///     Функция загрузки сообщений из файла, десериализованных из json в List <Message> объект.
        /// </summary>
        public static async void Load()
        {
            if (!File.Exists(MessagesPath))
                return;
            try
            {
                using var streamReader = new StreamReader(MessagesPath);
                Program.Messages = JsonConvert.DeserializeObject<List<Message>>(await streamReader.ReadToEndAsync());
            }
            catch (Exception)
            {
            }
            if (!File.Exists(RegDataPath))
                return;
            try
            {
                using var streamReader = new StreamReader(RegDataPath);
                Program.RegDatas = JsonConvert.DeserializeObject<List<RegData>>(await streamReader.ReadToEndAsync());
            }
            catch (Exception)
            {
            }
        }
    }
}