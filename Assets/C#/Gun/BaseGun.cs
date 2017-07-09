using UnityEngine;
using System.Collections;
using System.Reflection.Emit;

public abstract class BaseGun : MonoBehaviour 
{
    [SerializeField]
    protected Camera _camera;

    [SerializeField, Range(0,100)]
    protected float _force;

    [SerializeField, Range(1,100)]
    protected float _damage = 20;

    [SerializeField]
    protected int _totalBulletsQuanity;

    [SerializeField]
    private int _bulletsCage;

    [SerializeField]
    private string _name;

    protected int _currentBulletQuantity;

    public string Name 
    {
        get
        {
            return _name;
        }
    }

    public int CurrentBulletQuantity
    {
        get
        {
            return _currentBulletQuantity;
        }
    }

    public int CurrentBulletsCage
    {
        get
        {
            return  _totalBulletsQuanity/ _bulletsCage;  
        }
    }

    public int TotalBulletsQuantity
    {
        get
        {
            return _totalBulletsQuanity;
        }

        set
        {
            _totalBulletsQuanity = value;
        }
    }

    protected abstract void Fire();

    protected void DecrementBulletsAndShowQuantity()
    {
        _currentBulletQuantity = _currentBulletQuantity <= 0 ? 0 : --_currentBulletQuantity;
        if(UIController.Instance) UIController.Instance.SetBulletsQuantity(_currentBulletQuantity, CurrentBulletsCage);
    }

    private void Awake()
    {
        RechargeBulletsQuantity();
    }

    protected virtual void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RechargeBulletsQuantity();
        }
    }

    private void RechargeBulletsQuantity()
    {
        _currentBulletQuantity = (_totalBulletsQuanity - _bulletsCage) < 0 ? _totalBulletsQuanity : _bulletsCage;
        _totalBulletsQuanity -= _currentBulletQuantity;
        if(UIController.Instance) UIController.Instance.SetBulletsQuantity(_currentBulletQuantity, CurrentBulletsCage);
    }
}
