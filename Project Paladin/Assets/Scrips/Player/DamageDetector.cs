using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    CharacterController2D _controller;

    public int damageTaken;

    private void Awake()
    {
        damageTaken = 0;
        _controller = GetComponent<CharacterController2D>();

    }

    private void Update()
    {
        if (AttackManager.Instance.CurrentAttacks.Count > 0)
        {
            CheckAttack();
        }
    }

    private bool AttackIsValid(AttackInfo info)
    {
        if (info == null)
            return false;

        if (info._isRegistered)
            return false;

        if (info._isFinished)
            return false;

        if (info._currentHits >= info._maxHits)
            return false;

        if (info._attacker == _controller)
            return false;

        return true;
    }

    private void CheckAttack()
    {
        foreach(AttackInfo info in AttackManager.Instance.CurrentAttacks)
        {
            if (AttackIsValid(info))
            {
                if (info._mustCollide)
                {
                    if (IsCollided(info))
                    {
                        TakeDamage(info);
                    }
                }
            }
        }
    }

    private bool IsCollided(AttackInfo info)
    {
        //list of attackers triggers
        List<TriggerDetector> list = info._attacker.GetAllTriggers();

        if (list.Count > 0)
        {
            foreach (TriggerDetector trigger in list)
            {
                foreach (Collider2D col in trigger.CollidingParts)
                {
                    foreach (string _name in info.ColliderNames)
                    {
                        if (col.transform.root.gameObject == this.gameObject)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private void TakeDamage(AttackInfo info)
    {
        info._currentHits++;

        damageTaken += info._damage;

        if (damageTaken >= _controller.health)
        {
            Death();
        }
    }

    private void Death()
    {
        //Debug.Log(info._attacker.gameObject.name + " hits: " + this.gameObject.name);
        _controller._animator.SetTrigger("Death");

        _controller.GetComponentInChildren<BoxCollider2D>().enabled = false;
        _controller.GetComponentInChildren<CircleCollider2D>().enabled = false;
        _controller.GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
