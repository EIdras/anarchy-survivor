using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoulderCameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float smoothScaler = 5.0f;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * smoothScaler);
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * smoothScaler);
    }
}
