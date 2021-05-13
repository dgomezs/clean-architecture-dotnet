using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using WebApi.Errors;

namespace CleanArchitecture.TodoList.WebApi.Tests.Config
{
    public static class ErrorAssertionUtils
    {
        public static async Task AssertError(HttpResponseMessage response, RestErrorResponse errorResponse)
        {
            await AssertError(response, errorResponse.Code, errorResponse.Message, errorResponse.Errors,
                errorResponse.Status);
        }

        private static JToken ExpectedErrorResult(string code, string errorMessage,
            int expectedHttpStatusCode, IEnumerable<RestError> errors)

        {
            var serializedErrors = JsonSerializer.Serialize(errors, new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return JToken.Parse(
                $"{{\"message\":\"{errorMessage}\",\"status\":{expectedHttpStatusCode},\"errors\":{serializedErrors},\"code\":\"{code}\"}}");
        }

        private static async Task AssertError(HttpResponseMessage response, string code, string errorMessage,
            IEnumerable<RestError> errors,
            int expectedHttpStatusCode)
        {
            response.StatusCode.Should().Be(expectedHttpStatusCode);
            var body = JToken.Parse(await response.Content.ReadAsStringAsync()).ToString();
            var expectedResult = ExpectedErrorResult(code, errorMessage,
                expectedHttpStatusCode, errors).ToString();
            expectedResult.Should().Be(body);
        }
    }
}