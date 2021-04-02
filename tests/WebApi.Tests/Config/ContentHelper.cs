using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace CleanArchitecture.TodoList.WebApi.Tests.Config
{
    public static class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
            => new(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
    }
}