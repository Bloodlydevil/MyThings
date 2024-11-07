using MyThings.Job_System;
using MyThings.Data;
using System;
using TMPro;
using UnityEngine;

namespace MyThings.TMp
{
    /// <summary>
    /// A Class To Change The Title Color In Random Ways
    /// </summary>
    public class TitleColor : MonoBehaviour
    {

        [Tooltip("The Title Text")]
        [SerializeField] private TextMeshProUGUI m_Text;
        [Tooltip("The Default Gradient The Act as buffer")]
        [SerializeField] private TMP_ColorGradient m_Gradient;
        [Tooltip("The Colors To change into")]
        [SerializeField] private Color[] m_TargetColor;
        [Tooltip("The Maximum Amout Of Time Reqired Before Color Starts To Change")]
        [SerializeField] private float m_MaxChangeTime;
        /// <summary>
        /// The Buffer To Change The Colors
        /// </summary>
        private DoubleValue<Color>[] m_MinMaxColors = new DoubleValue<Color>[4];
        /// <summary>
        /// The Functions storage To Change The Color
        /// </summary>
        private Action<float> m_colorChange;
        /// <summary>
        /// The Caller Of The Update
        /// </summary>
        private Completion m_Change;


        private void Start()
        {
            for(int i=0;i<m_MinMaxColors.Length; i++)
            {
                m_MinMaxColors[i] = new DoubleValue<Color>();
            }    
            m_Change = new Completion(m_MaxChangeTime, Upda);
            m_Change.AddOnEnd(initialize);
            initialize();
        }

        #region Private

        /// <summary>
        /// The function To Call And Act as Update
        /// </summary>
        /// <param name="a">The Progress</param>
        private void Upda(float a)
        {
            m_colorChange(a);
            m_Text.colorGradientPreset = m_Gradient;
        }
        /// <summary>
        /// Intialie The The Colors To Change
        /// </summary>
        private void initialize()
        {
            int SelectPortion=UnityEngine.Random.Range(1,15);
            m_colorChange = null;
            

            if ((SelectPortion & 1) == 1)
            {
                
                m_MinMaxColors[0]._First = m_Gradient.topLeft;
                m_MinMaxColors[0]._Second = m_TargetColor[UnityEngine.Random.Range(0, m_TargetColor.Length - 1)];
                m_colorChange += (float a) => m_Gradient.topLeft = Color.LerpUnclamped(m_MinMaxColors[0]._First, m_MinMaxColors[0]._Second, a);
            }
            SelectPortion >>= 1;
            if ((SelectPortion & 1) ==1)
            {
                m_MinMaxColors[1]._First = m_Gradient.bottomLeft;
                m_MinMaxColors[1]._Second = m_TargetColor[UnityEngine.Random.Range(0, m_TargetColor.Length - 1)];
                m_colorChange += (float a) => m_Gradient.bottomLeft = Color.LerpUnclamped(m_MinMaxColors[1]._First, m_MinMaxColors[1]._Second, a);
            }
            SelectPortion >>= 1;
            if ((SelectPortion & 1) == 1)
            {
                m_MinMaxColors[2]._First = m_Gradient.topRight;
                m_MinMaxColors[2]._Second = m_TargetColor[UnityEngine.Random.Range(0, m_TargetColor.Length - 1)];
                m_colorChange += (float a) => m_Gradient.topRight = Color.LerpUnclamped(m_MinMaxColors[2]._First, m_MinMaxColors[2]._Second, a);
            }
            SelectPortion >>= 1;
            if ((SelectPortion & 1) == 1)
            {
                m_MinMaxColors[3]._First = m_Gradient.bottomRight;
                m_MinMaxColors[3]._Second = m_TargetColor[UnityEngine.Random.Range(0, m_TargetColor.Length - 1)];
                m_colorChange += (float a) => m_Gradient.bottomRight = Color.LerpUnclamped(m_MinMaxColors[3]._First, m_MinMaxColors[3]._Second, a);
            }
            m_Change.Start();
        }

        #endregion

    }
}