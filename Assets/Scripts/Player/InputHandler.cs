using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool rollInput;
    public bool lightAttackInput;
    public bool itemInput;
    public bool interractInput;
    public bool rightInput; // right arrow

    public bool rollFlag;
    public bool sprintFlag;
    public bool fogFlag; // true when in fog wall collider
    public float rollInputTimer;


    private PlayerControls _inputActions;
    private PlayerAttack playerAttack;
    private PlayerInventory playerInventory;
    private PlayerManager playerManager;
    private PlayerMovement playerMovement;
    private PlayerItemManager playerItemManager;

    private Vector2 movementInput;
    private Vector2 cameraInput;


    private void Awake()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerItemManager = GetComponent<PlayerItemManager>();
    }

    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerControls();
            _inputActions.PlayerMovement.Movement.performed += _inputActions => movementInput = _inputActions.ReadValue<Vector2>();
            _inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            _inputActions.PlayerActions.Roll.performed += i => rollInput = i.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            _inputActions.PlayerActions.Useitem.performed += i => itemInput = i.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            _inputActions.PlayerActions.LightAttack.performed += i => lightAttackInput = i.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        }
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        HandleAttackInput(delta);
        HandleQuickSlots();
        HandleItemUsage();
        HandleInterraction();
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
        rollInput = _inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        if (playerManager.isInteracting)
            return;
        if (rollInput)
        {
            rollInputTimer += delta;
            sprintFlag = true;
        }
        else
        {
            if (rollInputTimer > 0 && rollInputTimer < 0.4f)
            {
                sprintFlag = false;
                rollFlag = true;
            }
            rollInputTimer = 0f;
        }
    }
    private void HandleAttackInput(float delta)
    {
        lightAttackInput = _inputActions.PlayerActions.LightAttack.phase == UnityEngine.InputSystem.InputActionPhase.Started;

        if (lightAttackInput && !playerManager.isInteracting)
        {
            playerAttack.HandleLightAttack(playerInventory.rightWeapon);
        }
    }
    private void HandleItemUsage()
    {
        itemInput = _inputActions.PlayerActions.Useitem.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        if (itemInput)
        {
            playerItemManager.UseCurrentConsumable();
        }
    }

    private void HandleQuickSlots()
    {
        rightInput = _inputActions.PlayerItems.Right.phase == UnityEngine.InputSystem.InputActionPhase.Started;
        if (rightInput)
        {
            playerInventory.ChangeRightHandWeapon();
        }
    }
    private void HandleInterraction()
    {
        interractInput = _inputActions.PlayerActions.Interract.phase == UnityEngine.InputSystem.InputActionPhase.Started;
    }
}
