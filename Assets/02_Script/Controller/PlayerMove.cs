using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    #region Enum ĳ���� ����
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
        Peeking,
        Teleport,
        Interact,
    }
    #endregion

    #region �ʱ� ����
    // �ʱ� ĳ���� ����
    public PlayerState _state = PlayerState.Idle;

    // ĳ���� ������ٵ�
    Rigidbody2D rigid;

    // ĳ���� �̵�
    public float moveSpeed = 5f;

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

    // �޺����� ����
    int attackClick = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 1.3f;

    // ī�޶� ��ŷ
    CameraMove cameraMove;
    float peekingTIme = 0f;
    float peekingDir;

     // �ڷ���Ʈ
    Teleport teleport;
    bool canTeleport = false;

    // �� �̵� ���ͷ�Ʈ
    bool canInteract = false;
    public bool pressInteract = false;

    #endregion

    void Start()
    {
        #region ���� ã��
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        cameraMove = FindObjectOfType<CameraMove>();
        teleport = FindObjectOfType<Teleport>();
        #endregion
    }

    void Update()
    {
        
        OnKeyboard();

        #region _state Switch��
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
                UpdateHit();
                break;

            case PlayerState.Die:
                UpdateDie();
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
            
            case PlayerState.Teleport:
                UpdateTeleport();
                break;
            case PlayerState.Interact:
                UpdateInteract();
                break;
        }
        #endregion

        // ���Ͻ�
        if (rigid.velocity.y < 0 && _state != PlayerState.Wallside && _state != PlayerState.Dash && _state != PlayerState.Hit)
        {
            _state = PlayerState.Fall;
            canMove = true;
        }
    }

    #region FixedUpdate() (�뽬 ���� �ƴҶ� ���� ����)
    void FixedUpdate()
    {
        // ��� ���� �ƴ� ���� ���� ��� �̵� ����
        if (!isDashing || _state != PlayerState.Teleport)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
        }
    }
    #endregion

    #region ������Ʈ ��Ʈ
    void UpdateHit()
    {
        anim.Play("Hit");
        
        StartCoroutine(StopHit());
    }

    IEnumerator StopHit()
    {
        float hitDir = GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
        float hitdis = 2;

        rigid.velocity = new Vector2(-hitDir * hitdis,2);
        yield return new WaitForSeconds(0.3f);
        _state = PlayerState.Fall;
    }
    #endregion

    #region ������Ʈ���̵�
    void UpdateIdle()
    {
        anim.Play("Idle");
        jumpCount = 0;

        if (cameraMove != null)
        {
           cameraMove.SetIsPeeking(false, 0);
        }

    }
    #endregion

    void UpdateDie()
    {
        anim.Play("Die");

    }

    #region ������Ʈ��ŷ
    void UpdatePeeking()
    {
        
        anim.Play("Idle");

        if (Input.GetKey(KeyCode.UpArrow))
        {
            peekingDir = 1;
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            peekingDir = -1;
        }
        else
        {
            _state = PlayerState.Idle;
        }

        if(cameraMove != null)
        {
            cameraMove.SetIsPeeking(true, peekingDir);
        }
    }
    #endregion

    #region ������Ʈ �̵�
    void UpdateWalk()
    {
        anim.Play("Walk");
        
    }
    #endregion

    #region ������Ʈ ����, ����, �� ����
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
    #endregion

    #region ������Ʈ �ڷ���Ʈ
    void UpdateTeleport()
    {
        anim.Play("Teleport");
        teleport.Tele();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleport"))
        {
            teleport = collision.gameObject.GetComponent<Teleport>();
            canTeleport = true;
        }

        if (collision.CompareTag("Interact"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Teleport"))
        {
            canTeleport = false;
        }

        if (collision.CompareTag("Interact"))
        {
            canInteract = false;
            pressInteract = false;
        }
    }

    void UpdateInteract()
    {
        anim.Play("Idle");
        pressInteract = true;
        _state = PlayerState.Idle;
    }



    #endregion

    #region OnCollisionStay,Exit (�� -> Idle, �� ����-> Wallside) 
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            isCollWall = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // ���� ���鿡 ��Ҵ��� Ȯ��
                if (Mathf.Abs(contact.normal.x) > 0.5f && _state != PlayerState.Teleport)
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

    // ���̶� ���� ������
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            isCollWall = false;
        }
    }
    #endregion

    #region ������Ʈ �뽬
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
            moveSpeed = dashSpeed;
            anim.Play("Dash");
            dashTime -= Time.deltaTime;

            float dashDir = GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
            Vector3 movement = Vector3.right * dashDir * Time.deltaTime * moveSpeed;

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
        moveSpeed = 5f;
        isDashing = false;

        if (rigid.velocity.y < 0)
            _state = PlayerState.Fall;
        else
            _state = PlayerState.Idle;
    }
    #endregion

    // �뽬 ����
    void UpdateDashAttack()
    {
        anim.Play("DashAttack");
    }

    #region ������Ʈ ����
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
    #endregion

    #region ������Ʈ ��ٸ�
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
    #endregion

    #region OnKeyboard() (Ű���� �Է�)
    void OnKeyboard()
    {
        if(_state != PlayerState.Die)
        {
            // �����̴��� Ȯ��
            bool isMoving = false;

            // ��������Ʈ������ �ʱ�ȭ
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();

            // �̵�����
            float moveDir = 0;

            // Idle�� ���� ����
            if (!isMoving && _state != PlayerState.Jump && _state != PlayerState.Fall && _state != PlayerState.Dash && _state != PlayerState.Wallside &&
                _state != PlayerState.Attack && _state != PlayerState.Ladder && _state != PlayerState.Teleport && _state != PlayerState.Hit && _state != PlayerState.Interact)
            {
                moveSpeed = 5f;
                _state = PlayerState.Idle;
                attackClick = 0;
            }

            if (canInteract)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    _state = PlayerState.Interact;
                }
            }

            // �¿� �̵�
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (canMove && _state != PlayerState.Hit)
                {
                    moveDir = -1f;
                    sprite.flipX = true;
                    isMoving = true;
                }
            }

            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (canMove && _state != PlayerState.Hit)
                {
                    moveDir = 1f;
                    sprite.flipX = false;
                    isMoving = true;
                }
            }

            // �̵� ����
            if (isMoving && !isDashing)
            {
                transform.position += Vector3.right * moveDir * Time.deltaTime * moveSpeed;
                // ������ ���������� �̵� �ִϸ��̼� �ȳ���
                if (_state != PlayerState.Jump && _state != PlayerState.Fall && _state != PlayerState.Attack && _state != PlayerState.Hit)
                {
                    _state = PlayerState.Walk;
                }
            }

            // �ڷ���Ʈ
            if (_state == PlayerState.Idle && canTeleport)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    _state = PlayerState.Teleport;
                }
            }

            // ī�޶� ��ŷ
            if (_state == PlayerState.Idle && _state != PlayerState.Teleport)
            {
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
                {
                    peekingTIme += Time.deltaTime;
                    if (peekingTIme > 0.5f)
                    {
                        _state = PlayerState.Peeking;
                    }
                }
            }
            else
            {
                peekingTIme = 0f;
            }

            // ��ٸ� �̵�
            if (Input.GetKey(KeyCode.UpArrow) && _ladder)
            {
                transform.position += Vector3.up * Time.deltaTime * moveSpeed;
                _state = PlayerState.Ladder;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && _ladder)
            {
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
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
                if (isDashing)
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
        
    #endregion
}
