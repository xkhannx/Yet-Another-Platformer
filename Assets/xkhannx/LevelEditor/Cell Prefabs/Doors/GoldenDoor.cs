using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldenDoor : MonoBehaviour
{
    //bool entered = false;
    public void OpenGoldenDoor()
    {
        GetComponent<Animator>().SetTrigger("Open");
    }

    //PlayerController player;
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        if (entered) return;

    //        entered = true;

    //        player = collision.GetComponent<PlayerController>();
    //        player.controlsDisabled = true;
    //        OpenDoor();

    //        player.transform.DOMove(transform.position + new Vector3(0, -0.5f, 0), 0.5f).OnComplete(MoveEnded);
    //    }
    //}

    public void CloseGoldenDoor()
    {
        GetComponent<Animator>().SetTrigger("Close");
    }

    void RestartLevel()
    {
        FindObjectOfType<PlayerController>().Die();
        FindObjectOfType<PlayInLevelEditor>().Replay();
        //entered = false;
    }
}
