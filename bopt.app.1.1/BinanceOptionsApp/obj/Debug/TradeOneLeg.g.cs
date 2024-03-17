﻿#pragma checksum "..\..\TradeOneLeg.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "7163CD779A90557DFCBFE289912BC3D4121F79981E24AB3D0293FB0247CBEF14"
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
    /// TradeOneLeg
    /// </summary>
    public partial class TradeOneLeg : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 54 "..\..\TradeOneLeg.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal BinanceOptionsApp.ProviderControl fast;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\TradeOneLeg.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal BinanceOptionsApp.ProviderControl slow;
        
        #line default
        #line hidden
        
        
        #line 159 "..\..\TradeOneLeg.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buStart;
        
        #line default
        #line hidden
        
        
        #line 161 "..\..\TradeOneLeg.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buStop;
        
        #line default
        #line hidden
        
        
        #line 358 "..\..\TradeOneLeg.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid workTimeGrid;
        
        #line default
        #line hidden
        
        
        #line 377 "..\..\TradeOneLeg.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbOpenOrderType;
        
        #line default
        #line hidden
        
        
        #line 428 "..\..\TradeOneLeg.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid workTimeGrid1;
        
        #line default
        #line hidden
        
        
        #line 448 "..\..\TradeOneLeg.xaml"
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
            System.Uri resourceLocater = new System.Uri("/BinanceOptionsApp;component/tradeoneleg.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\TradeOneLeg.xaml"
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
            
            #line 159 "..\..\TradeOneLeg.xaml"
            this.buStart.Click += new System.Windows.RoutedEventHandler(this.BuStart_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.buStop = ((System.Windows.Controls.Button)(target));
            
            #line 161 "..\..\TradeOneLeg.xaml"
            this.buStop.Click += new System.Windows.RoutedEventHandler(this.BuStop_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.workTimeGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.tbOpenOrderType = ((System.Windows.Controls.TextBlock)(target));
            
            #line 377 "..\..\TradeOneLeg.xaml"
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
