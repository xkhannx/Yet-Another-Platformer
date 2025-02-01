using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climber : MonoBehaviour
{
    PlayerController player;
    [SerializeField] Sprite[] climbSprites;
    SpriteRenderer playerSprite;
    public void InitClimber()
    {
        player = GetComponent<PlayerController>();
        playerSprite = player.anim.GetComponent<SpriteRenderer>();
    }

    public void CheckClimb(bool canClimb)
    {
        if (!canClimb)
        {
            if (player.isClimbing)
            {
                player.StartClimb(false);
            }
            return;
        }

        if (!player.isClimbing && ((Mathf.Abs(player.yInput) > 0)
            || (player.rb.velocity.y < 0 && Mathf.Abs(player.xInput) > 0)))
        {
            player.StartClimb(true);
        }
    }

    public void DoClimb()
    {
        player.rb.velocity = new Vector3(player.xInput, player.yInput, 0) * player.playerData.climbSpeed;
    }

    float t = 0;
    int curSprite = 0;
    public void AnimClimb()
    {
        if (player.rb.velocity.magnitude > 0)
        {
            if (t < 0.0001f)
            {
                playerSprite.sprite = climbSprites[curSprite];
                t = 0.2f;
                curSprite = 1 - curSprite;
            }
            t -= Time.deltaTime;

        } else
        {
            t = 0;
        }
    }
}
