using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OvenFresh
{
    /*
     * This dual grid is the home for both of the underlying grids.
     * It is what will track both of the grids and also keep track of where the mover is
     * Between the two
     * The two grids themselves have no other control
     */
    public class DualGrid : MonoBehaviour
    {
        public GameObject gridPrefab;
        public GameObject moverPrefab;

        public DualGridConfiguration dualConfig;

        
    }
}
