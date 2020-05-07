using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : CharacterStateBase
{
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterController2D _controller = GetCharacterController2D(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterController2D _controller = GetCharacterController2D(animator);
        // Use GetAxisRaw to ensure our input is either 0, 1 or -1.

        _controller._velocity.y = 0f;

        if (_controller.jump)
        {
            // Calculate the velocity required to achieve the target jump height.
            _controller._velocity.y = Mathf.Sqrt(2 * _controller._jumpHeight * Mathf.Abs(Physics2D.gravity.y));
            _controller.OnJumpEvent.Invoke();
        }

        float acceleration = _controller._walkAcceleration;
        float deceleration = _controller._groundDeceleration;

        if (_controller._moveInput != 0f)
        {
            _controller._velocity.x = Mathf.MoveTowards(_controller._velocity.x, _controller._speed * _controller._moveInput, acceleration * Time.deltaTime);
        }
        else
        {
            _controller._velocity.x = Mathf.MoveTowards(_controller._velocity.x, 0, deceleration * Time.deltaTime);
        }

        _controller._velocity.y += Physics2D.gravity.y * Time.deltaTime;

        _controller.transform.Translate(_controller._velocity * Time.deltaTime);

        if (_controller.attack)
        {
            animator.SetBool("Attack", true);
        }

        //flip sprite
        _controller.FlipCheck();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
