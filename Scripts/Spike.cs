using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public GameManager gameManager;
    private Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerShrink(collision.gameObject); // ���� �ε����� �÷��̾� ũ�� ����
            Debug.Log("�浹, �÷��̾� ũ�� 20% ����");
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.9f; // �÷��̾��� ũ�⸦ 0.9��� ����
        player.transform.localScale = currentScale;
    }
}
