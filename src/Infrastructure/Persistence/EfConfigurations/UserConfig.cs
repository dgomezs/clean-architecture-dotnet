using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EfConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public const string IdShadowProperty = "InternalId";

        public void Configure(EntityTypeBuilder<User> user)
        {
            var jsonConfig = ConfigureJsonSerialization();

            user.ToTable("User");

            user.Property<long>(IdShadowProperty)
                .HasColumnType("long").ValueGeneratedOnAdd();

            user.Property(t => t.Id)
                .IsRequired()
                .HasConversion(v => v.Value,
                    v => new UserId(v));

            user.Ignore(t => t.DomainEvents);

            user.Property(t => t.Email)
                .IsRequired()
                .HasConversion(
                    v => v.Value,
                    v => EmailAddress.Create(v));

            // improve once this is available https://github.com/dotnet/efcore/issues/13947
            user.Property(p => p.Name)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonConfig),
                    v => JsonSerializer.Deserialize<PersonName>(v, jsonConfig));
            user
                .HasKey(IdShadowProperty);
        }

        private static JsonSerializerOptions ConfigureJsonSerialization()
        {
            return new()
            {
                Converters = {new PersonNameJsonConverter()}
            };
        }
    }

    internal class PersonNameJsonConverter : JsonConverter<PersonName>
    {
        public override PersonName? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            Dictionary<string, string> dict =
                JsonSerializer.Deserialize<Dictionary<string, string>>(ref reader) ??
                throw new JsonException("Can't convert person name");
            var firstName = dict[nameof(PersonName.FirstName)];
            var lastName = dict[nameof(PersonName.LastName)];
            return PersonName.Create(firstName, lastName);
        }

        public override void Write(Utf8JsonWriter writer, PersonName value, JsonSerializerOptions options)
        {
            var dict = new Dictionary<string, string>
            {
                {
                    nameof(value.FirstName), value.FirstName
                },
                {
                    nameof(value.LastName), value.LastName
                }
            };
            JsonSerializer.Serialize(writer, dict);
        }
    }
}