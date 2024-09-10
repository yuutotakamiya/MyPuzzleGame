using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombiCube : MonoBehaviour
{
    [SerializeField] GameObject combinedCubePrefab; // 合体後の新しいキューブのプレハブ

    [SerializeField] GameObject effctPrefab;//合体したときのエフェクトのプレハブ

     GameObject stage;

    bool isDestroy = false;

    public void IsDestroy()
    {
        isDestroy = true;
    }

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
           gameObject.GetComponent<Block>().IsCollision == true&&isDestroy!=true)
        {
            // 合体後のキューブの位置と回転を計算
            Vector3 combinedPosition = GetComponent<Block>().VTargetPosition + other.transform.position;
            Quaternion combinedRotation = Quaternion.identity;

            // 新しいキューブを生成
            if (other.GetComponent<Block>() == null)
            {
                return;
            }
            Instantiate(combinedCubePrefab, other.GetComponent<Block>().VTargetPosition,combinedRotation);

            //エフェクトを生成
            Instantiate(effctPrefab, transform.position, Quaternion.identity);

            isDestroy = true;
            other.GetComponent<CombiCube>().IsDestroy();
            // 元のキューブを削除
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
