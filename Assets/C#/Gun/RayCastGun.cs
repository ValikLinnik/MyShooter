using UnityEngine;
using System.Collections;
using MyNamespace;

public class RayCastGun : BaseGun 
{
    [SerializeField]
    private GameObject _shootEffect;

    private void OnEnable()
    {
        if(UIController.Instance) UIController.Instance.SetActiveAIM(true);
    }

    protected virtual void LateUpdate()
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

        Ray ray = _camera.ViewportPointToRay(new Vector2( .5f, .5f));

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _camera.farClipPlane))
        {
            var tempEnemy = hit.collider.GetComponent<Enemy>();
            if(!tempEnemy) return;

            if(_shootEffect) _shootEffect.SetActive(true);
            var tempHealth = hit.collider.GetComponent<HealthComponent>();
            if(tempHealth) tempHealth.TakeDamege(_damage);

            DecrementBulletsAndShowQuantity();

//            Rigidbody temp = hit.collider.gameObject.GetComponent<Rigidbody>();
//            if(!temp) return;
//            temp.AddForceAtPosition(_camera.transform.forward * _force, hit.point,ForceMode.Impulse);

        }
    }

}
