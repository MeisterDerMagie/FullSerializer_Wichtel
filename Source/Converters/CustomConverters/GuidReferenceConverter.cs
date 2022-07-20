using System;
using FullSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wichtel.Save.JsonConverter{
public class GuidReferenceConverter : fsConverter
{
    public override bool CanProcess(Type type)
    {
        return type == typeof(GuidReference);
    }
    
    public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
    {
        Guid guid = ((GuidReference)instance).GetReferenceGuid();
        serialized = new fsData(guid.ToString());
        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
    {
        if (data.Type != fsDataType.String) { return fsResult.Fail("Expected string fsData type but got " + data.Type); }
        
        string dataAsString = data.AsString;
        Guid guid = Guid.Parse(dataAsString);

        Debug.Log("Guid: " + guid);
        
        instance = new GuidReference(guid);
        
        return fsResult.Success;
    }

    public override object CreateInstance(fsData data, Type storageType)
    {
        return new GuidReference();
    }

    public override bool RequestCycleSupport(Type storageType)
    {
        return false;
    }
}
}