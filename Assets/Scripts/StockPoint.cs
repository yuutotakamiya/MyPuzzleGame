using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockPoint : MonoBehaviour
{
    [SerializeField] GameObject floorCubePrefab; // 床の新しいキューブのプレハブ

    BlockManager blockManager;

    [SerializeField] GameObject CreateBlockEffect;
    // Start is called before the first frame update
    void Start()
    {
       blockManager = GameObject.Find("BlockManager").GetComponent<BlockManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == this.tag)
        {

            Vector3 floorPosition = new Vector3(transform.position.x,4.76f,transform.position.z);
            Quaternion combinedRotation = Quaternion.identity;

            // 新しいキューブを生成
            GameObject cube = Instantiate(floorCubePrefab, floorPosition, Quaternion.identity);

            //エフェクトを生成
            Instantiate(CreateBlockEffect, transform.position, Quaternion.identity);

            Destroy(cube.GetComponent<Block>());
            Destroy(cube.GetComponent<CombiCube>());

            // 元のキューブを削除
            Destroy(other.gameObject);
            Destroy(transform.parent.gameObject);

            blockManager.AddCurrentNum();
        }
    }
}
