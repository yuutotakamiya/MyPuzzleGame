using UnityEngine;
using DG.Tweening;

public class MoveBlockWithRayLeft : MonoBehaviour
{
    public float moveDuration = 1f; // 移動にかかる時間
    public float rayDistance = 5f; // Rayの飛距離
    public LayerMask hitLayers; // レイヤーマスク

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            // キューブの現在の位置
            Vector3 currentPosition = transform.position;

            // 左方向へのRay
            Vector3 direction = Vector3.left;
            Ray ray = new Ray(currentPosition, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, hitLayers))
            {
                // Rayがヒットした位置
                Vector3 targetPosition = hit.point;
                MoveToTarget(targetPosition);
            }
            else
            {
                // Rayがヒットしなかった場合、指定距離だけ移動
                Vector3 targetPosition = currentPosition + direction * rayDistance;
                MoveToTarget(targetPosition);
            }
        }
    }

    void MoveToTarget(Vector3 targetPosition)
    {
        // DOTweenを使ってブロックを目標位置に移動させる
        transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutQuad);
    }
}
