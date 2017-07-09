using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class Bomb : MonoBehaviour 
{
    [SerializeField]
    private GameObject _effect;

    [SerializeField]
    private Rigidbody _rigidbody;

    public Rigidbody Rigidbody
    {
        get
        {
            return _rigidbody;
        }
    }

    private float _damage;

    public float Damage
    {
        set
        {
            _damage = value;
        }
    }

    public void Initialize(Vector3 direction, float damage)
    {
        Damage = damage;

        if(Rigidbody) Rigidbody.AddForce(direction, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if(_effect)
        {
            var tempEffect = Instantiate(_effect) as GameObject;

            if(tempEffect)
            {
                tempEffect.transform.parent = null;
                tempEffect.SetActive(true);
                Vector3 pos = transform.position;
                pos.y = .5f;
                tempEffect.transform.position = pos;  

                var temp = tempEffect.GetComponent<ExplosionPhysicsForce>();
                if(temp) temp.Initialize(_damage);
            }
        }

        if(Rigidbody) Rigidbody.velocity = Vector3.zero; 
        this.PutInPool();
    }
}
