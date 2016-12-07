using Assets.Scripts.Interfaces;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Behaviours.Net
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(FashionBulletBehaviour))]
    public class NetBulletBehaviour : NetworkBehaviour, INetBulletBehaviour
    {

        [SerializeField]
        private int _damage;
        public int Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        [SerializeField]
        [SyncVar]
        private int _myTeam;
        public int MyTeam
        {
            get { return _myTeam; }
            set { _myTeam = value; }
        }   

        [SerializeField]
        private float _speed;
        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        [SerializeField]
        private float _timeToLive;
        public float TimeToLive
        {
            get
            {
                return _timeToLive;
            }
            set { _timeToLive = value; }
        }

        [SerializeField]
        [SyncVar]
        private Vector3 _direction;
        public Vector3 Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                SetVelocity();
            }
        }

        private void SetVelocity()
        {
            GetComponent<Rigidbody>().velocity = Direction.normalized * Speed;
            SetOrientation();
        }

        private void SetOrientation()
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);
        }


        public delegate void MessageSender(GameObject target);

        public MessageSender Sender;

        public void SendDamage(GameObject target)
        {
            target.SendMessage("CmdDoDamage", Damage, SendMessageOptions.DontRequireReceiver);
        }

        void OnCollisionEnter(Collision Other)
        {
            Sender(Other.gameObject);
            Destroy(gameObject);
        }

        public void SetLayer()
        {
            if (MyTeam == Global.TeamId.A) gameObject.layer = Global.BulletLayerId.A;
            if (MyTeam == Global.TeamId.B) gameObject.layer = Global.BulletLayerId.B;
        }

        // Use this for initialization
        void Start()
        {
            SetVelocity();
            GetComponent<FashionBulletBehaviour>().SetMaterial(MyTeam);
            SetLayer();
            Sender += SendDamage;
            Destroy(gameObject,TimeToLive);
        }

        // Update is called once per frame
        //void Update() {}

    }
}
