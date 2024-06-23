using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum settings
{
    Audio = 0,
    Control,
    Back
};

public class Settings : MonoBehaviour
{
    [SerializeField] private Text Audio_text;
    [SerializeField] private Text Control_text;
    [SerializeField] private Text Back_text;
    [SerializeField] private Key_Data Key;

    private Color DefaultColor = new Color(0.27f, 0.27f, 0.27f);
    private Color SelectColor = Color.red;

    

    settings set;

    private void OnEnable()
    {
        set = settings.Audio;


        Audio_text.color = SelectColor;
        Control_text.color = DefaultColor;
        Back_text.color = DefaultColor;
              
    }
    private void Update()
    {
        if (Input.GetKeyDown(Key.UpKey))
        {

            if (set.Equals(settings.Audio))
                set = settings.Back;
            else
                set -= 1;

            SetControlMode(set);
        }
        if (Input.GetKeyDown(Key.DownKey))
        {
            if (set.Equals(settings.Back))
                set = settings.Audio;
            else
                set += 1;

            SetControlMode(set);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(Key.ShootKey))
        {
            switch (set)
            {
                case settings.Audio:

                    break;
                case settings.Control:

                    break;
                case settings.Back:
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
    public void SetControlMode(settings controlType)
    {
        
        switch (controlType)
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
