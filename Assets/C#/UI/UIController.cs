using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIController : MonoBehaviour 
{
    #region SERIALIZE FIELDS

    [SerializeField]
    private Slider _bombPowerSlider;

    [SerializeField]
    private Slider _lifeSlider;

    [SerializeField]
    private Image _aimImage;

    [SerializeField]
    private Image _pointAimImage;

    [SerializeField]
    private GameObject _mapImage;

    [SerializeField]
    private BloodyMask _bloodyMaskEffect;

    [SerializeField]
    private Image _gameOverImage;

    [SerializeField]
    private Text _messageText;

    [SerializeField, Range(1,10)]
    private int _messageCount = 5;

    [SerializeField]
    private Text _weaponName;

    [SerializeField]
    private Text _bulletQuanity;

    [SerializeField]
    private Text _cages;

    [SerializeField]
    private Text _cargoQuantity;

    [SerializeField]
    private Text _hpText;

    [SerializeField]
    private Text _totalShotEnemies;

    #endregion

    private Queue<string> _messages = new Queue<string>();

    #region SINGLETONE SECTION

    private static UIController _instance;

    public static UIController Instance
    {
        get
        {
            return _instance; 
        }
    }

    #endregion

    #region UNITY EVENTS

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    #endregion

    #region PUBLIC METHODS

    public void SetHP(int value)
    {
        if(_hpText) _hpText.text = value.ToString();
    }

    public void SetCargoQuantity(int quantity)
    {
        if(_cargoQuantity) _cargoQuantity.text = quantity.ToString();
    }

    public void SetWeaponName(AmmoType name)
    {
        if(_weaponName) _weaponName.text = name.ToString();
    }

    public void SetBulletsQuantity(int bullets, int cages)
    {
        if(_bulletQuanity) _bulletQuanity.text = bullets.ToString();
        if(_cages) _cages.text = cages.ToString();
    }

    public void AddMessage(string message)
    {
        if(!_messageText) return;
        _messages.Enqueue(message);
        if(_messages.Count > _messageCount) _messages.Dequeue();

        _messageText.text = string.Empty;

        foreach (var item in _messages)
        {
            _messageText.text += item + "\n";
        }
    }

    public void ShowMessage(string text)
    {
        if(!_messageText) return;
        _messageText.text = text;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetActiveGameOverScreen(bool flag)
    {
        if(!_gameOverImage) return;
        _gameOverImage.gameObject.SetActive(flag);

    }

    public void BloodyScreen()
    {
        if(_bloodyMaskEffect) _bloodyMaskEffect.StartEffect();
    }

    public void SetActiveMapImage(bool flag)
    {
        if(_mapImage) _mapImage.SetActive(flag);
    }

    public void SetActivePointAIM(bool flag)
    {
        if(!_pointAimImage)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "point aim is null");
            return;
        }

        _pointAimImage.gameObject.SetActive(flag);
    }

    public void SetActiveAIM(bool flag)
    {
        if(!_aimImage)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "aim is null");
            return;
        }

        _aimImage.gameObject.SetActive(flag);
    }

    public void SetActiveBombPowerSlider(bool flag)
    {
        if(!_bombPowerSlider)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "bomb slider is null");
            return;
        }

        _bombPowerSlider.gameObject.SetActive(flag);
    }

    public void SetBompPowerValue(float value)
    {
        if(!_bombPowerSlider)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "bomb slider is null");
            return;
        }

        _bombPowerSlider.value = value;
    }

    public void SetLifeValue(float value)
    {
        if(!_lifeSlider)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "_lifeSlider is null");
            return;
        }

        _lifeSlider.maxValue = value > _lifeSlider.maxValue ? value : _lifeSlider.maxValue;
        _lifeSlider.value = value;
    }

    public int TotalShotEnemies
    {
        private get
        {
            return 0;
        }
        set
        {
            if(_totalShotEnemies) _totalShotEnemies.text = value.ToString("D");
        }
    }

    #endregion

}
