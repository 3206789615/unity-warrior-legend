using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   protected  Rigidbody2D rb;
   [HideInInspector] public Animator anim;
   [HideInInspector] public PhysicsCheck physicsCheck;

    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    public Vector2 Dir;
    public float hurtForce;

    public Transform attacker;
    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    public float lostTime;
    public float lostTimeCounter;


    public bool isHurt;
    public bool isDead;

    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;

    public Vector2 centerOffset;
    public Vector2 checkSize;
    public float checkDistance;
    public LayerMask attackLayer;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        //waitTimeCounter = waitTime;
    }

    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }
    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);        
        currentState.LogicUpdate();
        TimeCounter();

    }

    private void FixedUpdate()
    {
        if (!isHurt&&!isDead&&!wait)
            Move();
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }
    public virtual void Move()
    {       
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, 0);
    }

    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter<=0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);               

            }
        }

        if (!FoundPlayer()&&lostTimeCounter>0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        else if(FoundPlayer())
        {
            lostTimeCounter = lostTime;
        }
    }

    public virtual bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, checkSize, 0, faceDir, checkDistance, attackLayer);
    }

    public void SwitchState(NPCState state)
    {
        var newState = state switch
        {
            NPCState.Patrol => patrolState,
            NPCState.Chase=>chaseState,
            _=>null
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter(this);
    }

    public virtual void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;

        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);

        isHurt = true;
        anim.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }

    protected IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        isHurt = false;
    }

    public void OnDie()
    {
        gameObject.layer = 2;
        anim.SetBool("Dead", true);
        isDead = true;
    }

    public void DestoryAfterAnimation()
    {
        Destroy(this.gameObject);
    }
   
    private void OnDrawGizmosSelected()
    {
        // 根据当前缩放计算朝向
        Vector2 direction = new Vector2(-transform.localScale.x, 0f);

        if (direction.x == 0)
            direction = Vector2.left;

        // 让检测偏移跟随蜗牛朝向
        Vector2 realOffset = new Vector2(
            Mathf.Abs(centerOffset.x) * direction.x,
            centerOffset.y
        );

        // BoxCast 的起点
        Vector2 startPosition = (Vector2)transform.position + realOffset;

        // BoxCast 的终点
        Vector2 endPosition = startPosition + direction * checkDistance;

        // BoxCast 扫描区域的中心
        Vector2 castCenter = (startPosition + endPosition) / 2f;

        // 整个扫描区域的尺寸
        Vector2 castSize = new Vector2(
            checkSize.x + checkDistance,
            checkSize.y
        );

        Gizmos.color = Color.red;

        // 绘制 BoxCast 的整体范围
        Gizmos.DrawWireCube(castCenter, castSize);

        // 绘制检测盒起点
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(startPosition, checkSize);

        // 绘制检测盒终点
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(endPosition, checkSize);

        // 画出检测方向
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPosition, endPosition);
    }
}
