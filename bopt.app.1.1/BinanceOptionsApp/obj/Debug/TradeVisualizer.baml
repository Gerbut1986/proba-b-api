   M S B A M L   `   `   `  ����L  HBinanceOptionsApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:BinanceOptionsApp.ControlsBinanceOptionsApp.Controls  5      N JVisualMarketsEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullV>clr-namespace:VisualMarketsEngine;assembly=VisualMarketsEngineVisualMarketsEngine 5      '    !BinanceOptionsApp.TradeVisualizer   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      =controls(clr-namespace:BinanceOptionsApp.Controls     5      Mvm>clr-namespace:VisualMarketsEngine;assembly=VisualMarketsEngine     5      .��5
      � .+�5   
   �� -   5          ��Name$
  tabs��5      .@�5      �� -   5         ��Name$ tiDiagnostics��5   +   $��Diagnostics��6   .��5      � 
������� � RowDefinitions 5      �� 5      $��2* �6(   66   �� 5      $��24 �6(   66   �� 5      $��* �6(   65   .+�5      0   *BinanceOptionsApp.Controls.DiagnosticsGrid  -   5          Name$ diagnosticsControl��5   /   $��0��6K   6Z   � 5      $��1��6   .+�5      �� ���	#3f5555ff67   �� 5      $��Center=�6$   $��Center=�6?   $��16(�6\   
����ح�LOG6�   5      �� 5      $
��Auto=�6#   $��2��6H   $
��Auto=�6U   .��5      �� -   5         ��Name$ 
logControl��5   $   $
��Wrap=�68   $��Courier)�6L   $��5q�6a   6n   5      5      5      �� 5      -   5        $ tiLiveChart��5   *   $��
Live Chart��6   $y�	Collapsed=�6?   .��5      $  VisualMarketsEngine.ChartGroup  -   5          Name$ vmLive��5       
�������
�������6W   5      �� 5       -   5        $ tiBacktestChart��5    .   $��Backtest Chart��6   $y�	Collapsed=�6G   .��5!        -   5        $ 
vmBacktest��5!       
�������
�������6]   5"      �� 5#      -	   5        $ tiLiveOrders��5#   +   $��Live Orders��6   $y�	Collapsed=�6A   .��5$      &   System.Windows.Controls.DataGrid  -
   5          Name$ dgLiveOrders��5$         AutoGenerateColumns .     
IsReadOnly . 	   CanUserAddRows	 .  
   CanUserDeleteRows
 .     CanUserReorderColumns .     CanUserResizeRows .     Columns 5&      0  *System.Windows.Controls.DataGridTextColumn  5'      ,  &System.Windows.Controls.DataGridColumn   Header$
 Side��6-   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *Side+6V     5(      $ Volume��6-    6=   �� *Volume+6Z     5)      $ 
Open Price��6-    6A   �� *	OpenPrice+6a     5*      $ 	Open Time��6-    6@   �� *
OpenTime+ �� StringFormat$ {0:HH:mm:ss.fff}��6�     5+      $ Open Execution Time(ms)��6-    6N   �� *OpenExecutionTimeMs+6x     5,      $ Open Slippage(pt)��6-    6H   �� *OpenSlippagePt+6m     5-      $ Close Price��6-    6B   �� *
ClosePrice+6c     5.      $ 
Close Time��6-    6A   �� *	CloseTime+$ {0:HH:mm:ss.fff}��6�     5/      $ Close Execution Time(ms)��6-    6O   �� *CloseExecutionTimeMs+6z     50      $ Close Slippage(pt)��6-    6I   �� *CloseSlippagePt+6o     51      $ 
Profit(pt)��6-    6A   �� *
ProfitPt+6`   53      54      �� 55      -   5        $ tiBacktestOrders��55   /   $��Backtest Orders��6   $y�	Collapsed=�6I   .��56        -   5        $ dgBacktestOrders��56       .   . 	 .  
 .   .   .   58        59      $
 Side��6-    6;   �� *Side+6V     5:      $ Volume��6-    6=   �� *Volume+6Z     5;      $ 
Open Price��6-    6A   �� *	OpenPrice+6a     5<      $ 	Open Time��6-    6@   �� *
OpenTime+$ {0:HH:mm:ss.fff}��6�     5=      $ Close Price��6-    6B   �� *
ClosePrice+6c     5>      $ 
Close Time��6-    6A   �� *	CloseTime+$ {0:HH:mm:ss.fff}��6�     5?      $ 
Profit(pt)��6-    6A   �� *
ProfitPt+6`   5A      5B      �� 5C      -   5        $ tiDebug��5C   %   $��Debug��6   $y�	Collapsed=�66   .��5D        -   5        $ dgDebug��5D       .   . 	 .  
 .   .   .     EnableRowVirtualization . !   EnableColumnVirtualization .  %� IsVirtualizing . 5G      5H      5I      5J      5K      