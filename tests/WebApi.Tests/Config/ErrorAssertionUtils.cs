using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Shared.Errors;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using WebApi;

namespace CleanArchitecture.TodoList.WebApi.Tests.Config
{
    public static class ErrorAssertionUtils
    {
        public static async Task AssertError(HttpResponseMessage response, RestErrorResponse errorResponse)
        {
            await AssertError(response, errorResponse.ErrorKey, errorResponse.Message, errorResponse.Errors,
                errorResponse.Status);
        }

        private static JToken ExpectedErrorResult(string errorKey, string errorMessage,
            int expectedHttpStatusCode, IEnumerable<Error> errors)

        {
            var serializedErrors = JsonSerializer.Serialize(errors, new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return JToken.Parse(
                $"{{\"message\":\"{errorMessage}\",\"status\":{expectedHttpStatusCode},\"errors\":{serializedErrors},\"errorKey\":\"{errorKey}\"}}");
        }

        private static async Task AssertError(HttpResponseMessage response, string errorKey, string errorMessage,
            IEnumerable<Error> errors,
            int expectedHttpStatusCode)
        {
            response.StatusCode.Should().Be(expectedHttpStatusCode);
            var body = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expectedResult = ExpectedErrorResult(errorKey, errorMessage,
                expectedHttpStatusCode, errors);
            body.Should().BeEquivalentTo(expectedResult);
        }
    }
}