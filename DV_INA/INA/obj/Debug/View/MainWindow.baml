   M S B A M L   `   `   `  ����>  :INA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:INA.ViewModelINA.ViewModel  5   	       INA.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1	ViewModelclr-namespace:INA.ViewModel     5   	   -   5          �Name$  INAMainWindow��5   7    � Title$	 INA��5   	   $	��464��6   $	��500��6"    � Icon$" /INA;component/images/db.ico��6.    � 
ResizeMode$ CanMinimize=�6R   ���	#FFE9EAF0 � 	Resources 5      ��%    5      .��5	      �  J� RenderTransformOrigin$ 0.624,0.627&�6   .+�5
   
   � $
��Left=�6   $��31��6*   $��-11,370,0,0q�66   $	��Top=�6K   $	��500��6c   .+�5      �� -   5         ��Name$ btStart��5      $��Starten��6&   $
��Left=�68   $	��Top=�6S   $��90��6k   $��30��6v   $��51,0,0,0q�6�   {�.  6�   �� 5      -   5        $ 	btBeenden��5      $��Beenden��6(   $��	360,0,0,0q�6:   $	��Top=�6M   $��30��6e   $
��Left=�6�   $��90��6�   5      �� 5   
   -   5         ��Name$ 	filesView��5      $
��Left=�6&   $	��128��6A   $��	40,88,0,0q�6N   $	��Top=�6a   $	��400��6y    �� SelectionMode$ Single=�6�   ��6�   �� *_loadedFiles+	 �� View	 5      �� 5      .P�5      �� 
 �� Width$	
 240��6%   $��	Dateiname��61   �� 5      $

 auto��6%    �� CellTemplate 5      �� 5      .R�5   "   �� -   5        $��20��5   )   $��20��64   .��5   &   ]� .+�5   *   �� $&�� /INA;component/images/delete.png��60   5   '   5   #   5      5      5      5       � 5&   
   -   5         �Name$ progBar��5&      $
��Left=�6'   $��27��6B   $��
40,221,0,0q�6N   $	��Top=�6b   $	��400��6z    � Minimum$ 0Q�6�    � Maximum$	 100Q�6�   ���	#FF689BE6 � Value 6�   �� *ProgressStatus+6�   �� 5'   
   -   5         ��Name$ tbInfo3��5'      $
��Left=�6#   $	��112��6>   $��
40,253,0,0q�6K    �� TextWrapping$
 Wrap=�6_   $	��Top=�6�   $	��400��6�   $��Visible=�6�   |�.  ��6s   �� *	_tbInfo+6�   o� 5+   
    o� 
IsMainMenu . $
��Left=�6!   $��31��6<   $��-1,0,0,0q�6H   $	��Top=�6Z   $	��493��6r   $��3��6~   .@�5,      m� $��_Datei��6   .@�5-      m� -   5        $��_Datei hinzufügen��5-       m� Icon 5.      �� 5/      $��16��6    $��16��6+   $#��/INA;component/images/add.png��67   51      �� 52      m� 53      -	   5        $��_Beenden��53       54      �� 55      $��16��6    $��16��6+   $$��/INA;component/images/exit.png��67   57      58      m� 59      -
   5        $��_Verwaltung��59      .@�5:      m� -   5         m�Name$ menu_opencompmgmnt��5:      $!��_Computerverwaltung öffnen��67    5;      �� 5<      $��16��6    $��16��6+   $%��/INA;component/images/queue.png��67   5>      m� 5?      -   5        $ menu_clearMSMQ��5?      $��_MessageQueue leeren��63    5@      �� 5A      $��16��6    $��16��6+   $+��%/INA;component/images/deletequeue.png��67   5C      5D      5E      �� 5F   
   $	��INA��6   $
��Left=�6   $��	33,36,0,0q�69   $	��Top=�6L   $��85��6d   ���#1a1d9e$��Imprint MT Shadow)�6�   $��36(�6�   5L      5M      