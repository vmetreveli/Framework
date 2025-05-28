using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Framework.Infrastructure.Exceptions;

public class ApiProblemDetails : ProblemDetails
{
    public bool IsApiProblemDetails { get; } = true;
    public string? TraceId { get; } = Activity.Current?.TraceId.ToString();
    public string ExternalEndpoint { get; set; }
    public Dictionary<string, string[]> ValidationErrors { get; set; } = new();
    public LogLevel Severity { get; set; } = LogLevel.Error;
    public new IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();
}