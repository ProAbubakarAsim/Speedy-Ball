using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace OnefallGames
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { private set; get; }

        [SerializeField] private ItemController coinControllerPrefab = null;
        [SerializeField] private Color InGameFogColor;
        [SerializeField] private ItemController laserControllerPrefab = null;
        [SerializeField] private ItemController magnetControllerPrefab = null;
        [SerializeField] private ItemController SpeedBoosterPrefab = null;




        [SerializeField] private ItemController AdHurdlePrefab = null;

        [SerializeField] private ObstacleController[] obstacleControllerPrefabs = null;
        [SerializeField] private List<PlatformConfiguration> listPlatformConfiguration = new List<PlatformConfiguration>();

        private List<ItemController> listCoinController = new List<ItemController>();
        private List<ItemController> listLaserController = new List<ItemController>();
        private List<ItemController> listMagnetController = new List<ItemController>();
        private List<ItemController> listBoostController = new List<ItemController>();
        private List<ItemController> listAdController = new List<ItemController>();



        private List<ObstacleController> listObstacleController = new List<ObstacleController>();
        private List<PlatformController> listPlatformController = new List<PlatformController>();

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

        private void Start()
        {
            RenderSettings.fogColor = InGameFogColor;
        }

        public List<PlatformController> GetActivePlatforms()
        {
            List<PlatformController> listResult = new List<PlatformController>();
            foreach(PlatformController o in listPlatformController)
            {
                if(o.gameObject.activeInHierarchy)
                {
                    listResult.Add(o);
                }
            }

            return listResult;
        }

        public ItemController GetCoinController()
        {
            //Find an inactive ItemController as the coin item
            ItemController coinController = listCoinController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (coinController == null)
            {
                //Didn't find one -> create new one
                coinController = Instantiate(coinControllerPrefab, Vector3.zero, Quaternion.identity);
                coinController.gameObject.SetActive(false);
                listCoinController.Add(coinController);
            }

            return coinController;
        }
        public ItemController GetBoosterController()
        {
            ItemController Speedboost = listBoostController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (Speedboost == null)
            {
                
                Speedboost = Instantiate(SpeedBoosterPrefab, Vector3.zero, Quaternion.identity);
                Speedboost.gameObject.SetActive(false);

                //listSpeedBoostController.Add(Speedboost);

                listBoostController.Add(Speedboost);

            }
            return Speedboost;
        }


        //public ItemController GetADController()
        //{
        //    ItemController Speedboost = listCoinController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
        //    if (Speedboost == null)
        //    {
        //        Debug.Log("Get Ad Controlleerrrrrrrrrr in Pool Manager");
        //        Speedboost = Instantiate(AdHurdlePrefab, Vector3.zero, Quaternion.identity);
        //        Speedboost.gameObject.SetActive(false);
        //        //listSpeedBoostController.Add(Speedboost);

        //        listBoostController.Add(Speedboost);

        //    }
        //    return Speedboost;
        //}
        public ItemController GetADController()
        {


            ItemController Adhurdles = listAdController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (Adhurdles == null)
            {
                Adhurdles = Instantiate(AdHurdlePrefab, Vector3.zero, Quaternion.identity);

                Adhurdles.gameObject.SetActive(false);
                //listSpeedBoostController.Add(Speedboost);
              
                listAdController.Add(Adhurdles);

            }
            return Adhurdles;
        }
        public ItemController GetLaserController()
        {
            //Find an inactive ItemController as the laser item
            ItemController laserController = listLaserController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (laserController == null)
            {
                //Didn't find one -> create new one
                laserController = Instantiate(laserControllerPrefab, Vector3.zero, Quaternion.identity);
                laserController.gameObject.SetActive(false);
                listLaserController.Add(laserController);
            }

            return laserController;
        }

        public ItemController GetMagnetController()
        {
            //Find an inactive ItemController as the magnet item
            ItemController magnetController = listMagnetController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();

            if (magnetController == null)
            {
                //Didn't find one -> create new one
                magnetController = Instantiate(magnetControllerPrefab, Vector3.zero, Quaternion.identity);
                magnetController.gameObject.SetActive(false);
                listMagnetController.Add(magnetController);
            }

            return magnetController;
        }

        public ObstacleController GetObstacleController(ObstacleSize obstacleSize)
        {
            //Find an inactive ObstacleController
            ObstacleController obstacleController = listObstacleController.Where(a => !a.gameObject.activeInHierarchy && a.ObstacleSize.Equals(obstacleSize)).FirstOrDefault();

            if (obstacleController == null)
            {
                //Didn't find one -> create new one
                ObstacleController prefab = obstacleControllerPrefabs.Where(a => a.ObstacleSize.Equals(obstacleSize)).FirstOrDefault();
                obstacleController = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                obstacleController.gameObject.SetActive(false);
                listObstacleController.Add(obstacleController);
            }

            return obstacleController;
        }

        public PlatformController GetPlatformController(PlatformType platformType)
        {
            //Find an inactive PlatformController in the list
            PlatformController platformController = listPlatformController.Where(a => !a.gameObject.activeInHierarchy && a.PlatformType == platformType).FirstOrDefault();

            if(platformController == null)
            {
                //Didn't find one -> create new one
                PlatformController prefab = listPlatformConfiguration.Where(a => a.PlatformType.Equals(platformType)).FirstOrDefault().PlatformControllerPrefab;
                platformController = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                platformController.gameObject.SetActive(false);
                listPlatformController.Add(platformController);
            }

            return platformController;
        }

        public FinishPlatformController GetFinishPlatformController(PlatformType platformType)
        {
            FinishPlatformController prefab = listPlatformConfiguration.Where(a => a.PlatformType.Equals(platformType)).FirstOrDefault().FinishPlatformControllerPrefab;
            FinishPlatformController finishPlatformController = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            finishPlatformController.gameObject.SetActive(false);
            return finishPlatformController;
        }
    }
}
