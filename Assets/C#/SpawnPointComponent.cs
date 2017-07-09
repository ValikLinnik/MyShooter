using UnityEngine;
using System.Collections;

public class SpawnPointComponent : MonoBehaviour 
{
    [SerializeField]
    private Renderer _renderer;

    private void LateUpdate()
    {
        if(_renderer && _renderer.isVisible) 
        {
            //gameObject.SetActive(false);
            Debug.LogFormat("<size=20><color=red><b><i>{0}</i></b></color></size>", gameObject.name);
        }
    }
}
