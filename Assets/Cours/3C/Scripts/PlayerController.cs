using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    [Header("Options")]
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController character;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PlayerInput playerInput;

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
    
    public void EnableControl(bool enable)
    {
        playerInput.enabled = enable;
    }
    
    public PlayerInput GetPlayerInput()
    {
        return playerInput;
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
        // Si l'input est "X", on met des dégâts au joueur (pour tester)
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            PlayerManager.Instance.TakeDamage(10);
        }
        MovePlayerTopDown();
        LookAt();
    }

    void MovePlayerTopDown()
    {
        Vector3 moveDirection = new Vector3(moveInputDirection.x, 0, moveInputDirection.y).normalized;
        Vector3 velocity = moveDirection * PlayerManager.Instance.moveSpeed;
        
        animator.SetFloat("Speed", velocity.magnitude);
        character.Move(velocity * Time.deltaTime);
    }
    
    void LookAt()
    {
        Vector3 lookDirection = new Vector3(lookInputDirection.x, 0, lookInputDirection.y);
        
        if (lookDirection.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            FireProjectile();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, transform.rotation);
            projectile.GetComponent<Projectile>().speed = 15f;
            animator.SetTrigger("Shoot");
        }
    }
}
