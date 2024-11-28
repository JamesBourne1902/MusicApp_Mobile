using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class AvailableSongs
{
    public string[][] availableSongs;

    public string ToJson()
    {
        return JsonConvert.SerializeObject(availableSongs, Formatting.Indented);
    }

    public static string[][] FromJson(string json)
    {
        return JsonConvert.DeserializeObject<string[][]>(json);
    }
}
