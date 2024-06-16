using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintKey : MonoBehaviour
{
    [SerializeField] private Key_Data key;
    [SerializeField] private Key CheckKey;

    private Text text;


    private void Awake()
    {
        text = GetComponent<Text>();
        text.text = key.GetKey(CheckKey).ToString();
    }

    private void Update()
    {
        text.text = key.GetKey(CheckKey).ToString();
    }
}
