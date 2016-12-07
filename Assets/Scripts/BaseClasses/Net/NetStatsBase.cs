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
                RefreshHealthBar();
            }
        }

        private void RefreshHealthBar()
        {
            if (MaxHealth == 0) return;
            float healthRelation = (float)(Health * 1.0f)/ (float)(MaxHealth * 1.0f);
            _healthBarInstance.transform.localScale = new Vector3(healthRelation, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
            var color = Color.Lerp(Color.red, Color.green, healthRelation);
            _healthBarInstance.GetComponent<SpriteRenderer>().color = color;
        }

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
            IsDead = false;
            CanBeDamaged = true;
            Health = MaxHealth;
        }

        // Update is called once per frame
        public virtual void Update()
        {
            _healthBarInstance.GetComponent<FollowBehaviour>().Target = HealthBarPosition;
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
