using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCreator : MonoBehaviour
{
    [SerializeField] Material maskMat;
    [SerializeField] Material staticMat;
    [SerializeField] Material dynMat;

    List<SpriteRenderer> curSprites;
    List<SpriteRenderer> spriteMasks;

    List<GameObject> shadowLayers;

    Color shadowColor = new Color(0, 0, 0, 0.35f);
    GameObject shadowsParent;
    int ppu = 8;
    [SerializeField] int numLayers = 5;
    public void CreateShadows()
    {
        shadowLayers = new List<GameObject>();

        SetUpMasks();

        for (int i = 1; i < numLayers; i++)
        {
            SelectSpriteList(i);
            GameObject shadowLayerParent = new GameObject("shadow layer " + i.ToString() + " parent");
            shadowLayerParent.transform.parent = shadowsParent.transform;

            shadowLayers.Add(shadowLayerParent);

            if (curSprites.Count == 0)
                continue;

            GenerateShadows(i, shadowLayerParent);
        }

        shadowsParent.transform.parent = FindObjectOfType<PlaygroundCreator>().transform;


        for (int i = 1; i < numLayers; i++)
        {
            GameObject[] curLayerShadows = GameObject.FindGameObjectsWithTag("Shadow" + i.ToString());

            if (curLayerShadows.Length == 0)
                continue;

            curSprites.Clear();
            foreach (var shadow in curLayerShadows)
            {
                curSprites.Add(shadow.GetComponent<SpriteRenderer>());
            }

            ShapeSize(false);

            SpriteRenderer combinedShadow = shadowLayers[i - 1].AddComponent<SpriteRenderer>();
            combinedShadow.sprite = CombineShadows();
            combinedShadow.sortingOrder = i * 10 - 5;

            combinedShadow.transform.position = minCorner;
            combinedShadow.material = staticMat;

            combinedShadow.material.renderQueue = 3000 + i * 2 - 1;
            combinedShadow.material.SetInt("_StencilRef", (i - 1) * 2);
        }

        for (int i = 0; i < shadowLayers.Count; i++)
        {
            foreach (Transform child in shadowLayers[i].transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    Sprite CombineShadows()
    {
        var newTex = new Texture2D(
            Mathf.RoundToInt((maxCorner.x - minCorner.x) * ppu),
            Mathf.RoundToInt((maxCorner.y - minCorner.y) * ppu),
            TextureFormat.ARGB32, false);

        newTex.filterMode = FilterMode.Point;

        for (int x = 0; x < newTex.width; x++)
        {
            for (int y = 0; y < newTex.height; y++)
            {
                newTex.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }

        for (int i = 0; i < curSprites.Count; i++)
        {
            Texture2D croppedTex = curSprites[i].sprite.texture;

            for (int x = 0; x < croppedTex.width; x++)
            {
                for (int y = 0; y < croppedTex.height; y++)
                {
                    if (croppedTex.GetPixel(x, y).a > 0)
                    {
                        int xx = Mathf.RoundToInt((curSprites[i].transform.position.x - minCorner.x)
                            * ppu) + x;
                        int yy = Mathf.RoundToInt((curSprites[i].transform.position.y - minCorner.y)
                            * ppu) + y;

                        newTex.SetPixel(xx, yy, shadowColor);
                    }
                }
            }
        }

        newTex.Apply();

        var newSprite = Sprite.Create(
            newTex,
            new Rect(0, 0, newTex.width, newTex.height),
            new Vector2(0, 0),
            8
            );
        newSprite.name = "Mask sprite";

        return newSprite;
    }

    void GenerateShadows(int curLayer, GameObject _shadowLayerParent)
    {
        for (int i = 1; i <= curLayer; i++)
        {
            //Debug.Log("Layer " + curLayer.ToString() + ", shadow cascade " + i.ToString());

            Sprite projectedShape = ProjectShadow(spriteMasks[curLayer - 1].sprite.texture, (curLayer - i + 1) * 2);

            GameObject newShadow = new GameObject("new shadow" + i.ToString());
            newShadow.tag = "Shadow" + i.ToString();

            SpriteRenderer shadowSprite = newShadow.AddComponent<SpriteRenderer>();
            shadowSprite.sprite = projectedShape;
            shadowSprite.sortingOrder = i * 10 - 5;

            shadowSprite.transform.position = new Vector2(
                spriteMasks[curLayer - 1].transform.position.x,
                spriteMasks[curLayer - 1].transform.position.y) + Vector2.down * (curLayer - i + 1) * 0.25f;

            shadowSprite.transform.parent = _shadowLayerParent.transform;
        }
    }

    Sprite ProjectShadow(Texture2D combinedShape, int numSteps)
    {
        var newTex = new Texture2D(
            combinedShape.width + numSteps,
            combinedShape.height + numSteps,
            TextureFormat.ARGB32, false);
        newTex.filterMode = FilterMode.Point;

        for (int x = 0; x < newTex.width; x++)
        {
            for (int y = 0; y < newTex.height; y++)
            {
                newTex.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }

        for (int x = 0; x < combinedShape.width; x++)
        {
            for (int y = 0; y < combinedShape.height; y++)
            {
                if (combinedShape.GetPixel(x, y).a > 0)
                {
                    for (int i = 0; i < numSteps; i++)
                    {
                        newTex.SetPixel(x + i + 1, y + numSteps - i - 1, shadowColor);
                    }
                }
            }
        }

        newTex.Apply();

        var newSprite = Sprite.Create(
            newTex,
            new Rect(0, 0, newTex.width, newTex.height),
            new Vector2(0, 0),
            8
            );
        newSprite.name = "Shadow sprite";

        return newSprite;
    }

    Sprite CombineObjects()
    {
        var newTex = new Texture2D(
            Mathf.RoundToInt((maxCorner.x - minCorner.x) * ppu), 
            Mathf.RoundToInt((maxCorner.y - minCorner.y) * ppu), 
            TextureFormat.ARGB32, false);

        newTex.filterMode = FilterMode.Point;

        for (int x = 0; x < newTex.width; x++)
        {
            for (int y = 0; y < newTex.height; y++)
            {
                newTex.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }

        for (int i = 0; i < curSprites.Count; i++)
        {
            Texture2D croppedTex = CroppedTexture(curSprites[i].sprite);

            for (int x = 0; x < croppedTex.width; x++)
            {
                for (int y = 0; y < croppedTex.height; y++)
                {
                    if (croppedTex.GetPixel(x, y).a > 0)
                    {
                        int xx = (int)(curSprites[i].transform.position.x - curSprites[i].size.x / 2 - minCorner.x)
                            * ppu + x;
                        int yy = (int)(curSprites[i].transform.position.y - curSprites[i].size.y / 2 - minCorner.y)
                            * ppu + y;
                        
                        newTex.SetPixel(xx, yy, shadowColor);
                    }
                }
            }
        }

        newTex.Apply();
        
        var newSprite = Sprite.Create(
            newTex,
            new Rect(0, 0, newTex.width, newTex.height),
            new Vector2(0, 0),
            8
            );
        newSprite.name = "Mask sprite";

        return newSprite;
    }

    Texture2D CroppedTexture(Sprite sprite)
    {
        var croppedTexture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);

        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }

    Vector2 minCorner;
    Vector2 maxCorner;
    void ShapeSize(bool center)
    {
        minCorner = new Vector2(float.MaxValue, float.MaxValue);
        maxCorner = new Vector2(-float.MaxValue, -float.MaxValue);

        foreach (var sprite in curSprites)
        {
            Vector2 pos = sprite.transform.position;
            Vector2 curMinCorner = center ? pos - sprite.size / 2 : pos;
            if (curMinCorner.x < minCorner.x)
            {
                minCorner.x = curMinCorner.x;
            }
            if (curMinCorner.y < minCorner.y)
            {
                minCorner.y = curMinCorner.y;
            }

            Vector2 curMaxCorner = center ? pos + sprite.size / 2 : pos + sprite.size;
            if (curMaxCorner.x > maxCorner.x)
            {
                maxCorner.x = curMaxCorner.x;
            }
            if (curMaxCorner.y > maxCorner.y)
            {
                maxCorner.y = curMaxCorner.y;
            }
        }
    }

    private void SelectSpriteList(int i)
    {
        switch (i)
        {
            case 0:
                curSprites = FindObjectOfType<PlaygroundCreator>().spriteLayer0;
                break;
            case 1:
                curSprites = FindObjectOfType<PlaygroundCreator>().spriteLayer1;
                break;
            case 2:
                curSprites = FindObjectOfType<PlaygroundCreator>().spriteLayer2;
                break;
            case 3:
                curSprites = FindObjectOfType<PlaygroundCreator>().spriteLayer3;
                break;
            case 4:
                curSprites = FindObjectOfType<PlaygroundCreator>().spriteLayer4;
                break;
        }
    }

    void SetUpMasks()
    {
        shadowsParent = new GameObject("Shadows Parent");

        spriteMasks = new List<SpriteRenderer>();

        for (int i = 1; i < numLayers; i++)
        {
            SelectSpriteList(i);
            GameObject layer1Mask = new GameObject("Layer Mask " + i.ToString());
            SpriteRenderer mask = layer1Mask.AddComponent<SpriteRenderer>();
            mask.transform.parent = shadowsParent.transform;

            if (curSprites.Count == 0)
            {
                spriteMasks.Add(mask);
                mask.enabled = false;
                continue;
            }

            ShapeSize(true);

            mask.sprite = CombineObjects();
            mask.material = maskMat;
            mask.color = new Color(0, 0, 0, 1f / 255f);
            mask.transform.position = minCorner;

            spriteMasks.Add(mask);
            mask.material.renderQueue = 3000 + i * 2;
            mask.material.SetInt("_StencilRef", i*2);
        }
    }


}
