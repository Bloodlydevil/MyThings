using UnityEngine;
using UnityEngine.Splines;

namespace MyThings.Spline
{
    // Currently Its Working For Global But Not For Local



    /// <summary>
    /// When The Attached Object Moves Then This Needs To Move (Assuming That A Single Attacher String Will Be Attaching To An Object)
    /// </summary>
    public class SplineAttacher : MonoBehaviour
    {
        [SerializeField] private SplineContainer m_SplineContainer;

        [SerializeField] private bool m_Local;

        private int m_Knot;

        private float m_DefaultScale;

        public Vector3 Position => m_SplineContainer.Spline[m_Knot].Position;

        public Vector3 DeltaLocation { get; set; }

        /// <summary>
        ///  Cahnge The Spline To Follow The Object
        /// </summary>
        /// <param name="attachedObject">The Position Of The Object</param>
        public void Change(Transform attachedObject)
        {
            var spline = m_SplineContainer.Spline;
            var knot = spline[m_Knot];
            if (m_Local)
            {
                // I Am Assuming That The Local Transform Will Change When The Object Moves
                // and The Object Is Just Inside Draggable Surface
                knot.Position = attachedObject.localPosition + DeltaLocation;
            }
            else
            {
                knot.Position = attachedObject.position *
                    (m_DefaultScale)
                    + DeltaLocation * m_DefaultScale / (attachedObject.localScale.x / attachedObject.lossyScale.x);
            }
            spline[m_Knot] = knot;
        }
        /// <summary>
        /// The Object To Follow
        /// </summary>
        /// <param name="location">The Location Of The Object</param>
        /// <param name="DeltaLocation">The Delta Location To Apply Every Time in Change</param>
        /// <param name="ToFollow">The object To Follow</param>
        public void Follow(Vector3 location, Vector3 DeltaLocation, SplineAttachedObject ToFollow)
        {
            m_Knot = m_SplineContainer.Spline.Count;
            m_SplineContainer.Spline.Add(new BezierKnot(location + DeltaLocation));
            ToFollow.FollowedBy(this);
            this.DeltaLocation = DeltaLocation;
            m_DefaultScale = (ToFollow.transform.localScale.x / ToFollow.transform.lossyScale.x);


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

            m_SplineContainer.Spline[m_Knot] = new BezierKnot(location+ DeltaLocation);

            ToFollow.FollowedBy(this);

            this.DeltaLocation = DeltaLocation;
        }
        public void SetSplineContainer(SplineContainer container)
        {
            m_SplineContainer= container;
        }
    }
}