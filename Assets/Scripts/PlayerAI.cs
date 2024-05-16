using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerAI : Agent
{
    public Transform target; // The target the AI player is moving towards
    public float moveSpeed = 2.0f; // The move speed of the AI player
    public LayerMask wallLayer;
    public Collider2D triggerCollider;
    [SerializeField] private Rigidbody2D rb;
    private float horizontal;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform isGround;
    [SerializeField] private float jumpingPower;
    private void Update()
    {
        if (triggerCollider.IsTouchingLayers(wallLayer))
        {
            Flip();
        }
        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            // Move the AI player towards the target with the specified speed
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(isGround.position, 0.2f, groundLayer);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add observations of the target's position
        sensor.AddObservation(target.position);
        // Add observations of the agent's position
        sensor.AddObservation(transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the continuous actions from the action buffers
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        // Calculate the movement vector based on the actions received
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        // Move the agent based on the calculated direction and moveSpeed
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Calculate the distance between the agent and the target
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // If the agent reaches the target, give it a positive reward and end the episode
        if (distanceToTarget < 1.0f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        // Make sure the array size matches the number of continuous actions defined
        if (continuousActions.Length == 2)  // Adjust the size based on your defined continuous actions
        {
            continuousActions[0] = Input.GetAxisRaw("Horizontal");
            continuousActions[1] = Input.GetAxisRaw("Vertical");
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        // Установите moveSpeed в его абсолютное значение (без учета направления)
        moveSpeed = Mathf.Abs(moveSpeed);
    }
}
