using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    PlayerController controller;
    Rigidbody2D rb;
    bool jumpStarted;

    // Calculated/copied physics parameters
    float jumpSpeed;
    float coyoteTimer;
    float jumpBufferTimer;
    [HideInInspector] public float gravity;

	public void InitJumper()
    {
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        jumpSpeed = (2 * controller.playerData.jumpRealHeight * controller.playerData.runSpeed) / controller.playerData.jumpHalfLength;
        gravity = (2 * controller.playerData.jumpRealHeight * controller.playerData.runSpeed * controller.playerData.runSpeed) / (controller.playerData.jumpHalfLength * controller.playerData.jumpHalfLength);

        Physics2D.gravity = new Vector2(0, -gravity);
    }

	public void CheckJump()
	{
		ManageCoyoteTime();

		if (Input.GetButtonDown("Jump"))
		{
			jumpBufferTimer = controller.playerData.jumpBufferTime;
		}
		else
		{
			if (jumpBufferTimer > -1)
			{
				jumpBufferTimer -= Time.deltaTime;
			}
		}

		Jump(JumpAction.Jump);

		if (Input.GetButtonUp("Jump"))
		{
			Jump(JumpAction.CutJump);
		}
	}

	public void Jump(JumpAction jumpAction, Vector2 jumpDir = default)
	{
		switch (jumpAction)
		{
			case JumpAction.Jump:
				if (coyoteTimer > 0 && jumpBufferTimer > 0)
				{
					if (controller.isClimbing)
					{
						controller.StartClimb(false);
					}
					jumpStarted = true;
					rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
					jumpBufferTimer = -1;
					coyoteTimer = -1;
				}
				break;
			case JumpAction.CutJump:
				if (rb.velocity.y > 0 && jumpStarted)
				{
					jumpStarted = false;
					rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
				}
				break;
		}
	}

	private void ManageCoyoteTime()
    {
        if (controller.grounded || controller.isClimbing)
        {
            coyoteTimer = controller.playerData.coyoteTime;
        }
        else
        {
            if (rb.velocity.y <= 0 && coyoteTimer >= -1)
            {
                coyoteTimer -= Time.fixedDeltaTime;
            }
            else
            {
                coyoteTimer = -1;
            }
        }
    }
}

public enum JumpAction { Jump, CutJump }