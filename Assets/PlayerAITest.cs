using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace Platformer
{
    public class PlayerAITest : Agent
    {
        //[SerializeField] private Transform targetTransform;
        //[SerializeField] public float m_jumpForce;

        //private Vector3 initialPosition;


        //public override void OnEpisodeBegin()
        //{
        //    // This method is automatically called at the beginning of each episode.
        //    Debug.Log("Position on Episode Begin: " + transform.position);
        //    initialPosition = new Vector3(0.14f, -1.28f, 0f);
        //    transform.position = initialPosition;
        //}

        //public override void CollectObservations(VectorSensor sensor)
        //{
        //    sensor.AddObservation(transform.position);
        //    sensor.AddObservation(targetTransform.position);
        //    Debug.Log($"Number of Observations: {sensor.ObservationSize()}");
        //}

        //public override void OnActionReceived(ActionBuffers actions)
        //{
        //    float moveX = actions.ContinuousActions[0];
        //    Debug.Log("Move X " + moveX);
        //    float moveY = actions.ContinuousActions[1];
        //    Debug.Log("Move Y " + moveY);

        //    float moveSpeed = 1f;
        //    Vector3 movement = new Vector3(moveX, 0, moveY) * Time.deltaTime * moveSpeed;
        //    transform.position += movement;

        //    // Jumping logic
        //    if (actions.ContinuousActions[0] == 1 && IsGrounded())  // Assuming DiscreteActions[0] is used for jumping
        //    {
        //        float jumpForce = 5f;  // Adjust the jump force as needed
        //        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);


        //    }
        //}

        //private bool IsGrounded()
        //{

        //    float raycastDistance = 0.1f;
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance);
        //    return hit.collider != null;
        //}


        //public override void Heuristic(in ActionBuffers actionsOut)
        //{
        //    ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        //    continuousActions[0] = Input.GetAxisRaw("Horizontal");
        //    continuousActions[1] = Input.GetAxisRaw("Vertical");
        //}

        //private void OnTriggerEnter2D(Collider2D other)
        //{
        //    if (other.CompareTag("Finish"))
        //    {
        //        SetReward(+1f);
        //        EndEpisode();
        //    }
        //    //else if (other.CompareTag("Wall"))
        //    //{
        //    //    SetReward(-1f);
        //    //    EndEpisode();
        //    //}
        //    else if (other.CompareTag("Death"))
        //    {
        //        // If the character falls into a deadly trap, manually reset the position and restart the episode.
        //        transform.position = initialPosition;
        //        EndEpisode();
        //    }
        //}

        public enum AgentState
        {
            Idle,
            Running,
            Jumping,
            Attacking

        }

        private HeroKnight heroKnight; // Reference to your HeroKnight script

        private Animator animator;

        private float moveX;
        private float attackAction;
        [SerializeField] private Transform targetTransform;
        private int collectedCoins = 0;


        private AgentState currentState;

        public override void OnEpisodeBegin()
        {

            Debug.Log("Position on Episode Begin: " + transform.position);
            
            collectedCoins = 0;
        }

        public override void Initialize()
        {
            heroKnight = GetComponent<HeroKnight>();
            if (heroKnight == null)
            {
                Debug.LogError("HeroKnight component not found!");
            }

            animator = GetComponent<Animator>();
            currentState = AgentState.Idle;

            Debug.Log("AI training is in progress...");
        }


        public override void CollectObservations(VectorSensor sensor)
        {
            Debug.Log("Collecting Observations...");
            sensor.AddObservation(transform.position);
            sensor.AddObservation(targetTransform.position);
            Debug.Log($"Number of Observations: {sensor.ObservationSize()}");
        }


        public override void OnActionReceived(ActionBuffers actions)
        {
                  
            // Extract actions from the neural network
            float moveX = actions.ContinuousActions[0];
            float moveY = actions.ContinuousActions[1];
            float jumpAction = actions.ContinuousActions[2]; 
            float attackAction = actions.ContinuousActions[3]; 

            //Jumping
            if (actions.ContinuousActions[1] == 1 && IsGrounded())  
            {
                float jumpForce = 5f;  
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
            }

            float moveSpeed = 2f;
            transform.position +=  new Vector3(moveX, moveY,0) * Time.deltaTime * moveSpeed;
            
            ExecuteActions();
            
        }


        private bool IsGrounded()
        {

            float raycastDistance = 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance);
            return hit.collider != null;
        }


        //private void UpdateState(float moveX, float jumpAction, float attackAction)
        //{
        //    Debug.Log("Update...");
        //    // Update state based on conditions
        //    if (jumpAction == 1 && heroKnight.m_grounded && !heroKnight.m_rolling)
        //    {
        //        currentState = AgentState.Jumping;
        //        Debug.Log("Jump...");
        //    }

        //    else if (attackAction == 1f && heroKnight.m_timeSinceAttack > 0.25f && !heroKnight.m_rolling)
        //    {
        //        currentState = AgentState.Attacking;
        //        Debug.Log("Attacking...");
        //    }
        //    else if (Mathf.Abs(moveX) > 0f)
        //    {
        //        currentState = AgentState.Running;
        //        Debug.Log("Running");
        //    }
        //    else
        //    {
        //        currentState = AgentState.Idle;
        //    }

        //    // Additional checks based on Animator parameters
        //    if (animator.GetBool("Grounded"))
        //    {
        //        currentState = AgentState.Idle; // Adjust state based on Animator parameter
        //        Debug.Log("Idle Block...");
        //    }
        //}





        private void ExecuteActions()
        {
            Debug.Log("Execute Actions...");
            switch (currentState)
            {
                case AgentState.Idle:
                    // Implement idle actions
                    heroKnight.m_delayToIdle -= Time.deltaTime;
                    if (heroKnight.m_delayToIdle < 0)
                        heroKnight.m_animator.SetInteger("AnimState", 0);
                    break;

                case AgentState.Running:
                    // Implement moving actions
                    GetComponent<SpriteRenderer>().flipX = moveX < 0;
                    heroKnight.m_body2d.velocity =
                        new Vector2(moveX * heroKnight.m_speed, heroKnight.m_body2d.velocity.y);
                    heroKnight.m_animator.SetFloat("AirSpeedY", heroKnight.m_body2d.velocity.y);

                    // Additional logic for movement if needed
                    break;

                case AgentState.Jumping:
                    // Implement jumping actions
                    if (heroKnight.m_grounded && !heroKnight.m_rolling)
                    {
                        heroKnight.m_animator.SetTrigger("Jump");
                        heroKnight.m_grounded = false;
                        heroKnight.m_animator.SetBool("Grounded", heroKnight.m_grounded);
                        heroKnight.m_body2d.velocity =
                            new Vector2(heroKnight.m_body2d.velocity.x, heroKnight.m_jumpForce);
                        heroKnight.m_groundSensor.Disable(0.2f);
                    }

                    break;

                case AgentState.Attacking:
                    // Implement attacking actions
                    if (attackAction == 1 && heroKnight.m_timeSinceAttack > 0.25f && !heroKnight.m_rolling)
                    {
                        heroKnight.m_currentAttack++;
                        heroKnight.m_currentAttack = (heroKnight.m_currentAttack > 3) ? 1 : heroKnight.m_currentAttack;
                        heroKnight.m_currentAttack =
                            (heroKnight.m_timeSinceAttack > 1.0f) ? 1 : heroKnight.m_currentAttack;
                        heroKnight.m_animator.SetTrigger("Attack" + heroKnight.m_currentAttack);
                        heroKnight.m_timeSinceAttack = 0.0f;
                    }

                    break;


            }
        }
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
            continuousActions[0] = Input.GetAxisRaw("Horizontal");
            continuousActions[1] = Input.GetAxisRaw("Vertical");
            continuousActions[2] = Input.GetKeyDown(KeyCode.Space) ? 1f : 0f;  // Jump
            continuousActions[3] = Input.GetMouseButtonDown(0) ? 1f : 0f;       // Attack (assuming left mouse click)
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                SetReward(+1f);
                EndEpisode();
            }
            else if (other.CompareTag("Death"))
            {
                // If the character falls into a deadly trap, manually reset the position and restart the episode.
               OnEpisodeBegin();
                Debug.Log("Death ");
                EndEpisode();
            }
            else if (other.CompareTag("Coins"))
            {
                // Assuming "Coin" is the tag for coins
                SetReward(+1.0f); // Adjust the reward for collecting a coin
                collectedCoins++;

                // Optionally, you can log the number of collected coins
               // Debug.Log("Collected Coins: " + collectedCoins);

                // Deactivate the collected coin GameObject (assuming it's a 3D object)
                //other.gameObject.SetActive(true);
            }

        }


    }
}
