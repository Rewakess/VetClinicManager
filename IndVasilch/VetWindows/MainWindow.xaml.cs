using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using VetCore;
using VetData;

namespace VetWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly VetDataBaseWork vetData;

        public MainWindow()
        {
            InitializeComponent();
            vetData = new VetDataBaseWork();
        }

        private void LoadDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vetData.LoadScheduledVisits();
                var visits = vetData.VisitsRepository.GetAllVisits().ToList();

                if (visits == null || !visits.Any())
                {
                    MessageBox.Show("Нет визитов для отображения. Возможно, база данных пуста ", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                visitsDataGrid.ItemsSource = null;
                visitsDataGrid.Items.Clear();
                visitsDataGrid.ItemsSource = visits;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки базы данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowOwnersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ownersWindow = new OwnersWindow(vetData);
                ownersWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна клиентов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddVisitButton_Click(object sender, RoutedEventArgs e)
        {
            var addVisitWindow = new AddVisitWindow(vetData);
            if (addVisitWindow.ShowDialog() == true)
            {
                LoadDatabaseButton_Click(null, null);
            }
        }
        private void ShowUpcomingVisitsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var visits = vetData.VisitsRepository.GetUpcomingVisits().ToList();

                visitsDataGrid.ItemsSource = null;
                visitsDataGrid.Items.Clear();
                visitsDataGrid.ItemsSource = visits;

                if (visits.Count == 0)
                {
                    MessageBox.Show("Нет предстоящих визитов для отображения", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отображении будущих визитов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowAllVisitsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var allVisits = vetData.VisitsRepository.GetAllVisits().ToList();

                if (allVisits == null || !allVisits.Any())
                {
                    MessageBox.Show("Нет визитов для отображения", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                visitsDataGrid.ItemsSource = null;
                visitsDataGrid.Items.Clear();
                visitsDataGrid.ItemsSource = allVisits;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки всех визитов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExportAllVisitsButton_Click(object sender, RoutedEventArgs e)
        {
            var visits = vetData.VisitsRepository.GetAllVisits().ToList();

            if (visits.Count == 0)
            {
                MessageBox.Show("Нет визитов для экспорта в файл", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dlg = new SaveFileDialog()
            {
                FileName = "Все визиты",
                DefaultExt = ".csv",
                Filter = "CSV-files (*.csv)|*.csv"
            };

            if (dlg.ShowDialog() != true) return;

            try
            {
                using (var writer = new StreamWriter(dlg.FileName, false, Encoding.UTF8))
                {
                    writer.WriteLine("ID;Имя питомца;Возраст;Вид питомца;Порода;Дата Визита;Причина;Хозяин;Номер телефона");

                    visits.ForEach(
                        v => writer.WriteLine($"{v.ID};{v.PetName};{v.Age};{v.AnimalType};{v.Breed};{v.VisitTime:dd.MM.yyyy HH:mm};{v.Reason};{v.OwnerName};{v.OwnerPhone}")
                    );
                }


                MessageBox.Show("Экспорт выполнен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteVisitButton_Click(object sender, RoutedEventArgs e)
        {
            if (visitsDataGrid.SelectedItem is ScheduledVisit selectedVisit)
            {
                var result = MessageBox.Show($"Удаляем визит {selectedVisit.PetName}? ({selectedVisit.VisitTime:dd.MM.yyyy HH.mm})",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        vetData.VisitsRepository.DeleteVisit(selectedVisit.ID);
                        MessageBox.Show("Визит удален", "Удалено", MessageBoxButton.OK, MessageBoxImage.Information);

                        var updatedVisits = vetData.VisitsRepository.GetAllVisits().ToList();
                        visitsDataGrid.ItemsSource = null;
                        visitsDataGrid.Items.Clear();
                        visitsDataGrid.ItemsSource = updatedVisits;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка визита : {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите визит для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
