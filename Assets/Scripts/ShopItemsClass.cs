using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopItemsClass
{
    public string Name;
    public int Price;
    public string PowerUp;
    public string Description;
    public bool Status;
    public long updatedOn;

    public ShopItemsClass(string name, string powerUp , string description, int price, bool status)
    {
        this.Name = name;
        this.PowerUp = powerUp;
        this.Price = price;
        this.Description = description;
        this.Status = status;
        var timestamp = this.GetTimeUnix();
        this.updatedOn = timestamp;
    }

    public long GetTimeUnix()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }

}
