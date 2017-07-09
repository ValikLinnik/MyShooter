using UnityEngine;
using System.Collections;

public class TimeDisabler : MonoBehaviour 
{
    [SerializeField, Range(.1f, 5f)]
    private float _timeToShow = .5f;

    private float _timeToDisable;

    private void OnEnable()
    {
        _timeToDisable = Time.time + _timeToShow;
    }

    private void Update()
    {
        if(Time.time >= _timeToDisable) gameObject.SetActive(false);   
    }
}
