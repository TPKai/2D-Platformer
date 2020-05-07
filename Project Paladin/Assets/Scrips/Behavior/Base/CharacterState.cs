using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : CharacterStateBase
{
    public List<StateData> listAbilityData = new List<StateData>();

    public void UpdateAll(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        foreach (StateData d in listAbilityData)
        {

            d.UpdateAbility(characterState, animator, stateInfo);
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach(StateData d in listAbilityData)
        {
            d.OnEnter(this, animator, stateInfo);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateAll(this, animator, stateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (StateData d in listAbilityData)
        {
            d.OnExit(this, animator, stateInfo);
        }
    }
}
