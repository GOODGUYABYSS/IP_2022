using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class KeyboardInputSystem : MonoBehaviour
{
    public TMP_InputField inputField;
    public bool capslockStatus = false;

    private void Start()
    {
        Debug.Log("In: " + gameObject.transform.parent);
    }

    public void KeyboardInput(GameObject keyGameObject)
    {
        try
        {
            if (keyGameObject.tag == "Backspace")
            {
                inputField.text = inputField.text.Remove(inputField.text.Length - 1);
                return;
            }

            else if (keyGameObject.tag == "Space")
            {
                inputField.text += " ";
                return;
            }

            else if (keyGameObject.tag == "CapsLock")
            {
                if (capslockStatus)
                {
                    capslockStatus = false;
                }

                else
                {
                    capslockStatus = true;
                }

                return;
            }

            else if (capslockStatus)
            {
                inputField.text += keyGameObject.name.ToUpper()[0];
                return;
            }

            inputField.text += keyGameObject.name.ToLower()[0];
        }

        catch
        {
            Debug.Log("You ran out of space");
        }
    }
}
