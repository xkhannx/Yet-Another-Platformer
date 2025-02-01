using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class CollisionManager : MonoBehaviour
{
	public LayerMask collisionMask;

	public float skinWidth;
	public float rayLength = 0.05f;
	[Range(2, 10)] public int downRayCount = 2;

	float downRaySpacing;

	BoxCollider2D boxCollider;
	Vector2 bottomLeft;

	public int edge = 0;
	void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}
    
    public RaycastHit2D GroundRay()
	{
		UpdateRaycastOrigins();

		float directionY = -1;
		for (int i = 0; i < downRayCount; i++)
		{
			Debug.DrawRay(bottomLeft + Vector2.right * downRaySpacing * i, Vector2.up * directionY * rayLength, Color.red);
		}

		RaycastHit2D groundHit = new RaycastHit2D();
		int raysPresent = 0;
		for (int i = 0; i < downRayCount; i++)
		{
			Vector2 rayOrigin = bottomLeft + Vector2.right * downRaySpacing * i;
			RaycastHit2D ray = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			if (ray)
			{
				raysPresent += (int)Mathf.Pow(2, i);
				groundHit = ray;
			}
		}

        if (raysPresent == 1)
        {
			edge = -1;
        }
		else if (raysPresent == (int)Mathf.Pow(2, downRayCount - 1))
        {
			edge = 1;
		}
        else
        {
			edge = 0;
        }

        return groundHit;
	}

	void UpdateRaycastOrigins()
	{
		Bounds bounds = boxCollider.bounds;

		bottomLeft = new Vector2(bounds.min.x - skinWidth, bounds.min.y + skinWidth);
	}

	void CalculateRaySpacing()
	{
		Bounds bounds = boxCollider.bounds;

		downRaySpacing = (bounds.size.x + 2 * skinWidth) / (downRayCount - 1);
	}
}