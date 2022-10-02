using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform _handPos;
    Spawner _spawner;

    [Header("SOUND EFFECT")]
    [SerializeField] private AudioSource _score;
    [SerializeField] private AudioSource _laugh;

    private void Start()
    {
        _spawner = FindObjectOfType<Spawner>();
    }

    public void DogTrigger(bool isWin, GameObject Obj, float Xpos)
    {
        this.transform.position = new Vector3(Xpos, -1.1f, this.transform.position.z);

        if (isWin)
        {
            _anim.Play("Win");
            Instantiate(Obj, _handPos);
            _score.Play();
        }
        else
        {
            _anim.Play("Fail");
            UserInterface.instance.OnNotification("Fly Away");
            UserInterface.instance.OnFlyAway();
            _laugh.Play();
        }

        this.transform.LeanMoveLocalY(0.4f, _speed).setDelay(0.2f).setOnComplete(()
            => this.transform.LeanMoveLocalY(-1.1f, _speed).setDelay(0.5f).setOnComplete(() 
            => Idle(isWin)));
    }

    public void Idle(bool isWin)
    {
        if (isWin) Destroy(_handPos.GetChild(0).gameObject);

        _anim.Play("Idle");
        UserInterface.instance.OnCloseNotification();
        GameManager.instance.EnemyCheck();        
    }
}
