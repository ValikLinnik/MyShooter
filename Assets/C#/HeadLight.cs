using UnityEngine;
using System.Collections;

public class HeadLight : MonoBehaviour 
{
    [SerializeField]
    private Light _light;

    [SerializeField, Range(1,1000)]
    private float _lifeTime = 60;

    [SerializeField]
    private AnimationCurve _fadeCurve;

    private float _workTime;

    private float _originIntensity;

    private void Start()
    {
        if(_light) _originIntensity = _light.intensity;
    }

    public bool IsOn
    {
        get
        {
            return (!_light ? false : _light.enabled);
        }
        
        set
        {
            if(!_light) return;
            _light.enabled = value;
        }
    }

    private void Update()
    {
        if(!_light) return;
        if(!_light.enabled) return;

        _workTime += Time.deltaTime;
        _workTime = Mathf.Clamp(_workTime, 0, _lifeTime);
        if(_fadeCurve != null) _light.intensity = _originIntensity * _fadeCurve.Evaluate(_workTime / _lifeTime);
    }
}
