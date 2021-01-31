using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IO;
using Newtonsoft.Json;
using Winvestate_Offer_Management_API.Classes;
using Winvestate_Offer_Management_API.Database;
using Winvestate_Offer_Management_Models;

namespace Winvestate_Offer_Management_API.Middleware
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        //private ApiLog _apiLogService;

        public ApiLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //await LogRequest(httpContext);


            try
            {
                //_apiLogService = new ApiLog();

                var request = httpContext.Request;
                if (request.Path.StartsWithSegments(new PathString("/api")))
                {
                    var stopWatch = Stopwatch.StartNew();
                    var requestTime = DateTime.Now;
                    var requestBodyContent = await ReadRequestBody(request);
                    var originalBodyStream = httpContext.Response.Body;

                    var loUser = "";
                    if (request.Path.ToString().ToLower().EndsWith("token"))
                    {

                        var loToken = JsonConvert.DeserializeObject<ApiUserService>(requestBodyContent);
                        loUser = loToken.ApiKey;
                    }
                    else
                    {
                        loUser = HelperMethods.GetCallerFromToken(httpContext.User.Identity);
                    }

                    using (var responseBody = new MemoryStream())
                    {
                        var response = httpContext.Response;
                        response.Body = responseBody;
                        await _next(httpContext);
                        stopWatch.Stop();

                        string responseBodyContent = null;
                        responseBodyContent = await ReadResponseBody(response);
                        await responseBody.CopyToAsync(originalBodyStream);

                        await SafeLog(requestTime,
                            stopWatch.ElapsedMilliseconds,
                            response.StatusCode,
                            request.Method,
                            request.Path,
                            request.QueryString.ToString(),
                            requestBodyContent,
                            responseBodyContent,
                            loUser,
                            request.HttpContext.Connection.RemoteIpAddress.ToString());
                    }
                }
                else
                {
                    await _next(httpContext);
                }
            }
            catch (Exception)
            {
                await _next(httpContext);
            }
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            var loInput = ReadStreamInChunks(requestStream);
            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                    0,
                    readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await request.Body.CopyToAsync(requestStream);

            var bodyAsText = ReadStreamInChunks(requestStream);
            request.Body.Position = 0;

            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task SafeLog(DateTime requestTime,
                            long responseMillis,
                            int statusCode,
                            string method,
                            string path,
                            string queryString,
                            string requestBody,
                            string responseBody,
                            string pUser,
                            string pIpAdress)
        {
            //var loUser = "";
            if (path.ToLower().EndsWith("login") || path.ToLower().EndsWith("token"))
            {

                //var loToken = JsonConvert.DeserializeObject<ApiUserService>(requestBody);
                //loUser = loToken.ApiKey;

                requestBody = "(Request logging disabled for " + path.ToLower();
                responseBody = "(Response logging disabled for " + path.ToLower();
            }

            //if (requestBody.Length > 100)
            //{
            //    requestBody = $"(Truncated to 100 chars) {requestBody.Substring(0, 100)}";
            //}

            //if (responseBody.Length > 100)
            //{
            //    responseBody = $"(Truncated to 100 chars) {responseBody.Substring(0, 100)}";
            //}

            //if (queryString.Length > 100)
            //{
            //    queryString = $"(Truncated to 100 chars) {queryString.Substring(0, 100)}";
            //}

            var loApiLog = new ApiLogService()
            {
                RequestTime = requestTime,
                ResponseMills = responseMillis,
                StatusCode = statusCode,
                Method = method,
                Path = path,
                QueryString = queryString,
                RequestBody = requestBody,
                ResponseBody = responseBody,
                ApplicationName = "WinvestateTest",
                ApiCaller = pUser,
                IpAddress = pIpAdress
            };

#if PROD
            loApiLog.ApplicationName = "WinvestateProd";
#endif

            _ = Task.Run(() => WriteLogs(loApiLog));
        }

        private static async Task WriteLogs(ApiLogService pApiLog)
        {
            var loApiLog = ApiLog.GetModel(pApiLog);
            await Task.Run(() => Crud<ApiLog>.InsertLog(loApiLog, out _));
            //await Task.Run(() => Common.PrepareLogger(JsonConvert.SerializeObject(loApiLog)));
            //var loRequestResponse = new RequestResponse
            //{
            //    Request = pApiLog.RequestBody,
            //    Response = pApiLog.ResponseBody
            //};
            //_ = Task.Run(() => Common.Logger.LogInfo(JsonConvert.SerializeObject(loRequestResponse)));
        }

        private class RequestResponse
        {
            public string Request { get; set; }
            public string Response { get; set; }
        }
    }
}
