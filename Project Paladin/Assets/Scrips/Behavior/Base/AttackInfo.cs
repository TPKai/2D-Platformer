using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfo : MonoBehaviour
{
    public CharacterController2D _attacker = null;
    public Attack _attackAbility;
    public int _damage;
    public List<string> ColliderNames = new List<string>();
    public bool _mustCollide;
    public bool _mustFaceAttacker;
    public float _lethalRange;
    public int _maxHits;
    public int _currentHits;
    public bool _isRegistered;
    public bool _isFinished;

    public void ResetInfo(Attack attack, CharacterController2D attacker)
    {
        _isRegistered = false;
        _isFinished = false;
        _attackAbility = attack;
        _attacker = attacker;
    }

    public void Register(Attack attack)
    {
        _isRegistered = true;

        _attackAbility = attack;
        _damage = attack._damage;
        ColliderNames = attack.ColliderNames;
        _mustCollide = attack._mustCollide;
        _mustFaceAttacker = attack._mustFaceAttacker;
        _lethalRange = attack._lethalRange;
        _maxHits = attack._maxHits;
        _currentHits = 0;
    }

    private void OnDisable()
    {
        _isFinished = true;
    }
}
