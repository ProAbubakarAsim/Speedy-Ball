using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{


    public class MyTutorial : MonoBehaviour
    {
        [SerializeField] private GameObject RightHand, LeftHand, MidHand;

        // Start is called before the first frame update
        void Start()
        {
            RightHand.SetActive(false);
            LeftHand.SetActive(false);
            MidHand.SetActive(false);

            if (IngameManager.Instance.LevelNumberForTutorial == 0)
            {
                StartCoroutine(StartTutorial()) ;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        
        IEnumerator StartTutorial()
        {
            yield return new WaitForSeconds(0.1f);
            RightHand.SetActive(true);
            yield return new WaitForSeconds(1f);
            RightHand.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            LeftHand.SetActive(true);
            yield return new WaitForSeconds(1f);
            LeftHand.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            MidHand.SetActive(true);
            yield return new WaitForSeconds(3f);
            MidHand.SetActive(false);
            StopCoroutine(StartTutorial()) ;
        }
    } 
}
