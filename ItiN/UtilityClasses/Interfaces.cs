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
using System.Threading;
using System.Text.RegularExpressions;
using mshtml;
using SHDocVw;



namespace ItiN.Interfaces
{
    public interface IElementCollection
    {
        IHTMLElementCollection Elements { get; }
    }



    /// <summary>
    /// This interface is used by <see cref="Attribute"/> to compare a searched attribute
    /// with a given attribute.
    /// </summary>
    public interface ICompare
    {
        bool Compare(string value);
    }

    public interface IAttributeBag
    {
        string GetValue(string attributename);
    }

    public interface IElementsContainer
    {
        Button Button(string elementClassName);
        Button Button(Regex elementClassName);
        Button Button(Attribute findBy);
        ButtonCollection Buttons { get; }

        Element Element(string elementClassName);
        Element Element(Regex elementClassName);
        Element Element(Attribute findBy);
        ElementCollection Elements { get; }

        CheckBox CheckBox(string elementId);
        CheckBox CheckBox(Regex elementId);
        CheckBox CheckBox(Attribute findBy);
        CheckBoxCollection CheckBoxes { get; }

        SelectList SelectList(string elementId);
        SelectList SelectList(Regex elementId);
        SelectList SelectList(Attribute findBy);
        SelectListCollection SelectLists { get; }

        TextField TextField(string elementId);
        TextField TextField(Regex elementId);
        TextField TextField(Attribute findBy);
        TextFieldCollection TextFields { get; }

        Span Span(string elementId);
        Span Span(Regex elementId);
        Span Span(Attribute findBy);
        SpanCollection Spans { get; }

        Div Div(string elementId);
        Div Div(Regex elementId);
        Div Div(Attribute findBy);
        DivCollection Divs { get; }

        #region WatiN Elements not implemented in ItiN
        


        //FileUpload FileUpload(string elementId);
        //FileUpload FileUpload(Regex elementId);
        //FileUpload FileUpload(Attribute findBy);
        //FileUploadCollection FileUploads { get; }

        //Form Form(string elementId);
        //Form Form(Regex elementId);
        //Form Form(Attribute findBy);
        //FormCollection Forms { get; }

        //Label Label(string elementId);
        //Label Label(Regex elementId);
        //Label Label(Attribute findBy);
        //LabelCollection Labels { get; }

        //Link Link(string elementId);
        //Link Link(Regex elementId);
        //Link Link(Attribute findBy);
        //LinkCollection Links { get; }

        //Para Para(string elementId);
        //Para Para(Regex elementId);
        //Para Para(Attribute findBy);
        //ParaCollection Paras { get; }

        //RadioButton RadioButton(string elementId);
        //RadioButton RadioButton(Regex elementId);
        //RadioButton RadioButton(Attribute findBy);
        //RadioButtonCollection RadioButtons { get; }



        //Table Table(string elementId);
        //Table Table(Regex elementId);
        //Table Table(Attribute findBy);
        //TableCollection Tables { get; }
        ////    TableSectionCollection TableSections { get; }

        //TableCell TableCell(string elementId);
        //TableCell TableCell(Regex elementId);
        //TableCell TableCell(Attribute findBy);
        //TableCell TableCell(string elementId, int index);
        //TableCell TableCell(Regex elementId, int index);
        //TableCellCollection TableCells { get; }

        //TableRow TableRow(string elementId);
        //TableRow TableRow(Regex elementId);
        //TableRow TableRow(Attribute findBy);
        //TableRowCollection TableRows { get; }

        //TableBody TableBody(string elementId);
        //TableBody TableBody(Regex elementId);
        //TableBody TableBody(Attribute findBy);
        //TableBodyCollection TableBodies { get; }

        //Image Image(string elementId);
        //Image Image(Regex elementId);
        //Image Image(Attribute findBy);
        //ImageCollection Images { get; }

#endregion
    }

    /// <summary>
    /// This interface is used by <see cref="TextField"/> to support both
    /// HTML input element of type text password textarea hidden and 
    /// for a HTML textarea element.
    /// </summary>
    internal interface ITextElement
    {
        int MaxLength
        {
            get;
        }

        bool ReadOnly
        {
            get;
        }

        string Value
        {
            get;
        }

        void Select();

        void SetValue(string value);

        string ToString();

        string Name
        {
            get;
        }
    }

    internal interface IWebBrowser2Processor
    {
        HTMLDocument HTMLDocument();
        void Process(IWebBrowser2 webBrowser2);
        bool Continue();
    }
}

