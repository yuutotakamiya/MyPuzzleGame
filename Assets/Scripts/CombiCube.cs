using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombiCube : MonoBehaviour
{
    [SerializeField] GameObject combinedCubePrefab; // 合体後の新しいキューブのプレハブ

    // Start is called before the first frame update
    void Start()
    {
        
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
            Instantiate(combinedCubePrefab, combinedPosition, combinedRotation);

            // 元のキューブを削除
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
