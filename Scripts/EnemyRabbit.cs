using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRabbit : MonoBehaviour
{
    public float speed = 4f; // �̵� �ӵ�
    public GameManager gameManager;
    private Rigidbody2D rb;
    private bool movingRight = true;
    public SpriteRenderer rabbit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (movingRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("�浹 �߻�: " + collision.gameObject.name);
            Debug.Log($"Player Position: {collision.transform.position}");
            Debug.Log($"Enemy Position: {transform.position}");

            float relativeY = collision.transform.position.y - transform.position.y;

            if (relativeY > 0 && rb.velocity.y < 0)
            {
                Debug.Log("���");
                Destroy(gameObject); // ���� ������Ʈ �� �ı�
            }
            else
            {
                PlayerShrink(collision.gameObject); // ���� �ε����� �÷��̾� ũ�� ����
                Debug.Log("�� �浹, �÷��̾� ũ�� 20% ����");
                Destroy(gameObject); // ���� ������Ʈ �� �ı�
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            movingRight = !movingRight;
            if (rabbit.flipX == true)
            {
                rabbit.flipX = false;
            }
            else if (rabbit.flipX == false)
            {
                rabbit.flipX = true;
            }
        }
    }

    void PlayerShrink(GameObject player)
    {
        Vector3 currentScale = player.transform.localScale;
        currentScale *= 0.9f; // �÷��̾��� ũ�⸦ 0.9��� ����
        player.transform.localScale = currentScale;
    }
}
