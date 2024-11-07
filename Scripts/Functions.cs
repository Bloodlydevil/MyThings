using System;
using UnityEngine;
namespace MyThings
{
    /// <summary>
    /// All the Random static function used in game
    /// </summary>
    public static class Functions
    {
        /// <summary>
        /// the smallest value in game
        /// </summary>
        public static readonly float SmallNo = 0.00001f;


        /// <summary>
        /// it cuts the value if its out of range -> a will always be in the range (LowerLimit,UpperLimit)
        /// </summary>
        /// <typeparam name="t">The Type Used (int,float or somthing similar)</typeparam>
        /// <param name="a">value to cut</param>
        /// <param name="LowerLimit">the lower limit</param>
        /// <param name="UpperLimit">the Upper limit</param>
        public static void ValueCut<t>(ref t a, t LowerLimit, t UpperLimit) where t: IComparable
        {
            if (a.CompareTo(LowerLimit) == -1)
                a = LowerLimit;
            else if (a.CompareTo(UpperLimit) == 1)
                a = UpperLimit;
        }
        /// <summary>
        /// Used For Debug Set The Value To False If True and run The Command
        /// </summary>
        /// <param name="condition">The Condition</param>
        /// <param name="action">The Task</param>
        public static void IFTrueSetFalse(ref bool condition,Action action)
        {
            if(condition)
            {
                condition = false;
                action();
            }
        }


        #region Convert a Value from one range to another


        /// <summary>
        /// Function To Convert a Value From One Range To Another
        /// </summary>
        /// <param name="Value">The Value</param>
        /// <param name="PrevMin">The Previous Min Of The Value</param>
        /// <param name="PrevMax">The Previous Max Of The Value</param>
        /// <param name="NewMin">The New Min Of The Value</param>
        /// <param name="NewMax">The New Max Of The Value</param>
        /// <returns>The New Value In The Range Of NewMin And NewMax</returns>
        public static int Convert(int Value,int PrevMin,int PrevMax,int NewMin,int NewMax)
        {
            return Value * (NewMax - NewMin) /( PrevMax - PrevMin) + NewMin;
            //if integer does some problem
            //return (Value * (NewMax - NewMin) + NewMin * (PrevMax - PrevMin)) / (PrevMax - PrevMin);
        }
        public static float Convert(float Value, float PrevMin, float PrevMax, float NewMin, float NewMax)
        {
            return Value * (NewMax - NewMin) / (PrevMax - PrevMin) + NewMin;
        }
        public static double Convert(double Value, double PrevMin, double PrevMax, double NewMin, double NewMax)
        {
            return Value * (NewMax - NewMin) / (PrevMax - PrevMin) + NewMin;
        }


        #endregion


        #region ValueConverge   ->   it change a value to another no with some rate  ->  change the value of no by ConvergeRate(add or subtract) if no is not equal to ConvergeNo


        /// <summary>
        /// it change a value to another no with some rate  ->  change the value of no by ConvergeRate(add or subtract) if no is not equal to ConvergeNo
        /// for int
        /// </summary>
        /// <param name="no"> the no to converge </param>
        /// <param name="ConvergeNo">the value to coverge to</param>
        /// <param name="ConvergeRate">the rate at which value converge</param>
        public static void ValueConverge(ref int no, int ConvergeNo, int ConvergeRate)
        {
            if (no == ConvergeNo) return;
            if (no > ConvergeNo)
            {
                no -= ConvergeRate;
                if (no < ConvergeNo)
                    no = ConvergeNo;
            }
            else
            {
                no += ConvergeRate;
                if (no > ConvergeNo)
                    no = ConvergeNo;
            }
            /* Correct Ans But Might Be Slow
            int Direction = no>ConvergeNo?1:-1;
            no -= ConvergeRate * Direction;
            if (no * Direction < ConvergeNo * Direction)
                no = ConvergeNo;*/
        }
        public static void ValueConverge(ref float no, float ConvergeNo, float ConvergeRate)
        {
            if (no == ConvergeNo) return;
            if (no > ConvergeNo)
            {
                no -= ConvergeRate;
                if (no < ConvergeNo)
                    no = ConvergeNo;
            }
            else
            {
                no += ConvergeRate;
                if (no > ConvergeNo)
                    no = ConvergeNo;
            }
        }
        public static void ValueConverge(ref double no, double ConvergeNo, double ConvergeRate)
        {
            if (no == ConvergeNo) return;
            if (no > ConvergeNo)
            {
                no -= ConvergeRate;
                if (no < ConvergeNo)
                    no = ConvergeNo;
            }
            else
            {
                no += ConvergeRate;
                if (no > ConvergeNo)
                    no = ConvergeNo;
            }
        }
        #endregion


        #region ValueChecker  -> it checks if the value is equal to another no.  ->  used for Float and double values to not get Float error


        /// <summary>
        /// it checks if the value is equal to another no.  ->  used for Float values to not get Float error
        /// </summary>
        /// <param name="no">value to check</param>
        /// <param name="checker">value to check from</param>
        /// <returns>if no is almost equal to checker</returns>
        public static bool ValueChecker(float no, float checker)
        {
            return Math.Abs(no - checker) < SmallNo;
        }
        public static bool ValueChecker(double no, double checker)
        {
            return Math.Abs((float)(no - checker)) < SmallNo;
        }

        #endregion

        #region Motion
        public static float GetAcceleration( float FinalVelocity,float InitialVelocity,float Time)
        {
            return (FinalVelocity - InitialVelocity) / Time;
        }
        public static Vector3 GetAcceleration( Vector3 FinalVelocity, Vector3 InitialVelocity, float Time)
        {
            return (FinalVelocity - InitialVelocity) / Time;
        }
        public static Vector2 GetAcceleration( Vector2 FinalVelocity, Vector2 InitialVelocity, float Time)
        {
            return (FinalVelocity - InitialVelocity) / Time;
        }
        public static float GetAccelerationH(float FinalVelocity,float InitialVelocity,float Height)
        {
            return ((FinalVelocity * FinalVelocity) - (InitialVelocity * InitialVelocity)) * 0.5f / Height;
        }
        #endregion

        #region Rotation
        public static float GetAngularAccelerationR(float AngleInRadians, float Time)
        {
            return 2*AngleInRadians/(Time*Time);
        }
        public static float GetAngularAcceleration(float Angle, float Time)
        {
            return 2 * Angle*Mathf.Deg2Rad / (Time * Time);
        }
        #endregion


    }
}