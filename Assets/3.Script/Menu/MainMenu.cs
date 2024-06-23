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

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text start;
    [SerializeField] private Text option;
    [SerializeField] private Text exit;
    [SerializeField] private Key_Data Key;
    [SerializeField] private GameObject Setting;
    [SerializeField] private GameObject FadeObject;
    
    private Fade FadeScreen;

    private bool Enter = false;

    private Color DefaultColor = new Color(0.4f, 0.4f, 0.4f);
    //private Color DefaultColor = Color.red;
    private Color SelectColor = new Color(0.76f, 0.76f, 0.76f);

    

    eMain main;

    private void Awake()
    {
        FadeScreen = FadeObject.transform.GetComponent<Fade>();
        FadeScreen.StartFadeIn();
        main = eMain.Start;
        start.color = SelectColor;
        option.color = DefaultColor;
        exit.color = DefaultColor;
        
    }

    private void Update()
    {
        if(!Setting.activeSelf)
        {
            if (Input.GetKeyDown(Key.UpKey))
            {

                if (main.Equals(eMain.Start))
                    main = eMain.Exit;
                else
                    main -= 1;

                SetControlMode(main);
            }
            if (Input.GetKeyDown(Key.DownKey))
            {
                if (main.Equals(eMain.Exit))
                    main = eMain.Start;
                else
                    main += 1;

                SetControlMode(main);
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(Key.ShootKey))
            {
                if(main.Equals(eMain.Options))
                {
                    Setting.SetActive(true);
                }
                else
                {
                    Enter = true;
                    FadeScreen.StartFadeOut();
                }
               
            }

            if (Enter)
            {
                if (main.Equals(eMain.Start))
                {
                    if (FadeScreen.isFinish)
                        Scene_Manager.Instance.SetScene(eScene.Intro);
                }
                else
                {
                    if (FadeScreen.isFinish)
                        OnClickQuitButton();
                }
            }
        }
        
    }

    public void SetControlMode(eMain controlType)
    {
        switch (controlType)
        {
            case eMain.Start:
                start.color = SelectColor;
                option.color = DefaultColor;
                exit.color = DefaultColor;
                break;
            case eMain.Options:
                start.color = DefaultColor;
                option.color = SelectColor;
                exit.color = DefaultColor;
                break;
            case eMain.Exit:
                start.color = DefaultColor;
                option.color = DefaultColor;
                exit.color = SelectColor;
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
