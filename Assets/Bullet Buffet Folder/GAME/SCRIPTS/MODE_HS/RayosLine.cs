using System.Collections;
using UnityEngine;

public class RayosLine : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;
    LineRenderer LaserLine;

    void Start()
    {
        LaserLine = GetComponentInChildren<LineRenderer>();
        LaserLine.SetWidth (.2f, .2f);
    }

    void Update()
    {
        if (LaserLine.GetComponent<Collider>())
        {
            LaserLine.enabled = false;
        }

        LaserLine.SetPosition(0, StartPoint.position);
        LaserLine.SetPosition(1, EndPoint.position);
        Vector3 dir = StartPoint.position - EndPoint.position;
    }
}
