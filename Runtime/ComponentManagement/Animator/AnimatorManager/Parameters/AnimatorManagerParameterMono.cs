using UnityEngine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    public class AnimatorManagerParameterMono : MonoBehaviour, IVariable<AnimatorManagerParameter>
    {
        [SerializeField] AnimatorManagerParameter _parameter;

        public AnimatorManagerParameter Value { get => _parameter; set => _parameter = value; }
    }
}