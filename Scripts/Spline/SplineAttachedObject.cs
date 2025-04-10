using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyThings.Spline
{
    /// <summary>
    /// A Spline Class To Use For Moving The Spline With The Object
    /// Use This With The Object Which Can Move
    /// </summary>
    public class SplineAttachedObject : MonoBehaviour ,IDragHandler
    {
        /// <summary>If The Object Location Be Taken And Updated In Update</summary>
        [field: SerializeField] public bool InUpdate { get; set; } = false;
        /// <summary>If The Object Location Be Taken And Updated In FixedUpdate</summary>
        [field: SerializeField] public bool InFixedUpdate { get; set; } = false;
        /// <summary>If The Object Location Be Taken And Updated In LateUpdate</summary>
        [field: SerializeField] public bool InLateUpdate { get; set; } = false;

        [field:SerializeField] public bool InDrag { get; set; } = false;

        [field:SerializeField] public Transform SplineTransform { get; set; }

        [SerializeField] private List<SplineAttacher> m_splineFollowers = new List<SplineAttacher>();
        private void Update()
        {
            if (InUpdate) UpdateAttacher();
        }
        private void FixedUpdate()
        {
            if(InFixedUpdate) UpdateAttacher();
        }
        private void LateUpdate()
        {
            if(InLateUpdate) UpdateAttacher();
        }
        /// <summary>
        /// Update The Attachers
        /// </summary>
        public void UpdateAttacher()
        {
            for (int i = 0; i < m_splineFollowers.Count; i++)
                m_splineFollowers[i].Change(SplineTransform);
        }
        public void OnDrag(PointerEventData eventData)
        {
            if(InDrag) UpdateAttacher();
        }
        /// <summary>
        /// The Attached Object Must Be Folled By
        /// </summary>
        /// <param name="splineFollower">The Spline To Follow</param>
        public void FollowedBy(SplineAttacher splineFollower)
        {
            m_splineFollowers.Add(splineFollower);
        }
        /// <summary>
        /// The Attached Object To Stop Following
        /// </summary>
        /// <param name="splineFollower">The Spline To UnFollow</param>
        public void UnfollowedBy(SplineAttacher splineFollower)
        {
            m_splineFollowers.Remove(splineFollower);
        }
        public void AddToDeltaLocation(Vector3 ExtraDis)
        {
            for (int i = 0; i < m_splineFollowers.Count; i++)
            {
                m_splineFollowers[i].DeltaLocation += ExtraDis;
            }
        }

    }
}