using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLevelEditor : MonoBehaviour
{
    float bottomLim, topLimEditor, leftLim, rightLimEditor;
    LevelEditor levelEditor;

    void Start()
    {
        camPos = transform.position;

        bottomLim = 180 / 8 / 2 + 0.25f;
        leftLim = 320 / 8 / 2;
        rightLimEditor = FindObjectOfType<GridManager>().gridSizeX - leftLim;
        topLimEditor = FindObjectOfType<GridManager>().gridSizeY - bottomLim;

        transform.position = new Vector3(leftLim, bottomLim, -10);

        levelEditor = FindObjectOfType<LevelEditor>();
    }

    public float camSpeed = 100;
    Vector3 camPos;
    void Update()
    {
        if (levelEditor.playModeEnabled)
        {
            camPos = FollowPlayer();

            if (levelEditor.boundary == null)
            {
                camPos.x = Mathf.Clamp(camPos.x, leftLim, rightLimEditor);
                camPos.y = Mathf.Clamp(camPos.y, bottomLim, topLimEditor);
            } else
            {
                camPos.x = Mathf.Clamp(camPos.x, leftLim, Mathf.Max(levelEditor.boundary.X - leftLim, leftLim));
                camPos.y = Mathf.Clamp(camPos.y, bottomLim, Mathf.Max(levelEditor.boundary.Y - bottomLim, bottomLim));
            }
        } else
        {
            if (levelEditor.editModeEnabled)
                camPos = transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * camSpeed * Time.deltaTime;

            camPos.x = Mathf.Clamp(camPos.x, leftLim, rightLimEditor);
            camPos.y = Mathf.Clamp(camPos.y, bottomLim, topLimEditor);
        }

        transform.position = camPos;
    }

    PlayerController player;
    Vector3 FollowPlayer()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

        Vector3 playerPos = player.transform.position;
        playerPos.z = -10;

        return playerPos;
    }
}
