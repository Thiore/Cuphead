using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum settings
{
    Audio = 0,
    Control,
    Back
};

public class Settings : MonoBehaviour
{
    [SerializeField] private Button Audio;
    [SerializeField] private Button Control;
    [SerializeField] private Button Back;

    private Color DefaultColor = new Color(100, 100, 100);
    private Color SelectColor = new Color(195, 35, 35);

    private Text Audio_text;
    private Text Control_text;
    private Text Back_text;

    settings set;

    private void OnEnable()
    {
        Audio_text = Audio.GetComponentInChildren<Text>();
        Control_text = Control.GetComponentInChildren<Text>();
        Back_text = Back.GetComponentInChildren<Text>();

        switch (set)
        {
            case settings.Audio:
                Audio_text.color = SelectColor;
                Control_text.color = DefaultColor;
                Back_text.color = DefaultColor;
                break;
            case settings.Control:
                Audio_text.color = DefaultColor;
                Control_text.color = SelectColor;
                Back_text.color = DefaultColor;
                break;
            case settings.Back:
                Audio_text.color = DefaultColor;
                Control_text.color = DefaultColor;
                Back_text.color = SelectColor;
                break;
        }
    }

    public void SetControlMode(int controlType)
    {
        set = (settings)controlType;
        switch (set)
        {
            case settings.Audio:
                Audio_text.color = SelectColor;
                Control_text.color = DefaultColor;
                Back_text.color = DefaultColor;
                break;
            case settings.Control:
                Audio_text.color = DefaultColor;
                Control_text.color = SelectColor;
                Back_text.color = DefaultColor;
                break;
            case settings.Back:
                Audio_text.color = DefaultColor;
                Control_text.color = DefaultColor;
                Back_text.color = SelectColor;
                break;
        }
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Intro");
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

}
