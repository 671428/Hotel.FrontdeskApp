using System;
using System.Windows;
using System.Windows.Controls;
using SharedModels;

namespace Hotel.FrontdeskApp
{
    public partial class AddTaskWindow : Window
    {
        public SharedModels.Task NewTask { get; private set; }

        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void SaveTaskButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int roomNumber = int.Parse(RoomNumberTextBox.Text);
                string taskType = (TaskTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
                string taskNotes = TaskNotesTextBox.Text;

                if (string.IsNullOrEmpty(taskType))
                {
                    MessageBox.Show("Please select a task type.");
                    return;
                }

                NewTask = new SharedModels.Task
                {
                    RoomNumber = roomNumber,
                    TaskType = taskType,
                    TaskNotes = taskNotes,
                    TaskStatus = "New",
                    CreatedAt = DateTime.Now
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving task: {ex.Message}");
            }
        }
    }
}
