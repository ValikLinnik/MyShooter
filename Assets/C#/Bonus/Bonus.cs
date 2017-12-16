using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BonusType
{
    Ammo,
    HP,
    Cargo
}

public enum AmmoType
{
    SimpleGun,
    RayCastGun,
    BombGun
}

public class Bonus : MonoBehaviour 
{
    [SerializeField]
    private BonusType _type;

    [SerializeField]
    private AmmoType _ammoType;

    [SerializeField]
    private float _value;

    public float Value
    {
        get
        {
            return _value;
        }

        set
        {
            _value = value;
        }
    }

    public BonusType Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }

    public AmmoType AmmoType
    {
        get
        {
            return _ammoType;
        }

        set
        {
            _ammoType = value;
        }
    }

    public event Action<Bonus> OnBonusTake;

    private void OnBonusTakeHandler()
    {
        if(OnBonusTake != null) OnBonusTake(this);
    }

    public void PickedBonus()
    {
        OnBonusTakeHandler();
    }

    public void SetRandomValue()
    {
        _type = _type.GetRandomItem<BonusType>();
        if(_type == BonusType.Ammo) 
        {
            _ammoType = _ammoType.GetRandomItem<AmmoType>();
            _value = Random.Range(10,31);
        }
        else if(_type == BonusType.Cargo)
        {
            _value = 1;
        }
        else
        {
            _value = 10;
        }
        Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", ToString());
    }

    public override string ToString()
    {
        return string.Format("[Bonus: Type={0}, AmmoType={1}, Value={2}]", Type, AmmoType, Value);
    }
}
