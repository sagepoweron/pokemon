using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class Area : MonoBehaviour
    {
        public string myscene;
        public bool isloaded;
        [SerializeField] AudioClip _music;

        [SerializeField] List<Area> myconnectedareas = new();

        public void PlayMusic()
        {
            AudioManager.Instance.PlayMusic(_music, fade: true);
        }

        public void LoadConnectedAreas()
        {
            for (int i = 0; i < myconnectedareas.Count; i++)
            {
                myconnectedareas[i].LoadScene();
            }
        }
        public void LoadScene()
        {
            if (!isloaded)
            {
                isloaded = true;
                
                StartCoroutine(Co_LoadScene());
            }
        }
        

        public void UnloadScene()
        {
            if (isloaded)
            {
                isloaded = false;
                //StartCoroutine(Co_UnLoadScene());
            }
        }


        private IEnumerator Co_LoadScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(myscene, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            //Scene scene = SceneManager.GetSceneByName(myscene);
            //SceneManager.SetActiveScene(scene);
        }

        private IEnumerator Co_UnLoadScene()
        {
            AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(myscene);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
