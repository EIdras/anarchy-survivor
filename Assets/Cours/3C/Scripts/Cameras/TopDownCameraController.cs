using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float smoothScaler = 5.0f;
    [SerializeField] private float zOffset = -2.0f;
    
    private Vector3 playerPos;
    private Vector3 cameraPos;

    void Update()
    {
        playerPos = player.transform.position;
        cameraPos = transform.position;
        transform.position = Vector3.Lerp(cameraPos, new Vector3(playerPos.x, cameraPos.y, playerPos.z + zOffset), Time.deltaTime * smoothScaler);
    }
}
