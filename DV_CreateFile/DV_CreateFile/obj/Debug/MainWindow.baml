   M S B A M L   `   `   `  ����  DV_CreateFile    DV_CreateFile.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Neue Datei erstellen��5   	   $��206.375��6&   $	��313��67    � 
ResizeMode$ CanMinimize=�6C   .��5      � $
��Left=�6   $	��317��6&   $��0,0,0,-1q�62    � ColumnDefinitions 5   
   �� 5      $
��165* �6   �� 5      $
��163* �6   .+�5   
   �� $��Zeilenanzahl:
(Min. 10)��6   $
��Left=�6;   $��	10,38,0,0q�6V   $	��Top=�6i   $	��117��6�   $��47��6�   �� 5   
   -   5         ��Name$ txtLines��5      $
��Left=�6$   $��23��6?   $��
111,48,0,0q�6K    �� TextWrapping$
 Wrap=�6_   $	��Top=�6s   $	��170��6�   $	��100��6�   $��2��6�   �� 5   
   -   5         ��Name$ btCreate��5      $��	Erstellen��6#   $
��Left=�67   $��
53,121,0,0q�6R   $	��Top=�6f   $��69��6~   $��22��6�   $��1��6�   �� 5   
   -   5         ��Name$ chbWrongamount��5      $��Nicht ausgeglichen��6+   $
��Left=�6H   $��
111,90,0,0q�6c   $	��Top=�6w   $��2��6�   �� 5   
   $��Beträge von��6   $
��Left=�6&   $��10,7,0,0q�6A   $	��Top=�6S    J� RenderTransformOrigin$ -0.105,-1.615&�6k   �� 5   
   -   5        $ txtMin��5      $
��Left=�6"   $��23��6=   $��
111,10,0,0q�6I   $
 Wrap=�6]   $	��Top=�6q   $��71��6�   $��2��6�   $��-1000��6�   �� 5   
   $	��bis��6   $
��Left=�6   $��23,7,0,0q�69   $	��Top=�6K   $ 
3.76,0.346&�6c   $��1��6�   �� 5   
   -   5        $ txtMax��5      $��1��6"   $
��Left=�62   $��23��6M   $��	53,10,0,0q�6Y   $
 Wrap=�6l   $	��Top=�6�   $��69��6�   $
��1000��6�   5      5      