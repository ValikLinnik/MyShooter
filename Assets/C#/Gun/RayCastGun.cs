using UnityEngine;
using System.Collections;
using MyNamespace;

public class RayCastGun : BaseGun 
{
    #region SERIALIZE FIELDS

    [SerializeField]
    private GameObject _shootEffect;

    #endregion

    #region UNITY EVENTS

    protected override void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftCommand)|| Input.GetKeyDown(KeyCode.Mouse0))
        {
            Fire();
        }

        base.LateUpdate();
    }

    #endregion

    #region PRIVATE METHODS

    protected override void Fire()
    {
        if (_currentBulletQuantity <= 0) return;

        Ray ray = Camera.ViewportPointToRay(new Vector2(.5f, .5f));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Camera.farClipPlane))
        {
            var tempEnemy = hit.collider.GetComponent<Enemy>();
            if (!tempEnemy) return;

            if (_shootEffect) _shootEffect.SetActive(true);
            var tempHealth = hit.collider.GetComponent<HealthComponent>();
            if (tempHealth) tempHealth.TakeDamege(_damage);

            DecrementBulletsAndShowQuantity();
        }
    }

    #endregion

}
