using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopItemsClass
{
    public string name;
    public int price;
    public int creditGeneration;
    public string powerUp;
    public string description;
    public bool status;
    public long updatedOn;

    public ShopItemsClass(string name, string powerUp, string description, int price, int creditGeneration, bool status)
    {
        this.name = name;
        this.powerUp = powerUp;
        this.price = price;
        this.description = description;
        this.status = status;
        this.creditGeneration = creditGeneration;
        var timestamp = this.GetTimeUnix();
        this.updatedOn = timestamp;
    }

    public long GetTimeUnix()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }

}