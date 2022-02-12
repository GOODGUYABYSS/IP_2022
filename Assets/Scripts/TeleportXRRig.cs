using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportXRRig : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject thePlayer;

    public void Teleport()
    {
            thePlayer.transform.position = teleportTarget.transform.position;
    }
}
