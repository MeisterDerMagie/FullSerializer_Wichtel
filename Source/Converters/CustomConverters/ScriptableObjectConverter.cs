using System;
using FullSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wichtel.Save.JsonConverter{
public class ScriptableObjectConverter : fsConverter
{
    public override bool CanProcess(Type type)
    {
        return type.IsSubclassOf(typeof(ScriptableObject));
    }
    
    public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
    {
        Guid scriptableObjectGuid = ScriptableObjectsGuidMapSingleton.Instance.scriptableObjectsGuidMap.ResolveReference(instance as ScriptableObject);;
        serialized = new fsData(scriptableObjectGuid.ToString());
        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
    {
        if (data.Type != fsDataType.String) { return fsResult.Fail("Expected string fsData type but got " + data.Type); }
        
        string dataAsString = data.AsString;
        Guid scriptableObjectGuid = Guid.Parse(dataAsString);

        bool scriptableObjectExists = ScriptableObjectsGuidMapSingleton.Instance.scriptableObjectsGuidMap.TryResolveGuid(scriptableObjectGuid);

        if (scriptableObjectExists)
        {
            instance = ScriptableObjectsGuidMapSingleton.Instance.scriptableObjectsGuidMap.ResolveGuid(scriptableObjectGuid);
        }
        else
        {
            instance = ScriptableObject.CreateInstance(storageType);
            ScriptableObjectsGuidMapSingleton.Instance.scriptableObjectsGuidMap.RegisterNewSoCreatedAtRuntime((ScriptableObject)instance, scriptableObjectGuid);
        }
            
        return fsResult.Success;
    }

    public override object CreateInstance(fsData data, Type storageType)
    {
        return DummyScriptableObjectReference.Instance.dummyScriptableObject;
    }

    public override bool RequestCycleSupport(Type storageType)
    {
        return false;
    }
}
}