using System;

namespace Worldreaver.UniUI
{
    public interface IProgressMode
    {
        /// <summary>
        /// return IDisposable (foregroundDid, foregroundDo, delayedDid, delayedDo)
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        (IDisposable, IDisposable, IDisposable, IDisposable) Initialized(IProgress progress);
    }
}