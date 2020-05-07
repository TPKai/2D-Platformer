using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "WebbGames/AbilityData/MoveForward")]
public class MoveForward : StateData
{
    public bool _debug;

    public AnimationCurve _speedGraph;
    public float _speed;

    public bool _controlledMove;

    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }
    
    public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (_debug)
        {
            Debug.Log(stateInfo.normalizedTime);
        }

        ConstantMove(characterState.GetCharacterController2D(animator), animator,stateInfo);
    }

    private void ConstantMove(CharacterController2D controller, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (controller._facingRight)
        {
            controller.transform.Translate(Vector2.right * _speed * _speedGraph.Evaluate(stateInfo.normalizedTime) * Time.deltaTime);
        }
        else
        {
            controller.transform.Translate(Vector2.left * _speed * _speedGraph.Evaluate(stateInfo.normalizedTime) * Time.deltaTime);
        }
    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {

    }
}
