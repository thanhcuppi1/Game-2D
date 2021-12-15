using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2D
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float m_moveSpeed;
        [SerializeField] private Rigidbody2D rb;
        PlayerController target;
        GameObject bullet;
        Vector2 moveDirection;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            target = GameObject.FindObjectOfType<PlayerController>();
            moveDirection = (target.transform.position - transform.position).normalized * m_moveSpeed;
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
            Destroy(gameObject,99999f);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name.Equals("Player"))
            {
                Debug.Log("Hit!");
                Instantiate(bullet, transform.position, transform.rotation);
                Destroy(gameObject,3f);
            }
        }
    }
}
