using System;
using FullSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wichtel.Save.JsonConverter{
public class VectorConverter : fsConverter
{
    public override bool CanProcess(Type type)
    {
        return type == typeof(Vector2) ||
               type == typeof(Vector3) ||
               type == typeof(Vector4) ||
               type == typeof(Vector2Int) ||
               type == typeof(Vector3Int);
    }
    
    public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
    {
        serialized = CreateVectorData(instance, storageType);
        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
    {
        if      (data.Type == fsDataType.String) instance = DeserializeSpecialVector(data, storageType);
        else if (data.Type == fsDataType.Object) instance = DeserializeNumericalVector(data, storageType);

        return (instance != null) ? fsResult.Success : fsResult.Fail("Could not deserialize Vector!");
    }

    public override object CreateInstance(fsData data, Type storageType)
    {
        if(storageType == typeof(Vector2)) return Vector2.zero;
        if(storageType == typeof(Vector3)) return Vector3.zero;
        if(storageType == typeof(Vector4)) return Vector4.zero;
        if(storageType == typeof(Vector2Int)) return Vector2Int.zero;
        if(storageType == typeof(Vector3Int)) return Vector3Int.zero;

        Debug.Log("Could not create instance of Vector. Received type: " + storageType);
        return null;
    }

    public override bool RequestCycleSupport(Type storageType) => false;

    private fsData CreateVectorData(object instance, Type storageType)
    {
        if (storageType == typeof(Vector2))
        {
            Vector2 vector = (Vector2) instance;
            
            if(vector == Vector2.zero) return new fsData("Vector2.zero");
            if(vector == Vector2.one) return new fsData("Vector2.one");
            if(vector == Vector2.up) return new fsData("Vector2.up");
            if(vector == Vector2.down) return new fsData("Vector2.down");
            if(vector == Vector2.right) return new fsData("Vector2.right");
            if(vector == Vector2.left) return new fsData("Vector2.left");
            if(vector == Vector2.negativeInfinity) return new fsData("Vector2.negativeInfinity");
            if(vector == Vector2.positiveInfinity) return new fsData("Vector2.positiveInfinity");
            
            var vectorData = fsData.CreateDictionary();
            vectorData.AsDictionary["x"] = new fsData(vector.x);
            vectorData.AsDictionary["y"] = new fsData(vector.y);
            return vectorData;
        }
        if(storageType == typeof(Vector3))
        {
            Vector3 vector = (Vector3) instance;
            
            if(vector == Vector3.zero) return new fsData("Vector3.zero");
            if(vector == Vector3.one) return new fsData("Vector3.one");
            if(vector == Vector3.up) return new fsData("Vector3.up");
            if(vector == Vector3.down) return new fsData("Vector3.down");
            if(vector == Vector3.right) return new fsData("Vector3.right");
            if(vector == Vector3.left) return new fsData("Vector3.left");
            if(vector == Vector3.back) return new fsData("Vector3.back");
            if(vector == Vector3.forward) return new fsData("Vector3.forward");
            if(vector == Vector3.negativeInfinity) return new fsData("Vector3.negativeInfinity");
            if(vector == Vector3.positiveInfinity) return new fsData("Vector3.positiveInfinity");
            
            var vectorData = fsData.CreateDictionary();
            vectorData.AsDictionary["x"] = new fsData(vector.x);
            vectorData.AsDictionary["y"] = new fsData(vector.y);
            vectorData.AsDictionary["z"] = new fsData(vector.z);
            return vectorData;
        }
        if(storageType == typeof(Vector4))
        {
            Vector4 vector = (Vector4) instance;
            
            if(vector == Vector4.zero) return new fsData("Vector4.zero");
            if(vector == Vector4.one) return new fsData("Vector4.one");
            if(vector == Vector4.negativeInfinity) return new fsData("Vector4.negativeInfinity");
            if(vector == Vector4.positiveInfinity) return new fsData("Vector4.positiveInfinity");
            
            var vectorData = fsData.CreateDictionary();
            vectorData.AsDictionary["x"] = new fsData(vector.x);
            vectorData.AsDictionary["y"] = new fsData(vector.y);
            vectorData.AsDictionary["z"] = new fsData(vector.z);
            vectorData.AsDictionary["w"] = new fsData(vector.w);
            return vectorData;
        }
        if(storageType == typeof(Vector2Int))
        {
            Vector2Int vector = (Vector2Int) instance;
            
            if(vector == Vector2Int.zero) return new fsData("Vector2Int.zero");
            if(vector == Vector2Int.one) return new fsData("Vector2Int.one");
            if(vector == Vector2Int.up) return new fsData("Vector2Int.up");
            if(vector == Vector2Int.down) return new fsData("Vector2Int.down");
            if(vector == Vector2Int.right) return new fsData("Vector2Int.right");
            if(vector == Vector2Int.left) return new fsData("Vector2Int.left");
            
            var vectorData = fsData.CreateDictionary();
            vectorData.AsDictionary["x"] = new fsData(vector.x);
            vectorData.AsDictionary["y"] = new fsData(vector.y);
            return vectorData;
        }
        if(storageType == typeof(Vector3Int))
        {
            Vector3Int vector = (Vector3Int) instance;
            
            if(vector == Vector3Int.zero) return new fsData("Vector3Int.zero");
            if(vector == Vector3Int.one) return new fsData("Vector3Int.one");
            if(vector == Vector3Int.up) return new fsData("Vector3Int.up");
            if(vector == Vector3Int.down) return new fsData("Vector3Int.down");
            if(vector == Vector3Int.right) return new fsData("Vector3Int.right");
            if(vector == Vector3Int.left) return new fsData("Vector3Int.left");
            
            var vectorData = fsData.CreateDictionary();
            vectorData.AsDictionary["x"] = new fsData(vector.x);
            vectorData.AsDictionary["y"] = new fsData(vector.y);
            vectorData.AsDictionary["z"] = new fsData(vector.z);
            return vectorData;
        }

        Debug.LogError("Could not serialize Vector!");
        return new fsData("Error: could not serialize Vector! Tried to serialize " + storageType + " through the VectorConverter.");
    }

    private object DeserializeSpecialVector(fsData data, Type storageType)
    {
        string dataAsString = data.AsString;
        
        if(storageType == typeof(Vector2))
        {
            if(dataAsString == "Vector2.zero") return Vector2.zero;
            if(dataAsString == "Vector2.one") return Vector2.one;
            if(dataAsString == "Vector2.up") return Vector2.up;
            if(dataAsString == "Vector2.down") return Vector2.down;
            if(dataAsString == "Vector2.right") return Vector2.right;
            if(dataAsString == "Vector2.left") return Vector2.left;
            if(dataAsString == "Vector2.negativeInfinity") return Vector2.negativeInfinity;
            if(dataAsString == "Vector2.positiveInfinity") return Vector2.positiveInfinity;
        }
        if(storageType == typeof(Vector3))
        {
            if(dataAsString == "Vector3.zero") return Vector3.zero;
            if(dataAsString == "Vector3.one") return Vector3.one;
            if(dataAsString == "Vector3.up") return Vector3.up;
            if(dataAsString == "Vector3.down") return Vector3.down;
            if(dataAsString == "Vector3.right") return Vector3.right;
            if(dataAsString == "Vector3.left") return Vector3.left;
            if(dataAsString == "Vector3.back") return Vector3.back;
            if(dataAsString == "Vector3.forward") return Vector3.forward;
            if(dataAsString == "Vector3.negativeInfinity") return Vector3.negativeInfinity;
            if(dataAsString == "Vector3.positiveInfinity") return Vector3.positiveInfinity;
        }
        if(storageType == typeof(Vector4))
        {
            if(dataAsString == "Vector4.zero") return Vector4.zero;
            if(dataAsString == "Vector4.one") return Vector4.one;
            if(dataAsString == "Vector4.negativeInfinity") return Vector4.positiveInfinity;
            if(dataAsString == "Vector4.positiveInfinity") return Vector4.negativeInfinity;
        }
        if(storageType == typeof(Vector2Int))
        {
            if(dataAsString == "Vector2Int.zero") return Vector2Int.zero;
            if(dataAsString == "Vector2Int.one") return Vector2Int.one;
            if(dataAsString == "Vector2Int.up") return Vector2Int.up;
            if(dataAsString == "Vector2Int.down") return Vector2Int.down;
            if(dataAsString == "Vector2Int.right") return Vector2Int.right;
            if(dataAsString == "Vector2Int.left") return Vector2Int.left;
        }
        if(storageType == typeof(Vector3Int))
        {
            if(dataAsString == "Vector3Int.zero") return Vector3Int.zero;
            if(dataAsString == "Vector3Int.one") return Vector3Int.one;
            if(dataAsString == "Vector3Int.up") return Vector3Int.up;
            if(dataAsString == "Vector3Int.down") return Vector3Int.down;
            if(dataAsString == "Vector3Int.right") return Vector3Int.right;
            if(dataAsString == "Vector3Int.left") return Vector3Int.left;
        }
        
        Debug.LogError("Could not deserialize Vector! Tried to deserialize " + storageType + " through the VectorConverter.");
        return null;
    }
    private object DeserializeNumericalVector(fsData data, Type storageType)
    {
        if(storageType == typeof(Vector2))
        {
            fsData xData = data.AsDictionary["x"];
            fsData yData = data.AsDictionary["y"];
            
            float x = (float)xData.AsDouble;
            float y = (float)yData.AsDouble;
            return new Vector2(x, y);
        }
        if(storageType == typeof(Vector3))
        {
            fsData xData = data.AsDictionary["x"];
            fsData yData = data.AsDictionary["y"];
            fsData zData = data.AsDictionary["z"];
            
            float x = (float)xData.AsDouble;
            float y = (float)yData.AsDouble;
            float z = (float)zData.AsDouble;
            return new Vector3(x, y, z);
        }
        if(storageType == typeof(Vector4))
        {
            fsData xData = data.AsDictionary["x"];
            fsData yData = data.AsDictionary["y"];
            fsData zData = data.AsDictionary["z"];
            fsData wData = data.AsDictionary["w"];
            
            float x = (float)xData.AsDouble;
            float y = (float)yData.AsDouble;
            float z = (float)zData.AsDouble;
            float w = (float)wData.AsDouble;
            return new Vector4(x, y, z, w);
        }
        if(storageType == typeof(Vector2Int))
        {
            fsData xData = data.AsDictionary["x"];
            fsData yData = data.AsDictionary["y"];
            
            int x = (int)xData.AsInt64;
            int y = (int)yData.AsInt64;
            return new Vector2Int(x, y);
        }
        if(storageType == typeof(Vector3Int))
        {
            fsData xData = data.AsDictionary["x"];
            fsData yData = data.AsDictionary["y"];
            fsData zData = data.AsDictionary["z"];
            
            int x = (int)xData.AsInt64;
            int y = (int)yData.AsInt64;
            int z = (int)zData.AsInt64;
            return new Vector3Int(x, y, z);
        }
        
        Debug.LogError("Could not deserialize Vector! Tried to deserialize " + storageType + " through the VectorConverter.");
        return null;
    }
}
}