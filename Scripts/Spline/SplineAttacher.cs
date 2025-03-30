using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace MyThings.Spline
{

    /// <summary>
    /// When The Attached Object Moves Then This Needs To Move
    /// </summary>
    public class SplineAttacher : MonoBehaviour
    {
        [SerializeField] private SplineContainer m_SplineContainer;

        private Dictionary<SplineAttachedObject, int> m_Knot = new Dictionary<SplineAttachedObject, int>();

        private Vector3 m_DeltaLocation;

        /// <summary>
        ///  Cahnge The Spline To Follow The Object
        /// </summary>
        /// <param name="currentPosition">The Position Of The Object</param>
        public void Change(SplineAttachedObject currentPosition)
        {
            if (m_Knot.TryGetValue(currentPosition, out var knotIndex))
            {
                var spline = m_SplineContainer.Spline;
                var knot = spline[knotIndex];
                var attachedTransform = currentPosition.transform;
                knot.Position = attachedTransform.position * attachedTransform.localScale.x / attachedTransform.lossyScale.x+ m_DeltaLocation;
                spline[knotIndex] = knot;
            }
            else
            {
                Debug.LogWarning(currentPosition.gameObject + " Did Not Join Using Add Firstly");
            }
        }
        /// <summary>
        /// The Object To Follow
        /// </summary>
        /// <param name="location">The Location Of The Object</param>
        /// <param name="DeltaLocation">The Delta Location To Apply Every Time in Change</param>
        /// <param name="ToFollow">The object To Follow</param>
        public void Follow( Vector3 location, Vector3 DeltaLocation, SplineAttachedObject ToFollow)
        {
            m_Knot.Add(ToFollow, m_SplineContainer.Spline.Count);
            m_SplineContainer.Spline.Add(new BezierKnot(location));
            ToFollow.FollowedBy(this);
            m_DeltaLocation = DeltaLocation;
        }
        /// <summary>
        /// The object To Change Follow
        /// </summary>
        /// <param name="ToUnFollow">THe Old Object To UnFollow</param>
        /// <param name="location">THe Location Of The Spline</param>
        /// <param name="DeltaLocation">The Delta Location To Use</param>
        /// <param name="ToFollow">The Object To Follow</param>
        public void ChangeFollow(SplineAttachedObject ToUnFollow,Vector3 location, Vector3 DeltaLocation, SplineAttachedObject ToFollow)
        {
            ToUnFollow.UnfollowedBy(this);

            var index = m_Knot[ToUnFollow];

            m_Knot.Remove(ToUnFollow);

            m_SplineContainer.Spline[index] = new BezierKnot(location);

            m_Knot.Add(ToFollow, index);

            ToFollow.FollowedBy(this);

            m_DeltaLocation = DeltaLocation;
        }
        public void SetSplineContainer(SplineContainer container)
        {
            if (m_SplineContainer != null)
                Destroy(m_SplineContainer);
            m_SplineContainer= container;
        }
    }
}