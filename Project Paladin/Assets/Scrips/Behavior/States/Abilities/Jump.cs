using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "WebbGames/AbilityData/Jump")]
public class Jump : StateData
{
    public float _jumpForce;
    public AnimationCurve _speedGraph;
    [Header("Extra Gravity")]
    //public AnimationCurve Pull;
    public bool _cancelPull;

    private bool _isJumped = false;

    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }

    public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        CharacterController2D _controller = characterState.GetCharacterController2D(animator);

        _controller.transform.Translate(Vector2.up * _jumpForce * _speedGraph.Evaluate(stateInfo.normalizedTime) * Time.deltaTime);
    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }
}
