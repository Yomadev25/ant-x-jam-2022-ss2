using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _gunPos;
    [SerializeField] private Transform _gunMesh;

    [SerializeField] private float _xySpeed = 18;
    float h;
    float v;
    KeyCode fireKey;
    public int ammo;

    [HideInInspector] public bool isHit;

    [SerializeField] private GameObject _effect;
    private PostProcessVolume _postProcess;
    ColorGrading _colorGrading;

    private void Start()
    {
        _postProcess = FindObjectOfType<PostProcessVolume>();
        _postProcess.profile.TryGetSettings(out _colorGrading);

        fireKey = KeyboardSetting.p1Key;
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        LocalMove(h, v, _xySpeed);
        ClampPosition();
        _gunMesh.LookAt(this.transform.position);

        if (Input.GetKeyDown(fireKey) && ammo > 0 && !isHit)
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
                isHit = true;
                enemy.TakeDamage();
            }
           
        }
        ammo--;
        UserInterface.instance.OnShoot();
        StartCoroutine(ShootEffect());
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
}
