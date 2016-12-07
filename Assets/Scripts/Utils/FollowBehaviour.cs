using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class FollowBehaviour : MonoBehaviour
    {
        public Transform Target;
        public bool SynchronizeRotation;
        public bool DestroyIfTargetDestroyed;
        public Vector3 Offset = Vector3.zero;
        

        void Update()
        {
            if (Target != null)
            {
                transform.position = Target.position + Offset;
                if(SynchronizeRotation) transform.rotation = Target.rotation;
            }
            else
            {
                if(DestroyIfTargetDestroyed) Destroy(gameObject);
            }
        }
    }
}
