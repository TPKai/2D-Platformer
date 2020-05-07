using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    public float _speed = 9f;
    [SerializeField, Tooltip("Acceleration while grounded.")]
    public float _walkAcceleration = 75f;
    [SerializeField, Tooltip("Acceleration while in the air.")]
    public float _airAcceleration = 30f;
    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    public float _groundDeceleration = 70f;
    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    public float _jumpHeight = 4f;
    [SerializeField]
    private Transform _groundCheck;
    private float _groundedRadius = 0.2f;
    [SerializeField]
    public LayerMask _groundLayer;

    [HideInInspector]
    public CircleCollider2D _circleCollider2D;
    [HideInInspector]
    public bool _facingRight = true;

    [HideInInspector]
    public Rigidbody2D _rb2d;

    [HideInInspector]
    public Vector2 _velocity;
    [HideInInspector]
    public Animator _animator;
    [HideInInspector]
    public bool _grounded = true;

    [HideInInspector]
    public float _moveInput = 0f;

    [HideInInspector]
    public bool attack = false;

    [HideInInspector]
    public bool jump = false;

    public int health = 10;

    //public List<Collider2D> CollidingParts = new List<Collider2D>();
    private List<TriggerDetector> TriggerDetectors = new List<TriggerDetector>();

    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;
    public UnityEvent OnJumpEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }


    private void Awake()
    {
        _circleCollider2D = GetComponentInChildren<CircleCollider2D>();

        _animator = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _animator.SetBool("Grounded", true);

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (OnJumpEvent == null)
            OnJumpEvent = new UnityEvent();
    }

    //private void Update()
    //{
    //// Use GetAxisRaw to ensure our input is either 0, 1 or -1.
    //float _moveInput = Input.GetAxisRaw("Horizontal");

    //if (_grounded)
    //{
    //    _velocity.y = 0f;

    //    if (Input.GetButtonDown("Jump"))
    //    {
    //        // Calculate the velocity required to achieve the target jump height.
    //        _velocity.y = Mathf.Sqrt(2 * _jumpHeight * Mathf.Abs(Physics2D.gravity.y));
    //        OnJumpEvent.Invoke();
    //    }
    //}

    //float acceleration = _grounded ? _walkAcceleration : _airAcceleration;
    //float deceleration = _grounded ? _groundDeceleration : 0;

    //if (_moveInput != 0f)
    //{
    //    _velocity.x = Mathf.MoveTowards(_velocity.x, _speed * _moveInput, acceleration * Time.fixedDeltaTime);
    //}
    //else
    //{
    //    _velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.fixedDeltaTime);
    //}

    //_velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

    //transform.Translate(_velocity * Time.fixedDeltaTime);

    //bool _wasGrounded = _grounded;
    //_grounded = false;

    ////// Retrieve all colliders we have intersected after velocity has been applied.
    ////Collider2D[] _hits = Physics2D.OverlapCircleAll(transform.position,_circleCollider2D.radius,_groundLayer);

    ////foreach (Collider2D _hit in _hits)
    ////{
    ////    //bool _setGrounded = false;

    ////    // Ignore our own collider.
    ////    if (_hit == _circleCollider2D)
    ////        continue;

    ////    ColliderDistance2D _colliderDistance = _hit.Distance(_circleCollider2D);

    ////    // Ensure that we are still overlapping this collider.
    ////    // The overlap may no longer exist due to another intersected collider
    ////    // pushing us out of this one.
    ////    if (_colliderDistance.isOverlapped)
    ////    {
    ////        transform.Translate(_colliderDistance.pointA - _colliderDistance.pointB);

    ////        // If we intersect an object beneath us, set grounded to true. 
    ////        if (Vector2.Angle(_colliderDistance.normal, Vector2.up) < 90 && _velocity.y < 0)
    ////        {
    ////            //_setGrounded = true;
    ////            _grounded = true;
    ////            if (!_wasGrounded)
    ////            {
    ////                OnLandEvent.Invoke();
    ////            }
    ////        }
    ////    }
    ////    //_grounded = _setGrounded;
    ////}

    //Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _groundLayer);
    //for (int i = 0; i < colliders.Length; i++)
    //{
    //    if (colliders[i].gameObject != gameObject)
    //    {
    //        _grounded = true;
    //        if (!_wasGrounded)
    //        {
    //            OnLandEvent.Invoke();
    //            _velocity.y = 0;
    //        }
    //        _wasGrounded = false;
    //    }
    //}
    //_animator.SetFloat("Speed X", Mathf.Abs(_velocity.x));
    //_animator.SetFloat("Speed Y", _velocity.y);
    //_animator.SetBool("Grounded", _grounded);

    //FlipCheck();

    //_animator.SetFloat("Speed X", Mathf.Abs(_velocity.x));
    //_animator.SetFloat("Speed Y", _velocity.y);
    //_animator.SetBool("Grounded", _grounded);
    //}

    private void Update()
    {
        bool wasGrounded = _grounded;
        _grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, _groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                    //_velocity.y = 0;
                }
                wasGrounded = false;
            }
        }
        _animator.SetFloat("Speed X", Mathf.Abs(_velocity.x));
        _animator.SetFloat("Speed Y", _velocity.y);
        _animator.SetBool("Grounded", _grounded);
    }

    public void FlipSprite()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void FlipCheck()
    {
        // If the input is moving the player right and the player is facing left...
        if (_velocity.x < 0 && _facingRight)
        {
            // ... flip the player.
            FlipSprite();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (_velocity.x > 0 && !_facingRight)
        {
            // ... flip the player.
            FlipSprite();
        }
    }

    public List<TriggerDetector> GetAllTriggers()
    {
        if (TriggerDetectors.Count == 0)
        {
            TriggerDetector[] arr = this.GetComponentsInChildren<TriggerDetector>();

            foreach(TriggerDetector d in arr)
            {
                TriggerDetectors.Add(d);
            }
        }
        return TriggerDetectors;
    }
}
