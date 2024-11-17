
using UnityEngine;
using System.Collections.Generic;

namespace DigitalRuby.Earth
{
    [RequireComponent(typeof(MeshRenderer))]
    public class EarthScript : SphereScript
    {
        [Range(-1000.0f, 1000.0f)]
        [Tooltip("Rotation speed around axis")]
        public float RotationSpeed = 1.0f;

        [Tooltip("Planet axis in world vector, defaults to start up vector")]
        public Vector3 Axis;

        [Tooltip("The sun, defaults to first dir light")]
        public Light Sun;

        private MeshRenderer meshRenderer;
        private MaterialPropertyBlock materialBlock;

        private void OnEnable()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            materialBlock = new MaterialPropertyBlock();
            Sun = (Sun == null ? Light.GetLights(LightType.Directional, -1)[0] : Sun);
            if (Axis == Vector3.zero)
            {
                Axis = transform.up;
            }
            else
            {
                Axis = Axis.normalized;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (materialBlock != null && Sun != null && meshRenderer != null)
            {
                meshRenderer.GetPropertyBlock(materialBlock);
                materialBlock.SetVector("_SunDir", -Sun.transform.forward);
                materialBlock.SetVector("_SunColor", new Vector4(Sun.color.r, Sun.color.g, Sun.color.b, Sun.intensity));
                meshRenderer.SetPropertyBlock(materialBlock);
            }

#if UNITY_EDITOR

            if (Application.isPlaying)
            {

#endif

                transform.Rotate(Axis, RotationSpeed * Time.deltaTime);

#if UNITY_EDITOR

            }

#endif

        }
    }
}