using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class ChessPuzzleController : PuzzleController
    {
        
        public override void Interact(Interactor interactor)
        {
            Debug.Log("Interacting with " + interactor.gameObject);
        }
    }

}
