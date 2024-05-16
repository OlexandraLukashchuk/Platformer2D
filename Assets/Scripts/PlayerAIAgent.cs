using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace Platformer
{
    public class PlayerAIAgent : Agent
    {

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


        private AgentState currentState;

        public override void OnEpisodeBegin()
        {

            Debug.Log("Position on Episode Begin: " + transform.position);
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
            Debug.Log("On Action Received...");

            // Extract actions from the neural network
            float moveX = actions.ContinuousActions[0];
            Debug.Log("Move X " + moveX);
            float moveY = actions.ContinuousActions[1];
            Debug.Log("Move y " + moveY);

            float jumpAction = Mathf.Clamp01(actions.ContinuousActions[2]);  // Clamp to be between 0 and 1

            float attackAction = Mathf.Clamp01(actions.ContinuousActions[3]);  // Clamp to be between 0 and 1


            float moveSpeed = 1f;
            transform.position += new Vector3(moveX, 0, moveY) * Time.deltaTime * moveSpeed;
            // Update state based on conditions and Animator parameters
            //UpdateState(moveX, jumpAction, attackAction);

            //// Execute actions based on the current state
            //ExecuteActions();

            //// Print the actions for debugging
            //Debug.Log($"MoveX: {moveX}");
            //Debug.Log($"MoveX: {moveY}");
            //Debug.Log($"JumpAction: {jumpAction}");

            //Debug.Log($"AttackAction: {attackAction}");
        }





        private void UpdateState(float moveX, float jumpAction, float attackAction)
        {
            Debug.Log("Update...");
            // Update state based on conditions
            if (jumpAction == 1 && heroKnight.m_grounded && !heroKnight.m_rolling)
            {
                currentState = AgentState.Jumping;
                Debug.Log("Jump...");
            }

            else if (attackAction == 1f && heroKnight.m_timeSinceAttack > 0.25f && !heroKnight.m_rolling)
            {
                currentState = AgentState.Attacking;
                Debug.Log("Attacking...");
            }
            else if (Mathf.Abs(moveX) > 0f)
            {
                currentState = AgentState.Running;
                Debug.Log("Running");
            }
            else
            {
                currentState = AgentState.Idle;
            }

            // Additional checks based on Animator parameters
            if (animator.GetBool("IdleBlock"))
            {
                currentState = AgentState.Idle; // Adjust state based on Animator parameter
                Debug.Log("Idle Block...");
            }
        }





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
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                SetReward(+1f);
                EndEpisode();
            }

        }


    }
}
