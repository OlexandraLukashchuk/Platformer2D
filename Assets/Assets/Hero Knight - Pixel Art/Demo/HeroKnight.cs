﻿using UnityEngine;
using System.Collections;

namespace Platformer
{

    public class HeroKnight : MonoBehaviour
    {

        [SerializeField] public float m_speed;
        [SerializeField] public float m_jumpForce;
        [SerializeField] float m_rollForce;
        [SerializeField] bool m_noBlood = false;
        [SerializeField] GameObject m_slideDust;
        [SerializeField] private GameObject PauseCanvas;
        [SerializeField] private GameObject FinishCanvas;
        [SerializeField] private GameObject Target;


        public Animator m_animator;
        public Rigidbody2D m_body2d;
        public Sensor_HeroKnight m_groundSensor;
        public Sensor_HeroKnight m_wallSensorR1;
        public Sensor_HeroKnight m_wallSensorR2;
        public Sensor_HeroKnight m_wallSensorL1;
        public Sensor_HeroKnight m_wallSensorL2;

        public PlayerHealth die;

        public bool m_isWallSliding = false;
        public bool m_grounded = false;
        public bool m_rolling = false;

        public bool isFinish = false;

        public int m_facingDirection = 1;
        public int m_currentAttack = 0;
        public float m_timeSinceAttack = 0.0f;
        public float m_delayToIdle = 0.0f;
        public float m_rollDuration = 8.0f / 14.0f;
        public float m_rollCurrentTime;


        // Use this for initialization
        void Start()
        {
            die = GetComponent<PlayerHealth>();

            m_animator = GetComponent<Animator>();

            m_body2d = GetComponent<Rigidbody2D>();
            m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
            m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
            m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
            m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
            m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        }

        // Update is called once per frame
        void Update()
        {
            // Increase timer that controls attack combo
            m_timeSinceAttack += Time.deltaTime;

            // Increase timer that checks roll duration
            if (m_rolling)
                m_rollCurrentTime += Time.deltaTime;

            // Disable rolling if timer extends duration
            if (m_rollCurrentTime > m_rollDuration)
                m_rolling = false;

            //Check if character just landed on the ground
            if (!m_grounded && m_groundSensor.State())
            {
                m_grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
            }

            //Check if character just started falling
            if (m_grounded && !m_groundSensor.State())
            {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
            }

            // -- Handle input and movement --
            float inputX = Input.GetAxis("Horizontal");

            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                m_facingDirection = 1;
            }

            else if (inputX < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                m_facingDirection = -1;
            }

            // Move
            if (!m_rolling)
            {
                m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

                //Set AirSpeed in animator
                m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

                // -- Handle Animations --
                //Wall Slide
                m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
                m_animator.SetBool("WallSlide", m_isWallSliding);
            }

            switch (true) {
                //Attack
                case var _ when Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling:
                    m_currentAttack++;

                    // Loop back to one after third attack
                    if (m_currentAttack > 3)
                        m_currentAttack = 1;

                    // Reset Attack combo if time since last attack is too large
                    if (m_timeSinceAttack > 1.0f)
                        m_currentAttack = 1;

                    // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                    m_animator.SetTrigger("Attack" + m_currentAttack);

                    // Reset timer
                    m_timeSinceAttack = 0.0f;
                    break;

                // Block
                case var _ when Input.GetMouseButtonDown(1) && !m_rolling:
                    m_animator.SetTrigger("Block");
                    m_animator.SetBool("IdleBlock", true);
                    break;



                case var _ when Input.GetMouseButtonUp(1):
                    m_animator.SetBool("IdleBlock", false);
                    break;

                // Roll
                case var _ when Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding:
                    m_rolling = true;
                    m_animator.SetTrigger("Roll");
                    m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
                    break;

                //Open the Pause Menu
                case var _ when Input.GetKeyDown("escape"):
                    PauseCanvas.SetActive(true);
                    break;

                //Jump
                case var _ when Input.GetKeyDown("space") && m_grounded && !m_rolling:
                    m_animator.SetTrigger("Jump");
                    m_grounded = false;
                    m_animator.SetBool("Grounded", m_grounded);
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                    m_groundSensor.Disable(0.2f);
                    break;

                //Run
                case var _ when Mathf.Abs(inputX) > Mathf.Epsilon:
                    // Reset timer
                    m_delayToIdle = 0.05f;
                    m_animator.SetInteger("AnimState", 1);
                    break;


                //Idle
                default:
                    // Prevents flickering transitions to idle
                    m_delayToIdle -= Time.deltaTime;
                    if (m_delayToIdle < 0)
                        m_animator.SetInteger("AnimState", 0);
                    break;
            }

            //Finish Level
            if (isFinish)
            {
                FinishCanvas.SetActive(true);
                Target.SetActive(false);
            }
        }

        // Animation Events
        // Called in slide animation.
        void AE_SlideDust()
        {
            Vector3 spawnPosition;

            if (m_facingDirection == 1)
                spawnPosition = m_wallSensorR2.transform.position;
            else
                spawnPosition = m_wallSensorL2.transform.position;

            if (m_slideDust != null)
            {
                // Set correct arrow spawn position
                GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
                // Turn arrow in correct direction
                dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Coins"))
            {
                Coin coin = collision.gameObject.GetComponent<Coin>();

                if (coin != null)
                {
                    // Call ChangeScore with the coinValue from the collected coin
                    ScoreManager.instance.ChangeScore(coin.coinValue);
                    Debug.Log("Collected Coin! Current Score: " + ScoreManager.instance.CurrentScore);
                }

                Destroy(collision.gameObject);
            }
            //25.01.2024
            //Finish Event Trigger

            else if (collision.CompareTag("Finish"))
            {
                Debug.Log("Worked");
                isFinish = true;
            }

            else if (collision.CompareTag("Trap"))
            {
                Debug.Log("Die, potato!");
                die.Die();
            }
        }
    }
}