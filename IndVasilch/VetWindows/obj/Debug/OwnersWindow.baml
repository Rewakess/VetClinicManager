   M S B A M L   `   `   `  ����E  AVetWindows, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:VetWindows
VetWindows  5   	       VetWindows.OwnersWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:VetWindows     5   	     � Title$+  %Клиенты и их питомцы��5   	   $	��600��6&   $
��1000��63    � WindowStartupLocation$ CenterScreen=�5	   	   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $<��6Список клиентов и их питомцев��6!   $��20(�6F   $��10q�6T   $��Center=�6`   �� 5   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��
10,0,10,10q�6;   $
��Left=�6O   .+�5      �� $'��!Поиск по телефону:��6   $��Center=�62   $��0,0,10,0q�6M   �� 5      -   5         ��Name$ phoneSearchTextBox��5      $	��200��62   $��25��6>   $��Center=�6J   6l   �� 5      -   5        $%��Добавить клиента��5      $	��150��60   $��30��6<   $��10q�6H   $��Right=�6T   6p   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ ownersDataGrid��5      $��2��5      $��10q�5         AutoGenerateColumns .     
IsReadOnly .    AlternatingRowBackground
 �����	   GridLinesVisibility$		 All=�5!      
   CanUserResizeRows
 .     CanUserAddRows .     CanUserDeleteRows .     SelectionMode$ Single=�5%         SelectionUnit$ FullRow=�5&         HeadersVisibility$ Column=�5'         RowHeaderWidth$ 0Q�5(         Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6M   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *	OwnerID+6M     5+      $ Имя клиента��6%   $	 150 6S    6:   �� *Name+6S     5,      $ Телефон��6%      SortMemberPath$ PhoneAsNumber��6P   $	 120 6o    66   �� *Phone+6o     5-      $ Email��6%   $	 150 6N    64   �� *Email+6N     5.      $ Питомцы��6%   $ 	PetsCount��6S   $ * 6n    66   �� *
PetsInfo+6n   50      51      52      