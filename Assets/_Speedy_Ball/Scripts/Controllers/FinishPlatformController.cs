using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class FinishPlatformController : MonoBehaviour
    {
        public GameObject[] left, right;
        private void Start()
        {
            int rand = Random.Range(0, left.Length);
            left[rand].SetActive(true);
            int rand1 = Random.Range(0, right.Length);
            right[rand1].SetActive(true);
        }
    }
}