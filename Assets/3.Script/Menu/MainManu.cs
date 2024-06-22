using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eMain
{
    Start = 0,
    Options,
    Exit
};

public class MainManu : MonoBehaviour
{
    [SerializeField] private Button start;
    [SerializeField] private Button option;
    [SerializeField] private Button exit;
    [SerializeField] private NextScene next;
    [SerializeField] private Key_Data Key;

    private Color DefaultColor = new Color(100, 100, 100);
    private Color SelectColor = new Color(195, 195, 195);

    private Text Start_text;
    private Text Option_text;
    private Text Exit_text;

    eMain main;

    private void Awake()
    {
        Start_text = start.GetComponentInChildren<Text>();
        Option_text = option.GetComponentInChildren<Text>();
        Exit_text = exit.GetComponentInChildren<Text>();
        next = GetComponent<NextScene>();
        main = eMain.Start;
        Start_text.color = SelectColor;
        Option_text.color = DefaultColor;
        Exit_text.color = DefaultColor;
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(Key.UpKey))
        {
            main -= 1;
            if (main.Equals(-1))
                main = eMain.Exit;
        }
        if (Input.GetKeyDown(Key.DownKey))
        {
            main += 1;
            if (main.Equals(3))
                main = eMain.Start;
        }
        SetControlMode(main);
        if(Input.GetKeyDown(KeyCode.KeypadEnter)||Input.GetKeyDown(Key.ShootKey))
        {
            switch(main)
            {
                case eMain.Start:
                    Start_text.color = SelectColor;
                    Option_text.color = DefaultColor;
                    Exit_text.color = DefaultColor;
                    break;
                case eMain.Options:
                    Start_text.color = DefaultColor;
                    Option_text.color = SelectColor;
                    Exit_text.color = DefaultColor;
                    break;
                case eMain.Exit:
                    OnClickQuitButton();
                    break;
            }
        }
    }

    public void SetControlMode(eMain controlType)
    {
        switch (controlType)
        {
            case eMain.Start:
                Start_text.color = SelectColor;
                Option_text.color = DefaultColor;
                Exit_text.color = DefaultColor;
                break;
            case eMain.Options:
                Start_text.color = DefaultColor;
                Option_text.color = SelectColor;
                Exit_text.color = DefaultColor;
                break;
            case eMain.Exit:
                Start_text.color = DefaultColor;
                Option_text.color = DefaultColor;
                Exit_text.color = SelectColor;
                break;
        }
    }

    public void OnClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }

}
