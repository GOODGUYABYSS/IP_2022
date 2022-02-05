using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject socketInteractor;

    private Grid gridSpawner;

    private void Awake()
    {
        Grid grid = new Grid(5, 5, 10, socketInteractor);
    }
}
