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

        switch (main)
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

    public void SetControlMode(int controlType)
    {
        switch (main)
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
