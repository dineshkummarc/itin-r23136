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
using ItiN;

namespace ItiN.Exceptions
{
    /// <summary>
    /// Base class for Exceptions thrown by WatiN.
    /// </summary>
    public class ItiNException : Exception
    {
        public ItiNException() : base() { }
        public ItiNException(string message) : base(message) { }
        public ItiNException(string message, Exception innerexception) : base(message, innerexception) { }
    }

    /// <summary>
    /// Thrown if no more alerts are available when calling PopUpWatcher.PopAlert.
    /// </summary>
    public class MissingAlertException : ItiNException
    { }

    /// <summary>
    /// Thrown if the searched for element can't be found.
    /// </summary>
    public class ElementNotFoundException : ItiNException
    {
        public ElementNotFoundException(string tagName, string attributeName, string value)
            :
          base(createMessage(attributeName, tagName, value))
        { }

        public ElementNotFoundException(string tagName, string attributeName, string value, Exception innerexception)
            :
          base(createMessage(attributeName, tagName, value), innerexception)
        { }

        private static string createMessage(string attributeName, string tagName, string value)
        {
            return "Could not find a '" + tagName + "' tag containing attribute " + attributeName + " with value '" + value + "'";
        }
    }

    /// <summary>
    /// Thrown if the searched for frame can't be found.
    /// </summary>
    public class FrameNotFoundException : ItiNException
    {
        public FrameNotFoundException(string attributeName, string value)
            :
          base("Could not find a Frame or IFrame by " + attributeName + " with value '" + value + "'")
        { }
    }

    /// <summary>
    /// Thrown if the searched for HtmlDialog can't be found.
    /// </summary>
    public class HtmlDialogNotFoundException : ItiNException
    {
        public HtmlDialogNotFoundException(string attributeName, string value, int waitTimeInSeconds)
            :
          base("Could not find a HTMLDialog by " + attributeName + " with value '" + value + "'. (Search expired after '" + waitTimeInSeconds.ToString() + "' seconds)")
        { }
    }

    /// <summary>
    /// Thrown if the searched for PopupDialog can't be found.
    /// </summary>
    public class PopupDialogNotFoundException : ItiNException
    {
        public PopupDialogNotFoundException(string attributeName, string value, int waitTimeInSeconds)
            :
          base("Could not find a PopupDialog by " + attributeName + " with value '" + value + "'. (Search expired after '" + waitTimeInSeconds.ToString() + "' seconds)")
        { }
    }
    

    /// <summary>
    /// Thrown if the searched for internet explorer (IE) can't be found.
    /// </summary>
    public class IENotFoundException : ItiNException
    {
        public IENotFoundException(string findBy, string value, int waitTimeInSeconds)
            :
          base("Could not find an IE window by " + findBy + " with value '" + value + "'. (Search expired after '" + waitTimeInSeconds.ToString() + "' seconds)")
        { }
    }

    /// <summary>
    /// Thrown if an element is disabled and the current action (like clicking a
    /// disabled link) is not allowed.
    /// </summary>
    public class ElementDisabledException : ItiNException
    {
        public ElementDisabledException(string elementId)
            :
          base("Element with Id:" + elementId + " is disabled")
        { }
    }

    /// <summary>
    /// Thrown if an element is readonly and the current action (like TextField.TypeText) a
    /// is not allowed.
    /// </summary>
    public class ElementReadOnlyException : ItiNException
    {
        public ElementReadOnlyException(string elementId)
            :
          base("Element with Id:" + elementId + " is readonly")
        { }
    }

    /// <summary>
    /// Thrown if the searched for selectlist item (option) can't be found.
    /// </summary>
    public class SelectListItemNotFoundException : ItiNException
    {
        public SelectListItemNotFoundException(string value)
            :
          base("No item with text or value '" + value + "' was found in the selectlist")
        { }
    }

    /// <summary>
    /// Thrown if waiting for a webpage or element times out.
    /// </summary>
    public class TimeoutException : ItiNException
    {
        public TimeoutException(string value)
            : base("Timeout while '" + value + "'")
        { }
        public TimeoutException(string value, Exception innerException)
            : base("Timeout while '" + value + "'", innerException)
        { }
    }

    /// <summary>
    /// Thrown if the specified attribute isn't a valid attribute of the element. 
    /// For example doing <c>TextField.GetAttribute("src")</c> will throw 
    /// this exception.
    /// </summary>
    public class InvalidAttributException : ItiNException
    {
        public InvalidAttributException(string atributeName, string elementTag)
            :
          base("Invalid attribute '" + atributeName + "' for element '" + elementTag + "'")
        { }
    }

    public class ReferenceCountException : ItiNException
    {
    }

    public class ReEntryException : ItiNException
    {
        public ReEntryException(ItiN.Attribute attribute)
            : base(createMessage(attribute))
        { }

        private static string createMessage(ItiN.Attribute attribute)
        {
            return string.Format("The compare methode of an Attribute class can't be reentered during execution of the compare. The exception occurred in an instance of '{0}' searching for '{1}' in attribute '{2}'.", attribute.GetType().ToString(), attribute.Value, attribute.AttributeName);
        }
    }

    /// <summary>
    /// Thrown if a (java) script failed to run. The innerexception returns the actual exception.
    /// </summary>
    public class RunScriptException : ItiNException
    {
        public RunScriptException(Exception innerException)
            : base("RunScript failed", innerException)
        { }
    }

}
