using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float _timeScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = _timeScale;
    }
}
