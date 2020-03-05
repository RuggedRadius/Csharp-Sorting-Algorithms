using AT2_3.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace AT2_3
{
    public enum SortMethod
    {
        Merge,
        Heap,
        Quick,
        Shell,
        Bubble,
        Quantum_Bogo
    }
    public partial class MainWindow : Window
    {
        List<int> salaries = new List<int>();

        public MainWindow()
        {
            InitializeComponent();

            // Initialise combo box selection
            cmbSortType.ItemsSource = Enum.GetValues(typeof(SortMethod));

            ToggleSonic(false);
        }       

        void DisplayList()
        {
            Dispatcher.Invoke(new Action(() => 
            {
                // Clear list view
                lstSalaries.Items.Clear();

                // Populate list view with salaries
                foreach (int salary in salaries)
                {
                    lstSalaries.Items.Add("$" + salary);
                }
            })); 
        }

        async void GenerateSalaries(List<int> salaryList, int amount)
        {
            // Set count in delay time based on size of number
            int modulusAmount = ((amount.ToString().Length) * (10 * amount.ToString().Length) * 10);

            for (int i = 0; i < amount; i++)
            {
                // Add salary to list
                salaryList.Add(SalaryMaker.GenerateRandomSalary());

                // Update label
                lblSalariesCount.Content = salaryList.Count.ToString();

                if (modulusAmount > 0)
                {
                    // Pause to display count in
                    if (i % (modulusAmount) == 0)
                    {
                        await SmallDelay(1);
                    }
                }
            }

            // Update list view
            DisplayList();
        }
        async Task SmallDelay(int delay)
        {
            await Task.Delay(System.TimeSpan.FromMilliseconds(delay));
        }
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int amount = Int32.Parse(txtGenerateAmount.Text);
                if (amount < 0)
                {
                    throw new Exception("Cannot generate a negative amount of salaries.");
                }
                GenerateSalaries(salaries, amount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Please enter a valid whole number into the generation amount field.\r\n\r\n" + ex.Message,
                    "Not a number fool",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
        }
        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSortType.SelectedItem == null)
            {
                MessageBox.Show(
                    "Select a sorting method from the drop down menu.",
                    "No method selected",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
            else
            {


                // Get selected sorting method
                SortMethod method = (SortMethod)cmbSortType.SelectedItem;

                // Sort salaries
                Task.Run(() => Sort(salaries, method));
            }
        }
        public void Sort(List<int> salaries, SortMethod type)
        {
            ToggleSonic(true);

            // Start the timer
            var timer = System.Diagnostics.Stopwatch.StartNew();

            // Sort according to method selection
            switch (type)
            {
                case SortMethod.Merge:
                    salaries = Sorting.MergeSort.Sort_Merge(salaries);
                    break;

                case SortMethod.Heap:
                    Sorting.HeapSort.Sort_Heap(salaries, salaries.Count);
                    break;

                case SortMethod.Quick:
                    Sorting.QuickSort.Quick_Sort(salaries, 0, salaries.Count - 1);
                    break;

                case SortMethod.Shell:
                    Sorting.ShellSort.Sort_Shell(salaries, salaries.Count);
                    break;

                case SortMethod.Bubble:
                    Sorting.BubbleSort.Sort_Bubble(salaries);
                    break;

                case SortMethod.Quantum_Bogo:
                    salaries = Sorting.QuantumBogoSort.Sort_QuantumBogo(salaries);
                    break;
            }

            // Stop timer
            timer.Stop();

            // Calculate time taken to complete sort method
            var elapsedTime = timer.ElapsedMilliseconds;

            // Turn off sanic
            ToggleSonic(false);

            // Update list view
            DisplayList();

            // Display results
            MessageBox.Show(
                string.Format("Sorted by {0}.\r\n\r\nTime taken: {1}ms.", type.ToString(), elapsedTime.ToString()),
                "Sorted",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );
        }
        public void ToggleSonic(bool state)
        {
            switch (state)
            {
                case true:
                    // Turn sanic on
                    this.Dispatcher.Invoke((Action)(() => 
                    {
                        imgSanic.Visibility = Visibility.Visible;
                    })); 
                    break;

                case false:
                    // Turn sanic off
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        imgSanic.Visibility = Visibility.Hidden;
                    }));
                    break;
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            // Clear salaries
            salaries.Clear();

            // Update label
            lblSalariesCount.Content = 0;
        }
        private void wndMain_Loaded(object sender, RoutedEventArgs e)
        {
            //Uri newUri = new Uri("sanic_music.mp3", UriKind.Relative);
            //SoundPlayer player = new SoundPlayer("sanic_music.mp3");
            //player.Load();
            //player.Play();
        }
    }
}
