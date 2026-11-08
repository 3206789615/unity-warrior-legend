using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SceneLoadEventSO loadEvent;
    public VoidEvent afterSceneLoadedEvent;

    public PlayerInputControl InputControl;
    public Vector2 inputDirection;
    public float speed;
    private Rigidbody2D rb;
    public float jumpForce;
    private PhysicsCheck physicsCheck;

    public PlayerAnimation playerAnimation;

    public CapsuleCollider2D coll;
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    public float hurtForce;
    public bool isHurt;
    public bool isAttack;
    public bool isDead;
    private void Awake()
    {        
        InputControl = new PlayerInputControl();
        InputControl.GamePlay.Jump.started += Jump;
        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        InputControl.GamePlay.Attack.started += PlayerAttack;
    }

    

    private void OnEnable()
    {
        InputControl.Enable();
        loadEvent.LoadRequestEvent += OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        InputControl.Disable();
        loadEvent.LoadRequestEvent -= OnLoadEvent;
        afterSceneLoadedEvent.OnEventRaised -= OnAfterSceneLoadedEvent;

    }

    

    private void Update()
    {
        inputDirection = InputControl.GamePlay.Move.ReadValue<Vector2>();
        CheckState();
    }

    private void FixedUpdate()
    {
        if(!isHurt&&!isAttack)
            Move();
    }

    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        InputControl.GamePlay.Disable();
    }

    private void OnAfterSceneLoadedEvent()
    {
        InputControl.GamePlay.Enable();
    }
    public void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        playerAnimation.PlayerAttack();
        isAttack = true;
    }

    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        InputControl.GamePlay.Disable();
    }

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }

}
