using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aerial : StateData
{
    public float speed = 9f;
    public float acceleration = 30f;
    public float deceleration = 0f;

    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    public override void UpdateAbility(CharacterState characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        CharacterController2D _controller = characterStateBase.GetCharacterController2D(animator);
        // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
        float _moveInput = Input.GetAxisRaw("Horizontal");

        if (_moveInput != 0f)
        {
            _controller._velocity.x = Mathf.MoveTowards(_controller._velocity.x, speed * _moveInput, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            _controller._velocity.x = Mathf.MoveTowards(_controller._velocity.x, 0, deceleration * Time.fixedDeltaTime);
        }

        _controller._velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        _controller.transform.Translate(_controller._velocity * Time.fixedDeltaTime);

        bool _wasGrounded = _controller._grounded;
        _controller._grounded = false;

        // Retrieve all colliders we have intersected after velocity has been applied.
        Collider2D[] _hits = Physics2D.OverlapCircleAll(_controller.transform.position, _controller._circleCollider2D.radius, _controller._groundLayer);

        foreach (Collider2D _hit in _hits)
        {
            //bool _setGrounded = false;

            // Ignore our own collider.
            if (_hit == _controller._circleCollider2D)
                continue;

            ColliderDistance2D _colliderDistance = _hit.Distance(_controller._circleCollider2D);

            // Ensure that we are still overlapping this collider.
            // The overlap may no longer exist due to another intersected collider
            // pushing us out of this one.
            if (_colliderDistance.isOverlapped)
            {
                _controller.transform.Translate(_colliderDistance.pointA - _colliderDistance.pointB);

                // If we intersect an object beneath us, set grounded to true. 
                if (Vector2.Angle(_colliderDistance.normal, Vector2.up) < 90)// && _controller._velocity.y < 0)
                {
                    //_setGrounded = true;
                    _controller._grounded = true;
                    Debug.Log("Landed");
                    _controller._velocity.y = 0f;
                    if (!_wasGrounded)
                    {
                        _controller.OnLandEvent.Invoke();
                    }
                    animator.SetBool("Grounded", _controller._grounded);
                }
            }
        }

        // If the input is moving the player right and the player is facing left...
        if (_controller._velocity.x < 0 && _controller._facingRight)
        {
            // ... flip the player.
            _controller.FlipSprite();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (_controller._velocity.x > 0 && !_controller._facingRight)
        {
            // ... flip the player.
            _controller.FlipSprite();
        }
    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }
}
