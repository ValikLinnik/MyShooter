using UnityEngine;
using System.Collections;

public class BonusController : MonoBehaviour 
{
	[SerializeField]
    private Bonus[] _bonusPrefabs;

    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private float _minDelay = .1f;

    [SerializeField]
    private float _maxDelay = 3f;

    private void Start()
    {
        StartBonus();
    }

    private void StartBonus()
    {
        if(!gameObject.activeSelf || !gameObject.activeInHierarchy) return;
        StopAllCoroutines();
        StartCoroutine(WaitAndSetBonus(Random.Range(_minDelay, _maxDelay)));
    }

    private IEnumerator WaitAndSetBonus(float  time)
    {
        yield return new WaitForSeconds(time);

        if(_spawnPoints == null) 
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "spawn points is null");
            yield break;
        }

        var tempTransform = _spawnPoints.GetRandomItem<Transform>();

        if(_bonusPrefabs == null || tempTransform == null) 
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "data is null");
            yield break;
        }

        var tempBonus = _bonusPrefabs.GetRandomItem<Bonus>();

        if(tempBonus == null) 
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "prefab is null");
            yield break;
        }

        tempBonus = tempBonus.GetInstance<Bonus>();

        var pos = tempBonus.transform.position;
        pos.x = tempTransform.position.x;
        pos.z = tempTransform.position.z;
        tempBonus.transform.position = pos;
        tempBonus.gameObject.SetActive(true);

        if(tempBonus.Type == BonusType.Ammo) tempBonus.AmmoType = (AmmoType)Random.Range(0,3);
        tempBonus.OnBonusTake += OnBonusTake;
       // Debug.LogFormat("<size=20><color=red><b><i>will be bonus {0}</i></b></color></size>", tempBonus.Type);
    }

    private void OnBonusTake(Bonus sender)
    {
        if(!sender) return;
        sender.OnBonusTake -= OnBonusTake;
        StartBonus();
    }
}
