using System;
using FullSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wichtel.Save.JsonConverter{
public class QuaternionConverter : fsConverter
{
    public override bool CanProcess(Type type)
    {
        return type == typeof(Quaternion);
    }
    
    public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
    {
        Quaternion quaternion = (Quaternion) instance;

        if (quaternion == Quaternion.identity)
        {
            serialized = new fsData("Quaternion.identity");
        }
        else
        {
            var quaternionData = fsData.CreateDictionary();
            quaternionData.AsDictionary["x"] = new fsData(quaternion.x);
            quaternionData.AsDictionary["y"] = new fsData(quaternion.y);
            quaternionData.AsDictionary["z"] = new fsData(quaternion.z);
            quaternionData.AsDictionary["w"] = new fsData(quaternion.w);

            serialized = quaternionData;
        }
        
        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
    {
        if (data.Type == fsDataType.String && data.AsString == "Quaternion.identity")
        {
            instance = Quaternion.identity;
        }
        else if (data.Type == fsDataType.Object)
        {
            fsData xData = data.AsDictionary["x"];
            fsData yData = data.AsDictionary["y"];
            fsData zData = data.AsDictionary["z"];
            fsData wData = data.AsDictionary["w"];
            
            float x = (float)xData.AsDouble;
            float y = (float)yData.AsDouble;
            float z = (float)zData.AsDouble;
            float w = (float)wData.AsDouble;

            instance = new Quaternion(x, y, z, w);
        }

        return fsResult.Success;
    }

    public override object CreateInstance(fsData data, Type storageType)
    {
        return Quaternion.identity;
    }

    public override bool RequestCycleSupport(Type storageType) => false;
}
}