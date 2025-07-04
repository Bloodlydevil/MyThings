using System;
using UnityEngine;

namespace MyThings.Hardware
{
    [Serializable]
    public class MobileMotionDetector
    {
        public enum Strength
        {
            None,
            Weak,
            Strong,
            Extreme
        }
        private Vector3 lastAcceleration;
        [SerializeField] private float shakeThreshold;
        [SerializeField] private float steadyThreshold;

        public MobileMotionDetector(float shakeThreshold = 0.25f, float steadyThreshold = 0.05f)
        {
            this.shakeThreshold = shakeThreshold;
            this.steadyThreshold = steadyThreshold;
        }
        public void Start()
        {
            // Initialize lastAcceleration to the current acceleration
            lastAcceleration = Input.acceleration;
        }
        /// <summary>
        /// Should be called once per frame to update acceleration data
        /// </summary>
        public void Update()
        {
            lastAcceleration = Input.acceleration;
        }

        /// <summary>
        /// Returns true if the device is shaking (movement exceeds threshold)
        /// </summary>
        public bool IsShaking()
        {
            float delta = (Input.acceleration - lastAcceleration).magnitude;
            return delta > shakeThreshold;
        }

        /// <summary>
        /// Returns true if the device is relatively steady
        /// </summary>
        public bool IsSteady()
        {
            float delta = (Input.acceleration - lastAcceleration).magnitude;
            return delta < steadyThreshold;
        }

        /// <summary>
        /// Returns the current acceleration vector
        /// </summary>
        public Vector3 GetAcceleration()
        {
            return Input.acceleration;
        }

        /// <summary>
        /// Returns the tilt/rotation of the phone in Euler angles (based on gravity)
        /// </summary>
        public Vector3 GetTilt()
        {
            return Input.gyro.attitude.eulerAngles;
        }

        /// <summary>
        /// Returns the delta change in acceleration since last update
        /// </summary>
        public float GetAccelerationDelta()
        {
            return (Input.acceleration - lastAcceleration).magnitude;
        }
        /// <summary>
        /// Determines the strength of the device's flat orientation facing upward based on the Z-axis acceleration.
        /// </summary>
        /// <remarks>The method evaluates the Z-axis acceleration of the device to determine its
        /// orientation. Values closer to -1.0 indicate a stronger flat upward orientation.</remarks>
        /// <returns>A <see cref="Strength"/> value indicating the intensity of the device's flat orientation facing upward: <see
        /// cref="Strength.Extreme"/> for strong upward orientation, <see cref="Strength.Strong"/> for moderate, <see
        /// cref="Strength.Weak"/> for slight, and <see cref="Strength.None"/> if the device is not significantly facing
        /// upward.</returns>
        public Strength FlatFacingUp()
        {
            return Input.acceleration.z switch
            {
                < -0.8f => Strength.Extreme,
                < -0.5f => Strength.Strong,
                < -0.2f => Strength.Weak,
                _ => Strength.None
            };
        }
        /// <summary>
        /// Determines the strength of the device's flat-facing-down orientation based on the Z-axis acceleration.
        /// </summary>
        /// <returns>A <see cref="Strength"/> value indicating the intensity of the flat-facing-down orientation: <see
        /// cref="Strength.Extreme"/> for Z-axis acceleration greater than 0.8, <see cref="Strength.Strong"/> for values
        /// greater than 0.5, <see cref="Strength.Weak"/> for values greater than 0.2, and <see cref="Strength.None"/>
        /// for values less than or equal to 0.2.</returns>
        public Strength FlatFacingDown()
        {
            return Input.acceleration.z switch
            {
                > 0.8f => Strength.Extreme,
                > 0.5f => Strength.Strong,
                > 0.2f => Strength.Weak,
                _ => Strength.None
            };
        }
        /// <summary>
        /// Determines the strength of the device's vertical tilt facing the user based on the Y-axis acceleration.
        /// </summary>
        /// <remarks>The method evaluates the Y-axis acceleration of the device to determine the tilt
        /// strength. Negative values indicate the device is tilted downward.</remarks>
        /// <returns>A <see cref="Strength"/> value indicating the degree of vertical tilt: <see cref="Strength.Extreme"/> for a
        /// strong downward tilt, <see cref="Strength.Strong"/> for a moderate downward tilt, <see
        /// cref="Strength.Weak"/> for a slight downward tilt, or <see cref="Strength.None"/> if the device is not
        /// significantly tilted.</returns>
        public Strength VerticleFacingUser()
        {
            return Input.acceleration.y switch
            {
                < -0.8f => Strength.Extreme,
                < -0.5f => Strength.Strong,
                < -0.2f => Strength.Weak,
                _ => Strength.None
            };
        }
        /// <summary>
        /// Determines the strength of the device's vertical orientation facing away from the user based on the Y-axis
        /// acceleration.
        /// </summary>
        /// <returns>A <see cref="Strength"/> value indicating the intensity of the device's vertical orientation: <see
        /// cref="Strength.Extreme"/> if the Y-axis acceleration is greater than 0.8, <see cref="Strength.Strong"/> if greater
        /// than 0.5, <see cref="Strength.Weak"/> if greater than 0.2, or <see cref="Strength.None"/> otherwise.</returns>
        public Strength VerticleFacingAway()
        {
            return Input.acceleration.y switch
            {
                > 0.8f => Strength.Extreme,
                > 0.5f => Strength.Strong,
                > 0.2f => Strength.Weak,
                _ => Strength.None
            };
        }
        /// <summary>
        /// Determines How Left the phone is tilted based on acceleration
        /// </summary>
        /// <returns>A <see cref="Strength"/> value indicating the degree of Leftward tilt: <see cref="Strength.Extreme"/> for
        /// acceleration greater than 0.8, <see cref="Strength.Strong"/> for acceleration greater than 0.5, <see
        /// cref="Strength.Weak"/> for acceleration greater than 0.2, or <see cref="Strength.None"/> for acceleration of
        /// 0.2 or less.</returns>
        public Strength LeftTilt()
        {
            return Input.acceleration.x switch
            {
                < -0.8f => Strength.Extreme,
                < -0.5f => Strength.Strong,
                < -0.2f => Strength.Weak,
                _ => Strength.None
            };
        }
        /// <summary>
        /// Determines How Right the phone is tilted based on acceleration
        /// </summary>
        /// <returns>A <see cref="Strength"/> value indicating the degree of rightward tilt: <see cref="Strength.Extreme"/> for
        /// acceleration greater than 0.8, <see cref="Strength.Strong"/> for acceleration greater than 0.5, <see
        /// cref="Strength.Weak"/> for acceleration greater than 0.2, or <see cref="Strength.None"/> for acceleration of
        /// 0.2 or less.</returns>
        public Strength RightTilt()
        {
            return Input.acceleration.x switch
            {
                > 0.8f => Strength.Extreme,
                > 0.5f => Strength.Strong,
                > 0.2f => Strength.Weak,
                _ => Strength.None
            };
        }
        /// <summary>
        /// Get both pitch and roll angles in degrees
        /// </summary>
        public Vector2 GetPitchAndRoll()
        {
            Vector3 acc = Input.acceleration.normalized;
            float pitch = Mathf.Atan2(acc.y, acc.z) * Mathf.Rad2Deg;
            float roll = Mathf.Atan2(acc.x, acc.z) * Mathf.Rad2Deg;
            return new Vector2(pitch, roll);
        }
    }
}