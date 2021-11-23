using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2D
{
    public class Spikes : MonoBehaviour
    {
        //[SerializeField] private float m_DmgOfSpikes;

        private Collider2D m_Collider;

        private void Start()
        {
            TryGetComponent(out m_Collider);
        }


    }
}
