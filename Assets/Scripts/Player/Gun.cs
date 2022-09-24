using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform _gunPos;
    [SerializeField] private Transform _gunMesh;

    [SerializeField] private float _xySpeed = 18;
    public int ammo;

    [HideInInspector] public bool isHit;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        LocalMove(h, v, _xySpeed);
        ClampPosition();
        _gunMesh.LookAt(this.transform.position);

        if (Input.GetButtonDown("Fire1") && ammo > 0 && !isHit)
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
                StartCoroutine(ShootEffect(true));
                isHit = true;
                enemy.TakeDamage();
            }
            else
            {
                StartCoroutine(ShootEffect(false));
            }
           
        }
        ammo--;
        UserInterface.instance.OnShoot();
    }

    IEnumerator ShootEffect(bool isTrigger)
    {
        yield return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = _gunPos.TransformDirection(Vector3.forward) * 100f;
        Gizmos.DrawRay(_gunPos.transform.position, direction);
    }
}
