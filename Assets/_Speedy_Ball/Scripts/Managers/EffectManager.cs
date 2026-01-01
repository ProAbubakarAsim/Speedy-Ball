using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace OnefallGames
{
    public class EffectManager : MonoBehaviour
    {

        public static EffectManager Instance { private set; get; }

        [SerializeField] private ParticleSystem collectCoinEffectPrefab = null;
        [SerializeField] private AudioSource CollectCoinSound ;

        [SerializeField] private ParticleSystem collectLaserEffectPrefab = null;
        [SerializeField] private ParticleSystem collectMagnetEffectPrefab = null;
        [SerializeField] private ParticleSystem playerExplodeEffectPrefab = null;

        [SerializeField] private ParticleSystem obstacleExplodeEffectPrefab = null;

        private List<ParticleSystem> listCollectLaserEffect = new List<ParticleSystem>();
        private List<ParticleSystem> listCollectMagnetEffect = new List<ParticleSystem>();
        private List<ParticleSystem> listCollectCoinEffect = new List<ParticleSystem>();
        private List<ParticleSystem> listPlayerExplodeEffect = new List<ParticleSystem>();
        private List<ParticleSystem> listObstacleExplodeEffect = new List<ParticleSystem>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        /// <summary>
        /// Play the given particle then disable it 
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        private IEnumerator CRPlayParticle(ParticleSystem par)
        {
            par.Play();
            yield return new WaitForSeconds(par.main.startLifetimeMultiplier);
            par.gameObject.SetActive(false);
        }


        /// <summary>
        /// Create a collect laser effect at given position.
        /// </summary>
        /// <returns></returns>
        public void CreateCollectLaserEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem collectLaserEffect = listCollectLaserEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (collectLaserEffect == null)
            {
                //Didn't find one -> create new one
                collectLaserEffect = Instantiate(collectLaserEffectPrefab, pos, Quaternion.identity);
                collectLaserEffect.gameObject.SetActive(false);
                listCollectLaserEffect.Add(collectLaserEffect);
            }

            collectLaserEffect.transform.position = pos;
            collectLaserEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(collectLaserEffect));
        }



        /// <summary>
        /// Create a collect magnet effect at given position.
        /// </summary>
        /// <returns></returns>
        public void CreateCollectMagnetEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem collectMagnetEffect = listCollectMagnetEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (collectMagnetEffect == null)
            {
                //Didn't find one -> create new one
                collectMagnetEffect = Instantiate(collectMagnetEffectPrefab, pos, Quaternion.identity);
                collectMagnetEffect.gameObject.SetActive(false);
                listCollectMagnetEffect.Add(collectMagnetEffect);
            }

            collectMagnetEffect.transform.position = pos;
            collectMagnetEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(collectMagnetEffect));
        }



        /// <summary>
        /// Create a collect coin effect at given position.
        /// </summary>
        /// <returns></returns>
        public void CreateCollectCoinEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem collectCoinEffect = listCollectCoinEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (collectCoinEffect == null)
            {
                //Didn't find one -> create new one
                collectCoinEffect = Instantiate(collectCoinEffectPrefab, pos, Quaternion.identity);
                collectCoinEffect.gameObject.SetActive(false);
                listCollectCoinEffect.Add(collectCoinEffect);
            }

            collectCoinEffect.transform.position = pos;
            collectCoinEffect.gameObject.SetActive(true);
            CollectCoinSound.Play();
            StartCoroutine(CRPlayParticle(collectCoinEffect));
        }




        /// <summary>
        /// Create a player explode effect at given position.
        /// </summary>
        /// <returns></returns>
        public void CreatePlayerExplodeEffect(Vector3 pos)
        {
            //Find in the list
            ParticleSystem playerExplodeEffect = listPlayerExplodeEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (playerExplodeEffect == null)
            {
                //Didn't find one -> create new one
                playerExplodeEffect = Instantiate(playerExplodeEffectPrefab, pos, Quaternion.identity);
                playerExplodeEffect.gameObject.SetActive(false);
                listPlayerExplodeEffect.Add(playerExplodeEffect);
            }

            playerExplodeEffect.transform.position = pos;
            playerExplodeEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(playerExplodeEffect));
        }



        /// <summary>
        /// Create a obstacle explode effect at given position.
        /// </summary>
        /// <returns></returns>
        public void CreateObstacleExplodeEffect(Vector3 pos, float zSize/*, Texture texture*/)
        {
            //Find in the list
            ParticleSystem obstacleExplodeEffect = listObstacleExplodeEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            //obstacleExplodeEffect.GetComponent<Renderer>().material.mainTexture = texture;
            if (obstacleExplodeEffect == null)
            {
                //Didn't find one -> create new one
                obstacleExplodeEffect = Instantiate(obstacleExplodeEffectPrefab, pos, Quaternion.identity);
                obstacleExplodeEffect.gameObject.SetActive(false);
                listObstacleExplodeEffect.Add(obstacleExplodeEffect);
            }

            obstacleExplodeEffect.transform.position = pos;
            var shape = obstacleExplodeEffect.shape;
            Vector3 scale = shape.scale;
            scale.y = zSize;
            shape.scale = scale;
            obstacleExplodeEffect.gameObject.SetActive(true);
            StartCoroutine(CRPlayParticle(obstacleExplodeEffect));
        }
    }
}
