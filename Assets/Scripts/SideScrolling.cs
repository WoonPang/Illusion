using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어가 좌우로 이동할 때 카메라가 같이 따라갈 수 있도록 하는게 좋다.
public class SideScrolling : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = player.position.x;
        transform.position = cameraPosition;
    }
}
