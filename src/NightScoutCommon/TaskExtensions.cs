using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Meiswinkel.NightScoutReporter.NightScoutCommon
{
    public static class TaskExtensions
    {
        public static ConfiguredTaskAwaitable OnAnyContext(this Task thisPtr)
        {
            if (thisPtr == null)
            {
                throw new ArgumentNullException(nameof(thisPtr));
            }

            return thisPtr.ConfigureAwait(continueOnCapturedContext: false);
        }

        public static ConfiguredTaskAwaitable<T> OnAnyContext<T>(this Task<T> thisPtr)
        {
            if (thisPtr == null)
            {
                throw new ArgumentNullException(nameof(thisPtr));
            }

            return thisPtr.ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
