   M S B A M L   `   `   `  ����A  =makets, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:makets.pagesmakets.pages  5   	        makets.pages.PasswordReset   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:makets.pages     5   	     � Title$  PasswordReset��5   	   $	��390��6   $	��350��6,   ���	#FF86ABC3 � 	Resources 5
         TextBoxBaseControlTemplate �� 
TargetType ��Name �� CornerRadius ��Name �� Triggers P� Property P� Value	 �� Value
 �� 
TargetName �� Property��%�  &	        �� 5   
   " ��6=   .R�5      �� $ Bd��5   
   #��z��5      
���   �#��z��5   
   $ 10��6>   ._�5      �� $ PART_ContentHost��6   5       5      P� 5       � {�$ False��6/   . �5      �� #	 � ��63   $
 Bd��6u    � ���� 5      #	 � ��63    � ��5      P� 5       � ��$
 Auto��6+   . �5      ��  � ��$		 100��61   5      P� 5       � ��$
 Auto��6,   . �5      ��  � ��$	 20��62   5      5      5      .��5      �  � RowDefinitions 5    
   �� 5!      �� 5"      �� 5#      .+�5%   
   �� $��1��6   .+�5&      �� -   5         ��Name$ EmailBox��5&   M   #��[  6   6M   �� 5'      $��30��6   $ 15��6!   ���#09283d �� Padding$ 4q�6H   $��0 10 0 0q�6T   ._�5(      �� -   5         ��Name$ SendEmailBtn��5(   -   $��Отправить��6   ���#09283d
������ 
�������$��22��6�   $	��Top=�6�   5)      5*      �� 5+   
   -   5        $ ClaimCodeBox��5+   �   $��2��6   #��[  6   $	��150��6V   $��30��6b   $	��Top=�6n   6�   �� 5,   
   $��30��6   $ 15��6   ���#09283d$ 4q�6D   $��2��6P   ._�5-      �� -   5        $��Отправить��5-      ���#09283d
������ 
�������$��22��6�   $	��Top=�6�   5.      5/      50      