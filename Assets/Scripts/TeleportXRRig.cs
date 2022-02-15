using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportXRRig : MonoBehaviour
{
    public Transform teleportTargetin;
    public Transform teleportTargetOut;
    public GameObject thePlayer;

    public void Teleport()
    {
        thePlayer.transform.position = teleportTargetin.transform.position;
    }

    public void TeleportOut()
    {
        thePlayer.transform.position = teleportTargetOut.transform.position;
    }
}
