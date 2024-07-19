using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombiCube : MonoBehaviour
{
    [SerializeField] GameObject combinedCubePrefab; // 合体後の新しいキューブのプレハブ

    [SerializeField] GameObject effctPrefab;

     GameObject stage;

    // Start is called before the first frame update
    void Start()
    {
        stage = GameObject.Find("stage");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("combicube"))
        {
            // 合体後のキューブの位置と回転を計算
            Vector3 combinedPosition = (transform.position + collision.transform.position) / 2;
            Quaternion combinedRotation = Quaternion.identity;

            // 新しいキューブを生成
            Instantiate(combinedCubePrefab,collision.transform.position, combinedRotation);
            Instantiate(effctPrefab,transform.position,Quaternion.identity);


            // 元のキューブを削除
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
    }
}
