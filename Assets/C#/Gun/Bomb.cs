using UnityEngine;
using System.Collections;
using UnityStandardAssets.Effects;

public class Bomb : MonoBehaviour 
{
    [SerializeField]
    private GameObject _effect;

    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private Transform _viewWrapper;

    [SerializeField]
    private string[] _prefabsNames;

    public Rigidbody Rigidbody
    {
        get
        {
            return _rigidbody;
        }
    }

    private float _damage;

    private Transform _view;

    public float Damage
    {
        set
        {
            _damage = value;
        }
    }

    private void AddView()
    {
        Debug.LogFormat("<size=18><color=olive>{0}</color></size>", "add view");
        if(_prefabsNames.IsNullOrEmpty() || !_viewWrapper) return;

        if(_view)
        {
            _view.PutInPool();
            _view = null;
            Debug.LogFormat("<size=18><color=olive>{0}</color></size>", "disabled old view");
        }

        var name = _prefabsNames.GetRandomItem();
        var view = Resources.Load<Transform>(name);
        if(!view) return;
        var temp = view.GetInstance();
        if(!temp) return;
        temp.SetParent(_viewWrapper);
        temp.localPosition = Vector3.zero;
        temp.localRotation = Quaternion.identity;
        _view = temp;
    }

    public void Initialize(Vector3 direction, float damage)
    {
        AddView();
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
