using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public static class ExtensionMethod
    {
        #region Contains 扩展方法
        /// <summary>
        /// Contains   
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return (source.IndexOf(value, comparisonType) >= 0);
        }
        #endregion
    }
}

