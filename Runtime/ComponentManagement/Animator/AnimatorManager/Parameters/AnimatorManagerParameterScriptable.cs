using UnityEngine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    [UnityEngine.CreateAssetMenu(fileName = "NewAnimatorParameter", menuName = "DoubleDash/ComponentManagement/Animator/AnimatorParameter")]
    public class AnimatorManagerParameterScriptable : ScriptableObject, IVariable<AnimatorManagerParameter>
    {
        [SerializeField] AnimatorManagerParameter _parameter;

        public AnimatorManagerParameter Value { get => _parameter; set => _parameter = value; }
    }
}