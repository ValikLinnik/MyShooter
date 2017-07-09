using UnityEngine;
using System.Collections;
using System.Reflection;
using MyNamespace;
using UnityEngine.UI;

public delegate void OnEnemyDie(Enemy enemy);

public enum EnemyStates
{
    Normal,
    Damaged,
    Die
}

public class Enemy : MonoBehaviour 
{
    #region EVENTS AND HANDLERS

    public event OnEnemyDie EnemyDie;

    private void EnemyDieHandler()
    {
        if (EnemyDie != null)
            EnemyDie(this);
    }

    #endregion

    #region SERIALIZE FIELDS

    [SerializeField, Range(1, 100)]
    private float _health = 50;

    [SerializeField]
    private HealthComponent _healthComponent;

    [SerializeField]
    private Slider _lifeSlider;

    [SerializeField]
    private UnityEngine.AI.NavMeshAgent _navMesh;

    [SerializeField, Range(.1f, 5f)]
    private float _attackDistance = 2;

    [SerializeField, Range(1, 50)]
    private float _attackDamage = 3;

    [SerializeField, Range(.1f, 3f)]
    private float _attackReCharge = 1;

    [SerializeField]
    private EnemyAnimatonComponent _animationComponent;

    [SerializeField, Range(1,5)]
    float _follingSpeed = 1;

    #endregion

    #region PRIVATE FIELDS

    private Transform _player;

    private HealthComponent _playerHealthComponent;

    private float _timeToNextAttack;

    private EnemyStates _enemyStates;

    #endregion

    public Transform Player
    {
        set
        {
            _player = value;
        }
    }

    private void Start()
    {
        if(_healthComponent) _healthComponent.OnDamageGet += OnDamageGet;
        if(_player) _playerHealthComponent = _player.GetComponent<HealthComponent>();
        if(_animationComponent) _animationComponent.OnAttack += OnAttack;
    }

    private void OnAttack ()
    {
        if(_playerHealthComponent) _playerHealthComponent.TakeDamege(_attackDamage);
    }

    public void Initialize(Transform myPoint, Transform playerTransform)
    {
        gameObject.SetActive(true);
        Player = playerTransform;
        _enemyStates = EnemyStates.Normal;
        _lifeSlider.gameObject.SetActive(true);

        if(_animationComponent) _animationComponent.RunAnimation();

        if(myPoint)
        {
            transform.position = myPoint.position;
            transform.rotation = myPoint.rotation;
        }

        if(_lifeSlider) 
        {
            _lifeSlider.maxValue = _health;
            _lifeSlider.value = _health;
        }

        if(_healthComponent) _healthComponent.CurrentHealthValue = _health;
    }

    private void Update()
    {
        if(_enemyStates != EnemyStates.Normal) return;
        
        if(_player && Vector3.Distance(transform.position, _player.position) <= _attackDistance && Time.time > _timeToNextAttack)
        {
            
            _timeToNextAttack = Time.time + _attackReCharge;
            if(_animationComponent) _animationComponent.AttackAnimation();

            return;
        }
        else if(_player && Vector3.Distance(transform.position, _player.position) > _attackDistance)
        {
            _navMesh.destination = _player.position;
            if(_animationComponent) _animationComponent.RunAnimation();
        }
    }

    private void OnDestroy()
    {
        if(_healthComponent) _healthComponent.OnDamageGet -= OnDamageGet;
        if(_animationComponent) _animationComponent.OnAttack -= OnAttack;

    }

    private void OnDamageGet (float arg1, float arg2)
    {
        if(_enemyStates == EnemyStates.Die) return;

        if(arg2 <= 0) 
        {
            _enemyStates = EnemyStates.Die;
           if(UIController.Instance) UIController.Instance.AddMessage("One enemy was killed");
            if(_animationComponent) _animationComponent.DieAnimation();
            _lifeSlider.gameObject.SetActive(false);
            _navMesh.destination = transform.position;
            Invoke("Die",2f);
            return;
        }

        if(_lifeSlider) 
        {
            _lifeSlider.value = arg2;
            if(_animationComponent) _animationComponent.HitAnimation();
            StopRun();
        }
    }

    private void StopRun()
    {
        _navMesh.destination = transform.position;
        _enemyStates = EnemyStates.Damaged;
        Invoke("StartRun",.5f);
    }

    private void StartRun()
    {
        _enemyStates = EnemyStates.Normal;
    }

    private void Die()
    {
        this.PutInPool(); 
        EnemyDieHandler();
    }
}
