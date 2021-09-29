using UnityEngine;
using DoubleDash.CodingTools;

namespace DoubleDash.ComponentManagement.AnimatorManagement
{
    [UnityEngine.CreateAssetMenu(fileName = "NewAnimatorState", menuName = "DoubleDash/ComponentManagement/Animator/AnimatorState")]
    public class AnimatorManagerStateScriptable : ScriptableObject, IVariable<AnimatorManagerState>
    {
        [SerializeField] AnimatorManagerState _state;

        public AnimatorManagerState Value { get => _state; set => _state = value; }
    }
}