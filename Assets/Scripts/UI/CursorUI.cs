using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zom.Pie.UI
{
    public class CursorUI : MonoBehaviour
    {
        public static CursorUI Instance { get; private set; }

        [SerializeField]
        Image image;

       
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                //gameObject.SetActive(false);
                Show(false); // Only for marketing
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            foreach (PuzzleController pc in PuzzleController.PuzzleControllers)
            {
                pc.OnPuzzleEnterStart += HandleOnEnterStart;
                pc.OnPuzzleExit += HandleOnExit;
                
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            
        }

        public void Show(bool value)
        {
            //hidden = !value;
            image.enabled = value;
        }

        public bool IsHidden()
        {
            return !image.enabled;
            //return hidden;
        }
        
        void HandleOnEnterStart(PuzzleController puzzleController)
        {

            image.enabled = false;
            //hidden = true; 
        }

        void HandleOnExit(PuzzleController puzzleController)
        {
            image.enabled = true;
            //hidden = false;
        }

        
    }

}
