﻿#pragma checksum "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\StakeHolders\AddVendor.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "63A9E9F75972FE9DDBE934C639214A49"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CustomControls.SanmarkSolutions.WPFCustomControls.MSTextBox;
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


namespace MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.StakeHolders {
    
    
    /// <summary>
    /// AddVendor
    /// </summary>
    public partial class AddVendor : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\StakeHolders\AddVendor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CustomControls.SanmarkSolutions.WPFCustomControls.MSTextBox.MSTextBox textBox_name;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\StakeHolders\AddVendor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CustomControls.SanmarkSolutions.WPFCustomControls.MSTextBox.MSTextBox textBox_address;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\StakeHolders\AddVendor.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CustomControls.SanmarkSolutions.WPFCustomControls.MSTextBox.MSTextBox textBox_telephone;
        
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
            System.Uri resourceLocater = new System.Uri("/MerchantSharp;component/sanmarksolutions/merchantsharpapp/view/stakeholders/addv" +
                    "endor.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\StakeHolders\AddVendor.xaml"
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
            
            #line 4 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\StakeHolders\AddVendor.xaml"
            ((MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.StakeHolders.AddVendor)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\StakeHolders\AddVendor.xaml"
            ((MerchantSharp.SanmarkSolutions.MerchantSharpApp.View.StakeHolders.AddVendor)(target)).KeyUp += new System.Windows.Input.KeyEventHandler(this.Window_KeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            this.textBox_name = ((CustomControls.SanmarkSolutions.WPFCustomControls.MSTextBox.MSTextBox)(target));
            return;
            case 3:
            this.textBox_address = ((CustomControls.SanmarkSolutions.WPFCustomControls.MSTextBox.MSTextBox)(target));
            return;
            case 4:
            this.textBox_telephone = ((CustomControls.SanmarkSolutions.WPFCustomControls.MSTextBox.MSTextBox)(target));
            return;
            case 5:
            
            #line 25 "..\..\..\..\..\..\SanmarkSolutions\MerchantSharpApp\View\StakeHolders\AddVendor.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

