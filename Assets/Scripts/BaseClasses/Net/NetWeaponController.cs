using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviours.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.BaseClasses.Net
{
    public abstract class NetWeaponController : NetworkBehaviour
    {
        public GameObject Body;
        [SerializeField]
        public FireModes Firemode;
        public GameObject[] Heads;
        private int _currentHeadIndex;

        public int CurrentHeadIndex
        {
            get { return _currentHeadIndex; }
            set { _currentHeadIndex = value % Heads.Length; }
        }
        public GameObject Bullet;
        public List<GameObject> Targets;
        public float BulletCooldown;
        private float _nextFire;
        public float RadarDistance;
        public SphereCollider Radar;

        public float AimSpeed;


        private Dictionary<FireModes, Action> _fireModeDictionary;
        public enum FireModes
        {
            Interleaved,
            Synchronized
        }


        public void Awake()
        {
            _fireModeDictionary = new Dictionary<FireModes, Action>
           {
                {FireModes.Synchronized, () =>
                {
                    foreach (GameObject headGameObject in Heads)
                    {
                        var clone = Instantiate(Bullet, headGameObject.transform.position, Bullet.transform.rotation) as GameObject;
                        clone.GetComponent<NetBulletBehaviour>().MyTeam = GetComponent<TeamBase>().MyTeam;
                        clone.GetComponent<NetBulletBehaviour>().Direction = Body.transform.forward;
                        NetworkServer.Spawn(clone);
                    }
                }
               },
               {
                   FireModes.Interleaved, () =>
                   {
                       var clone = Instantiate(Bullet, Heads[CurrentHeadIndex].transform.position, Bullet.transform.rotation) as GameObject;
                       clone.GetComponent<NetBulletBehaviour>().MyTeam = GetComponent<TeamBase>().MyTeam;
                       clone.GetComponent<NetBulletBehaviour>().Direction = Body.transform.forward;
                       NetworkServer.Spawn(clone);
                       CurrentHeadIndex++;
                   }
                       
               }
            };
        }

        // Use this for initialization
        public virtual void Start()
        {
            Radar.radius = RadarDistance;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            _nextFire += Time.deltaTime;
            LookForTargets();
        }

        [Command]
        public void CmdAttack()
        {
            _fireModeDictionary[Firemode].Invoke();

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

        public virtual void Attack()
        {
            CmdAttack();
            _nextFire = 0.0f;
        }

        public void RestartAim()
        {

            Body.transform.rotation = Quaternion.Slerp(Body.transform.rotation, transform.rotation, Time.deltaTime * 6.0f);
            StopAttack();
        }

        public virtual void StopAttack(){}

        public void AimTarget(Transform target)
        {
            var targetPosition = target.position;
            //targetPosition.y = 0;
            var directionVector3 = targetPosition - Body.transform.position;
            //directionVector3.y = 0;

            var lookrot = Quaternion.LookRotation(directionVector3);
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
