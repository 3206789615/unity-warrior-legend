using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailChaseState : BaseState
{
   
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        //Debug.Log("Enter");
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        currentEnemy.anim.SetBool("isHide", true);
        Character character = currentEnemy.GetComponent<Character>();
        character.shellInvulnerable = true;
    }

    public override void LogicUpdate()
    {
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);           
        }
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.transform.localScale = new Vector3(currentEnemy.faceDir.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isHide", false);
        Character character = currentEnemy.GetComponent<Character>();
        character.shellInvulnerable = false;
    }

    
}
