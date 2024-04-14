using Newtonsoft.Json;
using MongoDB.Bson;

public class ObjectIdConverter : JsonConverter<ObjectId>
{
    public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            string stringValue = (string)reader.Value;
            return string.IsNullOrEmpty(stringValue) ? ObjectId.Empty : new ObjectId(stringValue);
        }
        return ObjectId.Empty;
    }

    public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}
