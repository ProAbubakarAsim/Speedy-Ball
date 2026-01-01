using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class IngameViewController : MonoBehaviour
    {
        [SerializeField] private PlayingViewController playingViewController = null;
        [SerializeField] private ReviveViewController reviveViewController = null;
        [SerializeField] private EndGameViewController endGameViewController = null;


        public PlayingViewController PlayingViewController { get { return playingViewController; } }
        public ReviveViewController ReviveViewController { get { return reviveViewController; } }
        public EndGameViewController EndGameViewController { get { return endGameViewController; } }

        public void OnShow()
        {
            ViewManager.Instance.loading.SetActive(false);
            IngameManager.IngameStateChanged += IngameManager_IngameStateChanged;
        }

        private void OnDisable()
        {
            IngameManager.IngameStateChanged -= IngameManager_IngameStateChanged;
        }

        private void IngameManager_IngameStateChanged(IngameState obj)
        {
            if (obj == IngameState.Revive)
            {
                StartCoroutine(CRShowReviveView());
            }
            else if (obj == IngameState.GameOver || obj == IngameState.CompletedLevel)
            {
                StartCoroutine(CRShowEndGameView());
            }
            else if (obj == IngameState.Playing)
            {
                playingViewController.gameObject.SetActive(true);
                playingViewController.OnShow();

                reviveViewController.gameObject.SetActive(false);
                endGameViewController.gameObject.SetActive(false);
            }
        }

        private IEnumerator CRShowEndGameView()
        {
            yield return new WaitForSeconds(0.25f);
            endGameViewController.gameObject.SetActive(true);
            endGameViewController.OnShow();

            reviveViewController.gameObject.SetActive(false);
            playingViewController.gameObject.SetActive(false);
        }

        private IEnumerator CRShowReviveView()
        {
            yield return new WaitForSeconds(1.25f);
            reviveViewController.gameObject.SetActive(true);
            reviveViewController.OnShow();

            playingViewController.gameObject.SetActive(false);
            endGameViewController.gameObject.SetActive(false);
        }
    }
}
