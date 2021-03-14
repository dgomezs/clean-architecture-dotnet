using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.ValueObjects;

namespace WebApi.CustomConverters
{
    public class TodoListNameConverter : JsonConverter<TodoListName>
    {
        public override TodoListName? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var s = reader.GetString();
            return TodoListName.Create(s);
        }

        public override void Write(Utf8JsonWriter writer, TodoListName value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
    }
}