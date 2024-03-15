using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody2D rb;
    float axisH = 0.0f;
    public float speed = 4.0f;   // �̵� �ӵ�
    public float jump = 9.0f;    // ������
    public float dash = 20.0f;    // �뽬 �ӵ� 
    private float defaultspeed;

    public LayerMask groundLayer;  // ���� ���̾�
    bool goJump = false;           // ���� ���� �÷���
    bool onGround = false;         // ���� �÷���

    bool goDash = false;           // �뽬 �÷���
    public float defaultTime = 0.3f;      
    private float dashTime;        // �뽬 �ð�

    public GameObject objPrefab;   // �߻�ü
    public float maxShotDelay;
    public float curShotDelay;
    public float fireSpeedx = 30.0f;
    bool zero = true;

    private SpriteRenderer spriteRenderer;
    public string targetSceneName = "Stage1";

    public bool left = false;

    public int MovingAnim = 0;         // �ִϸ��̼�
    public GameObject PlayerMoving;
    private bool IsMoving = false;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();   // rigidbody2D ��������
        spriteRenderer = GetComponent<SpriteRenderer>();  
        dashTime = defaultTime;
        defaultspeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        get_input();
        SnowAttack();
        Reload();
        //MoveCheck();
    }

    void get_input()
    {
        axisH = Input.GetAxisRaw("Horizontal");   // ���� ���� �Է�
        IsMoving = false;
        MovingAnim = 0;

        // right
        if (axisH > 0.0f)
        {
            IsMoving = true;
            if (!left) //������ȯx ������
            {
                Debug.Log("������ �̵�");
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
                left = false;
            }
            else if (left) //������ȯo ������
            {
                Debug.Log("������ �̵�");
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                left = false;
            }
        }
        // left
        else if (axisH < 0.0f)
        {
            IsMoving = true;
            if (!left) //������ȯo ����
            {
                Debug.Log("���� �̵�");
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                left = true;
            }
            else if (left) //������ȯx ����
            {
                Debug.Log("���� �̵�");
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
                left = true;
            }
            
        }

        if (IsMoving)
        {
            MovingAnim++;
            Vector3 moveVelocity = new Vector3(axisH, 0, 0) * speed * Time.deltaTime;
            rb.transform.position += moveVelocity;
        }

        // stop
        if (Input.GetButtonUp("Horizontal"))
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * 0.000001f, rb.velocity.y);
        }

        // jump
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        // dash
        if (Input.GetKeyDown(KeyCode.X))
        {
            Dash();
        }
    }

    void FixedUpdate()
    {
        // ���� ����
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);

        // ���� ���ų� �ӵ��� 0�� �ƴ� ��
        if (onGround || axisH != 0)
        {
            // �ӵ� ����
            rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
        } 

        // ���� ������ ������ ������ ��
        if(onGround && goJump)
        {
            Debug.Log("����");
            Vector2 jumpPw = new Vector2(0, jump);     // ������ ���� ����
            rb.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;                            // ���� �÷��� ����
        }

        // �뽬�� ������ ��
        if(goDash)
        {
            if(dashTime <= 0)   // �뽬 �ð��� ������
            {
                dashTime = defaultTime;
                speed = defaultspeed;                                       // ���� �ӵ��� ���ư�
                rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
                goDash = false;                                             // �뽬 �÷��� ����
            }
            else                // �뽬 �ð� ���� ��
            {
                speed = dash;                                               // �뽬 �ӵ�
                rb.velocity = new Vector2(speed * axisH, rb.velocity.y);
                dashTime -= Time.deltaTime;                                 // �뽬 ���� �ð� ����
            }
        }
    }

    // jump function
    public void Jump()
    {
        goJump = true;
        Debug.Log("���� ��ư ����");
    }

    // dash fuction
    public void Dash()
    {
        goDash = true;
        Debug.Log("�뽬 ��ư ����");
    }

    public void SnowAttack()
    {
        if (Input.GetKey(KeyCode.Z) && curShotDelay >= maxShotDelay)
        {
            curShotDelay = 0;
            Vector3 pos = new Vector3(transform.position.x + 1, transform.position.y + 2, transform.position.z);
            GameObject snow = Instantiate(objPrefab, pos, transform.rotation);
            Rigidbody2D rigid = snow.GetComponent<Rigidbody2D>();
            axisH = Input.GetAxisRaw("Horizontal");
            if (axisH > 0.0f)
            {
                rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                zero = true;
            }
            else if (axisH < 0.0f)
            {
                rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
                zero = false;
            }
            else
            {
                if (zero)
                {
                    rigid.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                }
                else
                {
                    rigid.AddForce(Vector2.left * 10, ForceMode2D.Impulse);
                }
            }
        }
    }

    public void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    // ���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bear"|| collision.gameObject.tag == "Rabbit")
        {
            // ���� ���̰� enemy ���� ���� ���� ��
            if (rb.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                // attack
                OnAttack(collision.transform);
            }
            else
            {
                Debug.Log("������");
                // damage
                OnDamaged(collision.transform.position);
            }
        }
        if (collision.gameObject.tag == "Spike")
        {
            /*gameObject.transform.position = Vector3.zero; //��ֹ��� �񸮸� ����ġ*/
            Debug.Log("������");
            // damage
            OnDamaged(collision.transform.position);
        }
    }

    public void OnAttack(Transform enemy)
    {
        // reaction
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // enemy die
    }

    public void OnDamaged(Vector2 targetPos)
    {
        // health down
        gameManager.LifeDown();
        Debug.Log("��������");
        // change layer
        gameObject.layer = 11;
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // reaction force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);

        // ��� 3�ʵ��� ����
        Invoke("OffDamaged", 3);
    }

    public void OffDamaged()
    {
        Debug.Log("������");
        // chage layer
        gameObject.layer = 10;
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        /*// sprite alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        // sprite filp Y
        spriteRenderer.filpY = true;
        // collider disable
        capsuleCollier.enabled = false;
        // die effect jump
        rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);*/

    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("stage")) // �浹�� ��ü�� "Player" �±׸� ���� ���
        {
            Debug.Log("�� �ٲ�");
            SceneManager.LoadScene(targetSceneName);
        }
    }
}

