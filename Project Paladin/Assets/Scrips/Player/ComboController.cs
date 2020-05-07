using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ComboController : MonoBehaviour
{
    private CharacterController2D _controller;
    private int _noOfClicks;
    private bool _canAttack = true;
    private bool _canClick = true;
    private bool _attacking;

    void Awake()
    {
        _controller = gameObject.GetComponentInParent<CharacterController2D>();
    }

    void Update()
    {
        //attack
        if (Input.GetMouseButtonDown(0))
        {
            if (_controller._grounded)
                ComboStarter();
            else
                AirAttack();
        }
        if (_controller._grounded && _controller._animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Jump_Attack"))
            EndCombo();
    }

    void ComboStarter()
    {
        if (_noOfClicks == 0 && !_canAttack)
        {
            return;
        }
        if (_canClick)
            _noOfClicks++;
        if (_noOfClicks == 1)
        {
            _attacking = true;
            _controller._animator.SetInteger("Combo", 1);
            _canAttack = false;
        }
    }

    public void ComboCheck()
    {
        _canClick = false;

        if (_noOfClicks == 1 && (_controller._animator.GetCurrentAnimatorStateInfo(0).IsName("Player_attack_1")))
        {
            //If the first animation is still playing and only 1 click has happened, return to idle
            EndCombo();
        }
        else if (_noOfClicks >= 2 && (_controller._animator.GetCurrentAnimatorStateInfo(0).IsName("Player_attack_1")))
        {
            //If the first animation is still playing and at least 2 clicks have happened, continue the combo          
            _controller._animator.SetInteger("Combo", 2);
            _canClick = true;
        }
        else if (_noOfClicks == 2 && (_controller._animator.GetCurrentAnimatorStateInfo(0).IsName("Player_attack_2")))
        {
            //If the second animation is still playing and only 2 clicks have happened, return to idle    
            EndCombo();
        }
        else if (_noOfClicks >= 3 && (_controller._animator.GetCurrentAnimatorStateInfo(0).IsName("Player_attack_2")))
        {
            //If the second animation is still playing and at least 3 clicks have happened, continue the combo   
            _controller._animator.SetInteger("Combo", 3);
            _canClick = true;
        }
        else if (_controller._animator.GetCurrentAnimatorStateInfo(0).IsName("Player_attack_3"))
        { //Since this is the third and last animation, return to idle          
            EndCombo();
        }
    }

    void AirAttack()
    {
        if (_noOfClicks == 0 && !_canAttack)
            return;
        if (_canClick)
            _noOfClicks++;
        if (_noOfClicks == 1)
        {
            _attacking = true;
            _controller._animator.SetInteger("Combo", 1);
            _canAttack = false;
        }
    }

    public void EndCombo()
    {
        _controller._animator.SetInteger("Combo", 0);
        StartCoroutine(AttackCooldown());
        _canClick = true;
        _noOfClicks = 0;
        _attacking = false;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        _canAttack = true;
    }
}