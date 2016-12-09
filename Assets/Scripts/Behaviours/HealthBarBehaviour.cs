using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HealthBarBehaviour : MonoBehaviour
    {

        private INetKillable _context;

        public void SetContext(INetKillable context)
        {
            _context = context;

            _context.HealthChanged += RefreshHealthBar;

        }
        private void RefreshHealthBar()
        {
            if (_context.MaxHealth == 0) return;
            float healthRelation = (float)(_context.Health * 1.0f) / (float)(_context.MaxHealth * 1.0f);
            transform.localScale = new Vector3(healthRelation, _context.HealthBar.transform.localScale.y, _context.HealthBar.transform.localScale.z);
            var color = Color.Lerp(Color.red, Color.green, healthRelation);
            GetComponent<SpriteRenderer>().color = color;
        }


    }
}
