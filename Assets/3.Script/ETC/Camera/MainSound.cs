using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSound : MonoBehaviour
{
    private AudioSource CameraAudio;
    [SerializeField] private AudioClip TitleClip;

    private void Start()
    {
        CameraAudio = GetComponent<AudioSource>();
    }

}
