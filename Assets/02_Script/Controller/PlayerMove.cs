using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    #region Enum 캐릭터 상태
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

    #region 초기 변수
    // 초기 캐릭터 상태
    public PlayerState _state = PlayerState.Idle;

    // 캐릭터 리지드바디
    Rigidbody2D rigid;

    // 캐릭터 이동
    public float moveSpeed = 5f;

    // 애니메이션
    Animator anim;

    // 점프 제한
    int jumpCount;
    int maxJumpCount = 1;
    
    // 대쉬 변수
    float dashTime = 0f;
    float dashSpeed = 15f;
    bool isDashing = false;

    // 대쉬 중 벽에 닿았는지 확인
    bool isCollWall = false;

    // 사다리 변수
    bool _ladder;
    bool canMove = true;

    // 콤보공격 변수
    int attackClick = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 1.3f;

    // 카메라 피킹
    CameraMove cameraMove;
    float peekingTIme = 0f;
    float peekingDir;

     // 텔레포트
    Teleport teleport;
    bool canTeleport = false;

    // 문 이동 인터렉트
    bool canInteract = false;
    public bool pressInteract = false;

    #endregion

    void Start()
    {
        #region 변수 찾기
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        cameraMove = FindObjectOfType<CameraMove>();
        teleport = FindObjectOfType<Teleport>();
        #endregion
    }

    void Update()
    {
        
        OnKeyboard();

        #region _state Switch문
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

        // 낙하시
        if (rigid.velocity.y < 0 && _state != PlayerState.Wallside && _state != PlayerState.Dash && _state != PlayerState.Hit)
        {
            _state = PlayerState.Fall;
            canMove = true;
        }
    }

    #region FixedUpdate() (대쉬 중이 아닐때 물리 적용)
    void FixedUpdate()
    {
        // 대시 중이 아닐 때만 물리 기반 이동 적용
        if (!isDashing || _state != PlayerState.Teleport)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
        }
    }
    #endregion

    #region 업데이트 히트
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

    #region 업데이트아이들
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

    #region 업데이트피킹
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

    #region 업데이트 이동
    void UpdateWalk()
    {
        anim.Play("Walk");
        
    }
    #endregion

    #region 업데이트 점프, 낙하, 벽 옆면
    void UpdateJump()
    {
        anim.Play("Jump");
        if (rigid.velocity.y < 0)
        {
            // 점프 후에는 Fall 상태로 전환
            _state = PlayerState.Fall;
        }
        isDashing = false;
    }

    // 떨어질 때
    void UpdateFall()
    {
        anim.Play("Fall");
    }

    // 벽 측면에 붙었을 때
    void UpdateWallside()
    {
        anim.Play("Wallside");

    }
    #endregion

    #region 업데이트 텔레포트
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

    #region OnCollisionStay,Exit (땅 -> Idle, 벽 옆면-> Wallside) 
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            isCollWall = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 벽의 측면에 닿았는지 확인
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
                        // 대시 중 벽에 부딪힘
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

    // 맵이랑 닿지 않을때
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            isCollWall = false;
        }
    }
    #endregion

    #region 업데이트 대쉬
    void UpdateDash()
    {
        // 시간 지나거나 벽에 부딪히면 대쉬 끝
        if (dashTime <= 0f || isCollWall)
        {
            EndDash();
        }
        else
        {
            // 대쉬 스피드
            moveSpeed = dashSpeed;
            anim.Play("Dash");
            dashTime -= Time.deltaTime;

            float dashDir = GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
            Vector3 movement = Vector3.right * dashDir * Time.deltaTime * moveSpeed;

            // 벽과 충돌 체크
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dashDir, 0), movement.magnitude, LayerMask.GetMask("Map"));
            if (hit.collider != null)
            {
                // 벽과 충돌했을 경우 이동을 제한
                transform.position = hit.point;
                EndDash();
            }
            else
            {
                // 충돌하지 않았을 경우 정상 이동
                transform.position += movement;
            }
        }
    }

    // 대쉬 끝
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

    // 대쉬 어택
    void UpdateDashAttack()
    {
        anim.Play("DashAttack");
    }

    #region 업데이트 어택
    // 공격
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


    // 공격 애니메이션 이벤트 함수
    public void EndAttack()
    {
        _state = PlayerState.Idle;
    }
    #endregion

    #region 업데이트 사다리
    // 사다리 스크립트에서 bool값 받아오는 함수
    public void SetLadderState(bool status)
    {
        _ladder = status;
    }

    // _state가 Ladder 일 때 
    void UpdateLadder()
    {
        anim.Play("Ladder");
        canMove = false;

    }
    #endregion

    #region OnKeyboard() (키보드 입력)
    void OnKeyboard()
    {
        if(_state != PlayerState.Die)
        {
            // 움직이는지 확인
            bool isMoving = false;

            // 스프라이트렌더러 초기화
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();

            // 이동방향
            float moveDir = 0;

            // Idle로 상태 변경
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

            // 좌우 이동
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

            // 이동 적용
            if (isMoving && !isDashing)
            {
                transform.position += Vector3.right * moveDir * Time.deltaTime * moveSpeed;
                // 점프나 떨어질때는 이동 애니메이션 안나옴
                if (_state != PlayerState.Jump && _state != PlayerState.Fall && _state != PlayerState.Attack && _state != PlayerState.Hit)
                {
                    _state = PlayerState.Walk;
                }
            }

            // 텔레포트
            if (_state == PlayerState.Idle && canTeleport)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    _state = PlayerState.Teleport;
                }
            }

            // 카메라 피킹
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

            // 사다리 이동
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


            // 점프
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

            // 대쉬
            if (Input.GetKeyDown(KeyCode.D) && !isDashing && !isCollWall)
            {
                dashTime = 0.3f;
                isDashing = true;
                _state = PlayerState.Dash;
            }

            // 공격
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
