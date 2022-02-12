using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    public GameObject socketInteractor;
    public GameObject gridParent;

    private GridSystem gridSpawner;

    private void Awake()
    {
        GridSystem grid = new GridSystem(4, 4, 0.8f, 0, 0, 0, socketInteractor, gridParent);
    }
}
