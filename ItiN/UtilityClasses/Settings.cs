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
   

    public class Settings
    {
        public int WaitForCompleteTimeOut;
        public int WaitUntilExistsTimeOut;
        private struct settingsStruct
        {
           
            public bool autoCloseDialogs;
        }

        private settingsStruct settings;

        public Settings()
        {
            SetDefaults();
        }

        private Settings(settingsStruct settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Resets this instance to the initial defaults.
        /// </summary>
        public void Reset()
        {
            SetDefaults();
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Settings Clone()
        {
            return new Settings(settings);
        }

        private void SetDefaults()
        {
            settings = new settingsStruct();
            
            this.WaitUntilExistsTimeOut = 30;
            this.WaitForCompleteTimeOut = 30;
            settings.autoCloseDialogs = true;
        }

        /// <summary>
        /// Turn auto closing of dialogs on (<c>true</c>) or off (<c>false</c>).
        /// You need to set this value before creating or attaching to any 
        /// Internet Explorer to have effect.
        /// </summary>
        public bool AutoCloseDialogs
        {
            get { return settings.autoCloseDialogs; }
            set { settings.autoCloseDialogs = value; }
        }

        private static void IfValueLessThenZeroThrowArgumentOutOfRangeException(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", "time out should be 0 seconds or more.");
            }
        }
    }
}
