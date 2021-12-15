using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2D
{
    public class PlantController : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Hit,
            Attack,
        }

        [SerializeField] private Animator m_Animator;
        [SerializeField] private Rigidbody2D m_Rigidbody2D;
        [SerializeField] private int m_Hp;
        [SerializeField] GameObject bullet;
        public bool fire = false;

        private GameObject player;

        private int m_ChangeParamHash;
        private int m_StateParamHash;
        private State m_CurrentState;
        private bool m_GetHit;
        private float m_fireRate;
        private float m_nextFire;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            m_ChangeParamHash = Animator.StringToHash("Change");
            m_StateParamHash = Animator.StringToHash("State");
            //m_StartPosition = transform.position;

            SetState(State.Idle);
            m_fireRate = 0.3f;
            m_nextFire = 1;
        }

        private void Update()
        {
            if (player == null)
                return;
            if (fire == true)
                CheckIfTimeToFire();

        }

        private void CheckIfTimeToFire()
        {
            if (Time.time > m_nextFire)
            {
                Instantiate(bullet, transform.position, Quaternion.identity);
                m_nextFire = Time.time + m_fireRate;
            }
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_GetHit)
                return;

            if (collision.CompareTag("Attack"))
            {
                AudioManager.Instance.PlaySFX_EnemyGetHit();

                m_Hp -= 1;
                if (m_Hp <= 0)
                {
                    Destroy(gameObject);
                    GamePlayManager.Instance.EnemyDie();
                    return;
                }
                m_GetHit = true;
                SetState(State.Hit);

                //Vector2 knockbackDirection = transform.position - collision.transform.position;
                //knockbackDirection = knockbackDirection.normalized;
                //m_Rigidbody2D.AddForce(knockbackDirection * 10, ForceMode2D.Impulse);
            }
        }


        private void SetState(State state)
        {
            m_CurrentState = state;

            switch (state)
            {
                case State.Idle:
                    PlayIdleAnimation();
                    break;
                case State.Hit:
                    PlayHitAnimation();
                    break;
                case State.Attack:
                    PlayAttackAnimation();
                    break;
            }
        }

        [ContextMenu("Play Idle animation")]
        private void PlayIdleAnimation()
        {
            m_Animator.SetTrigger(m_ChangeParamHash);
            m_Animator.SetInteger(m_StateParamHash, 1);
        }
        [ContextMenu("Play Hit animation")]
        private void PlayHitAnimation()
        {
            m_Animator.SetTrigger(m_ChangeParamHash);
            m_Animator.SetInteger(m_StateParamHash, 2);
        }
        [ContextMenu("Play Attack animation")]
        private void PlayAttackAnimation()
        {
            m_Animator.SetTrigger(m_ChangeParamHash);
            m_Animator.SetInteger(m_StateParamHash, 3);
        }
    }
    
}
