using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace RequestResponseMiddlewareTest.Middleware
{
    public class LogRequestResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public LogRequestResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //讀取Request Body
            var requestBody = await FormatRequest(context.Request);

            //取得ResponseBodyStream的位置，我們要把這個不能被讀取的HttpStream偷偷換成一個可讀取的MemoryStream
            var originalResponseBodyStream = context.Response.Body;

            using (var newResponseBodyStream = new MemoryStream())
            {
                //換成一個可讀取的MemoryStream
                context.Response.Body = newResponseBodyStream;

                //處理Request
                await _next(context);

                //讀取Response Body
                var responseBody = await FormatResponse(newResponseBodyStream, originalResponseBodyStream);
            }
        }

        private async Task<string> FormatResponse(MemoryStream newResponseBodyStream, Stream originalResponseBodyStream)
        {
            //將串流指回開頭，準備讀取回傳內容
            newResponseBodyStream.Seek(0, SeekOrigin.Begin);

            //讀取回傳內容
            var responseBody = await new StreamReader(newResponseBodyStream).ReadToEndAsync();

            //在串流指回開頭，預備StreamCopy
            newResponseBodyStream.Seek(0, SeekOrigin.Begin);

            //Copy new stream data into original stream, without this, client will get empty response
            await newResponseBodyStream.CopyToAsync(originalResponseBodyStream);

            return responseBody;
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            //讓request body 可以被讀取兩次
            request.EnableRewind();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //不使用using(new StreamReader())去讀取
            //他會Dispose掉部分的資料，導致.Net core處理RequestBody時Null Pointer Exception
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            //將串流指回開頭
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }
    }
}