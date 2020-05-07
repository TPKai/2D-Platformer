using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "WebbGames/AbilityData/Attack")]
public class Attack : StateData
{
    public float _startAttackTime;
    public float _endAttackTime;
    public List<string> ColliderNames = new List<string>();
    public bool _mustCollide;
    public bool _mustFaceAttacker;
    public float _lethalRange;
    public int _maxHits;
    public int _damage;

    private List<AttackInfo> FinishedAttacks = new List<AttackInfo>();

    public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        characterState.GetCharacterController2D(animator).attack = false;
        animator.SetBool("Attack", false);

        //GameObject obj = Instantiate(Resources.Load("AttackInfo", typeof(GameObject))) as GameObject;
        GameObject obj = PoolManager.Instance.GetObject(PoolObjectType.ATTACKINFO);
        AttackInfo info = obj.GetComponent<AttackInfo>();

        //obj.SetActive(true);

        info.ResetInfo(this, characterState.GetCharacterController2D(animator));

        if (!AttackManager.Instance.CurrentAttacks.Contains(info))
        {
            AttackManager.Instance.CurrentAttacks.Add(info);
        }
    }

    public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        RegisterAttack(characterState, animator, stateInfo);
        DeregiserAttack(characterState, animator, stateInfo);
        CheckCombo(characterState, animator, stateInfo);
    }

    public void RegisterAttack(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (_startAttackTime <= stateInfo.normalizedTime && _endAttackTime > stateInfo.normalizedTime)
        {
            foreach(AttackInfo info in AttackManager.Instance.CurrentAttacks)
            {
                if (info == null)
                {
                    continue;
                }

                if (!info._isRegistered && info._attackAbility == this)
                {
                    info.Register(this);
                }
            }
        }
    }

    public void DeregiserAttack(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        if (stateInfo.normalizedTime >= _endAttackTime)
        {
            foreach(AttackInfo info in AttackManager.Instance.CurrentAttacks)
            {
                if (info == null)
                {
                    continue;
                }

                if (info._attackAbility == this && !info._isFinished)
                {
                    info._isFinished = true;
                    //Destroy(info.gameObject);
                    info.GetComponent<PoolObject>().TurnOff();
                }
            }
        }
    }

    public void CheckCombo(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        if(stateInfo.normalizedTime >= _startAttackTime + ((_endAttackTime - _startAttackTime)/3f))
        {
            if(stateInfo.normalizedTime < _endAttackTime + ((_endAttackTime - _startAttackTime) / 3f))
            {
                CharacterController2D _controller = characterState.GetCharacterController2D(animator);
                if (_controller.attack)
                {
                    //Debug.Log("Combo");
                    animator.SetBool("Attack", true);
                }
            }
        }
    }

    public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
    {
        ClearAttack();
    }

    public void ClearAttack()
    {
        FinishedAttacks.Clear();

        foreach(AttackInfo info in AttackManager.Instance.CurrentAttacks)
        {
            if (info == null || info._attackAbility == this /*info._isFinished*/)
            {
                FinishedAttacks.Add(info);
            }
        }

        foreach(AttackInfo info in FinishedAttacks)
        {
            if (AttackManager.Instance.CurrentAttacks.Contains(info))
            {
                AttackManager.Instance.CurrentAttacks.Remove(info);
            }
        }
    }
}
