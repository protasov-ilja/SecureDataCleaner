using System;
using System.Collections.Generic;
using System.Text;
using SecureDataCleanerLibrary.Models;

namespace SecureDataCleanerLibrary
{
    public class HttpHandler
    {
        HttpResult _currentLog;
        public HttpResult CurrentLog { get { return _currentLog; } }

        public string Process(string url, string body, string response, List<SecureDataInfo> secureDataInfoList)
        {
            var httpResult = new HttpResult
            {
                Url = url,
                RequestBody = body,
                ResponseBody = response
            };

            //очищаем secure данные в httpResult, либо создаем новый clearedHttpResult на основе httpResult
            var secureDataCleaner = new SecureDataCleaner(secureDataInfoList);
            httpResult = secureDataCleaner.CleanHttpResult(httpResult);

            Log(httpResult);

            return response;
        }

        /// <summary>
        /// Логирует данные запроса, они должны быть уже без данных которые нужно защищать 
        /// </summary>
        /// <param name="result"></param>
        protected void Log(HttpResult result)
        {
            _currentLog = new HttpResult
            {
                Url = result.Url,
                RequestBody = result.RequestBody,
                ResponseBody = result.ResponseBody
            };
        }
    }

}
