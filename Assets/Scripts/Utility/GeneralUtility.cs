using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class GeneralUtility
    {

        public static void LoadScene(MonoBehaviour caller, int sceneBuildIndex, int spawnPointIndex)
        {
            caller.StartCoroutine(CoroutineLoadScene(sceneBuildIndex, spawnPointIndex));
        }


        static IEnumerator CoroutineLoadScene(int sceneBuildIndex, int spawnPointIndex)
        {
            PlayerManager.Instance.SetDisable(true);
            
            //CameraFader.Instance.TryDisableAnimator();

            //yield return CameraFader.Instance.FadeOutCoroutine(5);

            yield return new WaitForSeconds(0.1f);

            Spawner.GetSpawner(PlayerManager.Instance.transform).SpawnPointId = spawnPointIndex;

            CacheManager.Instance.Update();
            GameManager.Instance.LoadScene(sceneBuildIndex);
        }
    }

}
