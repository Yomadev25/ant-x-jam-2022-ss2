using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _count;
    [SerializeField] private float _score;
    [SerializeField] private List<Vector3> waypoint = new List<Vector3>();

    [Header("FLY AREA")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;

    Dog _dog;
    bool isDie;

    IEnumerator Start()
    {
        _dog = FindObjectOfType<Dog>();
        _speed += 0.1f * GameManager.instance.round;

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
                _dog.DogTrigger(false, this.transform.GetChild(0).gameObject, 0f);               
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }

    public void TakeDamage()
    {
        if (isDie) return;
        StopAllCoroutines();
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        GameManager.instance.GetScore(_score);

        transform.eulerAngles = Vector3.zero;
        yield return new WaitForSeconds(0.5f);

        Vector3 endPoint = new Vector3(this.transform.position.x, 0f, 0f);
        while (true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, endPoint, 7f * Time.deltaTime);
            this.transform.LookAt(endPoint);

            if (Vector3.Magnitude(this.transform.position - endPoint) <= 0)
            {
                _dog.DogTrigger(true, this.transform.GetChild(0).gameObject, this.transform.position.x);
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }
}
