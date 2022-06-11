using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : ActionBase
{
    private NavMeshAgent agent;
    private Transform waypoint;
    private EnemyController controller;
    private Vector3 targetPoint;
    private float targetDistance;
    private float remainingDistance;
    private float pathfindingTolerance = 1;
    private bool canFindAWay;
    private bool isInjured;


    public FollowPlayer(Transform target, NavMeshAgent agent, float targetDistance, EnemyController controller)
    {
        Name = "Follow target";
        waypoint = target;
        this.targetDistance = targetDistance;
        this.agent = agent;
        remainingDistance = targetDistance;
        this.controller = controller;
    }

    protected override void OnStart()
    {
        base.OnStart();
        CalculatePath();
        //enemyController.OnInjured += ChangeInjuryState;
    }

    protected override TaskStatus OnUpdate()
    {
        if ((targetPoint - waypoint.position).sqrMagnitude > pathfindingTolerance)
        {
            CalculatePath();
        }
        if (agent.pathPending)
        {
            return TaskStatus.Continue;
        }

        if (IsTargetWithinReach())
        {
            agent.isStopped = true;
            return TaskStatus.Success;
        }

        if (!canFindAWay)
        {
            return TaskStatus.Failure;
        }

        if (controller.isKnockedOut)
        {
            return TaskStatus.Failure;
        }
        return TaskStatus.Continue;
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    private bool IsTargetWithinReach()
    {
        return Vector3.Distance(agent.transform.position, waypoint.position) < targetDistance;
    }

    private void ChangeInjuryState()
    {
        isInjured = !isInjured;
    }

    private void CalculatePath()
    {
        if (IsTargetWithinReach()) 
        {
            agent.isStopped = true;
        }
        canFindAWay = agent.SetDestination(waypoint.position);
        targetPoint = waypoint.position;
        agent.isStopped = false;
    }
}

public static class AiActionsExtensions
{
    public static BehaviorTreeBuilder FollowPlayer(this BehaviorTreeBuilder builder, Transform target, NavMeshAgent agent, float distance, EnemyController controller)
    {
        return builder.AddNode(new FollowPlayer(target, agent, distance, controller));
    }
}
