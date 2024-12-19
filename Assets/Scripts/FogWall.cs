using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWall : MonoBehaviour
{
    [SerializeField] private Vector3 teleportPosition;
    [SerializeField] private BossAI bossToActivate;
    private void OnTriggerStay(Collider other)
    {
        InputHandler playerInputHandler = other.GetComponent<InputHandler>();
        if (playerInputHandler != null)
        {
            if (playerInputHandler.interractInput)
            {
                other.transform.position = teleportPosition;
                bossToActivate.isActive = true;
            }
        }
    }
}
