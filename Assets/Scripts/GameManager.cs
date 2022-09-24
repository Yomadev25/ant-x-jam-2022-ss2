using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action onRoundStart, onNextWave, onRoundEnd, onNextRound, onGameover;
    public float score;

    int count = 10;
    [HideInInspector] public int index = 0;

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

        if (index >= count)
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
        Debug.Log("Spawn");       
    }

    public void RoundEnd()
    {
        Debug.Log("ROUND IS OVER");
    }

    public void NextRound()
    {

    }

    public void Gameover()
    {

    }
    #endregion
}
