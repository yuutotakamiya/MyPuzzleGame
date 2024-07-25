using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombiCube : MonoBehaviour
{
    [SerializeField] GameObject combinedCubePrefab; // ���̌�̐V�����L���[�u�̃v���n�u

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

            // ���̌�̃L���[�u�̈ʒu�Ɖ�]���v�Z
            Vector3 combinedPosition = (GetComponent<BlockManager>().VTargetPosition + other.transform.position) / 2;
            Quaternion combinedRotation = Quaternion.identity;

            // �V�����L���[�u�𐶐�
            Instantiate(combinedCubePrefab, other.GetComponent<BlockManager>().VTargetPosition, combinedRotation);

            //�G�t�F�N�g�𐶐�
            Instantiate(effctPrefab, transform.position, Quaternion.identity);

            // ���̃L���[�u���폜
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
    }
}
