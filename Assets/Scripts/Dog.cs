using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Animator _anim;
    [SerializeField] private Transform _handPos;
    Spawner _spawner;

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
        }
        else
        {
            _anim.Play("Fail");
        }

        this.transform.LeanMoveLocalY(0.4f, _speed).setDelay(0.2f).setOnComplete(()
            => this.transform.LeanMoveLocalY(-1.1f, _speed).setDelay(0.2f).setOnComplete(() 
            => Idle(isWin)));
    }

    public void Idle(bool isWin)
    {
        if (isWin) Destroy(_handPos.GetChild(0).gameObject);

        _anim.Play("Idle");
        _spawner.Spawn();
    }
}
