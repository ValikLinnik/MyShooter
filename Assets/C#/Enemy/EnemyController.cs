using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour 
{
	#region SERIALIZE FIELDS

    [SerializeField]
    private Enemy[] _enemyPrefabs;

    [SerializeField]
    private Renderer[] _spawnPoints;

    [SerializeField, Range(1, 10)]
    private int _quantityOfEnemys = 3;

    [SerializeField]
    private Transform _playerTransform;

    [SerializeField, Range(0,200)]
    private float _minDistance = 100;

    #endregion

    #region PRIVATE FIELDS

    private List<Transform> _usedTransforms = new List<Transform>();
    private int _quantity;
    private int _totalShotEnemies;

    #endregion

    #region UNITY EVENTS

    private void LateUpdate()
    {
        EnemyHandler();
    }

    #endregion

    private void EnemyHandler()
    {

        if(_quantity >= _quantityOfEnemys)
        {
            return;
        }

        _usedTransforms.Clear();

        for (int i = _quantity; i < _quantityOfEnemys; i++)
        {
            AddEnemy();
        }
    }

    private void AddEnemy()
    {
        if(_enemyPrefabs.IsNullOrEmpty()) return;

        var tempTransform = GetPosition();

        if(!tempTransform)
        {
            return;
        }

        _usedTransforms.Add(tempTransform);

        var temp = GetEnemy();
        if(!temp) 
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "temp is null");
            return;
        }

        temp.Initialize(tempTransform, _playerTransform);
        if(UIController.Instance) UIController.Instance.AddMessage(string.Format("Added {0} enemys, current quatnity:{1}", _quantity, _quantityOfEnemys));
    }

    private Enemy GetEnemy()
    {
        var prefab = _enemyPrefabs.GetRandomItem();

        if(!prefab)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "prefab is null");
            return null;
        }

        var temp = prefab.GetInstance<Enemy>();
        if(!temp) return null;

        temp.EnemyDie += EnemyDie;
        _quantity++;
        return temp;
    }

    private void EnemyDie (Enemy enemy)
    {
        if(!enemy) return;
        enemy.EnemyDie -= EnemyDie;
        _quantity--;
        _totalShotEnemies++;
        _quantityOfEnemys = _totalShotEnemies;
        if(UIController.Instance) UIController.Instance.TotalShotEnemies = _totalShotEnemies;
    }

    private Transform GetPosition()
    {
        if(_spawnPoints.IsNullOrEmpty()) return null;

        foreach(var item in _spawnPoints)
        {
            if(!item) continue;
            if(!item.isVisible && !_usedTransforms.Contains(item.transform) && (Vector3.Distance(_playerTransform.position, item.transform.position) > _minDistance)) return item.transform;
        }

        return null;
    }
}
