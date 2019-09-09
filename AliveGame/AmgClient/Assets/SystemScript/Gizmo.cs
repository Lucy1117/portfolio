using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.MyProject.MyGame
{
    public class Gizmo : MonoBehaviour
    {

        public float gizmoSize = .75f;

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, gizmoSize);
        }
    }
}
