
using Assets.MyThings.Scripts.Extension;
using MyThings.ExtendableClass;
using MyThings.Timer;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace MyThings.Vibration
{//Amplitude May Hav Problem As We Are Insuring That ITs Length Should Be Equal To The Pattern Length Witch IS May Be Wrong
    // if we give click in viberate then it simply ignores amplitude
    // mixture of predefined and normals
    /// <summary>
    /// In Pattern The First Value Is The Time Gap And The Second Value Is Duration Of Vibration
    /// The Repeat Is The Index From Which To Repeat The Vibration In Loop
    /// </summary>
    public class Vibrator :Singleton_C<Vibrator>
    {

        #region const

        /// <summary>
        /// The Max Amplitude Supported
        /// </summary>
        private const int Amplitude_Max = 255;
        /// <summary>
        /// The Min Amplitude Supported
        /// </summary>
        private const int Amplitude_Min = 1;
        /// <summary>
        /// Teh Minimum Api Required For Vibration With Effect
        /// </summary>
        private const int Vibration_MinApi = 26;
        /// <summary>
        /// The Minimum Api For The PreDefined Vibrations Support
        /// </summary>
        private const int Predefined_MinApi = 29;

        #endregion

        #region Fields

        /// <summary>
        /// The Vibrator Used To Vibrate
        /// </summary>
        private AndroidJavaObject vibrator;
        /// <summary>
        /// The Class Used TO Create effect
        /// </summary>
        private AndroidJavaClass vibrationEffectClass;
        /// <summary>
        /// The Default Amplitude Of THe Vibrator
        /// </summary>
        private int AmplitudeDefault = 255;
        /// <summary>
        /// Teh Api Level Of THe Phone
        /// </summary>
        private int ApiLevel = 1;
        /// <summary>
        /// If The Vibrator Was Inisialized Or Not
        /// </summary>
        private bool isInisialized = false;
        /// <summary>
        /// The Effects Id Stored
        /// </summary>
        private PredefinedEffect EffectsId;
        /// <summary>
        /// If The Vibration Is Allowed
        /// </summary>
        private bool VibrationAllowed=true;
        /// <summary>
        /// Active Chains for PreDefined Vibration
        /// </summary>
        private List<Chain> ActiveChains = new List<Chain>();
        /// <summary>
        /// Empty Objects For PreDefined Viberations
        /// </summary>
        private Queue<Chain> InActiveChains = new Queue<Chain>();

        /// <summary>
        /// Teh Log Level Used
        /// </summary>
        public LogLevel logLevel = LogLevel.Info;

        #endregion

        #region Properites

        /// <summary>
        /// Does The Device Support Vibration Effect
        /// </summary>
        private bool DoesSupportVibrationEffect => ApiLevel >= Vibration_MinApi;
        /// <summary>
        /// Does The Device Support PreDefined Effects
        /// </summary>
        private bool DoesSupportPredefinedEffect => ApiLevel >= Predefined_MinApi;
        /// <summary>
        /// The Api Level Used
        /// </summary>
        public int GetApiLevel => ApiLevel;
        /// <summary>
        /// Teh Default Amplitude Used
        /// </summary>
        public int GetDefaultAmplitude => AmplitudeDefault;
        /// <summary>
        /// Does Teh Device Has Vibrator
        /// </summary>
        public bool HasVibrator => vibrator != null && vibrator.Call<bool>("hasVibrator");
        /// <summary>
        /// Set If The Vibration Is Allowed
        /// </summary>
        public bool IsVibrationAllowed { get => VibrationAllowed;set=> VibrationAllowed = value; }

        #endregion

        #region Unity

        protected override void Awake()
        {
            base.Awake();
            Initialize();
            if (!isInisialized)
                Destroy(gameObject);
        }

        #endregion

        #region Private

        #region Vibration Callers

        /// <summary>
        /// Simple Vibration
        /// </summary>
        /// <param name="millisecond">The Duration Of Vibration</param>
        /// <param name="amplitude">The Amplitude Of The Vibration</param>
        private void vibrateEffect(long millisecond, int amplitude)
        {
            using (AndroidJavaObject effect = createEffect_OneShot(millisecond, amplitude))
            {
                vibrator.Call("vibrate", effect);
            }
        }
        /// <summary>
        /// Vibrate With The Give Pattern 
        /// </summary>
        /// <param name="pattern">The Pattern</param>
        /// <param name="repeat">The Index With Which To Repeat</param>
        private void vibrateEffect(long[] pattern, int repeat)
        {
            using (AndroidJavaObject effect = createEffect_WaveForm(pattern, repeat))
            {
                vibrator.Call("vibrate", effect);
            }
        }
        /// <summary>
        /// Vibrate With The Given Pattern And Amplitude
        /// </summary>
        /// <param name="pattern">The Pattern</param>
        /// <param name="amplitudes">The Amplitude During Pattern</param>
        /// <param name="repeat">The Index With Which To Repeat</param>
        private void vibrateEffect(long[] pattern, int[] amplitudes, int repeat)
        {
            using (AndroidJavaObject effect = createEffect_WaveForm(pattern, amplitudes, repeat))
            {
                vibrator.Call("vibrate", effect);
            }
        }
        /// <summary>
        /// Vibrate With The PreDefined Effect
        /// </summary>
        /// <param name="effectId">The Effect Id</param>
        private void vibratePredefined(int effectId)
        {
            using (AndroidJavaObject effect = createEffect_Predefined(effectId))
            {
                vibrator.Call("vibrate", effect);
            }
        }
        /// <summary>
        /// Vibrate for Legacy Device
        /// </summary>
        /// <param name="millisecond">The Duration Of Vibration</param>
        private void vibrateLegacy(long millisecond)
        {
            vibrator.Call("vibrate", millisecond);
        }
        /// <summary>
        /// Vibrate With PAttern For Legacy Device
        /// </summary>
        /// <param name="pattern">The PAttern</param>
        /// <param name="repeat">The Index With Which To Repeat</param>
        private void vibrateLegacy(long[] pattern, int repeat)
        {
            vibrator.Call("vibrate", pattern, repeat);
        }

        #endregion

        #region Vibration Effect

        /// <summary>
        /// Create An Effect For Single Vibration
        /// </summary>
        /// <param name="millisecond">The Vibration Duration</param>
        /// <param name="amplitude">The Amplitude Of THe Vibration</param>
        /// <returns>The Effect</returns>
        private AndroidJavaObject createEffect_OneShot(long millisecond, int amplitude)
        {
            return vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", millisecond, amplitude);
        }
        /// <summary>
        /// Create A Effect For PreDefined Effect
        /// </summary>
        /// <param name="EffectId">The Effect Id</param>
        /// <returns>The Effect</returns>
        private AndroidJavaObject createEffect_Predefined(int EffectId)
        {
            return vibrationEffectClass.CallStatic<AndroidJavaObject>("createPredefined", EffectId);
        }
        /// <summary>
        /// Create A Effect Of The WaveForm
        /// </summary>
        /// <param name="timings">The Pattern</param>
        /// <param name="amplitudes">The Amplitudes For The PAtter</param>
        /// <param name="repeat">The Index With Which To Repeat</param>
        /// <returns>The Effect</returns>
        private AndroidJavaObject createEffect_WaveForm(long[] timings, int[] amplitudes, int repeat)
        {
            return vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", timings, amplitudes, repeat);
        }
        /// <summary>
        /// Create A Effect Of The WaveForm
        /// </summary>
        /// <param name="timings">The Pattern</param>
        /// <param name="repeat">The Index With Which TO Repeat</param>
        /// <returns>The Effect</returns>
        private AndroidJavaObject createEffect_WaveForm(long[] timings, int repeat)
        {
            return vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", timings, repeat);
        }

        #endregion

        /// <summary>
        /// Initialize The Vibrator Witgh Its Functionality
        /// </summary>
        private void Initialize()
        {
#if UNITY_ANDROID
            if (Application.isConsolePlatform) Handheld.Vibrate();
#endif
            if(!isInisialized&&Application.platform==RuntimePlatform.Android)
            {
                using(AndroidJavaClass androidVersionClass=new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    ApiLevel = androidVersionClass.GetStatic<int>("SDK_INT");
                }
                using(AndroidJavaClass unityPlayer=new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    using(AndroidJavaObject currentActivity=unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    if (currentActivity == null) return;
                    vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                    if(DoesSupportVibrationEffect)
                    {
                        vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                        AmplitudeDefault=Mathf.Clamp(vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE"),Amplitude_Min,Amplitude_Max);
                    }
                    if(DoesSupportPredefinedEffect)
                    {
                        EffectsId.Effect_Click = vibrationEffectClass.GetStatic<int>("EFFECT_CLICK");
                        EffectsId.Effect_Double_Click = vibrationEffectClass.GetStatic<int>("EFFECT_DOUBLE_CLICK");
                        EffectsId.Effect_Heavy_Click = vibrationEffectClass.GetStatic<int>("EFFECT_HEAVY_CLICK");
                        EffectsId.Effect_Tick = vibrationEffectClass.GetStatic<int>("EFFECT_TICK");
                    }
                }
            }
            LogAuto("Vibration component initialized", LogLevel.Info);
            isInisialized = true;
        }
        /// <summary>
        /// Clamp All The Amplitudes To Be Under Range
        /// </summary>
        /// <param name="Amplitudes">Teh Amplitudes</param>
        private void ClampAmplitudes(int[] Amplitudes)
        {
            for (int i = 0; i < Amplitudes.Length; i++)
            {
                Amplitudes[i] = Mathf.Clamp(Amplitudes[i], Amplitude_Min, Amplitude_Max);
            }
        }
        /// <summary>
        /// Create String From Array
        /// </summary>
        /// <param name="array">The Array</param>
        /// <returns>The Concatinated String</returns>
        private string ArrayToStr(long[] array) => array == null ? "null" : string.Join(", ", array);
        /// <summary>
        /// Create String From Array
        /// </summary>
        /// <param name="array">Teh Array</param>
        /// <returns>The Concatinated String</returns>
        private string ArrayToStr(int[] array) => array == null ? "null" : string.Join(", ", array);
        /// <summary>
        /// Printer For The Vibrator
        /// </summary>
        /// <param name="text">The Text To Print</param>
        /// <param name="level">Teh Level Of THe Text</param>
        private void LogAuto(string text, LogLevel level)
        {
            if (text == null)
                return;
            switch (logLevel)
            {
                case LogLevel.Warning:
                    if (level == LogLevel.Warning)
                        Debug.LogWarning(text);
                    break;
                case LogLevel.Info:
                    if (level == LogLevel.Info)
                        Debug.Log(text);
                    else
                        Debug.LogWarning(text);
                    break;

            }
        }
        /// <summary>
        /// Convert The Given Pattern Into Array
        /// </summary>
        /// <param name="pattern">The Pattern</param>
        /// <param name="Seperator">The Seperator Used For The Elements</param>
        /// <returns>The Patterns</returns>
        private long[] ParsePattern(string pattern, string Seperator = ",")
        {
            if (string.IsNullOrEmpty(pattern)) return new long[0];
            pattern = pattern.Trim();
            string[] split = pattern.Split(Seperator);
            long[] result = new long[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                if (long.TryParse(split[i], out long Duration))
                {
                    result[i] = Duration < 0 ? 0 : Duration;
                    Debug.Log(result[i]);
                }
                else if (Enum.TryParse(split[i], true, out PreDefinedEffect effect))
                {
                    result[i]=PredefinedEffect.Convert(effect).Duration;
                    Debug.Log(result[i]);

                }
                else
                {
                    result[i] = 0;
                }
            }
            return result;
        }
        /// <summary>
        /// Convert The Given Amplitude Into Array
        /// </summary>
        /// <param name="amplitude">The Amplitude</param>
        /// <param name="Seperator">The Seperator Used For The Elements</param>
        /// <returns>The Amplitudes</returns>
        private int[] ParseAmplitude(string amplitude, string Seperator = ",")
        {
            if (string.IsNullOrEmpty(amplitude)) return new int[0];
            amplitude = amplitude.Trim();
            string[] split = amplitude.Split(Seperator);
            int[] result = new int[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                if (int.TryParse(split[i], out int Amplitude))
                {
                    result[i] = Mathf.Clamp(Amplitude, Amplitude_Min, Amplitude_Max);
                }
                else
                {
                    result[i] = 0;
                }
            }
            return result;
        }
        /// <summary>
        /// Deactivate All Active Chains
        /// </summary>
        private void DeActivate()
        {
            for (int i = 0; i < ActiveChains.Count; i++)
            {
                ActiveChains[i].Stop();
            }
            ActiveChains.Clear();
        }

        #endregion

        #region Public

        /// <summary>
        /// Vibrate Single Time
        /// </summary>
        /// <param name="millisecond">Teh Duration</param>
        /// <param name="amplitude">The Amplitude OF The Vibration</param>
        /// <param name="cancel">To Cancel Previous Vibration Or Not</param>
        public void Vibrate(long millisecond, int amplitude = Amplitude_Max, bool cancel = false)
        {
            if (!VibrationAllowed)
                return;
            string Log = string.Format("Vibrate ({0}, {1}, {2})", millisecond, amplitude, cancel);
            if (!isInisialized)
            {
                LogAuto(Log + ": Not initialized", LogLevel.Warning);
            }
            else if (!HasVibrator)
            {
                LogAuto(Log + ": Device doesn't have Vibrator", LogLevel.Warning);
            }
            else
            {
                if (cancel) Cancel();
                if (DoesSupportVibrationEffect)
                {
                    amplitude=Mathf.Clamp(amplitude, Amplitude_Min, Amplitude_Max);
                    if (amplitude != Amplitude_Max && !hasAmplitudeControl())
                    {
                        LogAuto(Log+ ": Device doesn't have Amplitude Control, but Amplitude was set", LogLevel.Warning);
                    }
                    if(amplitude==0)
                    {
                        amplitude = AmplitudeDefault;
                    }
                    amplitude = hasAmplitudeControl() ? amplitude : Amplitude_Max;
                    vibrateEffect(millisecond, amplitude);
                    LogAuto(Log+ ": Effect called", LogLevel.Info);
                }
                else
                {
                    vibrateLegacy(millisecond);
                    LogAuto(Log+ ": Legacy called", LogLevel.Info);
                }
            }
        }
        /// <summary>
        /// Vibrate With The Pattern Given
        /// </summary>
        /// <param name="pattern">The Pattern Given</param>
        /// <param name="amplitudes">The Amplitudes Given</param>
        /// <param name="repeat">The Index With Which To Repeat</param>
        /// <param name="cancel">To Cancel Previous Vibration Or Not</param>
        public void Vibrate(long[] pattern, int[] amplitudes=default,int repeat=-1,bool cancel=false)
        {
            if (!VibrationAllowed)
                return;
            string Log = string.Format("Vibrate (({0}), ({1}), {2}, {3})", ArrayToStr(pattern), ArrayToStr(amplitudes), repeat, cancel);
            if(!isInisialized)
            {
                LogAuto(Log+ ": Not initialized", LogLevel.Warning);
            }
            else if(!HasVibrator)
            {
                LogAuto(Log+ ": Device doesn't have Vibrator", LogLevel.Warning);
            }
            else
            {
                if(amplitudes!=null&&amplitudes.Length!=pattern.Length)
                {
                    LogAuto(Log + ": Length of Amplitudes array is not equal to Pattern array. Amplitudes will be ignored.", LogLevel.Warning);
                    amplitudes = null;
                }
                if(amplitudes!=null)
                {
                    ClampAmplitudes(amplitudes);
                }
                if (cancel) Cancel();
                if(DoesSupportVibrationEffect)
                {
                    if(amplitudes!=null&&!hasAmplitudeControl())
                    {
                        LogAuto(Log+ ": Device doesn't have Amplitude Control, but Amplitudes was set", LogLevel.Warning);
                        amplitudes = null;
                    }
                    if(amplitudes == null)
                    {
                        vibrateEffect(pattern, repeat);
                        LogAuto(Log+ ": Effect called", LogLevel.Info);
                    }
                    else
                    {
                        vibrateEffect(pattern, amplitudes, repeat);
                        LogAuto(Log+ ": Effect with amplitudes called", LogLevel.Info);
                    }
                }
                else
                {
                    vibrateLegacy(pattern, repeat);
                    LogAuto(Log+ ": Legacy called", LogLevel.Info);
                }
            }
        }
        /// <summary>
        /// Vibrate With The Pattern Given
        /// </summary>
        /// <param name="pattern">The Pattern Given</param>
        /// <param name="amplitudes">The Amplitudes Given</param>
        /// <param name="Seperator">The Seperator USed</param>
        /// <param name="repeat">The Index With Which To Repeat</param>
        /// <param name="cancel">To Cancel Previous Vibration Or Not</param>
        public void Vibrate(string pattern,string amplitudes=default,string Seperator=",", int repeat=-1,bool cancel=false)
        {
            Vibrate(ParsePattern(pattern, Seperator), ParseAmplitude(amplitudes, Seperator), repeat, cancel);
        }
        /// <summary>
        /// vibrate With the Predefined Effect
        /// </summary>
        /// <param name="effectId">The Effect</param>
        /// <param name="cancel">To Cancel Previous Vibration Or Not</param>
        public void VibratePredefined(PreDefinedEffect effectId,bool cancel=false)
        {
            if (!VibrationAllowed)
                return;
            string Log = string.Format("VibratePredefined ({0})", effectId);
            if(!isInisialized)
            {
                LogAuto(Log + ": Not initialized",LogLevel.Warning);
            }
            else if(!HasVibrator)
            {
                LogAuto(Log + ": Device doesn't have Vibrator", LogLevel.Warning);
            }
            else if(!DoesSupportPredefinedEffect)
            {
                LogAuto(Log + $": Device doesn't support Predefined Effects (Api Level >= {Predefined_MinApi})", LogLevel.Warning);
                if (cancel) Cancel();
                (long duration, int amplitude) = PredefinedEffect.Convert(effectId);
                Vibrate(duration,amplitude);
            }
            else
            {
                if(cancel)Cancel();
                vibratePredefined(EffectsId.Get(effectId));
                LogAuto(Log + ": Predefined effect called", LogLevel.Info);
            }
        }
        /// <summary>
        /// vibrate With the Given Pattern For PreDefined Viberations
        /// </summary>
        /// <param name="Pattern">Teh PAttern</param>
        /// <param name="Repeat">The Index tO Repeat From</param>
        /// <param name="Seperator">The Seperator</param>
        /// <param name="cancel">To Cancel Already Running viberations</param>
        public void VibratePredefined(string Pattern, int Repeat = -1, string Seperator = ",", bool cancel = false)
        {
            if (!VibrationAllowed)
                return;
            string Log = string.Format("VibratePredefined ({0})", Pattern);
            if (!isInisialized)
            {
                LogAuto(Log + ": Not initialized", LogLevel.Warning);
            }
            else if (!HasVibrator)
            {
                LogAuto(Log + ": Device doesn't have Vibrator", LogLevel.Warning);
            }
            else if (!DoesSupportPredefinedEffect)
            {
                LogAuto(Log + $": Device doesn't support Predefined Effects (Api Level >= {Predefined_MinApi})", LogLevel.Warning);
                Vibrate(Pattern,repeat:Repeat, Seperator:Seperator, cancel:cancel);
            }
            else
            {
                
                if (cancel)
                {
                    DeActivate();
                    Cancel();
                }
                if (InActiveChains.Count != 0)
                {
                    InActiveChains.Dequeue().ReSet(Pattern, Seperator, Repeat);
                }
                else
                {
                    new Chain(Pattern, Seperator, Repeat);
                }
            }
        }
        /// <summary>
        /// Does The Device Have Amplitude Controler
        /// </summary>
        /// <returns></returns>
        public bool hasAmplitudeControl()
        {
            if (HasVibrator && DoesSupportVibrationEffect)
            {
                return vibrator.Call<bool>("hasAmplitudeControl");
            }
            else
            { 
                return false; 
            }
        }
        /// <summary>
        /// Cancel The Current Running Vibration
        /// </summary>
        public void Cancel()
        {
            if(HasVibrator)
            {
                vibrator.Call("cancel");
                LogAuto("Cancel (): Called", LogLevel.Info);
            }
        }

        #endregion


        /// <summary>
        /// A Holder With PreDefined Effects
        /// </summary>
        private struct PredefinedEffect
        {
            public int Effect_Click;
            public int Effect_Double_Click;
            public int Effect_Heavy_Click;
            public int Effect_Tick;

            /// <summary>
            /// Get The Effect Id Associated With The Enum
            /// </summary>
            /// <param name="effect">The Effect</param>
            /// <returns>The Effect Id</returns>
            public readonly int Get(PreDefinedEffect effect)
            {
                return effect switch
                {
                    PreDefinedEffect.Click => Effect_Click,
                    PreDefinedEffect.DoubleClick => Effect_Double_Click,
                    PreDefinedEffect.HeavyClick => Effect_Heavy_Click,
                    PreDefinedEffect.Tick => Effect_Tick,
                    _ => 0
                };
            }
            public static (long Duration,int amplitude) Convert(PreDefinedEffect effect)
            {
                return effect switch
                {
                    PreDefinedEffect.Click => (150,255),
                    PreDefinedEffect.DoubleClick => (250,255),
                    PreDefinedEffect.HeavyClick => (350,255),
                    PreDefinedEffect.Tick => (100,255),
                    _ => (0,0)
                };
            }
        }
        /// <summary>
        /// The Chain Which Holds A Single Viberation PAttern
        /// </summary>
        private class Chain
        {
            private ITimer timer;
            int Repeat;
            int current;
            List<Node> nodes;
            public Chain(string Pattern,string Seperator,int repeat=-1)
            {
                _instance.ActiveChains.Add(this);
                timer = TimerManager.Create(0, Next, true);
                nodes =Get(Pattern, Seperator);
                if (repeat != -1)
                    Repeat = repeat / 2;
                else
                    Repeat = -1;
                timer.MaxTime = nodes[0].Time;
                timer.Start();
            }
            /// <summary>
            /// Get The Next node To Run
            /// </summary>
            private void Next()
            {
                
                var vibration=nodes[current];
                _instance.vibratePredefined(_instance.EffectsId.Get(vibration.Effect));
                current++;
                if (current==nodes.Count)
                {
                    if (Repeat == -1)
                    {
                        timer.Stop();
                        _instance.ActiveChains.Remove(this);
                        _instance.InActiveChains.Enqueue(this);
                        return;
                    }
                    else
                    {
                        current = Repeat;
                    }
                }
                timer.MaxTime = nodes[current].Time;
            }
            /// <summary>
            /// Create A List Out of String Pattern
            /// </summary>
            /// <param name="Pattern">String Pattern</param>
            /// <param name="seperator">The Seperator</param>
            /// <returns>The List Of Node</returns>
            private List<Node> Get(string Pattern,string seperator)
            {
                string[] Split=Pattern.Split(seperator);
                if ((Split.Length & 1) == 1)
                    return new List<Node>();
                List<Node> Result= new List<Node>(Split.Length / 2);

                for(int i = 0;i< Split.Length;i+=2)
                {
                    if (!long.TryParse(Split[i], out long Duration))
                    {
                        Debug.LogWarning(Split[i] + ": Could Not Be Converted");
                        continue;
                    }
                    if (!Enum.TryParse(Split[i + 1], true, out PreDefinedEffect effect))
                    {
                        Debug.LogWarning(Split[i + 1] + ":  Could Not Be Converted");
                        continue;
                    }
                    Result.Add(new Node(Duration, effect));
                }
                return Result;
            }
            /// <summary>
            /// Stop The Chain Running
            /// </summary>
            public void Stop()
            {
                _instance.InActiveChains.Enqueue(this);
                timer.Stop();
                nodes.Clear();
            }
            /// <summary>
            /// Re Use The Chain For New Node
            /// </summary>
            /// <param name="Pattern">The New Pattern</param>
            /// <param name="Seperator">The New Seperator</param>
            /// <param name="repeat">The index to Repeat From</param>
            public void ReSet(string Pattern,string Seperator,int repeat=-1)
            {
                _instance.ActiveChains.Add(this);
                nodes = Get(Pattern, Seperator);
                current = 0;
                if (repeat != -1)
                    Repeat = repeat / 2;
                else
                    Repeat = -1;
                timer.MaxTime = nodes[0].Time;
                timer.Start();
            }
        }
        /// <summary>
        /// A node to Store Time And The Predefined Viberation To Play
        /// </summary>
        private struct Node
        {
            public float Time;
            public PreDefinedEffect Effect;
            public Node(long time, PreDefinedEffect effect)
            {
                Time= time.MilliToSeconds();
                Effect = effect;
            }
        }

        /// <summary>
        /// The LogLevel Avalaible
        /// </summary>
        public enum LogLevel
        {
            Disabled,
            Info,
            Warning,
        }
        /// <summary>
        /// The Predefined Effect Available
        /// </summary>
        public enum PreDefinedEffect
        {
            Click,
            DoubleClick,
            HeavyClick,
            Tick
        }
    }
}
