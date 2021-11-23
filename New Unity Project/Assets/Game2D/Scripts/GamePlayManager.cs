using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game2D
{
    public enum GameState
    {
        Gameplay,
        Pause,
        Gameover
    }

    public class GamePlayManager : MonoBehaviour
    {
        private static GamePlayManager m_Instance;
        public static GamePlayManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<GamePlayManager>();
                return m_Instance;
            }
        }

        public Action onEnemyDied;

        [SerializeField] private GameplayPanel m_GameplayPanel;
        [SerializeField] private PausePanel m_PausePanel;
        [SerializeField] private GameoverPanel m_GameoverPanel;
        [SerializeField] private LevelsData m_LevelsData;

        private GameState m_GameState;
        private bool m_Win;

        public LevelsData LevelsData => m_LevelsData;

        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        void Start()
        {
            m_GameplayPanel.gameObject.SetActive(false);
            m_PausePanel.gameObject.SetActive(false);
            m_GameoverPanel.gameObject.SetActive(false);
            SetState(GameState.Gameplay);
        }

        private void SetState(GameState state)
        {
            m_GameState = state;
            m_GameplayPanel.gameObject.SetActive(m_GameState == GameState.Gameplay);
            m_PausePanel.gameObject.SetActive(m_GameState == GameState.Pause);
            m_GameoverPanel.gameObject.SetActive(m_GameState == GameState.Gameover);

            if (m_GameState == GameState.Pause)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        public bool IsActive()
        {
            return m_GameState == GameState.Gameplay;
        }

        public void Play()
        {
            SetState(GameState.Gameplay);
        }

        public void Pause()
        {
            SetState(GameState.Pause);
        }

        public void Continue()
        {
            SetState(GameState.Gameplay);
        }

        public void Gameover(bool win)
        {
            m_Win = win;
            SetState(GameState.Gameover);
            m_GameoverPanel.DisplayResult(m_Win);
        }

        public void NextLevel()
        {
            string nextLevel = m_LevelsData.GetNextLevel();
            if (!string.IsNullOrEmpty(nextLevel))
                SceneManager.LoadScene(nextLevel);
            else
                Debug.LogWarning("No more level");
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void EnemyDie()
        {
            if (onEnemyDied != null)
                onEnemyDied();
        }
    }
}