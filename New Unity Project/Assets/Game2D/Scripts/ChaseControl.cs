using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2D
{
    public class ChaseControl : MonoBehaviour
    {
        public FlyingEnemy m_flyingEnemy;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                 m_flyingEnemy.chase= true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                m_flyingEnemy.chase = false;
            }
        }
    }
}
