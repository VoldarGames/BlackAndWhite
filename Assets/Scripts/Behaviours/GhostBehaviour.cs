using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    public class GhostBehaviour : MonoBehaviour
    {

        public Vector3 TargetPosition;

        void Start()
        {
            TargetPosition = Vector3.zero;
        }
        // Update is called once per frame
        void Update ()
        {
            var newPosition = TargetPosition;
            newPosition.y = transform.localScale.y/2;
            transform.position = newPosition;

        }
    }
}
