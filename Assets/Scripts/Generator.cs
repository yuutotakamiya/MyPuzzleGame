using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public GameObject[] falls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 30 == 0)
        {
            GameObject fall = Instantiate(
                falls[Random.Range(0, falls.Length)],
                new Vector3(Random.Range(-14f, 14f), transform.position.y, transform.position.z),
                Quaternion.identity
                );
            Destroy(fall, 10f);
        }
    }
}
