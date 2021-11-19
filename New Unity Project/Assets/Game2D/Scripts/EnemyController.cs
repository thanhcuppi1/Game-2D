using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2D
{
    public class EnemyController : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Walk,
            Run,
            Hit,
        }

        [SerializeField] private Animator m_Animator;
        [SerializeField] private Rigidbody2D m_Rigidbody2D;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_WalkDistance;

        private int m_ChangeParamHash;
        private int m_StateParamHash;
        private State m_CurrentState;
        private int m_Direction = 1;
        private Vector3 m_StartPosition;

        // Start is called before the first frame update
        void Start()
        {
            m_ChangeParamHash = Animator.StringToHash("Change");
            m_StateParamHash = Animator.StringToHash("State");
            m_StartPosition = transform.position;

            SetState(State.Idle);
            SetDirection(1);
            StartCoroutine(UpdateAI());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (!Application.isPlaying)
                m_StartPosition = transform.position;
            Gizmos.DrawLine(new Vector2(m_StartPosition.x - m_WalkDistance, m_StartPosition.y),
                new Vector2(m_StartPosition.x + m_WalkDistance, m_StartPosition.y));
        }

        private IEnumerator UpdateAI()
        {
            while (true)
            {
                if (m_CurrentState == State.Idle)
                {
                    yield return new WaitForSeconds(3f);
                    SetState(State.Walk);
                }
                else if (m_CurrentState == State.Walk)
                {
                    float distance = Vector2.Distance(m_StartPosition, transform.position);
                    if (distance > m_WalkDistance)
                    {
                        if (transform.position.x > m_StartPosition.x && m_Direction == 1)
                        {
                            PlayIdleAnimation();
                            yield return new WaitForSeconds(1f);
                            PlayWalkAnimation();
                            SetDirection(-1);
                        }
                        else if (transform.position.x < m_StartPosition.x && m_Direction == -1)
                        {
                            PlayIdleAnimation();
                            yield return new WaitForSeconds(1f);
                            PlayWalkAnimation();
                            SetDirection(1);
                        }
                    }
                    m_Rigidbody2D.velocity = new Vector2(m_WalkSpeed * m_Direction, m_Rigidbody2D.velocity.y);
                }
                yield return null;
            }
        }

        private void SetDirection(int direction)
        {
            m_Direction = direction;
            transform.localScale = new Vector3(-m_Direction, 1, 1);
        }

        private void SetState(State state)
        {
            m_CurrentState = state;

            switch (state)
            {
                case State.Idle:
                    PlayIdleAnimation();
                    break;
                case State.Walk:
                    PlayWalkAnimation();
                    break;
                case State.Run:
                    PlayRunAnimation();
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

        [ContextMenu("Play Walk animation")]
        private void PlayWalkAnimation()
        {
            m_Animator.SetTrigger(m_ChangeParamHash);
            m_Animator.SetInteger(m_StateParamHash, 2);
        }

        [ContextMenu("Play Run animation")]
        private void PlayRunAnimation()
        {
            m_Animator.SetTrigger(m_ChangeParamHash);
            m_Animator.SetInteger(m_StateParamHash, 3);
        }

        [ContextMenu("Play Hit animation")]
        private void PlayHitAnimation()
        {
            m_Animator.SetTrigger(m_ChangeParamHash);
            m_Animator.SetInteger(m_StateParamHash, 4);
        }
    }
}