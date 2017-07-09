using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BombGun : BaseGun 
{
    [SerializeField]
    private Bomb _bullet;

    [SerializeField]
    private Transform _spawnPoint;

    [SerializeField, Range(1f, 10f)]
    private float _forceAddSpeed = 1f;

    private float _bombForce;
    private float BombForce
    {
        get
        {
            return _bombForce;
        }

        set
        {
            _bombForce = Mathf.Clamp01(value);
            if(UIController.Instance) UIController.Instance.SetBompPowerValue(_bombForce);
        }
    }

    private void OnEnable()
    {
        if(UIController.Instance)
        { 
            UIController.Instance.SetActiveBombPowerSlider(true); 
            UIController.Instance.SetActiveAIM(false);
            UIController.Instance.SetActivePointAIM(true);
        }
    }

    private void OnDisable()
    {
        if(UIController.Instance) 
        {
            UIController.Instance.SetActiveBombPowerSlider(false); 
            UIController.Instance.SetActivePointAIM(false);
        }
    }

    protected override void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            BombForce = 0;
        }

        if(Input.GetKey(KeyCode.Mouse0))
        {
            BombForce = _bombForce == 1 ? 0 : _bombForce + _forceAddSpeed * Time.deltaTime;
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            Fire();
        }

        base.LateUpdate();
    }

    protected override void Fire()
    {
        if(_currentBulletQuantity <= 0) return;

        var tempObj = _bullet.GetInstance<Bomb>();

        if(!tempObj) return;

        tempObj.gameObject.SetActive(true);
        tempObj.transform.position = _spawnPoint.position;

        if(tempObj)
        {
            float _fireForce = _force * BombForce;
            tempObj.Initialize(_camera.transform.forward * _fireForce, _damage);
            DecrementBulletsAndShowQuantity();
        }
    }
}
