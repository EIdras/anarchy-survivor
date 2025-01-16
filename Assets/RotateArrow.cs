using UnityEngine;

public class RotateArrow : MonoBehaviour
{
    public Transform player;  // Le joueur, ou l'objet qui regarde dans une direction
    public float angleOffset = 0; // Offset d'angle pour la flèche

    void Update()
    {
        // Calculer la direction de regard du joueur dans le plan X-Z
        Vector3 lookDirection = new Vector3(player.forward.x, 0, player.forward.z); 

        // Calculer l'angle entre la direction de regard du joueur et l'axe X
        float angle = Mathf.Atan2(lookDirection.z, lookDirection.x) * Mathf.Rad2Deg;
        angle += angleOffset;

        // Appliquer la rotation locale de la flèche autour de l'axe Z
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}