using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public Texture2D MousePlayer;

    [SerializeField] private State CurrentState;

    void OnMouseEnter()
    {
        var hotSpot = new Vector2(MousePlayer.width / (float)2, MousePlayer.height / (float)2);
        Cursor.SetCursor(MousePlayer, hotSpot, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseDown()
    {
        Debug.Log("Mouse Button clicked on Player");
        CurrentState.OnMouseDown();
    }

    void OnMouseUp()
    {
        CurrentState.OnMouseUp();
    }


    // Start is called before the first frame update
    void Start()
    {
        SetState(IdleState.Instance);
    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log("Right mouse button down");
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape button down");
            CurrentState.OnKeyPressed(KeyCode.Escape);
        }
    }

    private void SetState(State state)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit();
            CurrentState.ChangeState -= CurrentState_ChangeState;

        }
        CurrentState = state;
        CurrentState.ChangeState += CurrentState_ChangeState;
        CurrentState.OnEnter();
    }

    private void CurrentState_ChangeState(object sender, State e)
    {
        Debug.Log("ChangeState event intercepted");
        SetState(e);
    }

    private abstract class State
    {
        public abstract event EventHandler<State> ChangeState;
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnMouseDown();
        public abstract void OnMouseUp();
        public abstract void OnKeyPressed(KeyCode keyCode);
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

        public override event EventHandler<State> ChangeState;

        public override void OnEnter()
        {
            Debug.Log("Entering Idle State");
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Idle State");
        }

        public override void OnMouseDown()
        {
            Debug.Log("Idle State: Mouse Down");
            ChangeState?.Invoke(this, ChargeState.Instance);
        }

        public override void OnMouseUp()
        {
        }

        public override void OnKeyPressed(KeyCode keyCode)
        {
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

        public override event EventHandler<State> ChangeState;

        public override void OnEnter()
        {
            Debug.Log("Entering Charge State");
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Charge State");
        }
        public override void OnMouseDown()
        {
        }

        public override void OnMouseUp()
        {
            Debug.Log("Charge State: Mouse Up");
            ChangeState?.Invoke(this, ReleaseState.Instance);
        }

        public override void OnKeyPressed(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.Escape:
                {
                    Debug.Log("Charge State: Escape key pressed");
                    ChangeState?.Invoke(this, IdleState.Instance);
                    break;
                }
            }
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

        public override event EventHandler<State> ChangeState;

        public override void OnEnter()
        {
            Debug.Log("Entering Release State");
            ChangeState?.Invoke(this, IdleState.Instance);
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Release State");
        }
        public override void OnMouseDown()
        {
            Debug.Log("Release State: Mouse Down");
        }

        public override void OnMouseUp()
        {
        }

        public override void OnKeyPressed(KeyCode keyCode)
        {
        }

        private ReleaseState()
        {

        }
    }

}
