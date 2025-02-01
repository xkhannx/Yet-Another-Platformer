using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayDoor : MonoBehaviour
{
    public void OpenDoor()
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    void FullyOpen()
    {
        FindObjectOfType<PlayerController>().SetSpriteLayer(100);
        FindObjectOfType<PlayerController>().controlsDisabled = false;
        FindObjectOfType<PlayerShadows>().CreatePlayerShadow();
    }
}
