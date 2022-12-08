using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : FastSingleton<CameraFollow>
{
    [SerializeField] public Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] public float speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position =Vector3.Lerp(transform.position,player.transform.position + offset,speed*Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, player.transform.position + offset)< 0.01f) transform.position= player.transform.position + offset;
    }
}
