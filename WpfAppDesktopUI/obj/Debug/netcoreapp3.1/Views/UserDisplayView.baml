   M S B A M L   `   `   `  ����J  FWpfAppDesktopUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:WpfAppDesktopUI.ViewsWpfAppDesktopUI.Views  5      +    %WpfAppDesktopUI.Views.UserDisplayView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:WpfAppDesktopUI.Views     5        � Language$  
bs-Latn-BA�5      
�������$��24(�5	   J   .��5
      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� $��User Administration��6   $��48(�6/   $��0��6=   $��0��5      $��3��6$   $��0 0 0 20q�68   �� 5    
   $��Users:��6   $��1��6"   $��0��6/   �� 5"   
   -   5         ��Name$ Users��5"      $��2��6!   $��0��6.   $	��200��5#      $	��150��6#    �� SelectedItem$ SelectedUser��62   ��5$      �� 5%      .R�5&      ��  �� Orientation$ Vertical=�6!   .+�5'      �� ��6$   �� *Email+�� 5(      ��6$   �� *
RoleList+5)      5*      5,      �� 5.   
   $ Vertical=�6   $��1��6,   $��1��69   $��2��6I   $��20 0 0 0q�6Z   .+�5/      �� -   5         ��Name$ SelectedUserName��5/      $��0 0 0 10q�62   �� 50      $��Current roles��6   �� 51      $ 
Horizontal=�6   $��0 0 0 0q�62   .+�52      �� -   5        $ SelectedUserRoles��52       65   �� *SelectedRoleToRemove+6e   �� 53      -   5         ��Name$ RemoveSelectedRole��53      $��Remove Role��65   $��5 0 0 0q�6K   6^   54      �� 55      $ 
Horizontal=�6   .+�56      �� -   5         ��Name$ AvailableRoles��56       63   �� *SelectedRoleToAdd+6`   �� 57      -   5        $ AddSelectedRole��57      $��Add Role��62   $��5 0 0 0q�6E   6X   58      59      5:      5;      