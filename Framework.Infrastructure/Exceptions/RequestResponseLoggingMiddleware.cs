using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Framework.Infrastructure.Exceptions;

public class RequestResponseLoggingMiddleware(ILoggerFactory logger) : IMiddleware
{
    private static readonly string[] NotToLogKeys =
        { "password", "currentPassword", "repeatedPassword", "newPassword", "confirmNewPassword" };

    private static readonly string[] NotToLogHeaders = { };
    private readonly ILogger _logger = logger.CreateLogger(nameof(RequestResponseLoggingMiddleware));

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var messageId = context.Items["MessageId"]!.ToString();
        var requestStream = new StreamReader(context.Request.Body);
        string requestBodyString;

        if (context.Request.HasFormContentType)
        {
            requestBodyString = HandleFormDataRequest(context);
        }
        else
        {
            requestBodyString = HandleRegularJsonRequest(await requestStream.ReadToEndAsync());
            context.Request.Body.Seek(0, SeekOrigin.Begin);
        }

        var headers = context.Request.Headers.ToDictionary(x => x.Key, y => y.Value);

        foreach (var notToLogHeader in NotToLogHeaders) headers.Remove(notToLogHeader);

        _logger.LogInformation("{MessageId} {Method} {QueryString} {Headers} {RequestBody}",
            messageId,
            context.Request.Method,
            context.Request.QueryString.Value,
            headers,
            requestBodyString);

        await using var ms = new MemoryStream();
        var originalResponseBodyStream = context.Response.Body;
        context.Response.Body = ms;

        try
        {
            await next(context);

            ms.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(ms);
            var responseBodyString = await streamReader.ReadToEndAsync();
            ms.Seek(0, SeekOrigin.Begin);

            await ms.CopyToAsync(originalResponseBodyStream);

            _logger.LogInformation("{MessageId} {ResponseBody}", messageId, responseBodyString);
        }
        catch
        {
            context.Response.Body = originalResponseBodyStream;
            throw;
        }
    }


    private string HandleFormDataRequest(HttpContext context)
    {
        var jObject = new JObject();

        var formKeys = context.Request.Form.Keys;

        foreach (var key in formKeys)
            if (context.Request.Form.TryGetValue(key, out var s))
                jObject.Add(key, s.ToString());

        return jObject.ToString();
    }


    private string HandleRegularJsonRequest(string originalBodyString)
    {
        var jObject = new JObject();
        string requestBodyString = default;

        try
        {
            jObject = JObject.Parse(originalBodyString);

            foreach (var key in NotToLogKeys)
                if (jObject.ContainsKey(key))
                    jObject.Remove(key);

            requestBodyString = jObject.ToString();
        }
        catch
        {
            // ignored
        }

        return requestBodyString;
    }
}