using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2D
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Sprite m_OpenDoorSprite;
        [SerializeField] private Sprite m_CloseDoorSprite;

        private Collider2D m_Collider2D;
        private SpriteRenderer m_SpriteRender;

        private void Start()
        {
            TryGetComponent(out m_Collider2D);
            TryGetComponent(out m_SpriteRender);

            GamePlayManager.Instance.onEnemyDied += OnEnemyDied;

            StartCoroutine(CheckNumberOfEnemies());
        }

        private void OnDestroy()
        {
            GamePlayManager.Instance.onEnemyDied -= OnEnemyDied;
        }

        private void OnEnemyDied()
        {
            StartCoroutine(CheckNumberOfEnemies());
        }

        private IEnumerator CheckNumberOfEnemies()
        {
            yield return null;
            var allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (allEnemies.Length == 0)
                OpenDoor();
            else
                CloseDoor();
        }

        private void OpenDoor()
        {
            m_SpriteRender.sprite = m_OpenDoorSprite;
            m_Collider2D.enabled = true;
        }

        private void CloseDoor()
        {
            m_SpriteRender.sprite = m_CloseDoorSprite;
            m_Collider2D.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GamePlayManager.Instance.Gameover(true);
            }
        }
    }
}