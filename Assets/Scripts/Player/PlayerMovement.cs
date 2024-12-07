using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform cameraObject;
    private InputHandler inputHandler;
    public Vector3 moveDirection;
    private Vector3 rollDirection;

    public bool canRoll; // can roll while knocked down

    [HideInInspector] public AnimatorHandler animatorHandler;
    private PlayerManager playerManager;
    private PlayerStats playerStats;

    public new Rigidbody rigidbody;

    [Header("Air detection stats")]
    [SerializeField] private float groundDetectionStartPoint = 0.5f;
    [SerializeField] private float minimumDistanceToBeginFall = 1f;
    [SerializeField] private float groundDirectionRayDistance = 0.2f;
    private LayerMask ignoreForGroundCheck;
    public float airTimer;

    [Header("Movement Stats")]
    [SerializeField] private AnimationCurve rollSpeedCurve;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float sprintSpeed = 4f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private float fallingSpeed = 30f;
    [SerializeField] private float rollStaminaCost = 20f;

    private float rollTimer = 0f;
    // movement
    private Vector3 normalVector;
    private Vector3 targetPosition;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        cameraObject = Camera.main.transform;
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Start()
    {
        animatorHandler.Initialize();
        playerManager = GetComponent<PlayerManager>();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }
    private void Update()
    {
        if (!playerManager.isInteracting)
            canRoll = false;
    }

    public void HandleMovement(float delta)
    {
        if ((playerManager.isInteracting && !playerManager.isUsingItem) ||
            inputHandler.rollFlag)
            return;

        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag && playerStats.currentStamina > 1 && !playerManager.isUsingItem)
        {
            speed = sprintSpeed;
            playerManager.isSprinting = true;
        }
        if (playerManager.isUsingItem)
            speed *= 0.6f;
        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        projectedVelocity.y = 0;
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void Roll()
    {
        if (playerManager.isRolling == false)
            return;

        rollDirection = transform.forward;
        rollDirection *= rollSpeedCurve.Evaluate(rollTimer) * rollSpeed;
        //rollDirection *= rollSpeed;

        Quaternion rollRotation = Quaternion.LookRotation(rollDirection);
        transform.rotation = rollRotation;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(rollDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        rollTimer += Time.deltaTime;
    }

    public void HandleRolling(float delta)
    {
        if (playerManager.isInteracting && (playerManager.isInteracting && !canRoll))
            return;
        if (inputHandler.rollFlag && playerStats.currentStamina > 0)
        {
            rollDirection = cameraObject.forward * inputHandler.vertical;
            rollDirection += cameraObject.right * inputHandler.horizontal;
            rollDirection.Normalize();
            rollDirection.y = 0;
            if (rollDirection == Vector3.zero)
                rollDirection = transform.forward;

            Quaternion rollRotation = Quaternion.LookRotation(rollDirection);
            transform.rotation = rollRotation;

            animatorHandler.PlayTargetAnimation("Roll", true, true);
            playerStats.DealStaminaDamage(rollStaminaCost);
            canRoll = false;
            rollTimer = 0f;
        }
    }

    private void HandleRotation(float delta)
    {
        Vector3 targetDir;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        if (targetDir == Vector3.zero)
            targetDir = transform.forward;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * delta);
        transform.rotation = targetRotation;

        // makes player always look forward and not somewhere down
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = 0;
        transform.rotation = Quaternion.Euler(rot);
    }
    public void HandleFalling(float delta, Vector3 moveDirection)
    {
        playerManager.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = transform.position;
        origin.y += groundDetectionStartPoint;

        if (Physics.Raycast(origin, transform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }
        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 5f);
        }
        Vector3 direction = moveDirection;
        direction.Normalize();
        origin = origin + direction * groundDirectionRayDistance;

        targetPosition = transform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceToBeginFall, Color.red, 0.1f, false);

        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if (airTimer > 0.5f)
                {
                    print("Air time: " + airTimer);
                    animatorHandler.PlayTargetAnimation("Land", true, false);
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Empty", false, false);
                    airTimer = 0f;
                }
                playerManager.isInAir = false;
            }
        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }
            if (playerManager.isInAir == false)
            {
                if (playerManager.isInteracting == false)
                {
                    animatorHandler.PlayTargetAnimation("Falling", true, false);
                }
                Vector3 velocity = rigidbody.velocity;
                velocity.Normalize();
                rigidbody.velocity = velocity * (movementSpeed / 2);
                playerManager.isInAir = true;
            }
        }
        if (playerManager.isGrounded)
        {
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, delta);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }
}
