using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jsonPic : MonoBehaviour
{
    public int id;
    public bool hasDone;

    public void SetDone()
    {
        PlayerPrefs.SetString("json " + id, "true");
        hasDone = true;
    }
    public bool CheckDone()
    {
        var data = PlayerPrefs.GetString("json " + id, "true");
        bool HasDone = JsonConvert.DeserializeObject<bool>(data);
        return HasDone;
    }
}
