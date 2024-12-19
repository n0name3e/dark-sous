using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerMovement movement;
    private PlayerStats playerStats;
    public Animator Anim;
    private InputHandler inputHandler;
    private PlayerMovement playerMovement;
    private PlayerItemManager playerItemManager;
    private int vertical;
    private int horizontal;
    public bool canRotate;

    public void Initialize()
    {
        playerManager = GetComponentInParent<PlayerManager>();
        Anim = GetComponent<Animator>();
        inputHandler = GetComponentInParent<InputHandler>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerItemManager = GetComponentInParent<PlayerItemManager>();
        movement = GetComponentInParent<PlayerMovement>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool isRolling = false, bool isUsingItem = false)
    {
        Anim.SetBool("isInteracting", isInteracting);
        Anim.SetBool("isRolling", isRolling);
        Anim.SetBool("isUsingItem", isUsingItem);
        Anim.CrossFade(targetAnim, 0.2f);
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        // clamping
        float v = 0;

        if (verticalMovement > 0f && verticalMovement < 0.55f)
            v = 0.25f;
        else if (verticalMovement >= 0.55f)
            v = 0.5f;
        /*else if (verticalMovement <= 0f && verticalMovement > -0.55f)
            v = -0.5f;
        else if (verticalMovement < -0.55f)
            v = -1f;*/
        #endregion

        #region Horizontal
        float h = 0;

        if (horizontalMovement > 0f && horizontalMovement < 0.55f)
            h = 0.25f;
        else if (horizontalMovement >= 0.55f)
            h = 0.5f;
        /*else if (horizontalMovement <= 0f && horizontalMovement > -0.55f)
            h = -0.5f;
        else if (horizontalMovement < -0.55f)
            h = -1f;*/
        #endregion

        if (isSprinting)
        {
            v = 1f;
            h = horizontalMovement;
        }

        Anim.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
        Anim.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);        
    }

    public void GiveImmunity()
    {
        Anim.SetBool("isImmune", true);
    }
    public void RemoveImmunity()
    {
        Anim.SetBool("isImmune", false);
        playerStats.CheckCollidingWeapon();
    }

    /// <summary>
    /// Change animation to idle to undo dying
    /// </summary>
    public void Revive()
    {
        PlayTargetAnimation("Movement", false, false, false);
    }
    public void UseFlask()
    {
        playerItemManager.HealPlayer();
    }
    public void CanRotate()
    {
        canRotate = true;
    }
    public void StopRotate()
    {
        canRotate = false;
    }
    public void AllowRolling() // while interacting
    {
        movement.canRoll = true;
    }

    /*private void OnAnimatorMove()
     {
         if (inputHandler.isInteracting == false)
             return;

         float delta = Time.deltaTime;

         playerMovement.rigidbody.drag = 0;
         Vector3 deltaPosition = Anim.deltaPosition;
         deltaPosition.y = 0;
         Vector3 velocity = deltaPosition / delta;
         playerMovement.rigidbody.velocity = velocity;
    }*/
}
