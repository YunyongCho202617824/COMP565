using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private float gravityMultiplier;

    [SerializeField]
    private float jumpHorizontalSpeed;

    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        // Initialization of components and variables
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // Input handling
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Movement direction calculation
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        // Speed reduction if Shift key is pressed
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }

        // Animator parameter updates
        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);

        // Adjusting movement direction based on camera rotation
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        // Gravity calculation
        float gravity = Physics.gravity.y * gravityMultiplier;

        // Handling jump-related physics
        if (isJumping && ySpeed > 0 && Input.GetButton("Jump") == false)
        {
            gravity *= 2;
        }

        ySpeed += gravity * Time.deltaTime;

        // Handling grounded state
        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        // Checking for jump button press
        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        // Jumping logic
        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            isJumping = false;
            animator.SetBool("IsFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                animator.SetBool("IsJumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            // Handling non-grounded state
            characterController.stepOffset = 0;
            animator.SetBool("IsGrounded", false);
            isGrounded = false;

            // Triggering falling animation
            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("IsFalling", true);
            }
        }

        // Rotation based on movement direction
        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        // Applying movement if not grounded
        if (isGrounded == false)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);
        }
    }

    // This method is automatically called by Unity after the Animator has updated
    private void OnAnimatorMove()
    {
        // Applying animator's delta position to the CharacterController when grounded
        if (isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
    }

    // This method is called when the application gains or loses focus
    private void OnApplicationFocus(bool focus)
    {
        // Locking or unlocking the cursor based on application focus
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}