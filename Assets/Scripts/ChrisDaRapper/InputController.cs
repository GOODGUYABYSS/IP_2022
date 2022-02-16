using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class InputController : MonoBehaviour
{
    public GameObject rightHandControllerObject;

    public InputActionReference triggerActivationReference;

    public delegate void Thingy();

    public UnityEvent onTriggerActivate;

    private void Start()
    {
        triggerActivationReference.action.performed += ActivateBoxCollider;
    }

    public void ActivateBoxCollider(InputAction.CallbackContext obj)
    {
        Debug.Log("ActivateBoxCollider");
        onTriggerActivate.Invoke();
    }
}
