using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.BaseClasses;
using Assets.Scripts.Behaviours;
using UnityEngine;

namespace Assets.Scripts.TestScripts
{
    public class MancubusWeaponController : MonoBehaviour
    {

        public GameObject Body;
        public GameObject Head;
        public GameObject Bullet;
        public List<GameObject> Targets;
        public float BulletCooldown;
        private float _nextFire;
        public float RadarDistance;
        public SphereCollider Radar;
            
        public float AimSpeed;

        // Use this for initialization
        void Start()
        {
            Radar.radius = RadarDistance;
        }

        // Update is called once per frame
        void Update()
        {
            _nextFire += Time.deltaTime;
            LookForTargets();
        }

        private void LookForTargets()
        {
            if (Targets.Count <= 0)
            {
                RestartAim();
                return;
            }
            var target = Targets.FirstOrDefault();

            if (target != null)
            {
                AimTarget(target.transform);
                if (_nextFire > BulletCooldown) Attack();
            }
            else Targets.Remove(null);

        }

        private void Attack()
        {
            var clone = Instantiate(Bullet, Head.transform.position, Bullet.transform.rotation) as GameObject;
            clone.GetComponent<BulletBehaviour>().MyTeam = GetComponent<TeamBase>().MyTeam;
            clone.GetComponent<BulletBehaviour>().Direction = Body.transform.forward;
            _nextFire = 0.0f;
        }

        public void RestartAim()
        {

            Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, transform.rotation, Time.deltaTime * 6.0f);
        }

        public void AimTarget(Transform target)
        {
            var targetPosition = target.position;
            targetPosition.y = 0;
            var directionVector3 = targetPosition - transform.position;
            directionVector3.y = 0;

            var lookrot = Quaternion.LookRotation(directionVector3.normalized);
            Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, lookrot, Time.deltaTime * AimSpeed);

        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Unit" && !Targets.Contains(other.gameObject) && !other.isTrigger)
            {
                Targets.Add(other.gameObject);
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "Unit") Targets.Remove(other.gameObject);
        }

    }
}
