using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChangeKey : MonoBehaviour
{
    private bool isChangeKey = false;
    private int ChangeKeyNum = 0;
    [SerializeField] Key_Data keyData;

    private void Update()
    {
        if (isChangeKey)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        keyData.ChangeKey(keyCode, ChangeKeyNum);
                    }
                }
            }
        }
    }
}
