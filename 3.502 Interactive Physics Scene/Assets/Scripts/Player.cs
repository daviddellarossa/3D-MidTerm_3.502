using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    void Start()
    {
        SetState(new IdleState(this));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape button down");
            CurrentState.OnKeyPressed(KeyCode.Escape);
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Return button down");
            CurrentState.OnKeyPressed(KeyCode.Return);
        }

        CurrentState.OnMouseMove(Input.mousePosition);
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
        public virtual void OnEnter(){}
        public virtual void OnExit(){}
        public virtual void OnMouseDown(){}
        public virtual void OnMouseUp(){}
        public virtual void OnMouseMove(Vector3 vector){}
        public virtual void OnKeyPressed(KeyCode keyCode){}
    }

    private class IdleState : State
    {
        public MonoBehaviour Parent { get; }
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
            ChangeState?.Invoke(this, new ChargeState(Parent));
        }
        public IdleState(MonoBehaviour parent)
        {
            Parent = parent ?? throw new ArgumentNullException("parent");

        }
        public override void OnKeyPressed(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.Return:
                    var player = GameObject.FindGameObjectWithTag("Player");
                    var rigidbody = player.GetComponent<Rigidbody>();
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                    player.transform.position = new Vector3(-4f, 0.25f, 0f);
                    
                    break;
            }
        }
    }

    private class ChargeState : State
    {
        public MonoBehaviour Parent { get; }
        private GameObject _gameObject;
        private SpringJoint _springJoint;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Vector3 _currentPosition;
        private GameObject _forceIndicator;

        public override event EventHandler<State> ChangeState;
        
        public override void OnEnter()
        {
            Debug.Log("Entering Charge State");
            _initialPosition = Parent.transform.position;
            _initialRotation = Parent.transform.rotation;

            _gameObject = GameObject.FindGameObjectWithTag("Player");

            _forceIndicator = GameObject.FindGameObjectWithTag("LineRenderer");
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Charge State");
        }

        public override void OnMouseUp()
        {
            var impulseVector = (_initialPosition - _currentPosition);
            var impulseMagnitude = Mathf.Clamp(impulseVector.magnitude * 10, 0, 50);
            var impulse = impulseVector.normalized * impulseMagnitude;

            Debug.Log($"Init: {_initialPosition}; Curr: {_currentPosition}; Force: {impulse}");


            _gameObject.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.Impulse);

            _forceIndicator.transform.localScale = Vector3.zero;

            //Debug.Log("Charge State: Mouse Up");
            ChangeState?.Invoke(this, new ReleaseState(Parent));
        }

        public override void OnMouseMove(Vector3 vector)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 8; //Layer 8 contains the Plane
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 20, layerMask);

            _currentPosition = new Vector3(hit.point.x, 0.25f, hit.point.z);

            var forceDirection = _initialPosition - _currentPosition;
            var angle = Mathf.Atan2(forceDirection.z, forceDirection.x) * 180 / Mathf.PI;
            var quaternion = Quaternion.Euler(Vector3.down * (angle - 90));

            Debug.Log(forceDirection);
            _forceIndicator.transform.position = _initialPosition;
            _forceIndicator.transform.rotation = quaternion;
            _forceIndicator.transform.localScale = new Vector3(1, 1, forceDirection.magnitude);
        }

        public override void OnKeyPressed(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.Escape:
                {
                    Debug.Log("Charge State: Escape key pressed");
                    _forceIndicator.transform.localScale = Vector3.zero;

                    ChangeState?.Invoke(this, new IdleState(Parent));
                    break;
                }
            }
        }

        public ChargeState(MonoBehaviour parent)
        {
            Parent = parent ?? throw new ArgumentNullException("parent");
        }
    }

    private class ReleaseState : State
    {
        public MonoBehaviour Parent { get; }

        public override event EventHandler<State> ChangeState;

        public override void OnEnter()
        {
            Debug.Log("Entering Release State");
            ChangeState?.Invoke(this, new IdleState(Parent));
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Release State");
        }
        public override void OnMouseDown()
        {
            Debug.Log("Release State: Mouse Down");
        }

        public ReleaseState(MonoBehaviour parent)
        {
            Parent = parent ?? throw new ArgumentNullException("parent");
        }
    }
}
