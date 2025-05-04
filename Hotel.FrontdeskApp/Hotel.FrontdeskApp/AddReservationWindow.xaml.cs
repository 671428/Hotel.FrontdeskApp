using System;
using System.Windows;
using SharedModels;

namespace Hotel.FrontdeskApp
{
    public partial class AddReservationWindow : Window
    {
        public Reservation NewReservation { get; private set; }

        public AddReservationWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int roomNumber = int.Parse(RoomNumberTextBox.Text);
                string customerEmail = CustomerEmailTextBox.Text;
                DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.Now;
                DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.Now.AddDays(1);

                if (string.IsNullOrEmpty(customerEmail))
                {
                    MessageBox.Show("Please enter a customer email.");
                    return;
                }

                NewReservation = new Reservation
                {
                    RoomId = roomNumber, // midlertidig romnummer som RoomId, vi mapper riktig i MainWindow
                    UserId = customerEmail,
                    StartDate = startDate,
                    EndDate = endDate
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving reservation: {ex.Message}");
            }
        }
    }
}
