﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    ATTACKINFO,
    HAMMER_OBJ,
    HAMMER_VFX,
    DAMAGE_WHITE_VFX,
}

public class PoolObjectLoader : MonoBehaviour
{
    public static PoolObject InstantiatePrefab(PoolObjectType objType)
    {
        GameObject obj = null;

        switch (objType)
        {
            case PoolObjectType.ATTACKINFO:
                {
                    obj = Instantiate(Resources.Load("AttackInfo", typeof(GameObject)) as GameObject);
                    break;
                }
        }

        return obj.GetComponent<PoolObject>();
    }
}
