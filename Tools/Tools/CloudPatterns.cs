using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCA.Tools
{
    public static class CloudPatterns
    {
        public static T CallRetrying<T>(Func<T> method, int numRetries, int secondsBetweenFailingCall)
        {
            Exception exeption = null;
            numRetries = (numRetries < 1) ? 1 : numRetries;
            for (int i = 0; i < numRetries; i++)
            {
                try
                {
                    return method();
                }
                catch (Exception ex)
                {
                    exeption = ex;
                    System.Threading.Thread.Sleep(secondsBetweenFailingCall * 1000);
                }
            }
            throw exeption;
        }
    }
}
