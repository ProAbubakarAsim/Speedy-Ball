using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class PlatformController : MonoBehaviour
    {
        [SerializeField] private PlatformType platformType = PlatformType.GREEN_PLATFORM;
        [SerializeField] private MeshRenderer meshRenderer = null;
        public GameObject[] left, right, lights;

        public PlatformType PlatformType { get { return platformType; } }
        public MeshRenderer MeshRenderer { get { return meshRenderer; } }

        private List<ObstacleController> listObstacleController = new List<ObstacleController>();

        private void Start()
        {
            int rand = Random.Range(0, left.Length);
            left[rand].SetActive(true);
            int rand1 = Random.Range(0, right.Length);
            right[rand1].SetActive(true);
            int rand2 = Random.Range(0, 2);
            lights[rand2].SetActive(true); 
        }
        private void Update()
        {
            if (transform.position.z <= PlayerController.Instance.transform.position.z - 25f && IngameManager.Instance.IngameState == IngameState.Playing)
            {
                listObstacleController.Clear();
                IngameManager.Instance.CountPassedPlatform();
                IngameManager.Instance.CreateNextPlatform(true);
                gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// Disable all obstacles of this platform.
        /// </summary>
        public void DisableAllObstacles()
        {
            foreach(ObstacleController o in listObstacleController)
            {
                o.gameObject.SetActive(false);
            }
            listObstacleController.Clear();
        }



        /// <summary>
        /// Setup this platform with PlatformParameterData.
        /// </summary>
        public void OnSetup(PlatformParameterData parameterData)
        {
            listObstacleController.Clear();
            List<PlatformPosition> listPlatformPosition = new List<PlatformPosition>();
            listPlatformPosition.Add(PlatformPosition.CENTER);
            listPlatformPosition.Add(PlatformPosition.CENTER_FORWARD);
            listPlatformPosition.Add(PlatformPosition.CENTER_BACK);
            listPlatformPosition.Add(PlatformPosition.LEFT);
            listPlatformPosition.Add(PlatformPosition.LEFT_FORWARD);
            listPlatformPosition.Add(PlatformPosition.LEFT_BACK);
            listPlatformPosition.Add(PlatformPosition.RIGHT);
            listPlatformPosition.Add(PlatformPosition.RIGHT_FORWARD);
            listPlatformPosition.Add(PlatformPosition.RIGHT_BACK);

            List<PlatformPosition> listUsedPosition = new List<PlatformPosition>();

            //Creating obstacles
            for (int i = 0; i < parameterData.ObstacleAmount; i++)
            {
                //Random a PlatformPosition for this obstacle
                PlatformPosition platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                while (listUsedPosition.Contains(platformPosition))
                {
                    platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                }
                listUsedPosition.Add(platformPosition);

                //Create an obstacle
                ObstacleController obstacleController = PoolManager.Instance.GetObstacleController(parameterData.ObstacleSize);
                obstacleController.transform.position = GetObstaclePosition(platformPosition, obstacleController.MeshRenderer);
                obstacleController.gameObject.SetActive(true);
                listObstacleController.Add(obstacleController);

                //Handle moving actions for this obstacle
                if (Random.value <= parameterData.MovingObstacleFrequency)
                {
                    float obstacleMovingSpeed = Random.Range(parameterData.MinObstacleMovingSpeed, parameterData.MaxObstacleMovingSpeed);
                    switch (platformPosition)
                    {
                        case PlatformPosition.CENTER:
                            {
                                if (Random.value <= 0.5f && !listUsedPosition.Contains(PlatformPosition.LEFT))
                                {
                                    listUsedPosition.Add(PlatformPosition.LEFT);
                                    obstacleController.HandleMovingAction(-1, obstacleMovingSpeed);
                                }
                                else if (Random.value > 0.5f && !listUsedPosition.Contains(PlatformPosition.RIGHT))
                                {
                                    listUsedPosition.Add(PlatformPosition.RIGHT);
                                    obstacleController.HandleMovingAction(1, obstacleMovingSpeed);
                                }
                                break;
                            }
                        case PlatformPosition.CENTER_FORWARD:
                            {
                                if (Random.value <= 0.5f && !listUsedPosition.Contains(PlatformPosition.LEFT_FORWARD))
                                {
                                    listUsedPosition.Add(PlatformPosition.LEFT_FORWARD);
                                    obstacleController.HandleMovingAction(-1, obstacleMovingSpeed);
                                }
                                else if (Random.value > 0.5f && !listUsedPosition.Contains(PlatformPosition.RIGHT_FORWARD))
                                {
                                    listUsedPosition.Add(PlatformPosition.RIGHT_FORWARD);
                                    obstacleController.HandleMovingAction(1, obstacleMovingSpeed);
                                }
                                break;
                            }
                        case PlatformPosition.CENTER_BACK:
                            {
                                if (Random.value <= 0.5f && !listUsedPosition.Contains(PlatformPosition.LEFT_BACK))
                                {
                                    listUsedPosition.Add(PlatformPosition.LEFT_BACK);
                                    obstacleController.HandleMovingAction(-1, obstacleMovingSpeed);
                                }
                                else if (Random.value > 0.5f && !listUsedPosition.Contains(PlatformPosition.RIGHT_BACK))
                                {
                                    listUsedPosition.Add(PlatformPosition.RIGHT_BACK);
                                    obstacleController.HandleMovingAction(1, obstacleMovingSpeed);
                                }
                                break;
                            }
                        case PlatformPosition.LEFT:
                            {
                                if (!listUsedPosition.Contains(PlatformPosition.CENTER))
                                {
                                    listUsedPosition.Add(PlatformPosition.CENTER);
                                    obstacleController.HandleMovingAction(1, obstacleMovingSpeed);
                                }
                                break;
                            }
                        case PlatformPosition.LEFT_FORWARD:
                            {
                                if (!listUsedPosition.Contains(PlatformPosition.CENTER_FORWARD))
                                {
                                    listUsedPosition.Add(PlatformPosition.CENTER_FORWARD);
                                    obstacleController.HandleMovingAction(1, obstacleMovingSpeed);
                                }
                                break;
                            }
                        case PlatformPosition.LEFT_BACK:
                            {
                                if (!listUsedPosition.Contains(PlatformPosition.CENTER_BACK))
                                {
                                    listUsedPosition.Add(PlatformPosition.CENTER_BACK);
                                    obstacleController.HandleMovingAction(1, obstacleMovingSpeed);
                                }
                                break;
                            }
                        case PlatformPosition.RIGHT:
                            {
                                if (!listUsedPosition.Contains(PlatformPosition.CENTER))
                                {
                                    listUsedPosition.Add(PlatformPosition.CENTER);
                                    obstacleController.HandleMovingAction(-1, obstacleMovingSpeed);
                                }
                                break;
                            }
                        case PlatformPosition.RIGHT_FORWARD:
                            {
                                if (!listUsedPosition.Contains(PlatformPosition.CENTER_FORWARD))
                                {
                                    listUsedPosition.Add(PlatformPosition.CENTER_FORWARD);
                                    obstacleController.HandleMovingAction(-1, obstacleMovingSpeed);
                                }
                                break;
                            }
                        case PlatformPosition.RIGHT_BACK:
                            {
                                if (!listUsedPosition.Contains(PlatformPosition.CENTER_BACK))
                                {
                                    listUsedPosition.Add(PlatformPosition.CENTER_BACK);
                                    obstacleController.HandleMovingAction(-1, obstacleMovingSpeed);
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }
            }



            //Create coins
            if (Random.value <= parameterData.CoinFrequency && listUsedPosition.Count < listPlatformPosition.Count)
            {
                //Random a PlatformPosition for these coins
                PlatformPosition platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                while (listUsedPosition.Contains(platformPosition))
                {
                    platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                }
                listUsedPosition.Add(platformPosition);

                int coinAmount = parameterData.CoinAmount;

                //Create the center coin
                ItemController centerCoin = PoolManager.Instance.GetCoinController();
                centerCoin.transform.position = GetCoinPosition(platformPosition, coinAmount);
                centerCoin.gameObject.SetActive(true);
                coinAmount--;

                int spaceCount = 1;
                while (coinAmount > 0)
                {
                    //Create a coin at forward side of center coin
                    ItemController forwardCoin = PoolManager.Instance.GetCoinController();
                    forwardCoin.transform.position = centerCoin.transform.position + Vector3.forward * spaceCount;
                    forwardCoin.gameObject.SetActive(true);

                    coinAmount--;
                    if (coinAmount == 0)
                        break;

                    //Create a coin at back side of center coin
                    ItemController backCoin = PoolManager.Instance.GetCoinController();
                    backCoin.transform.position = centerCoin.transform.position + Vector3.back * spaceCount;
                    backCoin.gameObject.SetActive(true);

                    coinAmount--;
                    if (coinAmount == 0)
                        break;

                    spaceCount++;
                }
            }


            //Create a laser
            if (Random.value <= parameterData.LaserFrequency && listUsedPosition.Count < listPlatformPosition.Count)
            {
                //Random a PlatformPosition for the shield
                PlatformPosition platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                while (listUsedPosition.Contains(platformPosition))
                {
                    platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                }
                listUsedPosition.Add(platformPosition);

                //Create the shield
                ItemController shieldController = PoolManager.Instance.GetLaserController();
                shieldController.transform.position = GetShieldPosition(platformPosition, shieldController.MeshRenderer);
                shieldController.gameObject.SetActive(true);
            }


            //Create a magnet
            if (Random.value <= parameterData.MagnetFrequency && listUsedPosition.Count < listPlatformPosition.Count)
            {
                //Random a PlatformPosition for the shield
                PlatformPosition platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                while (listUsedPosition.Contains(platformPosition))
                {
                    platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                }
                listUsedPosition.Add(platformPosition);

                //Create the magnet
                ItemController shieldController = PoolManager.Instance.GetMagnetController();
                shieldController.transform.position = GetMagnetPosition(platformPosition, shieldController.MeshRenderer);
                shieldController.gameObject.SetActive(true);
            }
            //Create a SpeedBoost
            if (Random.value <= parameterData.SpeedBoostFrequency && listUsedPosition.Count < listPlatformPosition.Count)
            {
                //Random a PlatformPosition for the shield
                PlatformPosition platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                while (listUsedPosition.Contains(platformPosition))
                {
                    platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                }
                listUsedPosition.Add(platformPosition);

                //Create the magnet
                ItemController shieldController = PoolManager.Instance.GetBoosterController();
                shieldController.transform.position = GetBoostPosition(platformPosition, shieldController.MeshRenderer);
                shieldController.gameObject.SetActive(true);
            }
            
            //Create a AdHurdle
            if (Random.value <= parameterData.SpeedBoostFrequency && listUsedPosition.Count < listPlatformPosition.Count)
            {

                //Random a PlatformPosition for the shield
                PlatformPosition platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                while (listUsedPosition.Contains(platformPosition))
                {
                    platformPosition = listPlatformPosition[Random.Range(0, listPlatformPosition.Count)];
                }
                listUsedPosition.Add(platformPosition);

                //Create the magnet
                ItemController shieldController = PoolManager.Instance.GetADController();
                shieldController.transform.position = GetAdHurdlePosition(platformPosition, shieldController.MeshRenderer);
                shieldController.gameObject.SetActive(true);
            }



        }


        /// <summary>
        /// Get the position of the coin with given PlatformPosition and the amount of coin.
        /// </summary>
        /// <param name="platformPosition"></param>
        /// <param name="coinAmount"></param>
        /// <returns></returns>
        private Vector3 GetCoinPosition(PlatformPosition platformPosition, int coinAmount)
        {
            switch (platformPosition)
            {
                case PlatformPosition.CENTER:
                    return transform.position;
                case PlatformPosition.LEFT:
                    return transform.position + Vector3.left * 2f;
                case PlatformPosition.RIGHT:
                    return transform.position + Vector3.right * 2f;

                case PlatformPosition.CENTER_FORWARD:
                    return transform.position + Vector3.forward * (meshRenderer.bounds.extents.z - Mathf.CeilToInt(coinAmount / 2f));
                case PlatformPosition.CENTER_BACK:
                    return transform.position + Vector3.back * (meshRenderer.bounds.extents.z - Mathf.CeilToInt(coinAmount / 2f));


                case PlatformPosition.LEFT_FORWARD:
                    return transform.position + (Vector3.left * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - Mathf.CeilToInt(coinAmount / 2f)));
                case PlatformPosition.LEFT_BACK:
                    return transform.position + (Vector3.left * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - Mathf.CeilToInt(coinAmount / 2f)));


                case PlatformPosition.RIGHT_FORWARD:
                    return transform.position + (Vector3.right * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - Mathf.CeilToInt(coinAmount / 2f)));
                case PlatformPosition.RIGHT_BACK:
                    return transform.position + (Vector3.right * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - Mathf.CeilToInt(coinAmount / 2f)));


                default:
                    return transform.position;
            }
        }


        /// <summary>
        /// Get the position of the shield with given PlatformPosition and MeshRenderer of that shield.
        /// </summary>
        /// <param name="platformPosition"></param>
        /// <param name="shieldMeshRenderer"></param>
        /// <returns></returns>
        private Vector3 GetShieldPosition(PlatformPosition platformPosition, MeshRenderer shieldMeshRenderer)
        {
            switch (platformPosition)
            {
                case PlatformPosition.CENTER:
                    return transform.position;
                case PlatformPosition.LEFT:
                    return transform.position + Vector3.left * 2f;
                case PlatformPosition.RIGHT:
                    return transform.position + Vector3.right * 2f;

                case PlatformPosition.CENTER_FORWARD:
                    return transform.position + Vector3.forward * (meshRenderer.bounds.extents.z - shieldMeshRenderer.bounds.size.z);
                case PlatformPosition.CENTER_BACK:
                    return transform.position + Vector3.back * (meshRenderer.bounds.extents.z - shieldMeshRenderer.bounds.size.z);


                case PlatformPosition.LEFT_FORWARD:
                    return transform.position + (Vector3.left * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - shieldMeshRenderer.bounds.size.z));
                case PlatformPosition.LEFT_BACK:
                    return transform.position + (Vector3.left * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - shieldMeshRenderer.bounds.size.z));


                case PlatformPosition.RIGHT_FORWARD:
                    return transform.position + (Vector3.right * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - shieldMeshRenderer.bounds.size.z));
                case PlatformPosition.RIGHT_BACK:
                    return transform.position + (Vector3.right * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - shieldMeshRenderer.bounds.size.z));


                default:
                    return transform.position;
            }
        }



        /// <summary>
        /// Get the position of the magnet with given PlatformPosition and MeshRenderer of that magnet.
        /// </summary>
        /// <param name="platformPosition"></param>
        /// <param name="magnetMeshRenderer"></param>
        /// <returns></returns>
        private Vector3 GetMagnetPosition(PlatformPosition platformPosition, MeshRenderer magnetMeshRenderer)
        {
            switch (platformPosition)
            {
                case PlatformPosition.CENTER:
                    return transform.position;
                case PlatformPosition.LEFT:
                    return transform.position + Vector3.left * 2f;
                case PlatformPosition.RIGHT:
                    return transform.position + Vector3.right * 2f;

                case PlatformPosition.CENTER_FORWARD:
                    return transform.position + Vector3.forward * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z);
                case PlatformPosition.CENTER_BACK:
                    return transform.position + Vector3.back * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z);


                case PlatformPosition.LEFT_FORWARD:
                    return transform.position + (Vector3.left * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));
                case PlatformPosition.LEFT_BACK:
                    return transform.position + (Vector3.left * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));


                case PlatformPosition.RIGHT_FORWARD:
                    return transform.position + (Vector3.right * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));
                case PlatformPosition.RIGHT_BACK:
                    return transform.position + (Vector3.right * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));


                default:
                    return transform.position;
            }
        }

        private Vector3 GetBoostPosition(PlatformPosition platformPosition, MeshRenderer magnetMeshRenderer)
        {
            switch (platformPosition)
            {
                case PlatformPosition.CENTER:
                    return transform.position;
                case PlatformPosition.LEFT:
                    return transform.position + Vector3.left * 2f;
                case PlatformPosition.RIGHT:
                    return transform.position + Vector3.right * 2f;

                case PlatformPosition.CENTER_FORWARD:
                    return transform.position + Vector3.forward * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z);
                case PlatformPosition.CENTER_BACK:
                    return transform.position + Vector3.back * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z);


                case PlatformPosition.LEFT_FORWARD:
                    return transform.position + (Vector3.left * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));
                case PlatformPosition.LEFT_BACK:
                    return transform.position + (Vector3.left * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));


                case PlatformPosition.RIGHT_FORWARD:
                    return transform.position + (Vector3.right * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));
                case PlatformPosition.RIGHT_BACK:
                    return transform.position + (Vector3.right * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));


                default:
                    return transform.position;
            }
        }

        private Vector3 GetAdHurdlePosition(PlatformPosition platformPosition, MeshRenderer magnetMeshRenderer)
        {
            switch (platformPosition)
            {
                case PlatformPosition.CENTER:
                    return transform.position;
                case PlatformPosition.LEFT:
                    return transform.position + Vector3.left * 2f;
                case PlatformPosition.RIGHT:
                    return transform.position + Vector3.right * 2f;

                case PlatformPosition.CENTER_FORWARD:
                    return transform.position + Vector3.forward * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z);
                case PlatformPosition.CENTER_BACK:
                    return transform.position + Vector3.back * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z);


                case PlatformPosition.LEFT_FORWARD:
                    return transform.position + (Vector3.left * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));
                case PlatformPosition.LEFT_BACK:
                    return transform.position + (Vector3.left * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));


                case PlatformPosition.RIGHT_FORWARD:
                    return transform.position + (Vector3.right * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));
                case PlatformPosition.RIGHT_BACK:
                    return transform.position + (Vector3.right * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - magnetMeshRenderer.bounds.size.z));


                default:
                    return transform.position;
            }
        }





        /// <summary>
        /// Get the position of the obstacle with given PlatformPosition and MeshRenderer of that obstacle.
        /// </summary>
        /// <param name="platformPosition"></param>
        /// <param name="obstacleMeshRenderer"></param>
        /// <returns></returns>
        private Vector3 GetObstaclePosition(PlatformPosition platformPosition, MeshRenderer obstacleMeshRenderer)
        {
            switch (platformPosition)
            {
                case PlatformPosition.CENTER:
                    return transform.position;
                case PlatformPosition.LEFT:
                    return transform.position + Vector3.left * 2f;
                case PlatformPosition.RIGHT:
                    return transform.position + Vector3.right * 2f;

                case PlatformPosition.CENTER_FORWARD:
                    return transform.position + Vector3.forward * (meshRenderer.bounds.extents.z - obstacleMeshRenderer.bounds.extents.z - 0.5f);
                case PlatformPosition.CENTER_BACK:
                    return transform.position + Vector3.back * (meshRenderer.bounds.extents.z - obstacleMeshRenderer.bounds.extents.z - 0.5f);


                case PlatformPosition.LEFT_FORWARD:
                    return transform.position + (Vector3.left * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - obstacleMeshRenderer.bounds.extents.z - 0.5f));
                case PlatformPosition.LEFT_BACK:
                    return transform.position + (Vector3.left * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - obstacleMeshRenderer.bounds.extents.z - 0.5f));


                case PlatformPosition.RIGHT_FORWARD:
                    return transform.position + (Vector3.right * 2f) + (Vector3.forward * (meshRenderer.bounds.extents.z - obstacleMeshRenderer.bounds.extents.z - 0.5f));
                case PlatformPosition.RIGHT_BACK:
                    return transform.position + (Vector3.right * 2f) + (Vector3.back * (meshRenderer.bounds.extents.z - obstacleMeshRenderer.bounds.extents.z - 0.5f));


                default:
                    return transform.position;
            }
        }
    }
}
