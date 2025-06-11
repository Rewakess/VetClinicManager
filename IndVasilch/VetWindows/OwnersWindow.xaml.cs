using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VetCore;
using VetData;

namespace VetWindows
{
    public partial class OwnersWindow : Window
    {
        private readonly VetDataBaseWork vetData;
        private ICollectionView ownersView;

        public OwnersWindow(VetDataBaseWork vetData)
        {
            InitializeComponent();
            this.vetData = vetData;
            LoadOwnersData();
        }

        private void LoadOwnersData()
        {
            try
            {
                var owners = vetData.OwnersRepository.GetOwnersWithPets();

                ownersView = CollectionViewSource.GetDefaultView(owners);
                ownersDataGrid.ItemsSource = ownersView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных о клиентах: {ex.Message}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PhoneSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = phoneSearchTextBox.Text.Trim();

            if (ownersView != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    ownersView.Filter = null;
                }
                else
                {
                    ownersView.Filter = item =>
                    {
                        if (item is OwnerWithPets owner)
                        {
                            return !string.IsNullOrEmpty(owner.Phone) &&
                                   owner.Phone.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                        }
                        return false;
                    };
                }


                ownersView.Refresh();
            }
        }
        private void RegisterOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterOwnerWindow(vetData.OwnersRepository)
            {
                Owner = this
            };

            if (registerWindow.ShowDialog() == true)
            {
                LoadOwnersData();
            }
        }

    }
}