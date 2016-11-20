using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class ExtendedMonoBehaviour : MonoBehaviour
    {
        public void InvokeAction(Action action, float timeInSeconds)
        {
            StartCoroutine(ActionCoroutineInvoke(action, timeInSeconds));
        }

        private IEnumerator ActionCoroutineInvoke(Action action, float timeInSeconds)
        {
            yield return new WaitForSeconds(timeInSeconds);
            action.Invoke();
        }


        //public object InvokeFunction(Func<object[], object> function,object[] parameters, float timeInSeconds)
        //{
        //    StartCoroutine(WaitCoroutine(timeInSeconds));
        //    return FunctionCoroutineInvoke(function, parameters);
        //}

        //private IEnumerator WaitCoroutine(float timeInSeconds)
        //{
        //    yield return new WaitForSeconds(timeInSeconds);
        //}

        //private object FunctionCoroutineInvoke(Func<object[], object> function, object[] parameters)
        //{

        //    return function.Invoke(parameters);
        //}



    }

}
