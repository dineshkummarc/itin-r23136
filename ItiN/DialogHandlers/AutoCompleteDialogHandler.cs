using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ItiN;

namespace ItiN
{
    public class AutoCompleteDialogHandler : BaseDialogHandler
    {
        public const string dialogStyle = "94C82084";
        public enum ButtonsEnumeration
        {
            Yes = 6,
            No = 7,
            MoreInfo = 64
        }
        private enum DialogText
        {
            textControl = 65
        }
        private ButtonsEnumeration buttonToPush;

        /// <summary>
        /// Default constructor, it will click the No button in the dialog
        /// </summary>
        public AutoCompleteDialogHandler()
        {
            buttonToPush = ButtonsEnumeration.No;
        }

        public AutoCompleteDialogHandler(ButtonsEnumeration button)
        {
            buttonToPush = button;
        }

        public override bool HandleDialog(Window window)
        {
            if (IsSaveFileDialog(window))
            {
                NativeMethods.SetForegroundWindow(window.Hwnd);
                Thread.Sleep(50);
                NativeMethods.SetActiveWindow(window.Hwnd);
                Thread.Sleep(50);
                ButtonToPush(window).Click();
                return true;
            }

            return false;
        }

        public virtual bool IsSaveFileDialog(Window window)
        {
            bool val = false;
            if ((window.StyleInHex == dialogStyle))
            {
                val = true;
            }
            return val;
        }

        private WinButton ButtonToPush(Window window)
        {
            return new WinButton((int)buttonToPush, window.Hwnd);
        }
    }
}
