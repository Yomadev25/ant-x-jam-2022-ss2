using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Multiplayer.Online
{
    public class Enemy : MonoBehaviourPun
    {
        [SerializeField] private float _speed;
        [SerializeField] private int _count;
        [SerializeField] private float _score;
        [SerializeField] private List<Vector3> waypoint = new List<Vector3>();
        [SerializeField] private Animator _anim;

        [Header("FLY AREA")]
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minY;
        [SerializeField] private float _maxY;


        bool isDie;
        [HideInInspector] public int index;

        PhotonView _photonView;

        private void Awake()
        {
            index = GameManager.instance.index;
            Debug.Log(index);
        }

        IEnumerator Start()
        {
            // _speed += 0.1f * GameManager.instance.round;
            _photonView = GetComponent<PhotonView>();

            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < _count; i++)
                {
                    waypoint.Add(new Vector3(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY), 0f));
                }

                int x = 0;
                while (true)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, waypoint[x], _speed * Time.deltaTime);
                    this.transform.LookAt(waypoint[x]);

                    if (Vector3.Magnitude(this.transform.position - waypoint[x]) <= 0)
                    {
                        if (x < _count - 1) x++;
                        else break;
                    }
                    yield return null;
                }

                Vector3 endPoint = new Vector3(this.transform.position.x, 8f, 0f);
                while (true)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint, _speed * Time.deltaTime);
                    this.transform.LookAt(endPoint);

                    if (Vector3.Magnitude(this.transform.position - endPoint) <= 0)
                    {
                        _photonView.RPC(nameof(RPCDie), RpcTarget.All);
                    }
                    yield return null;
                }
            }
        }

        public void TakeDamage(int _player)
        {
            _photonView.RPC(nameof(RPCTakeDamage), RpcTarget.All, _player);
        }

        IEnumerator Die()
        {
            transform.eulerAngles = new Vector3(-90f, 180f, 0f);
            _anim.SetTrigger("Death");
            yield return new WaitForSeconds(0.5f);

            Vector3 endPoint = new Vector3(this.transform.position.x, 0f, 0f);
            while (true)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint, 7f * Time.deltaTime);
                this.transform.LookAt(endPoint);

                if (Vector3.Magnitude(this.transform.position - endPoint) <= 0)
                {
                    GameManager.instance.EnemyCheck();
                    Destroy(this.gameObject);
                }
                yield return null;
            }
        }

        [PunRPC]
        public void RPCTakeDamage(int _player)
        {
            if (isDie) return;
            isDie = true;
            GameManager.instance.GetScore(_player, index, _score);
            StopAllCoroutines();
            StartCoroutine(Die());
        }

        [PunRPC]
        public void RPCDie()
        {
            GameManager.instance.EnemyCheck();
            //UserInterface.instance.OnFlyAway(index);
            Destroy(this.gameObject);
        }
    }
}
