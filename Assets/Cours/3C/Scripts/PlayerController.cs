using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private Vector3 lerpedDirection = Vector3.zero;
    private const float lerpDirectionScaler = 10.0f;

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

    // appelé par PlayerInput
    public void OnFire()
    {
        if (currentFireCoroutine != null)
        {
            StopCoroutine(currentFireCoroutine);
        }

        Debug.Log("Firing");
        currentFireCoroutine = StartCoroutine(FireCoroutine());
    }

    // appelé par PlayerInput
    public void OnSprint(InputValue sprint)
    {
        float sprintValue = sprint.Get<float>();
        sprinting = sprintValue > 0.1f;
    }

    void Update()
    {
        // désormais géré par l'InputSystem
        //HandleInputs();


        //MovePlayerTopDown();
        MovePlayerThirdPerson();
    }

    void MovePlayerThirdPerson()
    {
        // HorizontalController et VerticalController vous donneront la valeur du joystick gauche
        // Horizontal et Vertical vous donneront les valeures de ZQSD
        float x = moveInputDirection.x;
        float y = moveInputDirection.y;

        Vector3 rawDirection = transform.forward * Mathf.Clamp01(y);

        rawDirection = (sprinting) ? rawDirection : rawDirection * 0.5f;

        lerpedDirection = Vector3.Lerp(lerpedDirection, rawDirection, Time.deltaTime * lerpDirectionScaler);

        character.Move(rawDirection  * speed * Time.deltaTime);

        float moveSpeed = Mathf.Clamp01(rawDirection.magnitude); // Between 0 & 1

        animator.SetFloat("Speed", moveSpeed);
        
        // plus le joueur va vite, moins il tourne
        float speedRotationScaler = (1.5f - speed);
        
        transform.localEulerAngles += new Vector3(0, -x * Time.deltaTime * 90 * speedRotationScaler, 0);
    }

    void MovePlayerTopDown()
    {
        // HorizontalController et VerticalController vous donneront la valeur du joystick gauche
        // Horizontal et Vertical vous donneront les valeures de ZQSD
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 rawDirection = new Vector3(x, 0, y);

        rawDirection = (sprinting) ? rawDirection : rawDirection * 0.5f;

        //TODO_Exercice_1: adoucir les chagements de vitesse

        lerpedDirection = Vector3.Lerp(lerpedDirection, rawDirection, Time.deltaTime * lerpDirectionScaler); 

        character.Move(rawDirection * speed * Time.deltaTime);

        float moveSpeed = Mathf.Clamp01(rawDirection.magnitude); // Between 0 & 1

        animator.SetFloat("Speed", moveSpeed);

        //Look forward while walking
        Vector3 lookAtOffset = rawDirection;
        lookAtOffset.y = 0;
        transform.LookAt(transform.position + lookAtOffset);
    }

    /*
    void HandleInputs()
    {
        // Sprint by holding A / CTRL
        sprinting = Input.GetButton("Fire1");

        // Firing by pressing B / ALT
        if (Input.GetButtonDown("Fire2"))
        {
            if (currentFireCoroutine != null)
            {
                StopCoroutine(currentFireCoroutine);
            }

            Debug.Log("Fire 2");
            //TODO_Exercice_1: complete the Coroutine
            currentFireCoroutine = StartCoroutine(FireCoroutine());
        }
    }
    */

    IEnumerator FireCoroutine()
    {
        animator.SetTrigger("Shoot");

        yield return new WaitForSeconds(0.2f);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, float.PositiveInfinity) && 
            hit.transform.gameObject.TryGetComponent<ExplodingBarrel>(out ExplodingBarrel barrel))
        {
            barrel.HitByPlayer();
        }
        
        yield return null;

    }
}
