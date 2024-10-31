using UnityEngine;

public class FollowPlayerWithoutRotation : MonoBehaviour
{
    public Transform player;
    public Camera mainCamera;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        transform.position = player.position + offset;
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    }
}