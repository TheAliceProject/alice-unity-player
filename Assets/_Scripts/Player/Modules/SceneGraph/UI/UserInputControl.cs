using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alice.Tweedle.Interop;
using Alice.Player.Unity;
using BeauRoutine;

namespace Alice.Player.Unity {

    public class UserInputControl : MonoBehaviour
    {
        [SerializeField]
        private UserInput stringInputPrefab;
        [SerializeField]
        private BooleanInput booleanInputPrefab;
        [SerializeField]
        private NumericInput doubleInputPrefab;
        [SerializeField]
        private NumericInput intReturnPrefab;

        public AsyncReturn<string> spawnStringInput(string message)
        {
            AsyncReturn<string> stringReturn = new AsyncReturn<string>();
            UserInput input = GameObject.Instantiate(stringInputPrefab, this.transform.parent);
            input.Spawn(message, stringReturn);
            return stringReturn;
        }

        public AsyncReturn<bool> spawnBooleanInput(string message)
        {
            AsyncReturn<bool> boolReturn = new AsyncReturn<bool>();
            BooleanInput input = GameObject.Instantiate(booleanInputPrefab, this.transform.parent);
            input.Spawn(message, boolReturn);
            return boolReturn;
        }

        public AsyncReturn<double> spawnDoubleInput(string message)
        {
            AsyncReturn<double> doubleReturn = new AsyncReturn<double>();
            NumericInput input = GameObject.Instantiate(doubleInputPrefab, this.transform.parent);
            input.Spawn(message, doubleReturn);
            return doubleReturn;
        }

        public AsyncReturn<int> spawnIntegerInput(string message)
        {
            AsyncReturn<int> intReturn = new AsyncReturn<int>();
            NumericInput input = GameObject.Instantiate(intReturnPrefab, this.transform.parent);
            input.Spawn(message, intReturn);
            return intReturn;
        }
    }

}