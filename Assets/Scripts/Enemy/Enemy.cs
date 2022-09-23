using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _count;
    [SerializeField] private float _cooldown;
    [SerializeField] private List<Vector3> waypoint = new List<Vector3>();

    [Header("FLY AREA")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    IEnumerator Start()
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
                yield return new WaitForSeconds(_cooldown);

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
                FindObjectOfType<Spawner>().Spawn();
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }

    public void TakeDamage()
    {
        FindObjectOfType<Spawner>().Spawn();
        Destroy(this.gameObject);       
    }
}
