using UnityEngine;
using System.Collections;
using System;

public class InputManager : MonoBehaviour 
{
    public event Action<Vector3> OnMouseDown;

    public void OnMouseDownHandler(Vector3 pos)
    {
        if(OnMouseDown != null)
        {
            OnMouseDown(pos);
        }
    }

    [SerializeField]
    private Collider _locationCollider;

    [SerializeField]
    private Transform _marker;

    [SerializeField]
    private Camera _camera;

    private void LateUpdate()
    {
        if(!_camera) return;

        RaycastHit hit;

        if(_locationCollider.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, _camera.farClipPlane))
        {
            var temp = hit.point;
            temp.y = _marker.position.y;
            _marker.position = temp;

            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnMouseDownHandler(temp);
            }
        }
    }
}
