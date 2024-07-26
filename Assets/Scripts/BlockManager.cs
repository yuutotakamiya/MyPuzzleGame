using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;//Sort���\�b�h���g�p���邽�߂ɕK�v

public class BlockManager : MonoBehaviour
{
    float moveDuration = 1f; // �ړ��ɂ����鎞��
    [SerializeField] float rayDistance = 10f; // Ray�̔򋗗�
    [SerializeField] LayerMask hitLayers; // ���C���[�}�X�N

    Tween moveTween;

    Rigidbody rb;

    bool isCollision = false;
    bool isMove = false;

    Vector3 TargetPosition;

    Vector3 startTouchPos;
    Vector3 endTouchPos;

    float flickValue_x;
    float flickValue_y;

    //isCollision�v���p�e�B
    public bool IsCollision
    {
        get { return isCollision; }
    }

    //isTargetPosition�̃v���p�e�B
    public Vector3 VTargetPosition
    {
        get { return TargetPosition; }
    }

     void Start()
     {
        rb = GetComponent<Rigidbody>();
     }

    
    void Update()
    {
        if (isMove) return;//�ړ����͓��͂𖳎�

        if (Input.GetMouseButtonDown(0) == true)
        {
            startTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }
        if (Input.GetMouseButtonUp(0) == true)
        {
            endTouchPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);


            flickValue_x = endTouchPos.x - startTouchPos.x;
            flickValue_y = endTouchPos.y - startTouchPos.y;

            Vector3 direction = Vector3.zero;
            if (flickValue_x  < -50)
            {
                //���Ɉړ�
                direction = Vector3.left;
            }
            else if (flickValue_x > 50)
            {
                //�E�Ɉړ�
                direction = Vector3.right;
            }
            else if (flickValue_y > 50)
            {
                //���Ɉړ�
                direction = Vector3.forward;
            }
            else if (flickValue_y < -50)
            {
                //���Ɉړ�
                direction = Vector3.back;
            }


            if (direction != Vector3.zero)
            {
                // �L���[�u�̌��݂̈ʒu
                Vector3 currentPosition = transform.position;

               
                Ray ray = new Ray(currentPosition, direction);

                //ray���΂����I�u�W�F�N�g�����ԂɎ擾
                RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, hitLayers);
                Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

                if (hits.Length >= 1)
                {
                    if(hits[0].collider.gameObject.tag == tag)
                    {
                        isCollision = true;
                    }
                    // Ray���q�b�g�����ʒu�Ɉړ�
                    TargetPosition = hits[hits.Length - 1].transform.position - direction * hits.Length;

                    //int movecount = 0;//�����̃u���b�N�Ɣ�r����u���b�N�̕ϐ�
                    int count = 1;
                    string CmpTag = tag;

                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (count == 0)
                        {
                            CmpTag = hits[i].collider.tag;
                        }
                        //�q�b�g�����I�u�W�F�N�g�����g�Ɠ����^�O�����ꍇ�̏���
                        if (hits[i].collider.gameObject.tag == CmpTag)
                        {
                            count++;
                            if (count == 2)
                            {
                                count = 0;

                                TargetPosition += direction;
                            }
                        }
                        else
                        {
                            count = 1;

                            CmpTag = hits[i].collider.gameObject.tag;
                        }
                    }

                    isMove = true;//�ړ��J�n

                    //DOTween���g���Ĉړ�
                    this.transform.GetComponent<Rigidbody>().DOMove(TargetPosition, moveDuration).OnComplete(() =>
                    {
                        isMove = false;//�ړ�������Ƀt���O�����Z�b�g����
                    });
                    
                }
            }
        }
    }

    public void OnClickRetry()
    {
        SceneManager.LoadScene("MyPuzzleScene");
    }

    
}
