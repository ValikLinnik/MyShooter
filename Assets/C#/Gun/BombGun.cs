using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BombGun : BaseGun 
{
    #region SERIALIZE FIELDS

    [SerializeField]
    private string _prefabPath;

    [SerializeField]
    private Transform _spawnPoint;

    [SerializeField, Range(1f, 10f)]
    private float _forceAddSpeed = 1f;

    #endregion

    #region PRIVATE FIELDS

    private Bomb _bullet;

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
            if (UIController.Instance)
                UIController.Instance.SetBompPowerValue(_bombForce);
        }
    }

    #endregion

    #region UNITY EVENTS

    protected override void OnEnable()
    {
        if (UIController.Instance)
        { 
            UIController.Instance.SetActiveBombPowerSlider(true); 
            UIController.Instance.SetActivePointAIM(true);
        }

        base.OnEnable();
    }

    private void OnDisable()
    {
        if (UIController.Instance)
        {
            UIController.Instance.SetActiveBombPowerSlider(false); 
            UIController.Instance.SetActivePointAIM(false);
        }
    }

    protected override void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftCommand)|| Input.GetKeyDown(KeyCode.Mouse0))
        {
            BombForce = 0;
        }

        if (Input.GetKey(KeyCode.RightShift)|| Input.GetKey(KeyCode.Mouse0))
        {
            BombForce = _bombForce == 1 ? 0 : _bombForce + _forceAddSpeed * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.RightShift)|| Input.GetKeyUp(KeyCode.Mouse0))
        {
            Fire();
        }

        base.LateUpdate();
    }

    #endregion

    #region PROTECTED METHODS

    protected override void Fire()
    {
        if (_currentBulletQuantity <= 0)
        {
            Debug.LogFormat("<size=18><color=olive>{0}</color></size>", "you have no bullets! recharge or change gun.");
            return;
        }

        if(!_bullet)
        {
            Debug.LogFormat("<size=18><color=olive>{0}</color></size>", "TRY TO DOWNLOAD PREFAB");
            _bullet = Resources.Load<Bomb>(_prefabPath);
        }

        if(!_bullet)
        {
            Debug.LogFormat("<size=18><color=olive>{0}</color></size>", "BULLET IS NULL!");
            return;
        }

        var tempObj = _bullet.GetInstance<Bomb>();

        if (!tempObj)
            return;

        tempObj.gameObject.SetActive(true);
        tempObj.transform.position = _spawnPoint.position;

        if (tempObj)
        {
            float _fireForce = _force * BombForce;
            tempObj.Initialize(Camera.transform.forward * _fireForce, _damage);
            DecrementBulletsAndShowQuantity();
        }
    }

    #endregion
}
