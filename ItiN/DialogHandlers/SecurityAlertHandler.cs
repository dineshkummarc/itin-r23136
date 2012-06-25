using System;
using System.Collections.Generic;
using System.Text;

namespace ItiN
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
    public class SecurityAlertHandler : BaseDialogHandler
    {
        private Buttons _buttonToPush;

        public const string SecurityAlertDialogStyle = "94C808C4";

        /// <summary>
        /// Defaults to clicking yes on the dialog handler.
        /// </summary>
        public SecurityAlertHandler()
        {
            this._buttonToPush = Buttons.Yes;
        }

        /// <summary>
        /// If None is selected, it will select No and close the pop up.
        /// </summary>
        /// <param name="buttonToPush"></param>
        public SecurityAlertHandler(SecurityAlertHandler.Buttons buttonToPush)
        {
            if (buttonToPush != Buttons.None)
            {
                this._buttonToPush = buttonToPush;
            }
            else
            {
                this._buttonToPush = Buttons.No;
            }

        }

        public enum Buttons
        {
            None = 0,
            Yes = 1,
            No = 2,
        }

        public override bool HandleDialog(Window window)
        {
            if (IsSecurityAlertDialog(window))
            {
                ButtonToPush(window).Click();
                return true;
            }
            return false;
        }

        private WinButton ButtonToPush(Window window)
        {
            return new WinButton((int)this._buttonToPush, window.Hwnd);
        }

        protected virtual bool IsSecurityAlertDialog(Window window)
        {
            return window.StyleInHex == SecurityAlertDialogStyle;
        }
    }
}
