using System;
using System.Globalization;
using FullSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Wichtel.Save.JsonConverter{
public class SaveFile_HeadDataConverter : fsConverter
{
    public override bool CanProcess(Type type)
    {
        return type == typeof(SaveFile_HeadData);
    }
    
    public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
    {
        SaveFile_HeadData headDataInstance = (SaveFile_HeadData) instance;

        serialized = fsData.CreateDictionary();
        
        //date
        fsData dateData;
        Serializer.TrySerialize(headDataInstance.timeStamp, out dateData);
        serialized.AsDictionary["date"] = dateData;
        
        //playtime
        fsData playtimeData;
        Serializer.TrySerialize(headDataInstance.runPlaytime, out playtimeData);
        serialized.AsDictionary["playtime"] = playtimeData;
        
        //screenshot width and height
        fsData screenshotWidthData = new fsData(headDataInstance.screenshotWidth);
        fsData screenshotHeightData = new fsData(headDataInstance.screenshotHeight);
        serialized.AsDictionary["screenshotWidth"] = screenshotWidthData;
        serialized.AsDictionary["screenshotHeight"] = screenshotHeightData;
        
        //screenshot
        byte[] screenshotAsByteArray = headDataInstance.screenshot.EncodeToPNG();
        string screenshotAsString = Convert.ToBase64String(screenshotAsByteArray);
        fsData screenshotData = new fsData(screenshotAsString);
        serialized.AsDictionary["screenshot"] = screenshotData;

        return fsResult.Success;
    }

    public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
    {
        SaveFile_HeadData headDataInstance = (SaveFile_HeadData) instance;
        
        //date
        fsData dateData = data.AsDictionary["date"];
        DateTime dateTime = DateTime.Now;
        Serializer.TryDeserialize(dateData, ref dateTime);
        headDataInstance.timeStamp = dateTime;
        
        //playtime
        fsData playtimeData = data.AsDictionary["playtime"];
        string playtime = "";
        Serializer.TryDeserialize(playtimeData, ref playtime);
        headDataInstance.runPlaytime = playtime;
        
        //screenshot width and height
        headDataInstance.screenshotWidth = (int)data.AsDictionary["screenshotWidth"].AsInt64;
        headDataInstance.screenshotHeight = (int)data.AsDictionary["screenshotHeight"].AsInt64;
        
        //screenshot
        fsData screenshotData = data.AsDictionary["screenshot"];
        string screenshotAsString = screenshotData.AsString;
        byte[] screenshotAsByteArray = Convert.FromBase64String(screenshotAsString);
        Texture2D screenshot = new Texture2D(headDataInstance.screenshotWidth, headDataInstance.screenshotHeight, TextureFormat.RGBA32, false);
        screenshot.LoadImage(screenshotAsByteArray, false);
        //screenshot.LoadRawTextureData(screenshotAsByteArray);
        headDataInstance.screenshot = screenshot;

        return fsResult.Success;
    }

    public override object CreateInstance(fsData data, Type storageType)
    {
        return new SaveFile_HeadData();
    }

    public override bool RequestCycleSupport(Type storageType) => false;
}
}