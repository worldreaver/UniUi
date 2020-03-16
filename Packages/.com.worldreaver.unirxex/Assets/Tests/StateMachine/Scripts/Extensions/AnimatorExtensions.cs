using UnityEngine;


public static class AnimatorExtensions
{
    #region method

    /// <summary>
    /// Is the currently playing animation finished?
    /// </summary>
    /// <param name="self">itself</param>
    /// <returns>Is the animation finished?</returns>
    public static bool IsCompleted(this Animator self)
    {
        return self.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f;
    }

    /// <summary>
    /// Is the currently playing animation in the specified state and finished?
    /// </summary>
    /// <param name="self">itself</param>
    /// <param name="stateHash">Hash of specified state</param>
    /// <returns></returns>
    public static bool IsCompleted(this Animator self, int stateHash)
    {
        return self.GetCurrentAnimatorStateInfo(0).shortNameHash == stateHash && self.IsCompleted();
    }

    /// <summary>
    /// Has the specified time (percentage) of the currently playing animation passed?
    /// </summary>
    /// <param name="self">itself</param>
    /// <param name="normalizedTime">Specified time (ratio) 0.0f (start) to 1.0f (end)</param>
    /// <returns>Has the specified time (percentage) of the animation passed?</returns>
    public static bool IsPassed(this Animator self, float normalizedTime)
    {
        return self.GetCurrentAnimatorStateInfo(0).normalizedTime > normalizedTime;
    }

    /// <summary>
    /// Play the animation from the beginning
    /// </summary>
    /// <param name="self">itself</param>
    /// <param name="shortNameHash">Animation hash</param>
    public static void PlayBegin(this Animator self, int shortNameHash)
    {
        self.Play(shortNameHash, 0, 0.0f);
    }

    #endregion
}