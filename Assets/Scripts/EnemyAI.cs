
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int patrolDestination;
    public float moveSpeed;
    [SerializeField] private Rigidbody2D rigidbody;
    public Collider2D triggerCollider;
    public LayerMask ground;
    public LayerMask wall;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (patrolDestination == 0)
        {
            transform.position = UnityEngine.Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.2f)
            {

                patrolDestination = 1;
            }
        }

        if (patrolDestination == 1)
        {
            transform.position = UnityEngine.Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.2f)
            {
                patrolDestination = 0;
            }
        }
    }

    void FixedUpdate()
    {
        if (!triggerCollider.IsTouchingLayers(ground) || triggerCollider.IsTouchingLayers(wall))
        {
            if (IsGrounded())
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        moveSpeed *= -1;
    }

    private bool IsGrounded()
    {
        // Implement a simple check to see if there's ground beneath the enemy
        RaycastHit2D hit = Physics2D.Raycast(triggerCollider.bounds.center, Vector2.down, triggerCollider.bounds.extents.y + 0.1f, ground);
        return hit.collider != null;
    }

}
