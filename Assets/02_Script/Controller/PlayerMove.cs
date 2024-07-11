using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviour
{
    // 캐릭터 상태
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
    }

    // 초기 캐릭터 상태
    PlayerState _state = PlayerState.Idle;

    // 캐릭터 이동속도
    float speed = 5f;

    // 애니메이션
    Animator anim;

    // 점프 제한
    int jumpCount;
    int maxJumpCount = 1;
    //bool isJumpping = false;

    // 대쉬 변수
    float dashTime = 0f;
    float dashSpeed = 15f;
    bool isDashing = false;

    // 벽에 닿았는지 확인 변수
    bool isCollWall = false;

    // 콤보공격 변수
    int attackClick = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 1.3f;

    Rigidbody2D rigid;


    void Start()
    {
        // 애니메이션 초기화
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
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
        }

        // 낙하시
        if (rigid.velocity.y < 0 && _state != PlayerState.Wallside && _state != PlayerState.Dash)
        {
            _state = PlayerState.Fall;
        }
    }

    void FixedUpdate()
    {
        // 대시 중이 아닐 때만 물리 기반 이동 적용
        if (!isDashing)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
        }
    }

    // 아이들
    void UpdateIdle()
    {
        anim.Play("Idle");
        jumpCount = 0;
    }

    // 이동
    void UpdateWalk()
    {
        anim.Play("Walk");
    }

    // 점프
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

    // 땅에 닿았을때 -> Idle로 변경 , 벽의 옆면에 닿았을때 -> Wallside로 변경 
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            isCollWall = false;
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 벽의 측면에 닿았는지 확인
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
                        // 대시 중 벽에 부딪힘
                        EndDash();
                    }
                    break;
                }
                else if (contact.normal.y >= 1f)
                {
                    if (_state == PlayerState.Wallside || _state == PlayerState.Fall)
                    {
                        _state = PlayerState.Idle;
                        jumpCount = 0;
                        //isJumpping = false;
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

    // 대쉬
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
            speed = dashSpeed;
            anim.Play("Dash");
            dashTime -= Time.deltaTime;

            float dashDir = GetComponent<SpriteRenderer>().flipX ? -1f : 1f;
            Vector3 movement = Vector3.right * dashDir * Time.deltaTime * speed;

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
        speed = 5f;
        isDashing = false;

        if (rigid.velocity.y < 0)
            _state = PlayerState.Fall;
        else
            _state = PlayerState.Idle;
    }

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

    public void EndAttack()
    {
        _state = PlayerState.Idle;
    }

    void UpdateDashAttack()
    {
        anim.Play("DashAttack");
    }

    // 키보드 입력
    void OnKeyboard()
    {
        // 움직이는지 확인
        bool isMoving = false;

        // 스프라이트렌더러 초기화
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        // 이동방향
        float moveDir = 0;

        // Idle로 상태 변경
        if (!isMoving && _state != PlayerState.Jump && _state != PlayerState.Fall && _state != PlayerState.Dash && _state != PlayerState.Wallside && _state != PlayerState.Attack)
        {
            speed = 5f;
            _state = PlayerState.Idle;
            attackClick = 0;
        }

        // 좌우 이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir = -1f;
            sprite.flipX = true;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir = 1f;
            sprite.flipX = false;
            isMoving = true;
        }
        
        // 이동 적용
        if (isMoving && !isDashing)
        {
            transform.position += Vector3.right * moveDir * Time.deltaTime * speed;
            // 점프나 떨어질때는 이동애니메이션 안나옴
            if (_state != PlayerState.Jump && _state != PlayerState.Fall && _state != PlayerState.Attack)
            {
                _state = PlayerState.Walk;
            }
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
            //isJumpping = true;
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
