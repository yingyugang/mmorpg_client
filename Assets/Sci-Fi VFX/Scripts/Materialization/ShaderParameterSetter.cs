using UnityEngine;

namespace Assets.Scripts.Materialization
{
    internal sealed class ShaderParameterSetter : MonoBehaviour
    {
        private const string ShaderName = "QFX/SF_VFX/Materialization";
        private const string ParameterName = "_Center";

        public Vector3 ParameterOffset;

        private Renderer _rend;

        private void Start()
        {
            _rend = GetComponent<Renderer>();
            _rend.material.shader = Shader.Find(ShaderName);
        }

        private void Update()
        {
            _rend.material.SetVector(ParameterName, transform.position + ParameterOffset);
        }
    }
}