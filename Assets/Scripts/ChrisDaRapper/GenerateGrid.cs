using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject socketInteractor;

    private GridSystem gridSpawner;

    private void Awake()
    {
        GridSystem grid = new GridSystem(5, 5, 10, socketInteractor);
    }
}
