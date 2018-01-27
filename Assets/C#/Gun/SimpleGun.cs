using UnityEngine;
using System.Collections;

public class SimpleGun : BaseGun 
{
    #region SERIALIZE FIELDS

    [SerializeField]
    private string _prefabPath;

    [SerializeField]
    private Transform _spawnPoint;

    #endregion

    #region UNITY EVENTS

    private Bullet _bullet;

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

    #region PRIVATE METHODS

    protected override void Fire()
    {
        if (_currentBulletQuantity <= 0)
            return;

        if (!_bullet)
        {
            _bullet = Resources.Load<Bullet>(_prefabPath);
        }

        if (!_bullet)
        {
            return;
        }

        var tempObj = _bullet.GetInstance<Bullet>();

        if (!tempObj)
            return;

        tempObj.gameObject.SetActive(true);
        tempObj.transform.position = _spawnPoint.position;

        if (tempObj)
        {
            tempObj.Initialize(_spawnPoint.forward * _force, _damage);
            DecrementBulletsAndShowQuantity();
        }
    }

    #endregion
}
