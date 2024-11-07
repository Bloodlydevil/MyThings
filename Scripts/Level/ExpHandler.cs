using System;
using UnityEngine;

namespace MyThings.Level
{

    public class ExpHandler : MonoBehaviour
    {
        /// <summary>
        /// Function To Get The Exp Required For The Level Required
        /// </summary>
        private Func<int, int> ExpGetter;
        private int MaxExp;
        private int CurrentExp;

        public void SetExpCalculator(Func<int, int> Exp) => ExpGetter = Exp;
        public void AddExp() { }
    }
}