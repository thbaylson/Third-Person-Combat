using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    protected readonly int AttackHash = Animator.StringToHash("Attack");

    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Consider a way to set range in the same way. Different weapons have different ranges.
        stateMachine.Weapon.SetAttack(stateMachine.AttackDamage, stateMachine.AttackKnockback);

        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // If we've finished the attacking animation, then switch to the chase state
        if (GetNormalizedTime(stateMachine.Animator) >= 1)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }

        // Make sure we always look at the player before we attack.
        // This helps make sure chaining attacks doesn't lock up Enemy rotation.
        FacePlayer();
        Move(deltaTime);
    }

    public override void Exit()
    {
    }
}
