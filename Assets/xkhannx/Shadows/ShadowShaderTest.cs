using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowShaderTest : MonoBehaviour
{
    [SerializeField] SpriteRenderer mask1;
    [SerializeField] SpriteRenderer mask2;
    [SerializeField] SpriteRenderer static1;
    [SerializeField] SpriteRenderer static2;
    [SerializeField] SpriteRenderer dyn1;
    [SerializeField] SpriteRenderer dyn2;

    void Start()
    {
        SwapStencil();
    }

    void SwapStencil()
    {
        static1.material.SetInt("_StencilRef", 0);
        static1.material.renderQueue = 3001;

        mask1.material.SetInt("_StencilRef", 2);
        mask1.material.renderQueue = 3004;
        static2.material.SetInt("_StencilRef", 2);
        static2.material.renderQueue = 3003;
        
        mask2.material.SetInt("_StencilRef", 4);
        mask2.material.renderQueue = 3002;
        
        dyn1.material.SetInt("_StencilRef", 0);
        dyn1.material.renderQueue = 3005;
        dyn2.material.SetInt("_StencilRef", 2);
        dyn2.material.renderQueue = 3006;
    }
}
