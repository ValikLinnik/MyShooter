using UnityEngine;
using System.Collections;
using MyNamespace;

public class Bullet : MonoBehaviour 
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField, Range(0.1f,5f)]
    private float _lifeTime = 5f;

    private WaitForSeconds _delay;

    private void Awake()
    {
        _delay = new WaitForSeconds(_lifeTime);          
    }

    public Rigidbody Rigidbody
    {
        get
        {
            return _rigidbody;
        }
    }

    public float Damage
    {
        get;
        set;
    }

    public void Initialize(Vector3 direction, float damage)
    {
        Damage = damage;
        if(Rigidbody) Rigidbody.AddForce(direction, ForceMode.Impulse);
        StopAllCoroutines();
        StartCoroutine(Disabler());
    }

    void OnCollisionEnter(Collision collision) 
    {
        var temp = collision.gameObject.GetComponent<HealthComponent>();
        if(temp) 
        {
            temp.TakeDamege(Damage);
        }

        if(Rigidbody) Rigidbody.velocity = Vector3.zero;
        StopAllCoroutines();
        this.PutInPool();
    }

    private IEnumerator Disabler()
    {
        yield return _delay;

        if(Rigidbody) Rigidbody.velocity = Vector3.zero;
        this.PutInPool();
    }
}
