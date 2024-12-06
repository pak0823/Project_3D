using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Start()
    {
        target = Shared.player.transform;
    }

    private void Update()
    {
        transform.position = target.position + offset;
    }
}