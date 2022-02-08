using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mission : MonoBehaviour
{
    public string Name;

    public string Description;
    public string ItemToObtain;
    public bool Status;
    public long updatedOn;

    public Mission(string name, string itemToObtain, bool status)
    {
        this.Name = name;
        this.ItemToObtain = itemToObtain;
        this.Status = status;
        var timestamp = this.GetTimeUnix();
        this.updatedOn = timestamp;
    }

    public long GetTimeUnix()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }

}
