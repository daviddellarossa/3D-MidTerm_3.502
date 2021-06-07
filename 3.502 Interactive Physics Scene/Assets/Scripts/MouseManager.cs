using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * 1. If Mouse is clicked
         *      create a ray trace, from the camera position to the mouse position.
         *      If the ray intercepts the ball,
         *          Save the current tranform position of the player's ball
         *          Create a Spring joint anchored in the current transform position, attached to the center of the player's ball
         *          When the player releases the mouse, let the ball go.
         *          When the ball is at its previous position, release the spring joint and destroy the spring joint
         * 2. If Mouse is over the player's ball, change cursor
         * 
         * Implement a state machine.
         * States:
         *  Idle
         *      Wait for the Mouse to be clicked on the ball
         *      When this happens, change state to Charge
         *  Charge
         *      Save the current tranform position of the player's ball
         *      Create a Spring joint anchored in the current transform position, attached to the center of the player's ball
         *      If ESC or Right Mouse Button is clicked, return to Idle
         *      If mouse released, change state to Release
         *  Release
         *      Let the ball go under the Spring joint force
         *      When the ball position is equal to or greater than the initial position, destroy the Spring force and change state to Idle
         * 
         */
    }
}
