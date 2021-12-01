using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2D
{
    public class ChaseControl : MonoBehaviour
    {
        public FlyingEnemy[] m_enemyArray;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                foreach (FlyingEnemy enemy in m_enemyArray)
                {
                    enemy.chase = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                foreach (FlyingEnemy enemy in m_enemyArray)
                {
                    enemy.chase = false;
                }
            }
        }
    }
}
