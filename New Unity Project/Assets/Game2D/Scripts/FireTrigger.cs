using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game2D
{
    public class FireTrigger : MonoBehaviour
    {

        public PlantController Plant;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Plant.fire = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Plant.fire = false;
            }
        }

    }
}
