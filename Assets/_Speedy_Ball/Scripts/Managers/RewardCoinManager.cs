using System.Collections;
using UnityEngine;

namespace OnefallGames
{
    public class RewardCoinManager : MonoBehaviour
    {

        /// <summary>
        /// Reward an amount of coins for Total Coins with given delay time
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="delay"></param>
        public void RewardTotalCoins(int amount, float delay)
        {
            StartCoroutine(CRRewardingTotalCoins(amount, delay));
        }

        /// <summary>
        /// Reward an amount of coins for Collected Coins with given delay time
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="delay"></param>
        public void RewardCollectedCoins(int amount, float delay)
        {
            StartCoroutine(CRRewardingCollectedCoins(amount, delay));
        }


        /// <summary>
        /// Remove all collected coins.
        /// </summary>
        /// <param name="delay"></param>
        public void RemoveCollectedCoins(float delay)
        {
            StartCoroutine(CRRemovingCollectedCoins(delay));
        }


        /// <summary>
        /// Remove an amount of total coins.
        /// </summary>
        /// <param name="delay"></param>
        public void RemoveTotalCoins(int amount, float delay)
        {
            StartCoroutine(CRRemovingTotalCoins(amount, delay));
        }


        private IEnumerator CRRewardingTotalCoins(int amount, float delay)
        {
            yield return new WaitForSeconds(delay);
            //Reward coins
            float t = 0;
            float runTime = 0.5f;
            while (t < runTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
                yield return null;
            }
        }


        private IEnumerator CRRemovingTotalCoins(int amount, float delay)
        {
            yield return new WaitForSeconds(delay);

            //Remove coins
            float t = 0;
            float runTime = 0.5f;
            while (t < runTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
                yield return null;
            }
        }

        private IEnumerator CRRewardingCollectedCoins(int amount, float delay)
        {
            //Reward coins
            float t = 0;
            float runTime = 0.5f;
            while (t < runTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
                yield return null;
            }
        }


        private IEnumerator CRRemovingCollectedCoins(float delay)
        {
            yield return new WaitForSeconds(delay);
            //Remove coins
            float t = 0;
            float runTime = 0.5f;
            while (t < runTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseOutQuad, t / runTime);
                yield return null;
            }
        }
    }
}
