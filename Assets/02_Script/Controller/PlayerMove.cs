using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    // ĳ���� ����
    public enum PlayerState
    {
        Idle,
        Jump,
        Fall,
        Walk,
        Dash,
        Ladder,
        Wallside,
        Hit,
        Die,
        Attack,
        DashAttack,
        Peeking
    }

    // �ʱ� ĳ���� ����
    PlayerState _state = PlayerState.Idle;

    // ĳ���� �̵��ӵ�
    public float speed = 5f;

    // �ִϸ��̼�
    Animator anim;

    // ���� ����
    int jumpCount;
    int maxJumpCount = 1;
    
    // �뽬 ����
    float dashTime = 0f;
    float dashSpeed = 15f;
    bool isDashing = false;

    // �뽬 �� ���� ��Ҵ��� Ȯ��
    bool isCollWall = false;

    // ��ٸ� ����
    bool _ladder;
    bool canMove = true;

    float peekingTIme = 0f;

    // �޺����� ����
    int attackClick = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 1.3f;

    Rigidbody2D rigid;

    CameraMove cameraMove;

    void Start()
    {
        // �ִϸ��̼� �ʱ�ȭ
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        cameraMove = FindObjectOfType<CameraMove>();
        
    }

    // Update is called once per frame
    void Update()
    {
        OnKeyboard();
        switch (_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;

            case PlayerState.Jump:
                UpdateJump();
                break;

            case PlayerState.Fall:
                UpdateFall();
                break;

            case PlayerState.Walk:
                UpdateWalk();
                break;

            case PlayerState.Dash:
                UpdateDash();
                break;

            case PlayerState.Ladder:
                UpdateLadder();
                break;

            case PlayerState.Wallside:
                UpdateWallside();
                break;

            case PlayerState.Hit:
                break;

            case PlayerState.Die:
                break;

            case PlayerState.Attack:
                UpdateAttack();
                break;

            case PlayerState.DashAttack:
                UpdateDashAttack();
                break;

            case PlayerState.Peeking:
                UpdatePeeking();
                break;
        }

        // ���Ͻ�
        if (rigid.velocity.y < 0 && _state != PlayerState.Wallside && _state != PlayerState.Dash)
        {
            _state = PlayerState.Fall;
            canMove = true;
        }


    }

    void FixedUpdate()
    {
        // ��� ���� �ƴ� ���� ���� ��� �̵� ����
        if (!isDashing)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
        }
    }

    // ���̵�
    void UpdateIdle()
    {
        anim.Play("Idle");
        jumpCount = 0;

        if(cameraMove != null)
        {
            SetIsPeeking(false, 0);
        }
    }

    void UpdatePeeking()
    {
        Debug.Log("��ŷ����");
        anim.Play("Idle");

        if (Input.GetKey(KeyCode.UpArrow) && peekingTIme > 1f)
        {
            SetIsPeeking(true, 1);
        }

        if (Input.GetKey(KeyCode.DownArrow) && peekingTIme > 1f)
        {
            SetIsPeeking(true, -1);
        }
    }

     void SetIsPeeking(bool isPeeking, float upAndDown)
     {
        cameraMove.GetIsPeeking(isPeeking, upAndDown);
     }

    // �̵�
    void UpdateWalk()
    {
        anim.Play("Walk");
    }

    // ����
    void UpdateJump()
    {
        anim.Play("Jump");
        if (rigid.velocity.y < 0)
        {
            // ���� �Ŀ��� Fall ���·� ��ȯ
            _state = PlayerState.Fall;
        }
        isDashing = false;
    }

    // ������ ��
    void UpdateFall()
    {
        anim.Play("Fall");
    }

    // �� ���鿡 �پ��� ��
    void UpdateWallside()
    {
        anim.Play("Wallside");

    }



    // ���� ������� -> Idle�� ���� , ���� ���鿡 ������� -> Wallside�� ���� 
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            isCollWall = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // ���� ���鿡 ��Ҵ��� Ȯ��
                if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    isCollWall = true;
                    if (_state != PlayerState.Dash)
                    {
                        _state = PlayerState.Wallside;
                        jumpCount = 0;
                    }
                    else
                    {
                        // ��� �� ���� �ε���
                        EndDash();
                    }
                    break;
                }
                else if (contact.normal.y >= 1f)
                {
                    if (_state == PlayerState.Wallside || _state == PlayerState.Fall || _state == PlayerState.Ladder)
                    {
                        _state = PlayerState.Idle;
                        jumpCount = 0;
                        canMove = true;

                        if (isDashing)
                        {
                            EndDash();
                        }
                    }
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            isCollWall = false;
        }
    }

    // �뽬
    void UpdateDash()
    {
        // �ð� �����ų� ���� �ε����� �뽬 ��
        if (dashTime <= 0f || isCollWall)
        {
            EndDash();
        }
        else
        {
            // �뽬 ���ǵ�
            speed = dashSpeed;
            anim.Play("Dash");
            dashTime -= Time.deltaTime;

            float dashDir = GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
            Vector3 movement = Vector3.right * dashDir * Time.deltaTime * speed;

            // ���� �浹 üũ
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dashDir, 0), movement.magnitude, LayerMask.GetMask("Map"));
            if (hit.collider != null)
            {
                // ���� �浹���� ��� �̵��� ����
                transform.position = hit.point;
                EndDash();
            }
            else
            {
                // �浹���� �ʾ��� ��� ���� �̵�
                transform.position += movement;
            }
        }
    }
    // �뽬 ��
    void EndDash()
    {
        speed = 5f;
        isDashing = false;

        if (rigid.velocity.y < 0)
            _state = PlayerState.Fall;
        else
            _state = PlayerState.Idle;
    }

    // ����
    void UpdateAttack()
    {
        if(Time.time - lastClickedTime > maxComboDelay)
        {
            attackClick = 0;
            
        }

        switch (attackClick)
        {
            case 1:
                anim.Play("Attack1");
                break;

            case 2:
                anim.Play("Attack2");
                break;

            case 3:
                anim.Play("Attack3");
                break;
            default:
                anim.SetBool("ComboEnd",true);
                break;
        }
        attackClick = Mathf.Clamp(attackClick, 0, 3);
        
    }


    // ���� �ִϸ��̼� �̺�Ʈ �Լ�
    public void EndAttack()
    {
        _state = PlayerState.Idle;
    }

    // �뽬 ����
    void UpdateDashAttack()
    {
        anim.Play("DashAttack");
    }

    // ��ٸ� ��ũ��Ʈ���� bool�� �޾ƿ��� �Լ�
    public void SetLadderState(bool status)
    {
        _ladder = status;
    }

    // _state�� Ladder �� �� 
    void UpdateLadder()
    {
        anim.Play("Ladder");
        canMove = false;

    }

    // Ű���� �Է�
    void OnKeyboard()
    {
        // �����̴��� Ȯ��
        bool isMoving = false;

        // ��������Ʈ������ �ʱ�ȭ
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        // �̵�����
        float moveDir = 0;

        // Idle�� ���� ����
        if (!isMoving && _state != PlayerState.Jump && _state != PlayerState.Fall && _state != PlayerState.Dash && _state != PlayerState.Wallside && _state != PlayerState.Attack && _state != PlayerState.Ladder)
        {
            speed = 5f;
            _state = PlayerState.Idle;
            attackClick = 0;
        }

        // �¿� �̵�
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(canMove)
            {
                moveDir = -1f;
                sprite.flipX = true;
                isMoving = true;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if(canMove)
            {
                moveDir = 1f;
                sprite.flipX = false;
                isMoving = true;
            }
        }
        
        // �̵� ����
        if (isMoving && !isDashing)
        {
            transform.position += Vector3.right * moveDir * Time.deltaTime * speed;
            // ������ ���������� �̵��ִϸ��̼� �ȳ���
            if (_state != PlayerState.Jump && _state != PlayerState.Fall && _state != PlayerState.Attack)
            {
                _state = PlayerState.Walk;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow) && _state == PlayerState.Idle)
        {
            peekingTIme += Time.deltaTime;
            if (peekingTIme > 1f)
            {
                _state = PlayerState.Peeking;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) && _state == PlayerState.Idle)
        {
            peekingTIme += Time.deltaTime;
            if (peekingTIme > 1f)
            {
                _state = PlayerState.Peeking;
            }
        }





        // ��ٸ� �̵�
        if (Input.GetKey(KeyCode.UpArrow) && _ladder)
        {
            transform.position += Vector3.up * Time.deltaTime * speed;
            _state = PlayerState.Ladder;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && _ladder)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            _state = PlayerState.Ladder;
        }


        // ����
        if (Input.GetKeyDown(KeyCode.A) && jumpCount < maxJumpCount)
        {
            if (_state == PlayerState.Wallside)
            {
                _state = PlayerState.Jump;
                float jumpDirection = sprite.flipX ? 1f : -1f;
                rigid.velocity = new Vector2(jumpDirection * 1f, 6f);
            }
            else
            {
                _state = PlayerState.Jump;
                rigid.velocity = Vector2.up * 6;
            }
            isMoving = true;
            jumpCount++;
        }

        // �뽬
        if (Input.GetKeyDown(KeyCode.D) && !isDashing && !isCollWall)
        {
            dashTime = 0.3f;
            isDashing = true;
            _state = PlayerState.Dash;
        }

        // ����
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(isDashing)
            {
                _state = PlayerState.DashAttack;
            }
            else
            {
                attackClick++;
                lastClickedTime = Time.time;
                _state = PlayerState.Attack;
            }
        }
    }
}
