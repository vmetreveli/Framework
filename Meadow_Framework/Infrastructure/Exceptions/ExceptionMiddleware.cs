using System.Collections;
using System.Text.Json;
using Meadow_Framework.Abstractions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using RestEase;

namespace Meadow_Framework.Infrastructure.Exceptions;

/// <summary>
///     Catches exceptions and constructs ApiProblemDetails for the response.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    private readonly Dictionary<Type, int> _statusCodes = new()
    {
        { typeof(InflowException), StatusCodes.Status400BadRequest },
        { typeof(AppValidationException), StatusCodes.Status400BadRequest },
        { typeof(ForbiddenException), StatusCodes.Status403Forbidden },
        { typeof(HttpException), StatusCodes.Status502BadGateway },
        { typeof(ObjectNotFoundException), StatusCodes.Status404NotFound },
        { typeof(OutdatedVersionException), StatusCodes.Status426UpgradeRequired },
        { typeof(ServiceUnavailableException), StatusCodes.Status503ServiceUnavailable },
        { typeof(ApiException), StatusCodes.Status502BadGateway },
        { typeof(Exception), StatusCodes.Status500InternalServerError }
    };

    public ExceptionMiddleware(RequestDelegate next, Dictionary<Type, int> statusCodes)
    {
        _next = next;
        foreach (var statusCode in statusCodes) _statusCodes[statusCode.Key] = statusCode.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InflowException ex)
        {
            await HandleAppException(ex, context);

            throw;
        }
        catch (ApiException ex)
        {
            await HandleApiException(ex, context);

            throw;
        }
        catch (Exception ex)
        {
            await HandleException(ex, context);

            throw;
        }
    }

    private static async Task SetResponse(HttpContext context, ApiProblemDetails apiProblemDetails)
    {
        context.Items["ApiProblemDetails"] = apiProblemDetails;
        context.Response.StatusCode =
            apiProblemDetails.Status.GetValueOrDefault(StatusCodes.Status500InternalServerError);
        context.Response.ContentType = "application/json";
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };


        await context.Response.WriteAsync(JsonSerializer.Serialize(apiProblemDetails, serializerOptions));
    }

    private async Task HandleAppException(InflowException ex, HttpContext context)
    {
        if (!context.Response.HasStarted)
        {
            ApiProblemDetails apiProblemDetails = new()
            {
                Status = _statusCodes.TryGetValue(ex.GetType(), out var statusCode)
                    ? statusCode
                    : _statusCodes[typeof(InflowException)],
                Type = ex.Code,
                Title = ex.Title,
                Detail = ex.Message,
                Instance = context.Request.Path,
                Severity = ex.LogLevel
            };

            if (ex is AppValidationException appValidationException)
                apiProblemDetails.ValidationErrors =
                    appValidationException.Failures.ToDictionary(failure => failure.Key, failure => failure.Value);
            else if (ex is HttpException httpException) apiProblemDetails.ExternalEndpoint = httpException.Endpoint;

            foreach (DictionaryEntry item in ex.Data) apiProblemDetails.Extensions.Add(item.Key.ToString(), item.Value);

            foreach (KeyValuePair<string, string> item in ex.ResponseHeaders) context.Response.Headers.Append(item.Key, item.Value);

            await SetResponse(context, apiProblemDetails);
        }
    }

    private async Task HandleApiException(ApiException ex, HttpContext context)
    {
        if (context.Response.HasStarted) return;

        var externalApiProblemDetails = TryGetExternalApiProblemDetails(ex);
        var apiProblemDetails = CreateApiProblemDetails(ex, context, externalApiProblemDetails);

        AddExtensions(apiProblemDetails, externalApiProblemDetails);
        AddHeaders(ex, context);

        await SetResponse(context, apiProblemDetails);
    }

    private static ApiProblemDetails? TryGetExternalApiProblemDetails(ApiException ex)
    {
        try
        {
            var details = ex.DeserializeContent<ApiProblemDetails>();
            return details?.IsApiProblemDetails == true ? details : null;
        }
        catch
        {
            return null;
        }
    }

    private ApiProblemDetails CreateApiProblemDetails(ApiException ex, HttpContext context,
        ApiProblemDetails? externalApiProblemDetails)
    {
        return new ApiProblemDetails
        {
            Status = externalApiProblemDetails?.Status ?? _statusCodes[typeof(InflowException)],
            Type = externalApiProblemDetails?.Type ?? "HTTP_ERROR",
            Title = externalApiProblemDetails?.Title ??
                    ex.Message.Replace(ex.RequestUri?.ToString(), ex.RequestUri?.PathAndQuery),
            Detail = externalApiProblemDetails?.Detail ??
                     ex.Message.Replace(ex.RequestUri.ToString(), ex.RequestUri.PathAndQuery),
            Instance = context.Request.Path,
            ExternalEndpoint = ex.RequestUri?.PathAndQuery,
            ValidationErrors = externalApiProblemDetails?.ValidationErrors ?? new Dictionary<string, string[]>(),
            Severity = externalApiProblemDetails?.Severity ?? LogLevel.Error
        };
    }

    private static void AddExtensions(ApiProblemDetails apiProblemDetails, ApiProblemDetails? externalApiProblemDetails)
    {
        if (externalApiProblemDetails?.Extensions == null) return;

        foreach (var item in externalApiProblemDetails.Extensions)
        {
            var value = item.Value is JObject jObject
                ? jObject.ToObject<Dictionary<string, object>>()
                : item.Value;

            apiProblemDetails.Extensions.Add(item.Key, value);
        }
    }

    private static void AddHeaders(ApiException ex, HttpContext context)
    {
        if (ex.Headers.TryGetValues("X-Sca-Requirements", out var scaRequirements))
            context.Response.Headers.Append("X-Sca-Requirements", new StringValues(scaRequirements.ToArray()));
    }

    private async Task HandleException(Exception ex, HttpContext context)
    {
        if (!context.Response.HasStarted)
        {
            ApiProblemDetails apiProblemDetails = new()
            {
                Status = _statusCodes.TryGetValue(ex.GetType(), out var statusCode)
                    ? statusCode
                    : _statusCodes[typeof(Exception)],
                Type = "ERROR",
                Title = ex.Message,
                Detail = ex.Message,
                Instance = context.Request.Path,
                Severity = LogLevel.Error
            };

            foreach (DictionaryEntry item in ex.Data) apiProblemDetails.Extensions.Add(item.Key.ToString(), item.Value);

            await SetResponse(context, apiProblemDetails);
        }
    }
}