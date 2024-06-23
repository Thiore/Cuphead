using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : SingleTon<Game_Manager>
{
    public bool isStartGame;
    protected override void Awake()
    {
        base.Awake();
        isStartGame = false;
    }
}
