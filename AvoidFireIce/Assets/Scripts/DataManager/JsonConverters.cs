using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.UIElements;

public class Vector2Converter : JsonConverter<Vector2>
{
    public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jobj = JObject.Load(reader);
        var x = (float)jobj["x"];
        var y = (float)jobj["y"];
        return new Vector2(x, y);
    }

    public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("x");
        writer.WriteValue(value.x);
        writer.WritePropertyName("y");
        writer.WriteValue(value.y);

        writer.WriteEndObject();
    }
}

public class QuaternionConverter : JsonConverter<Quaternion>
{
    public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jobj = JObject.Load(reader);
        var w = (float)jobj["W"];
        var x = (float)jobj["X"];
        var y = (float)jobj["Y"];
        var z = (float)jobj["Z"];
        return new Quaternion(x, y, z, w);
    }

    public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("W");
        writer.WriteValue(value.w);
        writer.WritePropertyName("X");
        writer.WriteValue(value.x);
        writer.WritePropertyName("Y");
        writer.WriteValue(value.y);
        writer.WritePropertyName("Z");
        writer.WriteValue(value.z);

        writer.WriteEndObject();
    }

    public class ScaleConverter : JsonConverter<Scale>
    {
        public override Scale ReadJson(JsonReader reader, Type objectType, Scale existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load(reader);
            var x = (float)jobj["x"];
            var y = (float)jobj["y"];
            var z = (float)jobj["z"];
            return new Scale(new Vector3(x, y, z));
        }

        public override void WriteJson(JsonWriter writer, Scale value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("x");
            writer.WriteValue(value.value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.value.z);

            writer.WriteEndObject();
        }
    }

    public class WallInfoConverter : JsonConverter<WallInfo>
    {
        public override WallInfo ReadJson(JsonReader reader, Type objectType, WallInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load(reader);
            var x = (float)jobj["positionX"];
            var y = (float)jobj["positionY"];
            return new WallInfo(new Vector2(x, y));
        }

        public override void WriteJson(JsonWriter writer, WallInfo value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("positionX");
            writer.WriteValue(value.position.x);
            writer.WritePropertyName("positionY");
            writer.WriteValue(value.position.y);

            writer.WriteEndObject();
        }
    }

    public class EnemyInfoConverter : JsonConverter<EnemyInfo>
    {
        public override EnemyInfo ReadJson(JsonReader reader, Type objectType, EnemyInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load(reader);
            var x = (float)jobj["positionX"];
            var y = (float)jobj["positionY"];
            return new EnemyInfo(new Vector2(x, y));
        }

        public override void WriteJson(JsonWriter writer, EnemyInfo value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("positionX");
            writer.WriteValue(value.position.x);
            writer.WritePropertyName("positionY");
            writer.WriteValue(value.position.y);

            writer.WriteEndObject();
        }
    }
}
