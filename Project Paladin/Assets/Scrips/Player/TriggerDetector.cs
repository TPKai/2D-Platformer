using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    private CharacterController2D owner;
    [SerializeField]
    private LayerMask _targetLayer;

    public List<Collider2D> CollidingParts = new List<Collider2D>();

    private void Awake()
    {
        owner = GetComponentInParent<CharacterController2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (CollidingParts.Contains(col))
        {
            return;
        }

        CharacterController2D attacker = col.transform.root.GetComponent<CharacterController2D>();

        if (attacker == null)
        {
            return;
        }

        if (col.gameObject == attacker.gameObject)
        {
            return;
        }

        if (_targetLayer == (_targetLayer | (1 << col.gameObject.layer)))
        {
            if (!CollidingParts.Contains(col))
            {
                CollidingParts.Add(col);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D attacker)
    {
        if (CollidingParts.Contains(attacker))
        {
            CollidingParts.Remove(attacker);
        }
    }
}
