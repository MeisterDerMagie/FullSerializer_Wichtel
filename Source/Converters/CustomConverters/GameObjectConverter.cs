using System;
using FullSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wichtel.Save.JsonConverter{
public class GameObjectConverter : fsConverter
{
    public override bool CanProcess(Type type)
    {
        return type == typeof(GameObject);
    }
    
    public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
    {
        serialized = new fsData("Error: Can't serialize GameObject references! Serialize GuidReferences instead!");
        
        return fsResult.Fail("Can't serialize GameObject references! Serialize GuidReferences instead!");
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
    {
        return fsResult.Fail("Can't serialize GameObject references! Serialize GuidReferences instead!");
    }

    public override object CreateInstance(fsData data, Type storageType)
    {
        Debug.LogError("Can't serialize GameObject references! Serialize GuidReferences instead!");
        return null;
    }

    public override bool RequestCycleSupport(Type storageType) => false;
}
}