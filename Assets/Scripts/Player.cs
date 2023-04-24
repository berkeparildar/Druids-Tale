using UnityEngine;

public class Player : MonoBehaviour
{
    protected CharacterController CharacterController;
    private Animator _animator;
    protected int Health;
    protected Vector3 JumpForce;
    protected float Speed;
    private bool _isJumping = true;
    protected string CurrentForm;

    private float _lastMouseX;
    protected Cat CatScript;
    protected Player PlayerScript;
    protected GameObject CatModel;
    protected GameObject PlayerModel;

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript = transform.GetComponent<Player>();
        CatScript = transform.GetComponent<Cat>();
        PlayerModel = transform.GetChild(0).gameObject;
        CatModel = transform.GetChild(1).gameObject;
        Speed = 3.0f;
        Health = 100;
        CurrentForm = "human";
        _animator = transform.GetChild(0).GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement(); 
        Morph();
    }

    protected virtual void Movement()
    {
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");
        _animator.SetFloat(Animator.StringToHash("movement"), verticalInput);
        _animator.SetFloat(Animator.StringToHash("horizontalMovement"), horizontalInput);
        var transform1 = transform;
        var direction = ((horizontalInput * Speed * transform1.right) + transform1.forward * (verticalInput * Speed));
        if (Input.GetKeyDown(KeyCode.Space) && CharacterController.isGrounded)
        {
            if (!_isJumping)
            {
                _animator.SetTrigger(Animator.StringToHash("jump"));
                _isJumping = true;
                JumpForce = new Vector3(0, 5, 0);
            }
        }

        if (_isJumping)
        {
            CharacterController.Move(new Vector3(direction.x, JumpForce.y, direction.z) * Time.deltaTime);
            JumpForce += Physics.gravity * Time.deltaTime;
            if (CharacterController.isGrounded)
            {
                _isJumping = false;
            }
        }
        else
        {
            CharacterController.SimpleMove(direction);
        }
    }

    protected virtual void Morph()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CatScript.enabled = true;
            CatModel.SetActive(true);
            PlayerScript.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // bear script
        }
    }
}
