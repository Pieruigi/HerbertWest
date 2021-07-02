using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zom.Pie.UI;

namespace Zom.Pie
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        int mainSceneIndex = 0;
        int loadingSceneIndex = 1; 
        int startingSceneIndex = 2;
        int startingPointIndex = 1;

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

        bool disableAll = false;
        public bool DisableAll
        {
            get { return disableAll; }
            set { disableAll = value; }
        }
        
        private Language language = Language.Italian; // Default: english
        public Language Language
        {
            get { return language; }
        }

        // Useful to avoid the game to save on scene enter every time we test a scene in the editor
        bool loadedByMain = false;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                SceneManager.sceneLoaded += HandleOnSceneLoaded;
                Application.targetFrameRate = 60;
                DontDestroyOnLoad(gameObject);
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
            loadedByMain = true;
            loading = true;
        }

        /// <summary>
        /// Start a new game.
        /// </summary>
        public void StartNewGame()
        {
            // Delete cache when a new game is started
            CacheManager.Instance.Delete();

            // Set the starting point
            PlayerSpawner.SpawnPointId = startingPointIndex;

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

            // Set the starting point id -1
            PlayerSpawner.SpawnPointId = -1;

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

        public void OpenInventory(bool useEnable)
        {
            if (disableAll)
                return;

            VirtualInputUI.Instance.Show(false);

            if (InventoryUI.Instance.IsOpen())
                return;

            InventoryUI.Instance.DoOpen(useEnable);
        }

        public void CloseInventory()
        {

            if (!InventoryUI.Instance.IsOpen())
                return;

            InventoryUI.Instance.DoClose();

            VirtualInputUI.Instance.Show(true);

        }

        /// <summary>
        /// This is called after the scene has been loaded.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        void HandleOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            // Reset disable all
            disableAll = false;

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

                    // Release inventory
                    //inventoryUI = null;

                    // Open main menu
                    //menuManager.Open();
                }
                else
                {
                    if(loadedByMain)
                        inGame = true;

                    // Get inventory
                    //inventoryUI = GameObject.FindObjectOfType<InventoryUI>();

                    // Close the game menu
                    //if (menuManager)
                    //    menuManager.Close();
                }
            }
        }
    }

}
