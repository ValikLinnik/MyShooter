using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BloodyMask : MonoBehaviour 
{
    [SerializeField]
    private Image _image;

	[SerializeField]
    private Color _startColor;

    [SerializeField]
    private Color _endColor;

    [SerializeField, Range(.1f,5f)]
    private float _showSpeed = .5f;

    public void StartEffect()
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowBloodyEffect());
    }

    private IEnumerator ShowBloodyEffect()
    {
        if(!_image)
        {
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", "image is null");
            yield break;
        }

        _image.color = _startColor;

        while (_image.color != _endColor)
        {
            _image.color = Color.Lerp(_image.color, _endColor, Time.deltaTime * _showSpeed);
            yield return null;
        }
    }
}
