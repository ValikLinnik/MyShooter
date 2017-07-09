using UnityEngine;
using System.Collections;

public class MapCamera : MonoBehaviour 
{
	[SerializeField]
    private Transform _playerTransform;

    private void LateUpdate()
    {
        if(!_playerTransform) return;

        var rotation = transform.eulerAngles;
        rotation.y = _playerTransform.eulerAngles.y;
        transform.eulerAngles = rotation;
    }
}
