using System;
using System.Windows;
using VetData;

namespace VetWindows
{
    public partial class RegisterOwnerWindow : Window
    {
        private readonly OwnersRepository ownersRepository;

        public RegisterOwnerWindow(OwnersRepository ownersRepository)
        {
            InitializeComponent();
            this.ownersRepository = ownersRepository;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Пожалуйста, заполните все поля ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                ownersRepository.AddOwner(name, phone, email);
                MessageBox.Show("Владелец успешно зарегистрирован ", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при регистрации владельца: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
