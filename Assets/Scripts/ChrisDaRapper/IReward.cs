using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReward<T>
{
    float GetReward(T value);
}
