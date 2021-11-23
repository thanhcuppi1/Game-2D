using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game2D
{
    [CreateAssetMenu(fileName = "LevelsData", menuName = "Game2D/Create Levels Data")]
    public class LevelsData : ScriptableObject
    {
        public List<string> levels;

        public string GetNextLevel()
        {
            Scene curScene = SceneManager.GetActiveScene();
            string nextLevel = "";
            for (int i = 0; i < levels.Count - 1; i++)
            {
                if (levels[i] == curScene.name)
                {
                    nextLevel = levels[i + 1];
                    break;
                }
            }
            return nextLevel;
        }
    }
}