using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunCheck : MonoBehaviour
{
    public PlayerMovement movement;

    [Header("[L = -1, R = 1]")]
    public int LR;

    void OnTriggerStay ()
    {
        movement.wallRunDir = LR;
    }
    void OnTriggerExit()
    {
        movement.wallRunDir = 0;
    }
}
