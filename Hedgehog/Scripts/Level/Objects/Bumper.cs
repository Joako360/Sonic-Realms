﻿using Hedgehog.Core.Actors;
using Hedgehog.Core.Triggers;
using Hedgehog.Core.Utils;
using UnityEngine;

namespace Hedgehog.Level.Objects
{
    /// <summary>
    /// Bumpers like the ones in Spring Yard and Carnival Night.
    /// </summary>
    [AddComponentMenu("Hedgehog/Objects/Bumper")]
    public class Bumper : ReactivePlatform
    {
        /// <summary>
        /// The velocity at which the controller bounces off.
        /// </summary>
        [SerializeField]
        [Tooltip("The velocity at which the controller bounces off.")]
        public float Velocity;

        /// <summary>
        /// Whether to make the controller bounce accurately (like a ball off a surface) or have its speed set
        /// directly.
        /// </summary>
        [SerializeField]
        [Tooltip("Whether to make the controller bounce accurately (like a ball off a surface) or have its speed set " +
                 "directly.")]
        public bool AccurateBounce;

        /// <summary>
        /// Whether to lock the controller's horizontal controller after being hit.
        /// </summary>
        [SerializeField]
        [Tooltip("Whether to lock the controller's horizontal control after being hit.")]
        public bool LockControl;

        public override bool ActivatesObject
        {
            get { return true; }
        }

        public void Reset()
        {
            Velocity = 10.0f;
            AccurateBounce = false;
            LockControl = false;
        }

        public override void Start()
        {
            base.Start();
            AutoActivate = false;
        }

        public override void OnPlatformEnter(TerrainCastHit hit)
        {
            hit.Controller.Detach();
            if(LockControl) hit.Controller.GroundControl.Lock();
           
            var normal = hit.NormalAngle;

            if (AccurateBounce)
            {
                hit.Controller.Velocity = new Vector2(hit.Controller.Velocity.x * Mathf.Abs(Mathf.Sin(normal)),
                    hit.Controller.Velocity.y * Mathf.Abs(Mathf.Cos(normal)));
                hit.Controller.Velocity += DMath.AngleToVector(normal) * Velocity;
            }
            else
            {
                hit.Controller.Velocity = DMath.AngleToVector(normal) * Velocity;
            }

            TriggerObject();
        }
    }
}
