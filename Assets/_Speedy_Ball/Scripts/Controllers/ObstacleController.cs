using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] private ObstacleSize obstacleSize = ObstacleSize.SMALL_OBSTACLE;
        [SerializeField] private MeshRenderer meshRenderer = null;
        [SerializeField] private BoxCollider boxCollider = null;
        [SerializeField] private SpikeController[] spikeControllers = null;

        public ObstacleSize ObstacleSize { get { return obstacleSize; } }
        public MeshRenderer MeshRenderer { get { return meshRenderer; } }


        private void Update()
        {
            if (transform.position.z <= PlayerController.Instance.transform.position.z - 10f && IngameManager.Instance.IngameState == IngameState.Playing)
            {
                meshRenderer.enabled = true;
                boxCollider.enabled = true;
                transform.SetParent(null);
                gameObject.SetActive(false);
            }
        }



        /// <summary>
        /// Handle moving action for this obstacle..
        /// turn == 1: move from current x axis to right then repeat..
        /// turn == -1: move from current x axis to left then repeat..
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="movingSpeed"></param>
        public void HandleMovingAction(int turn, float movingSpeed)
        {
            StartCoroutine(CRMoving(turn, movingSpeed));
        }



        /// <summary>
        /// Moving this obstacle repeatedly with given moving speed and turn.
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="movingSpeed"></param>
        /// <returns></returns>
        private IEnumerator CRMoving(int turn, float movingSpeed)
        {
            float t = 0;
            float currentX = (float)System.Math.Round(transform.position.x, 1);
            float targetX = (turn == -1) ? (currentX - 2f) : (currentX + 2f);
            float movingTime = 2f / movingSpeed;

            while (gameObject.activeInHierarchy)
            {
                t = 0;
                while (t < movingTime)
                {
                    t += Time.deltaTime;
                    float factor = t / movingTime;
                    Vector3 newPos = transform.position;
                    newPos.x = Mathf.Lerp(currentX, targetX, factor);
                    transform.position = newPos;
                    yield return null;
                }


                t = 0;
                while (t < movingTime)
                {
                    t += Time.deltaTime;
                    float factor = t / movingTime;
                    Vector3 newPos = transform.position;
                    newPos.x = Mathf.Lerp(targetX, currentX, factor);
                    transform.position = newPos;
                    yield return null;
                }
            }
        }



        /// <summary>
        /// Explode this obstacle.
        /// </summary>
        public void OnExplode()
        {
            //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.playerExploded);
            meshRenderer.enabled = false;
            this.gameObject.SetActive(false);
            boxCollider.enabled = false;
            Texture texture; 
            if (GetComponent<MeshRenderer>())
            {
                texture = GetComponent<MeshRenderer>().material.mainTexture;
            }
            else
            {
                texture = GetComponentInChildren<MeshRenderer>().material.mainTexture;
            }
            EffectManager.Instance.CreateObstacleExplodeEffect(transform.position, meshRenderer.bounds.size.z/*, texture*/);
            foreach(SpikeController o in spikeControllers)
            {
                o.OnExplode();
            }
        }
    }
}
