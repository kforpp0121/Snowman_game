using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    void Start()
    {
        
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");   // �÷��̾� �ν�
        if (player != null)
        {
            // ī�޶� ��ǥ ����
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;

            // ���� ���� ����ȭ
            // �� ���� �̵� ����
            if ( x < leftLimit)
            {
                x = leftLimit;
            }
            else if (x > rightLimit)
            {
                x = rightLimit;
            }

            // ���� ���� ����ȭ
            // �� �Ʒ��� �̵� ����
            if (y < bottomLimit)
            {
                y = bottomLimit;
            }
            else if (y > topLimit)
            {
                y = topLimit;
            }

            // ī�޶� ��ġ ����
            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;
        }
    }
}
