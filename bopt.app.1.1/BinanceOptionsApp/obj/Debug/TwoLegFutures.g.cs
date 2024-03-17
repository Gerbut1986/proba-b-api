﻿#pragma checksum "..\..\TwoLegFutures.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E6CB6D22AE737650FCFCC598D18C99A88DCCEAED8D3208F680F5E2C89F8F17C9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BinanceOptionsApp;
using BinanceOptionsApp.Controls;
using BinanceOptionsApp.Models;
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
using VisualMarketsEngine;


namespace BinanceOptionsApp {
    
    
    /// <summary>
    /// TwoLegFutures
    /// </summary>
    public partial class TwoLegFutures : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 54 "..\..\TwoLegFutures.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal BinanceOptionsApp.ProviderControl fast;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\TwoLegFutures.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal BinanceOptionsApp.ProviderControl slow;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\TwoLegFutures.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buStart;
        
        #line default
        #line hidden
        
        
        #line 161 "..\..\TwoLegFutures.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buStop;
        
        #line default
        #line hidden
        
        
        #line 364 "..\..\TwoLegFutures.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid workTimeGrid;
        
        #line default
        #line hidden
        
        
        #line 383 "..\..\TwoLegFutures.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbOpenOrderType;
        
        #line default
        #line hidden
        
        
        #line 435 "..\..\TwoLegFutures.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid workTimeGrid1;
        
        #line default
        #line hidden
        
        
        #line 455 "..\..\TwoLegFutures.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock logBlock;
        
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
            System.Uri resourceLocater = new System.Uri("/BinanceOptionsApp;component/twolegfutures.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\TwoLegFutures.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            this.fast = ((BinanceOptionsApp.ProviderControl)(target));
            return;
            case 2:
            this.slow = ((BinanceOptionsApp.ProviderControl)(target));
            return;
            case 3:
            this.buStart = ((System.Windows.Controls.Button)(target));
            
            #line 159 "..\..\TwoLegFutures.xaml"
            this.buStart.Click += new System.Windows.RoutedEventHandler(this.BuStart_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.buStop = ((System.Windows.Controls.Button)(target));
            
            #line 161 "..\..\TwoLegFutures.xaml"
            this.buStop.Click += new System.Windows.RoutedEventHandler(this.BuStop_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.workTimeGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.tbOpenOrderType = ((System.Windows.Controls.TextBlock)(target));
            
            #line 383 "..\..\TwoLegFutures.xaml"
            this.tbOpenOrderType.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.TbOpenOrderType_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.workTimeGrid1 = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.logBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
