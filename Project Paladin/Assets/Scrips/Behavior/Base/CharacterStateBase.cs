using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateBase : StateMachineBehaviour
{
    private CharacterController2D _characterController2D;
    public CharacterController2D GetCharacterController2D(Animator animator)
    {
        if (_characterController2D == null)
        {
            _characterController2D = animator.GetComponent<CharacterController2D>();
        }
        return _characterController2D;
    }
}