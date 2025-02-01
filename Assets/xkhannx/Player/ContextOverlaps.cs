using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextOverlaps : MonoBehaviour
{
    public LayerMask interactablesLayer;
    BoxCollider2D playerCollider;
    PlayerController player;

    List<Collider2D> results;
    ContactFilter2D contactFilter;
    public void InitContext()
    {
        results = new List<Collider2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = interactablesLayer;
        contactFilter.useTriggers = true;
    }

    public bool canClimb = false;
    public bool canExit = false;

    public void CheckContext()
    {
        ResetFlags();

        if (playerCollider.OverlapCollider(contactFilter, results) > 0)
        {
            foreach (Collider2D col in results)
            {
                switch (col.tag)
                {
                    case "Hooks":
                        canClimb = true;
                        break;
                    case "Exit":
                        canExit = true;
                        break;
                }
                
            }
        }
    }

    void ResetFlags()
    {
        canClimb = false;
        canExit = false;
    }
}
