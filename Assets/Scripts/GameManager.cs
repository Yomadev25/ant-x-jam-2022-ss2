using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action onRoundStart, onNextWave, onRoundEnd, onNextRound, onGameover;
    public float score;

    int totalCount = 10;
    int targetCount = 6;
    int count = 0;
    [HideInInspector] public int index = 0;
    [HideInInspector] public int round = 1;
    int level = 1;

    Gun gun;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        gun = FindObjectOfType<Gun>();

        RoundStart();
    }

    public void GetScore(float _score)
    {
        score += _score;
        count++;
        UserInterface.instance.OnScore();
    }

    #region ROUND MANAGER
    public void RoundStart()
    {
        NextWave();
    }

    public void EnemyCheck()
    {
        index++;

        if (index >= totalCount)
        {
            RoundEnd();
        }
        else
        {
            NextWave();
        }
    }

    public void NextWave()
    {   
        gun.ammo = 3;
        gun.isHit = false;
        UserInterface.instance.OnShoot();
        UserInterface.instance.OnNextWave();
        onNextWave?.Invoke();   
    }

    public void RoundEnd()
    {
        index = 9;

        if (count >= targetCount)
        {
            if (count >= 10)
            {               
                Perfect();
            }
            else
            {
                NextRound();
            }
        }
        else
        {
            Gameover();
        }
    }

    public void NextRound()
    {
        index = 0;
        count = 0;
        round++;
        UserInterface.instance.OnNextRound();
        UserInterface.instance.OnNotification("ROUND\n" + round.ToString());

        Invoke("RoundStart", 2f);
        UserInterface.instance.Invoke("OnCloseNotification", 2f);

        if (round > (10 * level)) targetCount++; level++;
    }

    public void Perfect()
    {
        score += 10000;
        UserInterface.instance.OnScore();

        UserInterface.instance.OnNotification("PERFECT \n 10000");
        Invoke("NextRound", 2.5f);
        UserInterface.instance.Invoke("OnCloseNotification", 2f);
    }

    public void Gameover()
    {
        UserInterface.instance.OnNotification("GAME OVER");
    }
    #endregion
}
