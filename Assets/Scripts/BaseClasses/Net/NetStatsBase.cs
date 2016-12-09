using Assets.Scripts.Behaviours;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.BaseClasses.Net
{
    public abstract class NetStatsBase : NetworkBehaviour,INetKillable
    {
        [SerializeField]
        private bool _isDead;
        [SerializeField]
        private int _maxHealth;
        [SerializeField]
        [SyncVar]
        private int _health;

        [SerializeField]
        private GameObject _healthBar;

        public GameObject HealthBar
        {
            get { return _healthBar; }
            set { _healthBar = value; }
        }

        [SerializeField]
        private Transform _healthBarPosition;

        private GameObject _healthBarInstance;

        public Transform HealthBarPosition
        {
            get { return _healthBarPosition; }
            set { _healthBarPosition = value; }
        }


        public bool IsDead
        {
            get { return _isDead; }
            set { _isDead = value; }
        }

        public bool CanBeDamaged { get; set; }

        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                HealthChanged.Invoke(); //RefreshHealthBar in HealthBarBehaviour;

            }
        }

        public OnHealthChanged HealthChanged { get; set; }

        

        public int MaxHealth
        {
            get { return _maxHealth; }
            set { _maxHealth = value; }
        }

        public float NextCanBeDamaged { get; set; }

        // Use this for initialization
        public virtual void Start()
        {
            _healthBarInstance = Instantiate(HealthBar);
            _healthBarInstance.GetComponent<HealthBarBehaviour>().SetContext(this);
            _healthBarInstance.GetComponent<FollowBehaviour>().Target = HealthBarPosition;
            IsDead = false;
            CanBeDamaged = true;
            Health = MaxHealth;

        }

        // Update is called once per frame
        public virtual void Update()
        {
            CheckCanBeDamage();

        }
        [Command]
        public virtual void CmdDoDamage(int damage)
        {
            if (CanBeDamaged)
            {
                Health = Health - damage;
                if (Health <= 0) CmdDie();
                CanBeDamaged = false;
            }
        }
        [Command]
        public virtual void CmdDie()
        {
            IsDead = true;
            Health = 0;
            Destroy(gameObject);
        }

        public void CheckCanBeDamage()
        {
            NextCanBeDamaged += Time.deltaTime;
            if (NextCanBeDamaged >= Global.CanbedamagedCooldownTime)
            {
                CanBeDamaged = true;
                NextCanBeDamaged = 0.0f;
            }
        }

      
    }
}
