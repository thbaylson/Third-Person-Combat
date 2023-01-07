using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    protected readonly int LocomotionHash = Animator.StringToHash("Locomotion");

    public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // Can the NavMesh actually get to the player
        bool navPathValid = stateMachine.Agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathInvalid;

        // Player is out of range or inaccessible, so go back to idle
        if (!IsInChaseRange() || !navPathValid)
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        //if (IsInAttackRange() && navPathValid)
        //{
        //    stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
        //    return;
        //}

        FacePlayer();
        MoveToPlayer(deltaTime);

        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

    private void MoveToPlayer(float deltaTime)
    {
        stateMachine.Agent.destination = stateMachine.Player.transform.position;
        Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

        Debug.Log($"Before Velocity Update: Controller.velocity: {stateMachine.Controller.velocity}; Agent Velocity: {stateMachine.Agent.velocity};");
        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        Debug.Log($"After Velocity Update: Controller.velocity: {stateMachine.Controller.velocity}; Agent Velocity: {stateMachine.Agent.velocity};");
    }
}
