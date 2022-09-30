using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;
using Photon.Realtime;

namespace Multiplayer.Online
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Transform _gunPos;
        [SerializeField] private Transform _gunMesh;

        [SerializeField] private float _xySpeed = 18;
        string horizontalKey;
        string verticalKey;
        KeyCode fireKey;
        float h;
        float v;
        public int ammo;

        [SerializeField] private GameObject _effect;
        private PostProcessVolume _postProcess;
        ColorGrading _colorGrading;

        PhotonView _photonView;
        Player _player;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            _postProcess = FindObjectOfType<PostProcessVolume>();
            _postProcess.profile.TryGetSettings(out _colorGrading);

            horizontalKey = "Horizontal";
            verticalKey = "Vertical";
            fireKey = KeyboardSetting.p1Key;

            _gunMesh.parent = null;
        }

        void Update()
        {
            if (_photonView.IsMine)
            {
                h = Input.GetAxis(horizontalKey);
                v = Input.GetAxis(verticalKey);

                LocalMove(h, v, _xySpeed);
                ClampPosition();

                if (Input.GetKeyDown(fireKey) && ammo > 0)
                {
                    Shoot();
                }

                _gunMesh.LookAt(this.transform.position);
            }                       
        }

        void LocalMove(float x, float y, float speed)
        {
            transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        }

        void ClampPosition()
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }

        void Shoot()
        {
            RaycastHit hit;
            if (Physics.Raycast(_gunPos.transform.position, _gunPos.transform.TransformDirection(Vector3.forward), out hit, 100f))
            {
                Debug.Log(hit.transform.name);

                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    StartCoroutine(ShootEffect());
                    Instantiate(_effect, enemy.transform.position, Quaternion.identity);
                    //enemy.TakeDamage(1);
                }
            }
            ammo--;
            //UserInterface.instance.OnShoot();
            _photonView.RPC(nameof(Effect), RpcTarget.All);
        }

        IEnumerator ShootEffect()
        {
            _colorGrading.active = true;
            yield return new WaitForSeconds(0.1f);
            _colorGrading.active = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 direction = _gunPos.TransformDirection(Vector3.forward) * 100f;
            Gizmos.DrawRay(_gunPos.transform.position, direction);
        }

        //RPC
        [PunRPC]
        public void Effect()
        {
            StartCoroutine(ShootEffect());
        }
    }
}
