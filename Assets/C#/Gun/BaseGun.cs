using UnityEngine;

public abstract class BaseGun : MonoBehaviour 
{
    #region SERIALIZE FIELDS

    [SerializeField, Range(0, 100)]
    protected float _force;

    [SerializeField, Range(1, 100)]
    protected float _damage = 20;

    [SerializeField]
    protected int _totalBulletsQuanity;

    [SerializeField]
    private int _bulletsCage;

    [SerializeField]
    private AmmoType _name;

    #endregion

    #region PRIVATE FIELDS

    protected int _currentBulletQuantity;

    #endregion

    #region PUBLIC PROPERTIES

    public Camera Camera
    {
        get;
        set;
    }

    public AmmoType Name
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
            return  _totalBulletsQuanity / _bulletsCage;  
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

    #endregion

    #region PROTECTED METHODS

    protected abstract void Fire();

    protected void DecrementBulletsAndShowQuantity()
    {
        _currentBulletQuantity = _currentBulletQuantity <= 0 ? 0 : --_currentBulletQuantity;
        //todo:remove from gun obj
        if (UIController.Instance) UIController.Instance.SetBulletsQuantity(_currentBulletQuantity, CurrentBulletsCage);
    }

    #endregion

    #region UNITY EVENTS

    private void Awake()
    {
        RechargeBulletsQuantity();
    }

    protected virtual void OnEnable()
    {
        if(UIController.Instance) UIController.Instance.SetActiveAIM(true);
    }

    protected virtual void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RechargeBulletsQuantity();
        }
    }

    #endregion

    #region PRIVATE METHODS

    private void RechargeBulletsQuantity()
    {
        _currentBulletQuantity = (_totalBulletsQuanity - _bulletsCage) < 0 ? _totalBulletsQuanity : _bulletsCage;
        _totalBulletsQuanity -= _currentBulletQuantity;
//        todo: remove me
        if (UIController.Instance) UIController.Instance.SetBulletsQuantity(_currentBulletQuantity, CurrentBulletsCage);
    }

    #endregion
}
