using MyThings.CAnimation.Outputs;
using MyThings.Data;
using UnityEngine;
using MyThings.Job_System;
// They Will Override The RGBA Value Of Targets Which May Cause Some Problems So Try To Solve It
namespace MyThings.CAnimation.Animation.ColorCA
{
    /// <summary>
    /// An Animator Class To Animate To A Target Color
    /// </summary>
    [System.Serializable]
    public class TColorCA : BColorCA
    {
        /// <summary>
        /// The Targets
        /// </summary>
        private Target[] _Targets;
        /// <summary>
        /// The Mins Of The Output Buffer
        /// </summary>
        private Color[] _MinColors;
        /// <summary>
        /// Constructor To Create The IAnimation To Animate RGBA Values To A Target 
        /// </summary>
        /// <param name="Time">The Animation Time</param>
        /// <param name="output">The Outputer Used</param>
        /// <param name="TimeScale">The TimeScaled Used</param>
        /// <param name="AllSame">If All Images Should Have Same Color (Does Not Work With Multi Target)</param>
        /// <param name="Update">If The Colors Should Be Updated From The Source</param>
        /// <param name="targets">The Targets</param>
        public TColorCA(float Time,IOutput<Color> output,bool TimeScale=true,bool AllSame=false,bool Update=false,params Target[] targets)
        {
            _output = output;
            _Targets= targets;
            if(targets.Length>1)
            {
                if(Update)
                _changer = new Completion(Time, MMUpdate, TimeScale);
                else
                    _changer= new Completion(Time,
                        (float a)=> 
                        {
                            _output.GetInput(ref _OutputBuffer);
                            MMUpdate(a); 
                        }
                        , TimeScale);
                _MinColors = new Color[targets.Length];
                _OutputBuffer = new Color[targets.Length];
            }
            else
            {
                if(AllSame)
                {
                    if (Update)
                        _changer = new Completion(Time, SSUpdate, TimeScale);
                    else
                        _changer = new Completion(Time,
                            (float a) =>
                            {
                                _output.GetInput(ref _OutputBuffer);
                                SSUpdate(a);
                            }
                            , TimeScale);
                    _MinColors =new Color[1];
                    _OutputBuffer = new Color[1];
                }
                else
                {
                    if (Update)
                        _changer = new Completion(Time, MSUpdate, TimeScale);
                    else
                        _changer = new Completion(Time,
                            (float a) =>
                            {
                                _output.GetInput(ref _OutputBuffer);
                                MSUpdate(a);
                            }
                            , TimeScale);
                    _MinColors =new Color[output.Length];
                    _OutputBuffer = new Color[output.Length];
                }
            }
            _output.GetInput(ref _MinColors);
            _output.GetInput(ref _OutputBuffer);
        }
        /// <summary>
        /// Single Target Single MinColor
        /// </summary>
        /// <param name="change">The Change</param>
        private void SSUpdate(float change)
        {
            _Targets[0].Change(ref _OutputBuffer[0], _MinColors[0], change);
            _output.SingOutput(_OutputBuffer[0]);
        }
        /// <summary>
        /// Single Target Single Min Color
        /// </summary>
        /// <param name="change">The Change</param>
        private void MSUpdate(float change)
        {
            for (int i = 0; i < _MinColors.Length; i++)
            {
                _Targets[0].Change(ref _OutputBuffer[i], _MinColors[i], change);
            }
            _output.MulOutput(_OutputBuffer);
        }
        /// <summary>
        /// Multi Color And Multi Target
        /// </summary>
        /// <param name="change">The Change</param>
        private void MMUpdate(float change)
        {
            for(int i=0;i< _MinColors.Length;i++)
            {
                _Targets[i].Change(ref _OutputBuffer[i], _MinColors[i], change);
            }
            _output.MulOutput(_OutputBuffer);
        }
    }
}