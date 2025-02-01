using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Parameters", menuName = "Gameplay/Player Parameters")]
public class PlayerParametersSO : ScriptableObject
{
    public float runSpeed = 8;
    public float jumpLength = 5;
    public float jumpHeight = 3;
    //public float maxFallSpeed = 10;

    public float accelerationTimeGrounded = 0.05f;
    public float accelerationTimeInAir = 0.1f;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.15f;

    public float climbSpeed = 3;
    public float jumpHalfLength
    {
        get
        {
            return jumpLength / 2;
        }
    }

    public float jumpRealHeight
    {
        get
        {
            return jumpHeight + 0.5f;
        }
    }
}
