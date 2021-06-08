using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MouseManager : MonoBehaviour
{
    //Mouse icon from https://freeiconshop.com/icon/cursor-icon-outline/

    [SerializeField] private State CurrentState;

    // Start is called before the first frame update
    void Start()
    {
        SetState(IdleState.Instance);
    }

    private void SetState(State state)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = state;
        CurrentState.OnEnter();
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

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left Mouse Button clicked");
        }
    }

    private abstract class State
    {
        public abstract void OnEnter();
        public abstract void OnExit();
    }

    private class IdleState : State
    {
        private static IdleState instance;

        public static IdleState Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IdleState();
                }

                return instance;
            }
        }

        public override void OnEnter()
        {
            Debug.Log("Entering Idle State");
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Idle State");
        }

        private IdleState()
        {
            
        }
    }

    private class ChargeState : State
    {
        private static ChargeState instance;
        public static ChargeState Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChargeState();
                }

                return instance;
            }
        }

        public override void OnEnter()
        {
            Debug.Log("Entering Charge State");
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Charge State");
        }
        private ChargeState()
        {
            
        }
    }

    private class ReleaseState : State
    {
        private static ReleaseState instance;
        public static ReleaseState Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReleaseState();
                }

                return instance;
            }
        }

        public override void OnEnter()
        {
            Debug.Log("Entering Release State");
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Release State");
        }

        private ReleaseState()
        {
            
        }
    }
}
