using Assets.Scripts.BaseClasses.Net;
using UnityEngine;

namespace Assets.Scripts.TestScripts.Net
{
    public class NetTowerWeaponController : NetWeaponController
    {
        
        public override void Start()
        {
            base.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Attack()
        {
            DoAttackAnimation(CurrentHeadIndex);
            base.Attack();
        }

        public override void StopAttack()
        {
            GetComponent<Animator>().Play("CannonIdle");
            base.StopAttack();
        }

        private void DoAttackAnimation(int currentHeadIndex)
        {
            if(currentHeadIndex == 0) GetComponent<Animator>().Play("CannonAnimationLeft");
            else GetComponent<Animator>().Play("CannonAnimationRight");
        }
    }
}