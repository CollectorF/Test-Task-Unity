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
    private float pathfindingTolerance = 1;
    private bool canFindAWay;
    private NavMeshPath path;


    public FollowPlayer(Transform target, NavMeshAgent agent, float targetDistance, EnemyController controller)
    {
        Name = "Follow target";
        waypoint = target;
        this.targetDistance = targetDistance;
        this.agent = agent;
        this.controller = controller;
    }

    protected override void OnStart()
    {
        base.OnStart();
        CalculatePath();
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

        if (TargetIsReached())
        {
            agent.isStopped = true;
            return TaskStatus.Success;
        }

        if (canFindAWay == false)
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

    private bool TargetIsReached()
    {
        return Vector3.Distance(agent.transform.position, waypoint.position) < targetDistance;
    }

    private void CalculatePath()
    {
        path = new NavMeshPath();
        agent.CalculatePath(waypoint.position, path);
        if (path.status == NavMeshPathStatus.PathPartial)
        {
            agent.isStopped = true;
        }
        else if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.isStopped = false;
        }
        canFindAWay = agent.SetDestination(waypoint.position);
        targetPoint = waypoint.position;
        if (TargetIsReached())
        {
            agent.isStopped = true;
        }
    }
}

public static class AiActionsExtensions
{
    public static BehaviorTreeBuilder FollowPlayer(this BehaviorTreeBuilder builder, Transform target, NavMeshAgent agent, float distance, EnemyController controller)
    {
        return builder.AddNode(new FollowPlayer(target, agent, distance, controller));
    }
}
