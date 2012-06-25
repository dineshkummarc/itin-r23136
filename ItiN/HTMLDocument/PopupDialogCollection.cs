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

using System;
using System.Collections;
using System.Diagnostics;

namespace ItiN
{
  /// <summary>
  /// A typed collection of open <see cref="HtmlDialog" />.
  /// </summary>
  public class PopupDialogCollection : IEnumerable
  {
    private ArrayList popupDialogs;

    public PopupDialogCollection(Process infoPathProcess) 
    {
        popupDialogs = new ArrayList();

        IntPtr hWnd = IntPtr.Zero;

        foreach (ProcessThread t in infoPathProcess.Threads)
        {
            int threadId = t.Id;

            NativeMethods.EnumThreadProc callbackProc = new NativeMethods.EnumThreadProc(EnumChildForTridentDialogFrame);
            NativeMethods.EnumThreadWindows(threadId, callbackProc, hWnd);
        }
    }

    private bool EnumChildForTridentDialogFrame(IntPtr hWnd, IntPtr lParam)
    {
        if (PopupDialog.IsWarningPopup(hWnd, DialogWatcher.WarningStyleInHex))
      {
          PopupDialog popupDialog = new PopupDialog(hWnd);
          popupDialogs.Add(popupDialog);
      }

      return true;
    }


      public int Length { get { return popupDialogs.Count; } }

      public PopupDialog this[int index] 
    { 
      get
      {
          return GetPopupDialogByIndex(popupDialogs, index);
      } 
    }

    public void CloseAll()
    {
      // Close all open HTMLDialogs and don't WaitForComplete for each HTMLDialog
      foreach(PopupDialog popupDialog in popupDialogs)
      {
          popupDialog.Close();
      }
    }
    
    public bool Exists(Attribute findBy)
    {
        foreach (PopupDialog popupDialog in popupDialogs)
      {
          if (findBy.Compare(popupDialog))
        {
          return true;
        }
      }
      
      return false;
    }

      private static PopupDialog GetPopupDialogByIndex(ArrayList popupDialogs, int index)
        {
            PopupDialog popupDialog = (PopupDialog)popupDialogs[index];

            return popupDialog;
        }

      
    /// <exclude />
    public Enumerator GetEnumerator() 
    {
      return new Enumerator(popupDialogs);
    }

    IEnumerator IEnumerable.GetEnumerator() 
    {
      return GetEnumerator();
    }

    /// <exclude />
      public class Enumerator : IEnumerator
      {
          ArrayList popupDialogs;
          int index;
          public Enumerator(ArrayList popupDialogs)
          {
              this.popupDialogs = popupDialogs;
              Reset();
          }

          public void Reset()
          {
              index = -1;
          }

          public bool MoveNext()
          {
              ++index;
              return index < popupDialogs.Count;
          }

          public PopupDialog Current
          {
              get
              {
                  return GetPopupDialogByIndex(popupDialogs, index);
              }
          }

          object IEnumerator.Current { get { return Current; } }
      }
  }
}