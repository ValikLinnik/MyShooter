using UnityEngine;
using System.Collections;

public class BillboardComponent : MonoBehaviour 
{
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
