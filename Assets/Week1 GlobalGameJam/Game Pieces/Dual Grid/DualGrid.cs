using System;
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

        public DualGridConfiguration dualConfig;
        public GameObject gridXY;
        public GameObject gridZY;

        void Awake()
        {
            //CenterGrids();
        }

        private void CenterGrids()
        {
            gridXY.transform.position = new Vector3(-(float)(dualConfig.configXY.width-1)*.5f,- (float)(dualConfig.configXY.height-1)*.5f,0f);
            gridZY.transform.position = new Vector3(0f,-(float)(dualConfig.configZY.height-1)*.5f,(float) (dualConfig.configZY.width-1)*.5f);
        }
    }
}
