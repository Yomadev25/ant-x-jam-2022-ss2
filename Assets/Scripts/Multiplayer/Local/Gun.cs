using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Multiplayer.Local
{
    public class Gun : MonoBehaviour
    {
        public enum n_Player { PLAYER_1, PLAYER_2 }
        [SerializeField] private n_Player nPlayer;

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
        [SerializeField] private GameObject _muzzle;
        private PostProcessVolume _postProcess;
        ColorGrading _colorGrading;

        [Header("SOUND EFFECT")]
        [SerializeField] private AudioSource _shoot;
        [SerializeField] private AudioSource _duckDie;
        [SerializeField] private AudioSource _scoreSound;
        public AudioSource _reloadSound;

        private void Start()
        {
            _postProcess = FindObjectOfType<PostProcessVolume>();
            _postProcess.profile.TryGetSettings(out _colorGrading);

            if (nPlayer == n_Player.PLAYER_1)
            {
                horizontalKey = "Horizontal";
                verticalKey = "Vertical";
                fireKey = KeyboardSetting.p1Key;
            }
            else
            {
                horizontalKey = "Horizontal2";
                verticalKey = "Vertical2";
                fireKey = KeyboardSetting.p2Key;
            }
        }

        void Update()
        {
            h = Input.GetAxis(horizontalKey);
            v = Input.GetAxis(verticalKey);

            LocalMove(h, v, _xySpeed);
            ClampPosition();
            _gunMesh.LookAt(this.transform.position);

            if (Input.GetKeyDown(fireKey) && ammo > 0)
            {
                Shoot();
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
                    enemy.TakeDamage(nPlayer == n_Player.PLAYER_1? 1 : 2);
                    _duckDie.Play();
                    _scoreSound.Play();
                }
            }
            ammo--;
            _shoot.Play();
            UserInterface.instance.OnShoot();
            StartCoroutine(ShootEffect());
        }

        IEnumerator ShootEffect()
        {
            _colorGrading.contrast.overrideState = true;
            _muzzle.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            _muzzle.SetActive(false);
            _colorGrading.contrast.overrideState = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Vector3 direction = _gunPos.TransformDirection(Vector3.forward) * 100f;
            Gizmos.DrawRay(_gunPos.transform.position, direction);
        }
    }
}
