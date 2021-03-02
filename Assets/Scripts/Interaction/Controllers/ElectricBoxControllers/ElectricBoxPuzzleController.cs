using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zom.Pie
{
    public class ElectricBoxPuzzleController : PuzzleController
    {
        [SerializeField]
        Transform fuseSpareContainer;

        [SerializeField]
        Transform nodeContainer;

        [SerializeField]
        Transform controlLightContainer;

        [SerializeField]
        MeshRenderer meshRenderer;

        List<GameObject> fuses;

        List<Transform> nodes;

        [SerializeField]
        List<int> controlLightMaterialIds;

        Dictionary<GameObject, Vector3> defaultPositions; // Fuses default positions

        // Each id represent the id of the corresponding fuse in the fuse list; the value is -1 if the fuse
        // is in its default position othervise is the id of the spot the fuse has been inserted in the box.
        List<int> placings; 

        bool interacting = false;

        GameObject selectedFuse = null;

        int rows = 3;
        int cols = 4;

        Transform powerNode;
        List<Transform> targetNodes;

        

        Material lightOffMaterial;
        Material lightOnMaterial;

        protected override void Start()
        {
            base.Start();

            // Fill the fuse list and store default position
            fuses = new List<GameObject>();
            defaultPositions = new Dictionary<GameObject, Vector3>();
            placings = new List<int>();
            for(int i=0; i<fuseSpareContainer.childCount; i++)
            {
                GameObject fuse = fuseSpareContainer.GetChild(i).gameObject;

                // Add fuse
                fuses.Add(fuse);

                // Store initial position
                defaultPositions.Add(fuse, fuse.transform.localPosition);

                // Each fuse starts from its default position ( in the spare container )
                placings.Add(-1);
            }

            // Get all the nodes
            nodes = new List<Transform>();
            for (int i = 0; i < nodeContainer.childCount; i++)
            {
                nodes.Add(nodeContainer.GetChild(i));
            }
            // Set the power node
            powerNode = nodes[0];
            targetNodes = new List<Transform>();
            targetNodes.Add(nodes[2]);
            targetNodes.Add(nodes[7]);
            targetNodes.Add(nodes[8]);
            targetNodes.Add(nodes[10]);

            // Set the control lights


            lightOffMaterial = meshRenderer.materials[controlLightMaterialIds[0]];
            lightOnMaterial = new Material(lightOffMaterial);
            lightOnMaterial.color = Color.green;
            lightOnMaterial.SetColor("_EmissionColor", Color.green * 2);

            
            

            // Check wheather the state is ready, compleated or missing.
            if (finiteStateMachine.CurrentStateId == 2)
            {
                // In missing state, check missing fuses which are the ones with the fsm attached to them.
                List<GameObject> missingFuses = fuses.FindAll(f => f.GetComponent<FiniteStateMachine>());

                // If we picked both then we set the state to ready
                if (AllFusesPicked(missingFuses))
                    finiteStateMachine.ForceState(ReadyState, false, false);
            }
            
        }

        public override void Interact(Interactor interactor)
        {
            if (interacting)
                return;

            StartCoroutine(DoInteraction(interactor));


        }

        bool AllFusesPicked(List<GameObject> missings)
        {
            foreach (GameObject fuse in missings)
                if (fuse.GetComponent<FiniteStateMachine>().CurrentStateId != 0)
                    return false;

            return true;
        }

        IEnumerator DoInteraction(Interactor interactor)
        {
            OnPuzzleInteractionStart?.Invoke(this);

            interacting = true;

            if (IsFuse(interactor.gameObject))
            {
                // We must check whether is already set or not ( is in spot or in the spare )
                if (IsInSpare(interactor.gameObject))
                {
                    Debug.LogFormat("Fuse {0} is in the spare", interactor.name);


                    if(selectedFuse == null || selectedFuse != interactor.gameObject)
                    {
                        Vector3 pos = Vector3.zero;
                        if (selectedFuse != null)
                        {
                            // We have already a fuse selected, so we must unselect it first
                            // Move it back
                            defaultPositions.TryGetValue(selectedFuse, out pos);
                            LeanTween.moveLocalZ(selectedFuse, pos.z, 0.25f);

                            // Enable collider
                            selectedFuse.GetComponent<Collider>().enabled = true;
                        }

                        // Set as selected
                        selectedFuse = interactor.gameObject;

                        // Move a little
                        defaultPositions.TryGetValue(selectedFuse, out pos);
                        LeanTween.moveLocalZ(selectedFuse, pos.z-0.02f, 0.25f);

                        // Disable interaction
                        selectedFuse.GetComponent<Collider>().enabled = false;
                    }



                }
                else
                {
                    // The fuse is already in the node, so we must move it back on the spare
                    Debug.LogFormat("Fuse {0} is in spot {1}", interactor.name, GetNodeId(interactor.gameObject));
                    // Get the fuse id
                    int fuseId = fuses.IndexOf(interactor.gameObject);

                    // Get the node 
                    Transform spot = nodes[placings[fuseId]];

                    // Reset fuse placing
                    placings[fuseId] = -1;

                    // Move the fuse back to its default position
                    interactor.transform.localPosition = defaultPositions[interactor.gameObject];

                    // Now if we have a fuse already selected we also move the fuse on the spot
                    if (selectedFuse)
                    {
                        // Get the selected fuse id
                        fuseId = fuses.IndexOf(selectedFuse);

                        // Set the fuse
                        placings[fuseId] = nodes.IndexOf(spot);

                        // Move the fuse
                        selectedFuse.transform.position = spot.position + selectedFuse.transform.up *0.0205f;

                        // Set the fuse interactable again
                        selectedFuse.GetComponent<Collider>().enabled = true;

                        // Uselect fuse
                        selectedFuse = null;

                    }
                    else
                    {
                        // No fuse selected, so set the spot interactable again
                        spot.GetComponent<Collider>().enabled = true;
                    }
                    
                }
            }
            else
            {
                // It's not a fuse, it's a spot on the electric panel
                // If we already selected a fuse then we must put it in the spot
                if (selectedFuse)
                {
                    // Get the node id
                    int spotId = nodes.IndexOf(interactor.transform);

                    // Get the selected fuse id
                    int fuseId = fuses.IndexOf(selectedFuse);

                    // Set the fuse
                    placings[fuseId] = spotId;

                    // Move the fuse
                    selectedFuse.transform.position = interactor.transform.position + selectedFuse.transform.up*0.0205f;

                    // Set the fuse interactable again
                    selectedFuse.GetComponent<Collider>().enabled = true;

                    // Uselect fuse
                    selectedFuse = null;

                    // Set node not interactable
                    interactor.GetComponent<Collider>().enabled = false;

                    
                    
                }

            }

            yield return new WaitForSeconds(0.25f);

            if (!selectedFuse && IsCompleted())
            {
                Debug.Log("IsCompleted...");
                SetStateCompleted();

                Exit();
            }

            

            interacting = false;

            OnPuzzleInteractionStop?.Invoke(this);

        }

        bool IsCompleted()
        {
            // Check if puzzle is completed
            bool notCompleted = false;
            //foreach (Transform node in targetNodes)
            for(int i=0; i<targetNodes.Count; i++)
            {
                Transform node = targetNodes[i];
               
                if (!CheckLightNode(i) || !IsPowered(node, null))
                {
                    notCompleted = true;

                    // Power off light
                    Material[] mats = meshRenderer.sharedMaterials;
                    mats[controlLightMaterialIds[i]] = lightOffMaterial;
                    meshRenderer.sharedMaterials = mats;
                }
                else
                {
                    // Power on light
                    Debug.LogFormat("Node {0} is powered", node);
                   
                    Material[] mats = meshRenderer.sharedMaterials;
                    mats[controlLightMaterialIds[i]] = lightOnMaterial;
                    meshRenderer.sharedMaterials = mats;
                }

                
            }

            return !notCompleted;
        }

        bool IsFuse(GameObject obj)
        {
            return obj.name.StartsWith("Fuse_");
        }

        int GetFuseType(GameObject fuse)
        {
            return int.Parse(fuse.name.Split('_')[1]);
        }

        bool IsInSpare(GameObject fuse)
        {
            int id = fuses.IndexOf(fuse);
            return placings[id] < 0;
        }

        int GetNodeId(GameObject fuse)
        {
            int id = fuses.IndexOf(fuse);
            return placings[id];
        }

        bool IsEmptyNode(Transform spot)
        {
            int spotId = nodes.IndexOf(spot);
            return !placings.Contains(spotId);
        }

        GameObject GetFuse(Transform spot)
        {
            // Get the spot id
            int spotId = nodes.IndexOf(spot);

            // Get the id of the corresponding fuse in the spot
            int fuseId = placings.IndexOf(spotId);
            return fuses[fuseId];
        }

        List<Transform> GetConnectedSpots(Transform spot)
        {
            List<Transform> ret = new List<Transform>();
            
            // Empty spot means no connection
            if (IsEmptyNode(spot))
                return ret;

            int fuseType = GetFuseType(GetFuse(spot));
            int spotId = nodes.IndexOf(spot);

            switch (fuseType)
            {
                case 1: // North and east
                    if (spotId >= cols) // North
                    {
                        if(!IsEmptyNode(nodes[spotId-cols]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId - cols]));
                            if(type == 2 || type == 3 || type == 5)
                                ret.Add(nodes[spotId - cols]);
                        }
                        
                    }
                        
                    if (spotId % cols < cols - 1) // East
                    {
                        if (!IsEmptyNode(nodes[spotId + 1]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId + 1]));
                            if (type == 2 || type == 4 || type == 5)
                                ret.Add(nodes[spotId + 1]);
                        }
                    }
                        
                    break;
                case 2: // South and west
                    if (spotId < (rows-1) * cols) // South
                    {
                        if (!IsEmptyNode(nodes[spotId + cols]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId + cols]));
                            if (type == 1 || type == 4 || type == 3)
                                ret.Add(nodes[spotId + cols]);
                        }
                    }
                        
                    if (spotId % cols > 0) // West
                    {
                        if (!IsEmptyNode(nodes[spotId - 1]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId - 1]));
                            if (type == 1 || type == 3 || type == 5)
                                ret.Add(nodes[spotId - 1]);
                        }
                    }
                        
                    break;
                case 3: // North, south and east
                    if (spotId >= cols) // North
                    {
                        if (!IsEmptyNode(nodes[spotId - cols]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId - cols]));
                            if (type == 2 || type == 3 || type == 5)
                                ret.Add(nodes[spotId - cols]);
                        }

                    }
                    if (spotId < (rows - 1) * cols) // South
                    {
                        if (!IsEmptyNode(nodes[spotId + cols]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId + cols]));
                            if (type == 1 || type == 4 || type == 3)
                                ret.Add(nodes[spotId + cols]);
                        }
                    }
                    if (spotId % cols < cols - 1) // East
                    {
                        if (!IsEmptyNode(nodes[spotId + 1]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId + 1]));
                            if (type == 2 || type == 4 || type == 5)
                                ret.Add(nodes[spotId + 1]);
                        }
                    }
                    break;
                case 4: // North and west
                    if (spotId >= cols) // North
                    {
                        if (!IsEmptyNode(nodes[spotId - cols]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId - cols]));
                            if (type == 2 || type == 3 || type == 5)
                                ret.Add(nodes[spotId - cols]);
                        }

                    }
                    if (spotId % cols > 0) // West
                    {
                        if (!IsEmptyNode(nodes[spotId - 1]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId - 1]));
                            if (type == 1 || type == 3 || type == 5)
                                ret.Add(nodes[spotId - 1]);
                        }
                    }
                    break;
                case 5: // South, west and east
                    if (spotId < (rows - 1) * cols) // South
                    {
                        if (!IsEmptyNode(nodes[spotId + cols]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId + cols]));
                            if (type == 1 || type == 4 || type == 3)
                                ret.Add(nodes[spotId + cols]);
                        }
                    }
                    if (spotId % cols > 0) // West
                    {
                        if (!IsEmptyNode(nodes[spotId - 1]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId - 1]));
                            if (type == 1 || type == 3 || type == 5)
                                ret.Add(nodes[spotId - 1]);
                        }
                    }
                    if (spotId % cols < cols - 1) // East
                    {
                        if (!IsEmptyNode(nodes[spotId + 1]))
                        {
                            int type = GetFuseType(GetFuse(nodes[spotId + 1]));
                            if (type == 2 || type == 4 || type == 5)
                                ret.Add(nodes[spotId + 1]);
                        }
                    }
                    break;
            }

            return ret;
        }

        bool IsPowered(Transform node, Transform parentNode)
        {
            Debug.LogFormat("Checking node {0}, parent:{1}", nodes.IndexOf(node), nodes.IndexOf(parentNode));

            if (IsEmptyNode(node))
            {
                // If the node is empty then it's not connected 
                return false;
            }
            
            if (node == powerNode)
                return true;

            // Check other connections
            List<Transform> others = GetConnectedSpots(node);
            
            // Remove the parent spot
            if(parentNode)
                others.Remove(parentNode);

            
            if (others.Count > 0)
            {
                foreach (Transform other in others)
                {
                    if (IsPowered(other, node))
                        return true;
                }
                    
            }
            
            // Not connected to any other node
            return false;
            
        }

        bool CheckLightNode(int i)
        {
            Transform node = targetNodes[i];

            if (IsEmptyNode(node))
                return false;

            if (i == 0 && ( GetFuseType(GetFuse(node)) == 2 || GetFuseType(GetFuse(node)) == 5))
            {
                
                return false;
            }
           
            if (i == 1 && (GetFuseType(GetFuse(node)) == 4 || GetFuseType(GetFuse(node)) == 2))
            {

                return false;
            }
            if (i == 2 && (GetFuseType(GetFuse(node)) == 1 || GetFuseType(GetFuse(node)) == 3))
            {

                return false;
            }
            if (i == 3 && (GetFuseType(GetFuse(node)) == 1 || GetFuseType(GetFuse(node)) == 4))
            {

                return false;
            }
            return true;
        }
    }

}
