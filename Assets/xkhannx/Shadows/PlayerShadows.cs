using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadows : MonoBehaviour
{
    [SerializeField] Material dynMat;
    SpriteRenderer playerSprite;
    List<SpriteRenderer> shadowSprites = new List<SpriteRenderer>();
    Color shadowColor = new Color(0, 0, 0, 0.35f);
    PlayerController player;
    GameObject playerShadowsParent;

    bool dead;
    void Start()
    {
        dead = true;
        playerSprite = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>();
    }

    public void CreatePlayerShadow()
    {
        GenerateShadows(3);
    }

    void Update()
    {
        if (dead) return;
        playerShadowsParent.transform.position = playerSprite.transform.position;
        UpdateShadows();
    }

    void UpdateShadows()
    {
        if (shadowSprites[0].sprite == playerSprite.sprite) return;

        for (int i = 0; i < shadowSprites.Count; i++)
        {
            shadowSprites[i].sprite = playerSprite.sprite;

            if (player.wiggleDir == 0)
            {
                if (player.isFacingRight)
                {
                    shadowSprites[i].transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    shadowSprites[i].transform.localScale = new Vector3(-1, 1, 1);
                }
            } else
            {
                if (player.wiggleDir == 1)
                {
                    shadowSprites[i].transform.localScale = new Vector3(1, 1, 1);
                } else
                {
                    shadowSprites[i].transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }
    }

    void GenerateShadows(int numLayers)
    {
        playerShadowsParent = new GameObject("Player shadows");

        for (int i = 1; i <= numLayers; i++)
        {
            for (int j = 1; j <= (numLayers - i + 1) * 2; j++)
            {
                GameObject newShadow = new GameObject("new shadow" + i.ToString());
                newShadow.tag = "Shadow" + i.ToString();
                newShadow.transform.position = j * new Vector3(0.125f, -0.125f, 0);
                newShadow.transform.parent = playerShadowsParent.transform;

                SpriteRenderer shadowSprite = newShadow.AddComponent<SpriteRenderer>(); 
                shadowSprite.sprite = playerSprite.sprite;
                shadowSprite.sortingOrder = i * 10 - 5;

                shadowSprite.color = shadowColor;
                shadowSprite.material = dynMat;
                shadowSprite.material.renderQueue = 3010 + j * 2 + i * numLayers + 1;
                shadowSprite.material.SetInt("_StencilRef", (i - 1) * 2);

                shadowSprites.Add(shadowSprite);
            }
        }
        dead = false;
    }

    public void DestroyShadows()
    {
        dead = true;
        Destroy(playerShadowsParent);
    }
}
