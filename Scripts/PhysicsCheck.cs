using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public bool manual;
    public CapsuleCollider2D coll;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRaduis;
    public LayerMask groundLayer;

    public bool isGround;

    public bool touchLeftWall;
    public bool touchRightWall;


    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if (!manual)
        {
            rightOffset = new Vector2(coll.bounds.size.x / 2 + coll.offset.x, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-coll.bounds.size.x / 2 + coll.offset.x, coll.bounds.size.y / 2);
        }
    }
    private void FixedUpdate()
    {
        Check();        
    }

    public void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset*transform.localScale, checkRaduis, groundLayer);

        //«ΩÃÂ≈–∂œ
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset * transform.localScale, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
