using UnityEngine;
using DG.Tweening;

public class MoveBlockWithRayLeft : MonoBehaviour
{
    public float moveDuration = 1f; // �ړ��ɂ����鎞��
    public float rayDistance = 5f; // Ray�̔򋗗�
    public LayerMask hitLayers; // ���C���[�}�X�N

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // �L���[�u�̌��݂̈ʒu
            Vector3 currentPosition = transform.position;

            // �������ւ�Ray
            Vector3 direction = Vector3.left;
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, hitLayers))
            {
                // Ray���q�b�g�����ʒu
                Vector3 targetPosition = hit.point;
                MoveToTarget(targetPosition);
            }
            else
            {
                // Ray���q�b�g���Ȃ������ꍇ�A�w�苗�������ړ�
                Vector3 targetPosition = currentPosition + direction * rayDistance;
                MoveToTarget(targetPosition);
            }
        }
    }

    void MoveToTarget(Vector3 targetPosition)
    {
        // DOTween���g���ău���b�N��ڕW�ʒu�Ɉړ�������
        transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutQuad);
    }
}
