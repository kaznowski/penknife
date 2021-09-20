using UnityEngine;

public static class AnimatorExtension
{
    public static bool IsPlaying(this Animator self, string paramName, int layerIndex = 0)
    {
        if (!Application.isPlaying)
            return false;
        var state = self.GetCurrentAnimatorStateInfo(layerIndex);
        return state.IsName(paramName);
    }

    public static bool ContainsParam(this Animator self, string paramName)
    {
        var p = self.parameters.FirstOrDefault(parameter => parameter.name == paramName);
        return p != null;
    }
}