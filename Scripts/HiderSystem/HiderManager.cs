using UnityEngine;
using UnityEngine.UI;
using MyThings.HiderSystem.Hiders;

namespace MyThings.HiderSystem
{

    /// <summary>
    /// The Class Which Acts As The Middle Man For All The IHider
    /// </summary>
    public static class HiderManager
    {
        /// <summary>
        /// Subscribe To The Hider System To Hide The GameObjects
        /// </summary>
        /// <param name="Time">The Time At Which To Show</param>
        /// <param name="Scale">Should The Time Be Scaled</param>
        /// <param name="Start">If The Object Should Be Hiden In The Start</param>
        /// <param name="objects">The Objects To Hide</param>
        /// <returns>The IHider</returns>
        public static IHider Subscribe(float Time, bool Scale, bool Start, params GameObject[] objects)
        {
            ObjectSIHider objectSIHider = new ObjectSIHider(Time, Scale, objects);
            if (Start)
                objectSIHider.Hide();
            return objectSIHider;
        }
        /*/// <summary>
        /// Subscribe To The Hider System To Hide Image By Reducing Alpha 
        /// </summary>
        /// <param name="Time">The Time At Which To Show</param>
        /// <param name="Scale">Should The Time Be Scaled</param>
        /// <param name="Start">If The Object Should Be Hiden In The Start</param>
        /// <param name="AnimationTime">The Animation Time</param>
        /// <param name="images">The Animations To Hide</param>
        /// <returns>The IHider</returns>
        public static IHider Subscribe(float Time, bool Scale, bool Start, float AnimationTime, params Image[] images)
        {
            UIHider Hider = new UIHider(Time, Scale, AnimationTime, images);
            if (Start)
                Hider.Hide();
            return Hider;
        }*/
        /// <summary>
        /// Subscribe To The Hider System To Hide Image By Reducing Alpha 
        /// </summary>
        /// <param name="Time">The Time Taken To Automaticaly Hide The Images</param>
        /// <param name="TimeShow">The Animation Length To Show The Images From State Of Hiden</param>
        /// <param name="TimeHide">The Animation Length To Hide The Images From State Of Shown</param>
        /// <param name="Start"></param>
        /// <param name="TimeScale">The Time Scale</param>
        /// <param name="Same">If All The Colors Of The Images Are Same Then This Should Be Made True To Save Computation Time And Power</param>
        /// <param name="Update">Make True If The Color Of THe Images Can Be Affected By Other Things(Images Color Will Be Taken Input Each Frame)</param>
        /// <param name="images">The Images To Apply Hide To</param>
        /// <returns>The Hider</returns>
        public static IHider Subscribe(float Time,float TimeShow=1,float TimeHide=1,bool Start=true,bool TimeScale=true,bool Same=false,bool Update=false,params Image[] images)
        {
            UIHider H = new UIHider(Time,TimeShow,TimeHide,TimeScale,Same,Update,images);
            if (Start)
                H.Hide();
            return H;
        }
    }
}