﻿#pragma checksum "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\MainWindows\StakeHolders.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "73D407145EB749C80C91ED797EDC393A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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
using System.Windows.Forms.Integration;
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


namespace MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.MainWindows {
    
    
    /// <summary>
    /// StakeHolders
    /// </summary>
    public partial class StakeHolders : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 38 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\MainWindows\StakeHolders.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid_vendorManager;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\MainWindows\StakeHolders.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid_customerManager;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\MainWindows\StakeHolders.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid_bankManager;
        
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
            System.Uri resourceLocater = new System.Uri("/MerchantSharp;component/sanmarksolutions/merchantsharpapp/view/mainwindows/stake" +
                    "holders.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\MainWindows\StakeHolders.xaml"
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
            this.grid_vendorManager = ((System.Windows.Controls.Grid)(target));
            
            #line 38 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\MainWindows\StakeHolders.xaml"
            this.grid_vendorManager.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.grid_vendorManager_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grid_customerManager = ((System.Windows.Controls.Grid)(target));
            
            #line 42 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\MainWindows\StakeHolders.xaml"
            this.grid_customerManager.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.grid_customerManager_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.grid_bankManager = ((System.Windows.Controls.Grid)(target));
            
            #line 46 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\MainWindows\StakeHolders.xaml"
            this.grid_bankManager.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.grid_bankManager_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

