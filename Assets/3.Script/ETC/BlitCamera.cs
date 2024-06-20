using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlitCamera : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(renderTexture, destination);
    }
}
