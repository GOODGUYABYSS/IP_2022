using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // public GameObject socketInteractorObj;

    private delegate bool TestDelegate();

    private TestDelegate testDelegateFunction;

    private GridSystem gridSpawner;
    private Mesh mesh;

    bool thing = false;

    private void Start()
    {
        testDelegateFunction = () =>
        {
            return true;
        };

        thing = testDelegateFunction();

        Debug.Log("thing: " + thing);
    }

    private void Hello(Test02 test02)
    {
        Debug.Log("Value: " + test02.hi);

        string name = mesh.name;
    }
}
