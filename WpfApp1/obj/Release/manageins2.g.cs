﻿#pragma checksum "..\..\manageins2.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "427887FB840E53AC37BEEF7AD09C73977B9DAD61"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WpfApp1;


namespace WpfApp1 {
    
    
    /// <summary>
    /// manageins2
    /// </summary>
    public partial class manageins2 : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 29 "..\..\manageins2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border bdrSearch;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\manageins2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox S_codeT;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\manageins2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock SearchW;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\manageins2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dtMopen;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\manageins2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn CheckBox_Col;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\manageins2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkAll;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\manageins2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOpen;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\manageins2.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnClose;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApp1;component/manageins2.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\manageins2.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.bdrSearch = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.S_codeT = ((System.Windows.Controls.TextBox)(target));
            
            #line 31 "..\..\manageins2.xaml"
            this.S_codeT.GotFocus += new System.Windows.RoutedEventHandler(this.S_codeT_GotFocus);
            
            #line default
            #line hidden
            
            #line 31 "..\..\manageins2.xaml"
            this.S_codeT.LostFocus += new System.Windows.RoutedEventHandler(this.S_codeT_LostFocus);
            
            #line default
            #line hidden
            
            #line 31 "..\..\manageins2.xaml"
            this.S_codeT.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.S_codeT_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.SearchW = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.dtMopen = ((System.Windows.Controls.DataGrid)(target));
            
            #line 42 "..\..\manageins2.xaml"
            this.dtMopen.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.DtMopen_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.CheckBox_Col = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 7:
            this.chkAll = ((System.Windows.Controls.CheckBox)(target));
            
            #line 95 "..\..\manageins2.xaml"
            this.chkAll.Click += new System.Windows.RoutedEventHandler(this.ChkAll_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnOpen = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\manageins2.xaml"
            this.btnOpen.Click += new System.Windows.RoutedEventHandler(this.BtnOpen_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnClose = ((System.Windows.Controls.Button)(target));
            
            #line 139 "..\..\manageins2.xaml"
            this.btnClose.Click += new System.Windows.RoutedEventHandler(this.BtnClose_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 49 "..\..\manageins2.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Checked += new System.Windows.RoutedEventHandler(this.CheckBox_Checked);
            
            #line default
            #line hidden
            
            #line 49 "..\..\manageins2.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Unchecked += new System.Windows.RoutedEventHandler(this.CheckBox_Unchecked);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

