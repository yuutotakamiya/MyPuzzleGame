using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folls : MonoBehaviour
{

    [SerializeField] float speed;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.one);
        transform.Translate(Vector3.down * speed, Space.World);
    }
}
