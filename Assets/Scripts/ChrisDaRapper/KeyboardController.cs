using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyboardController : MonoBehaviour
{
    static GameObject keyboardCanvasStatic;
    GameObject keyboardCanvas;
    public static bool allowChangePos = true;

    private void Awake()
    {
        Debug.Log("Debug 1");
        keyboardCanvas = gameObject.transform.Find("KeyboardCanvas").gameObject;
        keyboardCanvasStatic = keyboardCanvas;

        keyboardCanvasStatic.SetActive(false);
    }

    private void Update()
    {
        if (Authentication.loggedIn && allowChangePos)
        {
            // keyboardCanvasStatic.transform.position = new Vector3(-0.44f, 2f, 2.024f);
            // keyboardCanvas.transform.position = new Vector3(-9.684f, -1.63f, 8.321f);
            keyboardCanvas.transform.parent.position = new Vector3(-3.38f, 3.174f, -1.298416f);

            allowChangePos = false;
            // keyboardCanvasStatic.transform.rotation = Quaternion.Euler(new Vector3(-87.275f, 208.528f, -38.046f));
            keyboardCanvasStatic.transform.parent.rotation = Quaternion.Euler(new Vector3(51.137f, -88.077f, 0));

            // keyboardCanvas.transform.localScale = new Vector3(0.1426f, )
        }
    }

    public void SetInputField(TMP_InputField inputField)
    {
        Debug.Log("Inputfield: " + keyboardCanvasStatic.GetComponent<KeyboardInputSystem>().inputField);
        keyboardCanvasStatic.GetComponent<KeyboardInputSystem>().inputField = inputField;
    }

    public void TurnOnKeyboard()
    {
        keyboardCanvasStatic.SetActive(true);
    }

    public void TurnOffKeyboard()
    {
        keyboardCanvasStatic.SetActive(false);
    }
}
