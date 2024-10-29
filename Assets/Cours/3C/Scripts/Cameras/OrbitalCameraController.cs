using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCameraController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform pivot;
    [SerializeField] private Camera orbitalCamera;
    [SerializeField] private float smoothScaler = 5.0f;

    void Update()
    {

        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * smoothScaler);

        Vector2 lookDirection = player.LookInputDirection;
        Vector2 moveDirection = player.MoveInputDirection;

        // si on ne bouge pas le stick droit mais qu'on bouge le joueur, alors Caméra Epaule
        if (lookDirection.magnitude < 0.1f && moveDirection.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * smoothScaler);
        }
        else // sinon, on passe en Caméra orbitale
        {
            float x = lookDirection.x;
            float y = lookDirection.y;

            Vector3 rigEuler = new Vector3(0, x * Time.deltaTime * 150, 0);
            Vector3 pivotEuler = new Vector3(y * Time.deltaTime * 150, 0, 0);

            transform.localEulerAngles += rigEuler;
            pivot.localEulerAngles += pivotEuler;
        }
    }
}
