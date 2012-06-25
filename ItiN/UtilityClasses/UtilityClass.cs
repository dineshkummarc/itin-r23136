#region WatiN Copyright (C) 2006-2007 Jeroen van Menen

//Copyright 2006-2007 Jeroen van Menen
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

#endregion Copyright

#region ItiN Copyright (C) 2007 Bruce McLeod

//Copyright 2006-2007 Jeroen van Menen
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

#endregion Copyright


using System;
using System.Collections.Generic;
using System.Text;

namespace ItiN
{
    class UtilityClass
    {

        /// <summary>
        /// Compares the class names.
        /// </summary>
        /// <param name="hWnd">The hWND of the window if which the class name should be retrieved.</param>
        /// <param name="expectedClassName">Expected name of the class.</param>
        /// <returns></returns>
        public static bool CompareClassNames(IntPtr hWnd, string expectedClassName)
        {
            if (hWnd == IntPtr.Zero) return false;

            string className = NativeMethods.GetClassName(hWnd);

            return className.Equals(expectedClassName);
        }


        /// <summary>
        /// Determines whether the specified <paramref name="value" /> is null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(string value)
        {
            return (value == null || value.Length == 0);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value" /> is null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNullOrEmpty(string value)
        {
            return !IsNullOrEmpty(value);
        }
    }
}
