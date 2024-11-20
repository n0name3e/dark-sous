using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputHandler inputHandler;
    private Animator anim;
    private CameraHandler cameraHandler;
    private PlayerMovement playerMovement;

    public bool isInteracting;

    [Header("Flags")]
    public bool isRolling;
    public bool isSprinting;
    public bool isUsingItem;
    public bool isInAir;
    public bool isGrounded;
    public bool isImmune;


    void Start()
    {
        cameraHandler = CameraHandler.Instance;
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        isInteracting = anim.GetBool("isInteracting");
        isRolling = anim.GetBool("isRolling");
        isUsingItem = anim.GetBool("isUsingItem");
        isImmune = anim.GetBool("isImmune");

        float delta = Time.deltaTime;


        inputHandler.TickInput(delta);
        playerMovement.HandleMovement(delta);
        playerMovement.HandleRolling(delta);

        playerMovement.Roll();
        playerMovement.HandleFalling(delta, playerMovement.moveDirection);
    }
    public void HealWithFlask()
    {

    }
    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null)
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }
    private void LateUpdate()
    {
        inputHandler.rollFlag = false;
        inputHandler.sprintFlag = false;
        inputHandler.lightAttackInput = false;
        inputHandler.rightInput = false;
        inputHandler.itemInput = false;
        isSprinting = false;

        if (isInAir)
        {
            playerMovement.airTimer += Time.deltaTime;
        }
    }
}
