using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombiCube : MonoBehaviour
{
    [SerializeField] GameObject combinedCubePrefab; // ���̌�̐V�����L���[�u�̃v���n�u

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
            // ���̌�̃L���[�u�̈ʒu�Ɖ�]���v�Z
            Vector3 combinedPosition = (transform.position + collision.transform.position) / 2;
            Quaternion combinedRotation = Quaternion.identity;

            // �V�����L���[�u�𐶐�
            Instantiate(combinedCubePrefab, combinedPosition, combinedRotation);

            // ���̃L���[�u���폜
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
