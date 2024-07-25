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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == this.tag &&
           gameObject.GetComponent<BlockManager>().IsCollision == true)
        {

            // 合体後のキューブの位置と回転を計算
            Vector3 combinedPosition = (GetComponent<BlockManager>().VTargetPosition + other.transform.position) / 2;
            Quaternion combinedRotation = Quaternion.identity;

            // 新しいキューブを生成
            Instantiate(combinedCubePrefab, other.GetComponent<BlockManager>().VTargetPosition, combinedRotation);

            //エフェクトを生成
            Instantiate(effctPrefab, transform.position, Quaternion.identity);

            // 元のキューブを削除
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
    }
}
