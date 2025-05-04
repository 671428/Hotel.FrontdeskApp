using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using SharedModels;

namespace Hotel.FrontdeskApp
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();
        private List<Room> rooms = new();
        private List<Reservation> reservations = new();
        private List<SharedModels.Task> tasks = new();

        public MainWindow()
        {
            InitializeComponent();
            _ = LoadDataAsync(); // Starter data lasting med en gang vinduet åpnes
        }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            try
            {
                rooms = await _apiService.GetRoomsAsync();
                reservations = await _apiService.GetReservationsAsync();
                tasks = await _apiService.GetTasksAsync();

                // ✅ Bind til GUI
                RoomList.ItemsSource = null;
                RoomList.ItemsSource = rooms;

                ReservationList.ItemsSource = null;
                ReservationList.ItemsSource = reservations;

                TaskList.ItemsSource = null;
                TaskList.ItemsSource = tasks;


                // Marker ugyldige tasks med RoomNumber som ikke finnes
                var validRoomNumbers = rooms.Select(r => r.RoomNumber).ToHashSet();
                foreach (var task in tasks)
                {
                    if (!validRoomNumbers.Contains(task.RoomNumber))
                    {
                        task.TaskNotes = "⚠ Ugyldig romnummer!";
                    }
                }

                TaskList.ItemsSource = tasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from API: {ex.Message}");
            }
        
        }

        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow();
            if (addTaskWindow.ShowDialog() == true)
            {
                var newTask = addTaskWindow.NewTask;

                // Valider at romnummer finnes
                if (!rooms.Any(r => r.RoomNumber == newTask.RoomNumber))
                {
                    MessageBox.Show($"Room number {newTask.RoomNumber} does not exist.");
                    return;
                }

                var success = await _apiService.CreateTaskAsync(newTask);

                if (success)
                {
                    MessageBox.Show("Task registered successfully!");
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show("Failed to register task.");
                }
            }
        }

        private async void UpdateTaskStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem is SharedModels.Task selectedTask)
            {
                string newStatus = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Enter new status for task ID {selectedTask.Id} (New, In Progress, Finished):",
                    "Update Task Status",
                    selectedTask.TaskStatus);

                if (!string.IsNullOrWhiteSpace(newStatus))
                {
                    var success = await _apiService.UpdateTaskStatusAsync(selectedTask.Id, newStatus);

                    if (success)
                    {
                        MessageBox.Show("Task status updated successfully!");
                        await LoadDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update task status.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a task first!");
            }
        }
    
    private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        private async void AddReservation_Click(object sender, RoutedEventArgs e)
        {
            var addReservationWindow = new AddReservationWindow();
            if (addReservationWindow.ShowDialog() == true)
            {
                var newReservation = addReservationWindow.NewReservation;

                var success = await _apiService.CreateReservationAsync(newReservation);

                if (success)
                {
                    MessageBox.Show("Reservation added successfully.");
                    await LoadDataAsync(); // Laster inn på nytt for å vise ny reservasjon
                }
                else
                {
                    MessageBox.Show("Failed to add reservation.");
                }
            }
        }

        private async void DeleteReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Reservation ID to delete:", "Delete Reservation", "");

                if (int.TryParse(input, out int reservationId))
                {
                    var success = await _apiService.DeleteReservationAsync(reservationId);

                    if (success)
                    {
                        MessageBox.Show($"Reservation {reservationId} deleted.");
                        await LoadDataAsync(); // Oppdater listen etter sletting
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete reservation. It may not exist.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Reservation ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting reservation: {ex.Message}");
            }
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Room Number to check in:", "Check In", "");

                if (int.TryParse(input, out int roomNumber))
                {
                    var roomToCheckIn = rooms.Find(r => r.RoomNumber == roomNumber);

                    if (roomToCheckIn != null)
                    {
                        if (!roomToCheckIn.IsAvailable)
                        {
                            MessageBox.Show($"Room {roomNumber} is already checked in.");
                        }
                        else
                        {
                            roomToCheckIn.IsAvailable = false;
                            RoomList.ItemsSource = null;
                            RoomList.ItemsSource = rooms;
                            MessageBox.Show($"Guest checked in to room {roomNumber}.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Room not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Room Number.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during check-in: {ex.Message}");
            }
        }

        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Room Number to check out:", "Check Out", "");

                if (int.TryParse(input, out int roomNumber))
                {
                    var roomToCheckOut = rooms.Find(r => r.RoomNumber == roomNumber);

                    if (roomToCheckOut != null)
                    {
                        if (roomToCheckOut.IsAvailable)
                        {
                            MessageBox.Show($"Room {roomNumber} is already available.");
                        }
                        else
                        {
                            roomToCheckOut.IsAvailable = true;
                            RoomList.ItemsSource = null;
                            RoomList.ItemsSource = rooms;
                            MessageBox.Show($"Guest checked out from room {roomNumber}.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Room not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Room Number.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during check-out: {ex.Message}");
            }
        }

        private async void CancelReservation_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationList.SelectedItem is Reservation selectedReservation)
            {
                if (selectedReservation.IsCancelled)
                {
                    MessageBox.Show("This reservation is already cancelled.");
                    return;
                }

                selectedReservation.IsCancelled = true;

                var response = await _apiService.UpdateReservationAsync(selectedReservation);

                if (response)
                {
                    MessageBox.Show("Reservation cancelled successfully.");
                    await LoadDataAsync(); // Oppdater visning
                }
                else
                {
                    MessageBox.Show("Failed to cancel reservation.");
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation to cancel.");
            }
        }

        private async void CheckInReservation_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationList.SelectedItem is Reservation selectedReservation)
            {
                var room = rooms.FirstOrDefault(r => r.RoomId == selectedReservation.RoomId);
                if (room == null)
                {
                    MessageBox.Show("Reservation has an invalid RoomId.");
                    return;
                }

                if (!room.IsAvailable)
                {
                    MessageBox.Show("Room is already checked in.");
                    return;
                }

                room.IsAvailable = false;
                bool updated = await _apiService.UpdateRoomAsync(room);
                if (updated)
                {
                    MessageBox.Show("Guest checked in.");
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show("Failed to check in.");
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation first.");
            }
        }


        private async void ChangeRoom_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationList.SelectedItem is Reservation selectedReservation)
            {
                // Kun tilgjengelige rom + det rommet reservasjonen allerede har (for gjenvalg)
                var availableRooms = rooms.Where(r => r.IsAvailable || r.RoomId == selectedReservation.RoomId).ToList();

                if (!availableRooms.Any())
                {
                    MessageBox.Show("No rooms available to reassign.");
                    return;
                }

                string input = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Available rooms:\n{string.Join(", ", availableRooms.Select(r => r.RoomNumber))}\n\nEnter new room number:",
                    "Change Room");

                if (int.TryParse(input, out int newRoomNumber))
                {
                    var newRoom = availableRooms.FirstOrDefault(r => r.RoomNumber == newRoomNumber);

                    if (newRoom == null)
                    {
                        MessageBox.Show("Invalid room number.");
                        return;
                    }

                    // Bare oppdater RoomId
                    selectedReservation.RoomId = newRoom.RoomId;

                    bool reservationUpdated = await _apiService.UpdateReservationAsync(selectedReservation);

                    if (reservationUpdated)
                    {
                        MessageBox.Show("Room successfully changed.");

                        // 🔁 Oppdater room-liste slik at konverteren funker
                        rooms = await _apiService.GetRoomsAsync();

                        await LoadDataAsync();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update room assignment.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid input.");
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation to change room for.");
            }
        }

        public Room GetRoomById(int roomId)
        {
            return rooms.FirstOrDefault(r => r.RoomId == roomId);
        }

        private void ShowAllRooms_Click(object sender, RoutedEventArgs e)
        {
            RoomList.ItemsSource = rooms;
        }

        private void ShowCheckedInRooms_Click(object sender, RoutedEventArgs e)
        {
            RoomList.ItemsSource = rooms.Where(r => !r.IsAvailable).ToList();
        }

        private void ShowCheckedOutRooms_Click(object sender, RoutedEventArgs e)
        {
            RoomList.ItemsSource = rooms.Where(r => r.IsAvailable).ToList();
        }
    }
    
}

    







