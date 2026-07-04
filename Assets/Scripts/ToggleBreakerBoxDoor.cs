using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleBreakerBoxDoor : MonoBehaviour
{

    public bool inToggleZone = false;                                   // tells the game if you're in range to interact with the breaker bx
    public bool breakerBoxIsOpen = false;                               // determines what actions can be taken and what prompts are given
    public bool powerOutage = false;                                    // most action here is only taken during a power outtage
    
    [Header("Animations")]
    [SerializeField] private Animator breakerDoor;                      // controller for animations
    [SerializeField] private string openingDoor = "openingBreakerBox";  // the name of the opening door animation
    [SerializeField] private string closingDoor = "closingBreakerBox";  // the name of the closing door animation
   

    [Header("UI")]
    [SerializeField] private GameObject openBreakerBoxMessage = null;   // prompts user to open the breaker box door
    // there is no close message because it interferes with the turn power on message (and is kind of unnecessary)
    [SerializeField] private GameObject turnPowerOnMessage = null;      // bigger text on the wall, tells user where to go in a power outtage
    [SerializeField] private GameObject flipSwitchMessage = null;       // prompts user to flip breaker box switch (turning on the power)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Invisible"))
        {
            inToggleZone = true;
            Debug.Log($"Player near breaker box");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Invisible"))
        {
            inToggleZone = false;
            Debug.Log($"Player left breaker box");
        }
    }

    private void Update()
    {
            if (!powerOutage) return;

            if (inToggleZone){
                // add code here for showing the breaker box message -> maybe change the direction message, could be kinda cool even idk like the walls are talking to you
                // Debug.Log("breaker box toggle zone");
                AnimatorStateInfo state = breakerDoor.GetCurrentAnimatorStateInfo(0); // allows you to open or close from set state
                float t = state.normalizedTime % 1f; // calculates where to start open/closing animation

                
                if (breakerBoxIsOpen){
                    openBreakerBoxMessage.SetActive(false);
                    flipSwitchMessage.SetActive(true);
                    // pressing F closes the breaker box door
                    if (Input.GetKeyDown(KeyCode.F)) {
                        breakerDoor.Play(closingDoor, 0, 1f - t);
                        breakerBoxIsOpen = false;
                    }
                    // pressing E turns the power back on
                    if (Input.GetKeyDown(KeyCode.E)) {
                        PowerOn(); // currently does nothing but should handle the act of turning the power back on
                    }
                } else {
                    flipSwitchMessage.SetActive(false);
                    openBreakerBoxMessage.SetActive(true);
                    // pressing F opens the breaker box door
                    if (Input.GetKeyDown(KeyCode.F)) {
                        breakerDoor.Play(openingDoor, 0, 1f - t);
                        breakerBoxIsOpen = true;
                    }
                }
            }
    }

    private void PowerOn(){
        // this will trigger when the switch is flipped
        // changes the lighting in the scene
    }
    
}
