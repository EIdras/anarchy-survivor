using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private static SpawnGrenade SpawnGrenade;



    private void Awake()
    {
        Instance = this;
        //getcomponents SpawnGrenade.cs
        SpawnGrenade = GetComponent<SpawnGrenade>();
    }

    [Header("Options")]
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController character;
    [SerializeField] private PlayerInput playerInput;

    public Vector2 MoveInputDirection => moveInputDirection;
    private Vector2 moveInputDirection;
    public Vector2 LookInputDirection => lookInputDirection;
    private Vector2 lookInputDirection;

    void Start()
    {
        moveInputDirection = Vector2.zero;
        lookInputDirection = Vector2.zero;
    }
    
    public void EnableControl(bool enable)
    {
        playerInput.enabled = enable;
    }
    
    public PlayerInput GetPlayerInput()
    {
        return playerInput;
    }

    public void OnFire()
    {
        SpawnGrenade.ThrowGrenade();
    }
    
    public void OnMove(InputValue move)
    {
        moveInputDirection = move.Get<Vector2>();
    }
    
    public void OnLook(InputValue look)
    {
        lookInputDirection = look.Get<Vector2>();
    }

    public void OnPause()
    {
        Debug.Log("Pause");
        PlayerManager.Instance.TogglePause();
    }

    void Update()
    {
        MovePlayerTopDown();
        LookAt();
    }

    void MovePlayerTopDown()
    {
        Vector3 moveDirection = new Vector3(moveInputDirection.x, 0, moveInputDirection.y).normalized;
        Vector3 velocity = moveDirection * PlayerManager.Instance.moveSpeed;
        
        animator.SetFloat("Speed", velocity.magnitude);
        character.Move(velocity * Time.deltaTime);
        
        // Bloquage de la position Y
        Vector3 position = transform.position;
        position.y = 0;
        transform.position = position;
    }
    
    void LookAt()
    {
        Vector3 lookDirection = new Vector3(lookInputDirection.x, 0, lookInputDirection.y);
        
        if (lookDirection.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        }
    }
    
}
