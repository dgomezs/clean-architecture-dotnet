using System;
using Domain.Todos.ValueObjects;
using Newtonsoft.Json;

namespace WebApi.CustomConverters
{
    public class TodoListNameConverter : JsonConverter<TodoListName>
    {
        public override void WriteJson(JsonWriter writer, TodoListName value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Name);
        }

        public override TodoListName ReadJson(JsonReader reader, Type objectType, TodoListName existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            return TodoListName.Create(reader.ReadAsString());
        }
    }
}