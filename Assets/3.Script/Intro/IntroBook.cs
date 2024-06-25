using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBook : MonoBehaviour
{
    [SerializeField] private GameObject FadeObject;
    [SerializeField] private GameObject ArrowObject;
    [SerializeField] private Key_Data Key;

    private Animator bookAnim;
    private Fade FadeScreen;
    

    private void Awake()
    {
        FadeScreen = FadeObject.GetComponent<Fade>();
        FadeScreen.StartFadeIn();
        bookAnim = GetComponent<Animator>();
        Game_Manager.Instance.isStartGame = true;
        bookAnim.SetTrigger("Next");
    }
    private void Update()
    {
        
            

        if(Game_Manager.Instance.isStartGame)
        {
            AnimatorStateInfo StateInfo = bookAnim.GetCurrentAnimatorStateInfo(0);

            if (StateInfo.normalizedTime > 0.9f)
            {
                Debug.Log(StateInfo.normalizedTime);
                ArrowObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(Key.ShootKey))
                {
                    if (!StateInfo.IsName("Intro10-11"))
                        bookAnim.SetTrigger("Next");
                    else
                        FadeScreen.StartFadeOut();

                }
            }
            else
                ArrowObject.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Game_Manager.Instance.isStartGame = false;
                FadeScreen.StartFadeOut();
            }
        }

        if (!FadeScreen.isFade && FadeScreen.isFinish)
        {
            Scene_Manager.Instance.SetScene(eScene.Tutorial);
        }

    }


}
