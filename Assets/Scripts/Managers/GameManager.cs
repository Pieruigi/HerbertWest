using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Zom.Pie
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        int mainSceneIndex = 0;
        int loadingSceneIndex = 1; 
        int startingSceneIndex = 2;

        bool inGame = false;
        public bool InGame
        {
            get { return inGame; }
        }
        bool loading = false;
        public bool Loading
        {
            get { return loading; }
        }

        
        private Language language = Language.Italian; // Default
        public Language Language
        {
            get { return language; }
        }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                SceneManager.sceneLoaded += HandleOnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Game is loading...
            if (loading)
                return;

            if (InGame)
            {
                // Do ingame stuff...
            }
            else
            {
                // Do not in game stuff...
            }
        }

        /// <summary>
        /// Pause game.
        /// </summary>
        public void Pause()
        {
            Time.timeScale = 0f;
        }

        /// <summary>
        /// Resume game.
        /// </summary>
        public void Unpause()
        {
            Time.timeScale = 1f;
        }

        /// <summary>
        /// Check if there is a game to load.
        /// </summary>
        public bool IsSaveGameAvailable()
        {
            return CacheManager.Instance.IsSaveGameAvailable();
        }


        /// <summary>
        /// Load the main menu.
        /// </summary>
        public void LoadMainMenu()
        {
            LoadScene(mainSceneIndex);
        }

        /// <summary>
        /// Load a scene by its build index.
        /// </summary>
        /// <param name="index"></param>
        public void LoadScene(int index)
        {

            SceneLoader.LoadingSceneIndex = index;
            SceneManager.LoadScene(index);
            loading = true;
        }

        /// <summary>
        /// Start a new game.
        /// </summary>
        public void StartNewGame()
        {
            // Delete cache when a new game is started
            CacheManager.Instance.Delete();

            // Load level
            LoadScene(startingSceneIndex);

        }

        /// <summary>
        /// Load the last save game.
        /// </summary>
        public void ContinueGame()
        {
            // Load cache first
            CacheManager.Instance.Load();

            // Get the index of the scene that must be loaded
            string index;
            if (!CacheManager.Instance.TryGetValue(Constants.CacheCodeSceneIndex, out index))
                throw new System.Exception("Save game must be corrupted: unable to find the scene to load.");

            // Load the saved level
            LoadScene(int.Parse(index));

        }

        /// <summary>
        /// If in game returns to the main menu, otherwise exits.
        /// </summary>
        public void ExitGame()
        {
            if (inGame)
                LoadMainMenu();
            else
                Application.Quit();
        }

        /// <summary>
        /// This is called after the scene has been loaded.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            // Skip the loading screen
            if (scene.buildIndex != loadingSceneIndex)
            {
                // Loading completed
                loading = false;

                //menuManager = GameObject.FindObjectOfType<HW.UI.MenuManager>();

                // Update inGame flag
                if (scene.buildIndex == mainSceneIndex) // Not in game
                {
                    inGame = false;

                    //// Release inventory
                    //inventory = null;

                    // Open main menu
                    //menuManager.Open();
                }
                else
                {
                    inGame = true;

                    // Get inventory
                    //inventory = GameObject.FindObjectOfType<InventoryUI>();

                    // Close the game menu
                    //if (menuManager)
                    //    menuManager.Close();
                }
            }
        }
    }

}
