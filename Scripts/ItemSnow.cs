using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ItemSnow : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerExpand(other.gameObject); // ���� �ε����� �÷��̾� ũ�� ����
            Destroy(gameObject); // ���� ������Ʈ �� �ı�
        }
    }

    void PlayerExpand(GameObject player)
    {
        if (gameManager.life < 5) //�ִ� ���� 5��
        {
            Vector3 currentScale = player.transform.localScale;
            currentScale *= 1.1f; // �÷��̾��� ũ�⸦ 1.1��� �ø�
            player.transform.localScale = currentScale;
            Debug.Log("������ �� ȹ��, �÷��̾� ũ�� 20% ����");
            gameManager.LifeUp(); //��������
        }
        else if (gameManager.life >= 5) //������ 6������ �������� ���̻� �������� ����
        {
            Debug.Log("�ִ�ũ�⵵��, ���̻� �������� ����");
            Debug.Log(gameManager.life);
        }
    }
}

