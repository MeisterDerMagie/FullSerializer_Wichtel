using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullSerializer;

namespace Wichtel.Save.JsonConverter{
public class SaveableClassAndStructConverter : fsConverter
{
    public override bool CanProcess(Type type)
    {
        Attribute attribute = type.GetCustomAttribute(typeof(SaveableClassAttribute)) ?? type.GetCustomAttribute(typeof(SaveableStructAttribute));
        return (attribute != null);
    }
    
    public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
    {
        var data = fsData.CreateDictionary();

        var fields = GetSaveableFields(instance);
        var properties = GetSaveableProperties(instance);

        foreach (var field in fields)
        {
            var fieldData = new fsData();
            Serializer.TrySerialize(field.Value, out fieldData);
            data.AsDictionary[field.Key] = fieldData;
        }

        foreach (var property in properties)
        {
            var propertyData = new fsData();
            Serializer.TrySerialize(property.Value, out propertyData);
            data.AsDictionary[property.Key] = propertyData;
        }

        serialized = data;

        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
    {
        var loadedFieldsAndProperties = new Dictionary<string, object>();

        foreach (var kvp in data.AsDictionary)
        {
            object deserialized = null;
            Serializer.TryDeserialize(kvp.Value, ref deserialized);
            loadedFieldsAndProperties.Add(kvp.Key, deserialized);
        }
        
        foreach (KeyValuePair<string, object> field in loadedFieldsAndProperties)
        {
            object fieldValue = field.Value;

            instance.GetType()
                .GetField(field.Key, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                ?.SetValue(instance, fieldValue);
            
            instance.GetType()
                .GetProperty(field.Key, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                ?.SetValue(instance, fieldValue);
        }

        return fsResult.Success;
    }

    public override object CreateInstance(fsData data, Type storageType)
    {
        object instance = Activator.CreateInstance(storageType);
        return instance;
    }

    public override bool RequestCycleSupport(Type storageType)
    {
        return true;
    }

    private Dictionary<string, object> GetSaveableFields(object _object)
    {
        var fields = new Dictionary<string, object>();
        
        var saveableFields = _object.GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Where(f => f.GetCustomAttributes(typeof(SaveableAttribute)).Any());
        
        foreach(FieldInfo field in saveableFields)
        {
            object fieldValue = field.GetValue(_object);
            fields.Add(field.Name, fieldValue);
        }

        return fields;
    }

    private Dictionary<string, object> GetSaveableProperties(object _object)
    {
        var properties = new Dictionary<string, object>();
        
        var saveableProperties = _object.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Where(f => f.GetCustomAttributes(typeof(SaveableAttribute)).Any());
        
        foreach (var property in saveableProperties)
        {
            object propertyValue = property.GetValue(_object);
            properties.Add(property.Name, propertyValue);
        }

        return properties;
    }
}
}