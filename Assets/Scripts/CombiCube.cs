using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombiCube : MonoBehaviour
{
    [SerializeField] GameObject combinedCubePrefab; // ���̌�̐V�����L���[�u�̃v���n�u

    [SerializeField] GameObject effctPrefab;//���̂����Ƃ��̃G�t�F�N�g�̃v���n�u

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
            // ���̌�̃L���[�u�̈ʒu�Ɖ�]���v�Z
            Vector3 combinedPosition = GetComponent<Block>().VTargetPosition + other.transform.position;
            Quaternion combinedRotation = Quaternion.identity;

            // �V�����L���[�u�𐶐�
            if (other.GetComponent<Block>() == null)
            {
                return;
            }
            Instantiate(combinedCubePrefab, other.GetComponent<Block>().VTargetPosition,combinedRotation);

            //�G�t�F�N�g�𐶐�
            Instantiate(effctPrefab, transform.position, Quaternion.identity);

            isDestroy = true;
            other.GetComponent<CombiCube>().IsDestroy();
            // ���̃L���[�u���폜
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
