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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("combicube"))
        {
            // ���̌�̃L���[�u�̈ʒu�Ɖ�]���v�Z
            Vector3 combinedPosition = (transform.position + collision.transform.position) / 2;
            Quaternion combinedRotation = Quaternion.identity;

            // �V�����L���[�u�𐶐�
            Instantiate(combinedCubePrefab,collision.transform.position, combinedRotation);
            Instantiate(effctPrefab,transform.position,Quaternion.identity);


            // ���̃L���[�u���폜
            Destroy(collision.gameObject);
            Destroy(gameObject);

        }
    }
}
