using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Options")]
    [SerializeField] private Animator animator;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private CharacterController character;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private Coroutine currentFireCoroutine;

    private bool sprinting = false;
    public Vector2 MoveInputDirection => moveInputDirection;
    private Vector2 moveInputDirection;
    public Vector2 LookInputDirection => lookInputDirection;
    private Vector2 lookInputDirection;

    void Start()
    {
        moveInputDirection = Vector2.zero;
        lookInputDirection = Vector2.zero;
        
       StartCoroutine(FireContinuously());
    }

    // appelé par PlayerInput
    public void OnMove(InputValue move)
    {
        moveInputDirection = move.Get<Vector2>();
    }

    // appelé par PlayerInput
    public void OnLook(InputValue look)
    {
        lookInputDirection = look.Get<Vector2>();
    }

    void Update()
    {
        MovePlayerTopDown();
        LookAt();
    }

    void MovePlayerTopDown()
    {
        // Move player in the direction of the input, not the direction where the player is looking
        Vector3 moveDirection = new Vector3(moveInputDirection.x, 0, moveInputDirection.y);
        moveDirection.Normalize(); // Normalize to prevent faster diagonal movement
        Vector3 velocity = moveDirection * speed;
        
        animator.SetFloat("Speed", velocity.magnitude);
        
        character.Move(velocity * Time.deltaTime);
    }
    
    void LookAt()
    {
        // Utiliser uniquement lookInputDirection pour orienter la vue
        Vector3 lookDirection = new Vector3(lookInputDirection.x, 0, lookInputDirection.y);
        
        if (lookDirection.magnitude > 0.1f) // Ne modifier la rotation que si l'input est significatif
        {
            // Définir uniquement la rotation du joueur
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            FireProjectile();
            yield return new WaitForSeconds(0.2f); // Intervalle entre chaque projectile
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
            projectile.GetComponent<Projectile>().speed = 15f; // Définir la vitesse du projectile
            animator.SetTrigger("Shoot");
        }
    }
}
