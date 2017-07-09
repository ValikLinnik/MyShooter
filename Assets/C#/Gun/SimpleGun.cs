using UnityEngine;
using System.Collections;

public class SimpleGun : BaseGun 
{
    [SerializeField]
    private Bullet _bullet;

    [SerializeField]
    private Transform _spawnPoint;

    private void OnEnable()
    {
        if(UIController.Instance) UIController.Instance.SetActiveAIM(true);
    }

    protected override void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }

        base.LateUpdate();
    }

    protected override void Fire()
    {
        if(_currentBulletQuantity <= 0) return;

        var tempObj = _bullet.GetInstance<Bullet>();

        if(!tempObj) return;

        tempObj.gameObject.SetActive(true);
        tempObj.transform.position = _spawnPoint.position;

        if(tempObj) 
        {
            tempObj.Initialize(_camera.transform.forward * _force, _damage);
            DecrementBulletsAndShowQuantity();
        }
    }
}
