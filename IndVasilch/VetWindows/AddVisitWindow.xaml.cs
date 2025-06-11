using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VetCore;
using VetData;

namespace VetWindows
{
    public partial class AddVisitWindow : Window
    {
        private readonly VetDataBaseWork data;
        private List<OwnerWithPets> owners;

        public AddVisitWindow(VetDataBaseWork data)
        {
            InitializeComponent();
            this.data = data;
            LoadOwners();
        }

        private void LoadOwners()
        {
            owners = data.OwnersRepository.GetOwnersWithPets();
            OwnerNameComboBox.ItemsSource = owners.Select(o => o.Name).Distinct().ToList();
        }

        private void OwnerNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = OwnerNameComboBox.SelectedItem?.ToString();
            var filtered = owners.Where(o => o.Name == name);
            OwnerPhoneComboBox.ItemsSource = filtered.Select(o => o.Phone).Distinct().ToList();
            OwnerEmailComboBox.ItemsSource = filtered.Select(o => o.Email).Distinct().ToList();
        }

        private void OwnerPhoneComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterOtherCombos();
        }

        private void OwnerEmailComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterOtherCombos();
        }

        private void FilterOtherCombos()
        {
            var name = OwnerNameComboBox.SelectedItem?.ToString();
            var phone = OwnerPhoneComboBox.SelectedItem?.ToString();
            var email = OwnerEmailComboBox.SelectedItem?.ToString();

            var match = owners.Where(o =>
                (name == null || o.Name == name) &&
                (phone == null || o.Phone == phone) &&
                (email == null || o.Email == email)).ToList();

            if (name == null) OwnerNameComboBox.ItemsSource = match.Select(o => o.Name).Distinct();
            if (phone == null) OwnerPhoneComboBox.ItemsSource = match.Select(o => o.Phone).Distinct();
            if (email == null) OwnerEmailComboBox.ItemsSource = match.Select(o => o.Email).Distinct();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var name = OwnerNameComboBox.SelectedItem?.ToString();
                var phone = OwnerPhoneComboBox.SelectedItem?.ToString();
                var email = OwnerEmailComboBox.SelectedItem?.ToString();

                var owner = owners.FirstOrDefault(o => o.Name == name && o.Phone == phone && (string.IsNullOrWhiteSpace(email) || o.Email == email));
                if (owner == null)
                {
                    MessageBox.Show("Выберите корректного владельца ");
                    return;
                }

                if (string.IsNullOrWhiteSpace(PetNameTextBox.Text))
                {
                    MessageBox.Show("Введите имя питомца ");
                    return;
                }

                if (!int.TryParse(AgeTextBox.Text, out int age) || age < 0)
                {
                    MessageBox.Show("Возраст должен быть положительным числом ");
                    return;
                }

                if (!(AnimalTypeComboBox.SelectedItem is ComboBoxItem selectedTypeItem))
                {
                    MessageBox.Show("Выберите вид животного ");
                    return;
                }
                string animalType = selectedTypeItem.Content.ToString();

                if (!VisitDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Выберите дату визита ");
                    return;
                }

                if (string.IsNullOrWhiteSpace(VisitTimeTextBox.Text) ||
                    !TimeSpan.TryParseExact(VisitTimeTextBox.Text, "hh\\:mm", null, out TimeSpan time))
                {
                    MessageBox.Show("Введите время визита в формате HH:MM (например, 14:30).\n");
                    return;
                }

                DateTime visitDateTime = VisitDatePicker.SelectedDate.Value.Date + time;
                visitDateTime = new DateTime(
                    visitDateTime.Year,
                    visitDateTime.Month,
                    visitDateTime.Day,
                    visitDateTime.Hour,
                    visitDateTime.Minute,
                    0);

                var visit = new ScheduledVisit
                {
                    PetName = PetNameTextBox.Text,
                    Age = age,
                    OwnerID = owner.OwnerID,
                    VisitTime = visitDateTime,
                    Reason = ReasonTextBox.Text,
                    OwnerName = owner.Name,
                    OwnerPhone = owner.Phone,
                    OwnerEmail = owner.Email,
                    AnimalType = animalType,
                    Breed = string.IsNullOrWhiteSpace(BreedTextBox.Text) ? null : BreedTextBox.Text
                };

                data.VisitsRepository.AddVisit(visit);

                MessageBox.Show("Визит успешно добавлен ");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
