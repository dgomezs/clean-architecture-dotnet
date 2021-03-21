using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using Domain.Errors;
using Newtonsoft.Json.Linq;

namespace CleanArchitecture.TodoList.WebApi.Tests.Config
{
    public static class ErrorAssertionUtils
    {
        public static JToken ExpectedErrorResult(string errorMessage, string errorKey,
            HttpStatusCode expectedHttpStatusCode, List<Error> errors)

        {
            var serializedErrors = JsonSerializer.Serialize(errors, new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return JToken.Parse(
                $"{{\"message\":\"{errorMessage}\",\"status\":{(int) expectedHttpStatusCode},\"errors\":{serializedErrors},\"errorKey\":\"{errorKey}\"}}");
        }
    }
}