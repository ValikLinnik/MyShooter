using UnityEngine;
using System.Collections;

public class SimpleGun : BaseGun 
{
    #region SERIALIZE FIELDS

    [SerializeField]
    private Bullet _bullet;

    [SerializeField]
    private Transform _spawnPoint;

    #endregion

    #region UNITY EVENTS

    protected override void OnEnable()
    {
        if (UIController.Instance) UIController.Instance.SetActiveAIM(true);
    }

    protected override void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftCommand) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }

        base.LateUpdate();
    }

    #endregion

    protected override void Fire()
    {
        if(_currentBulletQuantity <= 0) return;

        var tempObj = _bullet.GetInstance<Bullet>();

        if(!tempObj) return;

        tempObj.gameObject.SetActive(true);
        tempObj.transform.position = _spawnPoint.position;

        if(tempObj) 
        {
            tempObj.Initialize(/*Camera.transform*/_spawnPoint.forward * _force, _damage);
            DecrementBulletsAndShowQuantity();
        }
    }
}
