using System;

namespace Worldreaver.UniUI
{
    public static class ProgressHelper
    {
        /// <summary>
        /// Caculate duration update
        /// </summary>
        /// <param name="_"></param>
        /// <param name="oneUnitValue">value of one unit</param>
        /// <param name="percentIncreate">percent increate of each time</param>
        /// <param name="time">base time</param>
        /// <returns></returns>
        public static float CaculateDuration(float _,
            float oneUnitValue,
            float percentIncreate,
            float time)
        {
            var count = Math.Abs(_) / oneUnitValue;
            if (count > 1f)
            {
                return (1 + count * percentIncreate / 100) * time;
            }

            return time * count;
        }
    }
}