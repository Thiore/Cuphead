using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlitCamera : MonoBehaviour
{
    [SerializeField] private RenderTexture sourTexture;
    [SerializeField] private RenderTexture destTexture;

   


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
            Graphics.Blit(sourTexture, destTexture);

    }
}
