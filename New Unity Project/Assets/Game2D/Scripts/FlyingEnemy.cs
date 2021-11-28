using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2D
{
    public class FlyingEnemy : MonoBehaviour
    {

        public enum State
        {
            Idle,
            Flying,
            Hit,
        }

        [SerializeField] private Animator m_Animator;
        [SerializeField] private Rigidbody2D m_Rigidbody2D;
        [SerializeField] public float m_Speed;
        [SerializeField] private int m_Hp;
        [SerializeField] public bool chase = false;
        [SerializeField] public Transform m_StartingPoint;

        private GameObject player;

        //private int m_Direction = 1;
        private int m_ChangeParamHash;
        private int m_StateParamHash;
        private State m_CurrentState;
        private Vector3 m_StartPosition;
        private bool m_GetHit;


        private void Awake()
        {

            m_ChangeParamHash = Animator.StringToHash("Change");
            m_StateParamHash = Animator.StringToHash("State");
            m_StartPosition = transform.position;

            SetState(State.Flying);
            //SetDirection(1);
            //StartCoroutine(UpdateAI());

        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void Update()
        {
            if (player == null)
                return;
            if (chase == true)
                Chase();
            else
                ReturnStartPoint();
            Flip();
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
                SetState(State.Flying);

                Vector2 knockbackDirection = transform.position - collision.transform.position;
                knockbackDirection = knockbackDirection.normalized;
                m_Rigidbody2D.AddForce(knockbackDirection * 5, ForceMode2D.Impulse);
            }
        }



        private void Chase()
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, m_Speed * Time.deltaTime);
            //if (Vector2.Distance(transform.position, player.transform.position) <= 2f)
            //{
            //   //...
            //}
            //if (m_Hp == 1)
            //{
            //    m_Speed = 5;
            //}
        }


        private void ReturnStartPoint()
        {
            transform.position = Vector2.MoveTowards(transform.position, m_StartingPoint.position, m_Speed * Time.deltaTime);
        }


        private void Flip()
        {
            if (transform.position.x > player.transform.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        private void SetState(State state)
        {
            m_CurrentState = state;

            switch (state)
            {
                case State.Idle:
                    PlayIdleAnimation();
                    break;
                case State.Flying:
                    PlayFlyingAnimation();
                    break;
                case State.Hit:
                    PlayHitAnimation();
                    break;
            }
        }
        [ContextMenu("Play Idle animation")]
        private void PlayIdleAnimation()
        {
            m_Animator.SetTrigger(m_ChangeParamHash);
            m_Animator.SetInteger(m_StateParamHash, 1);
        }



        [ContextMenu("Play Flying animation")]
        private void PlayFlyingAnimation()
        {
            m_Animator.SetTrigger(m_ChangeParamHash);
            m_Animator.SetInteger(m_StateParamHash, 2);
        }

        [ContextMenu("Play Hit animation")]
        private void PlayHitAnimation()
        {
            m_Animator.SetTrigger(m_ChangeParamHash);
            m_Animator.SetInteger(m_StateParamHash, 3);
        }
    }
}
