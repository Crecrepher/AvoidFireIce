using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;

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

    public class EditorObjInfoConverter : JsonConverter<EditorObjInfo>
    {
        public override EditorObjInfo ReadJson(JsonReader reader, Type objectType, EditorObjInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load(reader);
            var code = (int)jobj["ObjCode"];
            var element = (int)jobj["Element"];

            var posX = (float)jobj["PositionX"];
            var posY = (float)jobj["PositionY"];

            var rotW = (float)jobj["RotationW"];
            var rotX = (float)jobj["RotationX"];
            var rotY = (float)jobj["RotationY"];
            var rotZ = (float)jobj["RotationZ"];
            return new EditorObjInfo(code, element, new Vector2(posX, posY), new Quaternion(rotX, rotY, rotZ, rotW));
        }

        public override void WriteJson(JsonWriter writer, EditorObjInfo value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("ObjCode");
            writer.WriteValue(value.code);
            writer.WritePropertyName("Element");
            writer.WriteValue(value.element);
            writer.WritePropertyName("PositionX");
            writer.WriteValue(value.pos.x);
            writer.WritePropertyName("PositionY");
            writer.WriteValue(value.pos.y);
            writer.WritePropertyName("RotationW");
            writer.WriteValue(value.rot.w);
            writer.WritePropertyName("RotationX");
            writer.WriteValue(value.rot.x);
            writer.WritePropertyName("RotationY");
            writer.WriteValue(value.rot.y);
            writer.WritePropertyName("RotationZ");
            writer.WriteValue(value.rot.z);

            writer.WriteEndObject();
        }
    }

    public class MoveLoopConverter : JsonConverter<MoveLoop>
    {
        //public float startTime;
        //public float playTime;

        //public Vector2 startPos;
        //public Vector2 endPos;
        public override MoveLoop ReadJson(JsonReader reader, Type objectType, MoveLoop existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jobj = JObject.Load(reader);
            var x = (int)jobj["InitCode"];
            var y = (float)jobj["LoopTime"];
            var z = new List<MoveLoopBlock>();
            for (int i = 0; i < (int)jobj["LoopListCount"]; i++)
            {
                MoveLoopBlock loopBlock = new MoveLoopBlock();
                loopBlock.startTime = (float)jobj[$"LLsT{i}"];
                loopBlock.playTime = (float)jobj[$"LLpT{i}"];
                loopBlock.startPos = new Vector2((float)jobj[$"LLsP{i}X"], (float)jobj[$"LLsP{i}Y"]);
                loopBlock.endPos = new Vector2((float)jobj[$"LLeP{i}X"], (float)jobj[$"LLeP{i}Y"]);
            }
            return new MoveLoop(x, y, z);

        }

        public override void WriteJson(JsonWriter writer, MoveLoop value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("InitCode");
            writer.WriteValue(value.initCode);
            writer.WritePropertyName("LoopTime");
            writer.WriteValue(value.loopTime);
            writer.WritePropertyName("LoopListCount");
            writer.WriteValue(value.loopList.Count);
            for (int i = 0; i < value.loopList.Count; i++)
            {
                writer.WritePropertyName($"LLsT{i}");
                writer.WriteValue(value.loopList[i].startTime);
                writer.WritePropertyName($"LLpT{i}");
                writer.WriteValue(value.loopList[i].playTime);
                writer.WritePropertyName($"LLsP{i}X");
                writer.WriteValue(value.loopList[i].startPos.x);
                writer.WritePropertyName($"LLsP{i}Y");
                writer.WriteValue(value.loopList[i].startPos.y);
                writer.WritePropertyName($"LLeP{i}X");
                writer.WriteValue(value.loopList[i].endPos.x);
                writer.WritePropertyName($"LLeP{i}Y");
                writer.WriteValue(value.loopList[i].endPos.y);
            }
            writer.WriteEndObject();
        }
    }
}


