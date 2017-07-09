using UnityEngine;


public class EnemyAnimatonComponent : MonoBehaviour 
{
    public event System.Action OnAttack;

    private void OnAttackHandler()
    {
        if(OnAttack != null) OnAttack();
    }

    [SerializeField]
    private Animation _animation;

    [SerializeField]
    private string[] _attackKeys;

    [SerializeField]
    private string  _runKey = "run";

    [SerializeField]
    private string[] _dieKeys;

    [SerializeField]
    private string[] _hitKeys;

    public void AttackAnimation()
    {
        if(!_animation || _attackKeys.Length == 0) return;
        _animation.Play(_attackKeys.GetRandomItem());
    }

    public void RunAnimation()
    {
        if(!_animation) return;
        _animation.Play(_runKey);
    }

    public void DieAnimation()
    {
        if(!_animation || _dieKeys.Length == 0) return;
        _animation.Play(_dieKeys.GetRandomItem());
    }

    public void HitAnimation()
    {
        if(!_animation || _hitKeys.Length == 0) return;
        _animation.Play(_hitKeys.GetRandomItem());
    }

    public void Attack()
    {
        OnAttackHandler();
    }
}

public static class Extentions
{
    public static T GetRandomItem<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}