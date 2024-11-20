using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionPlanner : MonoBehaviour
{
    private InputHandler inputHandler;
    private PlayerManager playerManager;
    [SerializeField] private float actionMaxDelay = 0.2f;
    public float timer = 0.2f;
    public float inputTimer = 0f;
    private bool rollFlag;
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerManager = GetComponent<PlayerManager>();
    }
    void Update()
    {
        //print(inputTimer);
        timer -= Time.deltaTime;
        if (inputHandler.rollInput)
        {
            if (playerManager.isInteracting)
            {
                rollFlag = true;
                timer = actionMaxDelay;
            }
            inputTimer += Time.deltaTime;
        }
        else
            inputTimer = 0f;
        if (rollFlag && timer >= 0)
        {
            //print("flag");
            if (inputTimer <= 0.2f)
            {
                //print("roll;");
                inputHandler.rollFlag = true;
            }
            else
            {
                //print("sprint");
                inputHandler.rollFlag = false;
                inputHandler.sprintFlag = true;
            }
        }
        if (timer <= 0)
        {
            rollFlag = false;            
        }
    }
}
