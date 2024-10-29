using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float smoothScaler = 5.0f;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * smoothScaler);
    }
}
