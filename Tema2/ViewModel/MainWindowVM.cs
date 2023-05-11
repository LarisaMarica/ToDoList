using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Tema2.Command;
using Tema2.Model;
using Task = Tema2.Model.Task;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using Tema2.Services;
using System.Reflection.Emit;
using System.Windows.Documents;
using System.Windows.Media;

namespace Tema2.ViewModel
{
    public class MainWindowVM : BaseNotification
    {
        private ObservableCollection<TDL> toDoLists;
        private TDL selectedTDL;
        private ICommand selectedToDoListCommand;
        private ObservableCollection<Task> tasks;
        private Task selectedTask;
        private ObservableCollection<string> categories;

        //File
        private ICommand openDatabaseCommand;
        private ICommand createDatabaseCommand;
        private ICommand archiveDatabaseCommand;

        //TDL
        private ICommand newRootTDLCommand;
        private ICommand newSubTDLCommand;
        private ICommand editTDLCommand;
        private ICommand deleteTDLCommand;
        private ICommand moveUpTDLCommand;
        private ICommand moveDownTDLCommand;
        private ICommand changePathTDLCommand;
        private ICommand exitCommand;

        //Task
        private ICommand selectTaskCommand;
        private ICommand addNewTaskCommand;
        private ICommand editTaskCommand;
        private ICommand deleteTaskCommand;
        private ICommand setTaskAsDoneCommand;
        private ICommand moveTaskUpCommand;
        private ICommand moveTaskDownCommand;
        private ICommand modifyCategoriesCommand;
        private ICommand openAboutCommand;
        private ICommand openFindTasksCommand;

        //View
        //Sort
        private ICommand sortByDeadlineCommand;
        private ICommand sortByPriorityCommand;

        //Filter
        private ICommand filterByCategoryCommand;
        private ICommand filterByDoneTasksCommand;
        private ICommand filterByFinishedOverdueTasksCommand;
        private ICommand filterByUnfinishedOverdueTasksCommand;
        private ICommand filterByToDoTasksCommand;

        //Statistics

        private int tasksDueToday;
        private int tasksDueTomorrow;
        private int tasksOverdue;
        private int tasksDone;
        private int tasksToBeDone;

        public MainWindowVM()
        {
            Tasks = new ObservableCollection<Task>();
            ToDoLists = new ObservableCollection<TDL>();
            Categories = new ObservableCollection<string>()
            {
                "Cooking",
                "Fitness",
                "Health",
                "Home",
                "Work",
                "Cleaning",
                "Shopping",
                "School",
                "Reading",
                "Travel",
                "Finances"
            };
            tasksDueToday = 0;
            tasksDueTomorrow = 0;
            tasksOverdue = 0;
            tasksDone = 0;
            tasksToBeDone = 0;
        }
        public ObservableCollection<TDL> ToDoLists
        {
            get
            {
                return toDoLists;
            }
            set
            {
                toDoLists = value;
                NotifyPropertyChanged("ToDoLists");
            }
        }
        public ObservableCollection<Task> Tasks
        {
            get
            {
                return tasks;
            }
            set
            {
                tasks = value;
                NotifyPropertyChanged("Tasks");
            }
        }
        public ObservableCollection<string> Categories
        {
            get
            {
                return categories;
            }
            set
            {
                categories = value;
                NotifyPropertyChanged("Categories");
            }
        }
        public Task SelectedTask
        {
            get
            {
                return selectedTask;
            }
            set
            {
                selectedTask = value;
                NotifyPropertyChanged("SelectedTask");
            }
        }

        public TDL SelectedTDL
        {
            get
            {
                return selectedTDL;
            }
            set
            {
                selectedTDL = value;
                NotifyPropertyChanged("SelectedTDL");
            }
        }
        public ICommand SelectedToDoListCommand
        {
            get
            {
                if (selectedToDoListCommand == null)
                {
                    selectedToDoListCommand = new RelayCommand<object>(UpdateSelection);
                }
                return selectedToDoListCommand;
            }
        }
        public void UpdateSelection(object obj)
        {
            if (obj is TDL)
            {
                SelectedTDL = (TDL)obj;
                Tasks = new ObservableCollection<Task>(SelectedTDL.Tasks);
            }
        }
        public ICommand OpenDatabaseCommand
        {
            get
            {
                if (openDatabaseCommand == null)
                {
                    openDatabaseCommand = new RelayCommand<object>(OpenDatabase);
                }
                return openDatabaseCommand;
            }
        }
        private void OpenDatabase(object obj)
        {
            try
            {
                // Create an OpenFileDialog to get the file path from the user
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    InitialDirectory = "C:\\ComputerScience\\SEM2\\MVP\\Tema2\\Tema2\\Saves"
                };

                // Display the dialog and get the result
                bool? result = openFileDialog.ShowDialog();

                // If the user clicked the "Open" button, read the file and deserialize the data
                if (result == true)
                {
                    string filePath = openFileDialog.FileName;

                    using (StreamReader streamReader = new StreamReader(filePath))
                    {
                        string jsonString = streamReader.ReadToEnd();
                        ArchiveHelper loadData = JsonConvert.DeserializeObject<ArchiveHelper>(jsonString);

                        // Do something with the loaded data
                        ToDoLists = new ObservableCollection<TDL>(loadData.ToDoLists);
                        Categories = new ObservableCollection<string>(loadData.Categories);
                        SelectedTDL = null;
                        UpdateStatistics();

                        MessageBox.Show("Database loaded!");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or display an error message
                MessageBox.Show("Failed to load the database: " + ex.Message);

            }
        }
        private void UpdateStatistics()
        {
            TasksDueToday = 0;
            TasksDueTomorrow = 0;
            TasksOverdue = 0;
            TasksDone = 0;
            TasksToBeDone = 0;
            CountTaskStats(ToDoLists);
        }
        private void CountTaskStats(ObservableCollection<TDL> tdls)
        {
            foreach(TDL td in tdls)
            {
                foreach(Task t in td.Tasks)
                {
                    if (t.TaskDeadline.Date == DateTime.Today.Date)
                    {
                        TasksDueToday++;
                    }
                    else if (t.TaskDeadline.Date == DateTime.Today.AddDays(1).Date)
                    {
                        TasksDueTomorrow++;
                    }
                    else if (t.TaskFinishedDate.Date > t.TaskDeadline.Date)
                    {
                        TasksOverdue++;
                    }
                    if (t.TaskStatus == "Done")
                    {
                        TasksDone++;
                    }
                    else
                    {
                        TasksToBeDone++;
                    }
                }
                CountTaskStats(td.SubToDoLists);
            }
        }
        public ICommand CreateDatabaseCommand
        {
            get
            {
                if (createDatabaseCommand == null)
                {
                    createDatabaseCommand = new RelayCommand<object>(CreateDatabase);
                }
                return createDatabaseCommand;
            }
        }
        private void CreateDatabase(object obj)
        {
            ToDoLists = new ObservableCollection<TDL>();
            Tasks = new ObservableCollection<Task>();
            TasksDone = 0;
            TasksDueToday = 0;
            TasksDueTomorrow = 0;
            TasksOverdue = 0;
            TasksToBeDone = 0;
		}
        public ICommand ArchiveDatabaseCommand
        {
            get
            {
                if (archiveDatabaseCommand == null)
                {
                    archiveDatabaseCommand = new RelayCommand<object>(ArchiveDatabase);
                }
                return archiveDatabaseCommand;
            }
        }
        private void ArchiveDatabase(object obj)
        {
            try
            {
                ArchiveHelper saveData = new ArchiveHelper(ToDoLists.ToList(), Categories.ToList());
                var jsonString = JsonConvert.SerializeObject(saveData);

                // Create a SaveFileDialog to get the file path from the user
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    InitialDirectory = "C:\\ComputerScience\\SEM2\\MVP\\Tema2\\Tema2\\Saves"
                };

                // Display the dialog and get the result
                bool? result = saveFileDialog.ShowDialog();

                // If the user clicked the "Save" button, save the file
                if (result == true)
                {
                    string filePath = saveFileDialog.FileName;

                    using (StreamWriter streamWriter = new StreamWriter(filePath, false))
                    {
                        streamWriter.WriteLine(jsonString);
                        streamWriter.Flush();
                    }

                    MessageBox.Show("Database archived!");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or display an error message
                MessageBox.Show("Failed to archive the database: " + ex.Message);
            }
        }
        public ICommand NewRootTDLCommand
        {
            get
            {
                if (newRootTDLCommand == null)
                {
                    newRootTDLCommand = new RelayCommand<object>(NewRootTDL);
                }
                return newRootTDLCommand;
            }
        }
        private void NewRootTDL(object obj)
        {
            NewTDL newTDL = new NewTDL();

            Mediator.Register("NewTDL", CreateNewRootTDL);
            newTDL.ShowDialog();
            Mediator.Unregister("NewTDL");
        }
        private int nameCount;
        private void CreateNewRootTDL(object obj)
        {
            TDL tdl = (TDL)obj;
            nameCount = 0;
            TDLNameAlreadyExists(ToDoLists, tdl.Name);
            if(nameCount != 0)
            {
                MessageBox.Show("A To Do List with this name already exists!");
                return;
            }
            ToDoLists.Add(tdl);
        }
        public ICommand NewSubTDLCommand
        {
            get
            {
                if (newSubTDLCommand == null)
                {
                    newSubTDLCommand = new RelayCommand<object>(NewSubTDL);
                }
                return newSubTDLCommand;
            }
        }
        private void NewSubTDL(object obj)
        {
            if (SelectedTDL == null)
            {
                MessageBox.Show("Please select a ToDoList first!");
                return;
            }
            else
            {
                NewTDL newTDL = new NewTDL();
                Mediator.Register("NewTDL", CreateNewSubTDL);
                newTDL.ShowDialog();
                Mediator.Unregister("NewTDL");
            }
        }
        private void CreateNewSubTDL(object obj)
        {
            TDL tdl = (TDL)obj;
            nameCount = 0;
            TDLNameAlreadyExists(ToDoLists, tdl.Name);
            if(nameCount != 0)
            {
                MessageBox.Show("A ToDoList with this name already exists!");
                return;
            }
            SelectedTDL.SubToDoLists.Add(tdl);
            NotifyPropertyChanged("SelectedTDL");
        }
        private void TDLNameAlreadyExists(ObservableCollection<TDL> tdls,string name)
        {
            foreach (TDL td in tdls)
            {
                if(td.Name == name)
                {
                    nameCount++;
                }
                TDLNameAlreadyExists(td.SubToDoLists, name);
            }
        }
        public ICommand DeleteTDLCommand
        {
            get
            {
                if (deleteTDLCommand == null)
                    {
                    deleteTDLCommand = new RelayCommand<object>(DeleteTDL);
                }
                return deleteTDLCommand;
            }
        }
        private void DeleteTDL(object obj)
        {
            DeleteSelectedTDL(SelectedTDL, ToDoLists);
            UpdateStatistics();
        }
        private void DeleteSelectedTDL(TDL tdl, ObservableCollection<TDL> tdls)
        {
            foreach (TDL td in tdls)
            {
                if (td == tdl)
                {
                    tdls.Remove(td);
                    return;
                }
                DeleteSelectedTDL(tdl, td.SubToDoLists);
            }
        }
        public ICommand EditTDLCommand
        {
            get
            {
                if(editTDLCommand == null)
                {
                    editTDLCommand = new RelayCommand<object>(EditTDL);
                }
                return editTDLCommand;
            }
        }
        private void EditTDL(object obj)
        {
            if(SelectedTDL == null)
            {
                MessageBox.Show("Select a To Do List to edit!");
            }
            else
            {
                NewTDL newTDL = new NewTDL();
                Mediator.Register("NewTDL", EditSelectedTDL);
                newTDL.ShowDialog();
                Mediator.Unregister("NewTDL");
            }
        }
        private void EditSelectedTDL(object obj)
        {
            int index = ToDoLists.IndexOf(SelectedTDL);
            ToDoLists[index].Name = (obj as TDL).Name;
            ToDoLists[index].Photo = (obj as TDL).Photo;
        }

        public ICommand MoveUpTDLCommand
        {
            get
            {
                if (moveUpTDLCommand == null)
                {
                    moveUpTDLCommand = new RelayCommand<object>(MoveUpTDL);
                }
                return moveUpTDLCommand;
            }
        }
        private ObservableCollection<TDL> GetParentToDoList(TDL son)
        {
            foreach (TDL tdl in ToDoLists)
            {
                if (tdl.SubToDoLists.Contains(son))
                {
                    return tdl.SubToDoLists;
                }
                else
                {
                    GetParentToDoList(son);
                }
            }
            return null;
        }
        private void MoveUpTDL(object obj)
        {
            if (SelectedTDL == null || ToDoLists.Count < 2)
            {
                return;
            }
            int index = ToDoLists.IndexOf(SelectedTDL);
            if (index <= 0)
            {
                return;
            }
            if(ToDoLists.Contains(SelectedTDL))
            {
                TDL tdl = ToDoLists[index];
                ToDoLists.RemoveAt(index);
                ToDoLists.Insert(index - 1, tdl);
                return;
            }
            else
            {
                ObservableCollection<TDL> parent = GetParentToDoList(SelectedTDL);
                TDL tdl = parent[index];
                parent.RemoveAt(index);
                parent.Insert(index - 1, tdl);

            }

        }
        public ICommand MoveDownTDLCommand
        {
            get
            {
                if (moveDownTDLCommand == null)
                {
                    moveDownTDLCommand = new RelayCommand<object>(MoveDownTDL);
                }
                return moveDownTDLCommand;
            }
        }
        private void MoveDownTDL(object obj)
        {
            if (SelectedTDL == null || ToDoLists.Count < 2)
            {
                return;
            }
            int index = ToDoLists.IndexOf(SelectedTDL);
            if (index >= ToDoLists.Count - 1)
            {
                return;
            }
            TDL tdl = ToDoLists[index];
            ToDoLists.RemoveAt(index);
            ToDoLists.Insert(index + 1, tdl);
        }
        public ICommand ChangePathTDLCommand
        {
            get
            {
                if (changePathTDLCommand == null)
                {
                    changePathTDLCommand = new RelayCommand<object>(ChangePathTDL);
                }
                return changePathTDLCommand;
            }
        }
        private void ChangePathTDL(object obj)
        {
            if(SelectedTDL == null)
            {
                MessageBox.Show("Select a To Do List to change the path!");
            }
            else
            {
                ChangePath changePathWindow = new ChangePath();

                Mediator.Send("ToDoLists", ToDoLists);
                Mediator.Send("SelectedTDL", SelectedTDL);
                changePathWindow.ShowDialog();
                Mediator.Register("NewPath", ChangePath);
            }
        }
        private void ChangePath(object obj)
        {
            ToDoLists = obj as ObservableCollection<TDL>;
            NotifyPropertyChanged("ToDoLists");
            Mediator.Unregister("NewPath");
        }

        public ICommand ExitCommand
        {
            get
            {
                if (exitCommand == null)
                {
                    exitCommand = new RelayCommand<object>(Exit);
                }
                return exitCommand;
            }
        }
        private void Exit(object obj)
        {
            Environment.Exit(0);
        }
        public ICommand SelectTaskCommand
        {
            get
            {
                if (selectTaskCommand == null)
                {
                    selectTaskCommand = new RelayCommand<object>(SelectTask);
                }
                return selectTaskCommand;
            }
        }
        private void SelectTask(object obj)
        {
            SelectedTask = (Task)obj;
            NotifyPropertyChanged("SelectedTask");
        }
        public ICommand AddNewTaskCommand
        {
            get
            {
                if (addNewTaskCommand == null)
                {
                    addNewTaskCommand = new RelayCommand<object>(CreateNewTaskWindow);
                }
                return addNewTaskCommand;
            }
        }
        private void CreateNewTaskWindow(object obj)
        {
            if(ToDoLists.Count == 0) 
            {
                MessageBox.Show("Create a To Do List first!");
                return;
            }
            else
            {
                if(SelectedTDL == null)
                {
                    MessageBox.Show("Select a To Do List!");
                    return;
                }
                NewTask newTask = new NewTask();
                Mediator.Register("AddTask", AddNewTask);

                newTask.ShowDialog();

                Mediator.Unregister("AddTask");
                UpdateStatistics();
            }
        }
        private void AddNewTask(object newTask)
        {
            SelectedTDL.Tasks.Add(newTask as Task);
            UpdateSelection(SelectedTDL);
        }
        public ICommand EditTaskCommand
        {
            get
            {
                if (editTaskCommand == null)
                {
                    editTaskCommand = new RelayCommand<object>(CreateEditTaskWindow);
                }
                return editTaskCommand;
            }
        }
        private void CreateEditTaskWindow(object obj)
        {
            if(SelectedTask == null)
            {
                MessageBox.Show("Please select a task to edit");
            }
            else
            {
                NewTask newTask = new NewTask();
                Mediator.Register("AddTask", EditTask);
                newTask.ShowDialog();
                Mediator.Unregister("AddTask");
            }
        }
        public void EditTask(object obj)
        {
            int index = Tasks.IndexOf(SelectedTask);
            Tasks[index] = obj as Task;
            UpdateStatistics();
        }
        public ICommand DeleteTaskCommand
        {
            get
            {
                if (deleteTaskCommand == null)
                {
                    deleteTaskCommand = new RelayCommand<object>(DeleteTask);
                }
                return deleteTaskCommand;
            }
        }
        private void DeleteTask(object obj)
        {
            Tasks.Remove(SelectedTask);
            UpdateStatistics();
        }
        public ICommand SetTaskAsDoneCommand
        {
            get
            {
                if (setTaskAsDoneCommand == null)
                {
                    setTaskAsDoneCommand = new RelayCommand<object>(SetTaskAsDone);
                }
                return setTaskAsDoneCommand;
            }
        }
        private void SetTaskAsDone(object obj)
        {
            if (SelectedTask != null && Tasks.Contains(SelectedTask))
            {
                TasksDone++;
                NotifyPropertyChanged("TasksDone");
                TasksToBeDone--;
                NotifyPropertyChanged("TasksToBeDone");
                if (DateTime.Now.Date > SelectedTask.TaskDeadline.Date)
                {
                    TasksOverdue++;
                    NotifyPropertyChanged("TasksOverdue");
                }
                if (DateTime.Now.Day == SelectedTask.TaskDeadline.Day)
                {
                    TasksDueToday--;
                    NotifyPropertyChanged("TasksDueToday");
                }
                if (DateTime.Today.AddDays(1).Date == SelectedTask.TaskDeadline.Date)
                {
                    TasksDueTomorrow--;
                    NotifyPropertyChanged("TasksDueTomorrow");
                }
                SelectedTask.TaskStatus = "Done";
                SelectedTask.TaskFinishedDate = DateTime.Now;
            }
        }
        public ICommand MoveTaskUpCommand
        {
            get
            {
                if (moveTaskUpCommand == null)
                {
                    moveTaskUpCommand = new RelayCommand<object>(MoveTaskUp);
                }
                return moveTaskUpCommand;
            }
        }
        private void MoveTaskUp(object obj)
        {
            if (SelectedTask == null || Tasks.Count < 2)
            {
                return;
            }
            int index = Tasks.IndexOf(SelectedTask);
            if (index <= 0)
            {
                return;
            }
            Task task = Tasks[index];
            Tasks.RemoveAt(index);
            Tasks.Insert(index - 1, task);

        }
        public ICommand MoveTaskDownCommand
        {
            get
            {
                if (moveTaskDownCommand == null)
                {
                    moveTaskDownCommand = new RelayCommand<object>(MoveTaskDown);
                }
                return moveTaskDownCommand;
            }
        }
        private void MoveTaskDown(object obj)
        {
            if (SelectedTask == null || Tasks.Count < 2)
            {
                return;
            }
            int index = Tasks.IndexOf(SelectedTask);
            if (index >= Tasks.Count - 1)
            {
                return;
            }
            Task task = Tasks[index];
            Tasks.RemoveAt(index);
            Tasks.Insert(index + 1, task);
        }
        public ICommand ModifyCategoriesCommand
        {
            get
            {
                if (modifyCategoriesCommand == null)
                {
                    modifyCategoriesCommand = new RelayCommand<object>(ModifyCategoriesMethod);
                }
                return modifyCategoriesCommand;
            }
        }
        private void ModifyCategoriesMethod(object obj)
        {
            

            ModifyCategories modifyCategoriesWindow = new ModifyCategories();
            Mediator.Send("Categories", Categories);
            modifyCategoriesWindow.ShowDialog();

            Mediator.Register("NewCategories", SetCategories);
            Mediator.Unregister("NewCategories");
        }
        public void SetCategories(object newCategories)
        {
            Categories = newCategories as ObservableCollection<string>;
        }
        public static int CompareByDeadline(Task task1, Task task2)
        {
            return task1.TaskDeadline.CompareTo(task2.TaskDeadline);
        }
        public ICommand SortByDeadlineCommand
        {
            get
            {
                if (sortByDeadlineCommand == null)
                {
                    sortByDeadlineCommand = new RelayCommand<object>(SortByDeadline);
                }
                return sortByDeadlineCommand;
            }
        }
        private void SortByDeadline(object obj)
        {
            List<Task> sortedTasks = Tasks.ToList();
            sortedTasks.Sort(CompareByDeadline);
            Tasks = new ObservableCollection<Task>(sortedTasks);
        }
        public static int CompareByPriority(Task task1, Task task2)
        {
            // Define the priority order
            string[] priorityOrder = { "High", "Medium", "Low" };

            // Get the index of the priority value for each task
            int index1 = Array.IndexOf(priorityOrder, task1.TaskPriority);
            int index2 = Array.IndexOf(priorityOrder, task2.TaskPriority);

            // Compare the indices
            return index1.CompareTo(index2);
        }
        public ICommand SortByPriorityCommand
        {
            get
            {
                if (sortByPriorityCommand == null)
                {
                    sortByPriorityCommand = new RelayCommand<object>(SortByPriority);
                }
                return sortByPriorityCommand;
            }
        }
        private void SortByPriority(object obj)
        {
            List<Task> sortedTasks = Tasks.ToList();
            sortedTasks.Sort(CompareByPriority);
            Tasks = new ObservableCollection<Task>(sortedTasks);
        }
        public ICommand FilterByCategoryCommand
        {
            get
            {
                if (filterByCategoryCommand == null)
                {
                    filterByCategoryCommand = new RelayCommand<object>(FilterByCategory);
                }
                return filterByCategoryCommand;
            }
        }
        private void FilterByCategory(object obj)
        {

        }
        public ICommand FilterByDoneTasksCommand
        {
            get
            {
                if (filterByDoneTasksCommand == null)
                {
                    filterByDoneTasksCommand = new RelayCommand<object>(FilterByDoneTasks);
                }
                return filterByDoneTasksCommand;
            }
        }
        private void FilterByDoneTasks(object obj)
        {
            List<Task> filteredTasks = Tasks.ToList();
            filteredTasks = filteredTasks.Where(t => t.TaskStatus == "Done").ToList();
            Tasks = new ObservableCollection<Task>(filteredTasks);
        }
        public ICommand FilterByFinishedOverdueTasksCommand
        {
            get
            {
                if (filterByFinishedOverdueTasksCommand == null)
                {
                    filterByFinishedOverdueTasksCommand = new RelayCommand<object>(FilterByFinishedOverdueTasks);
                }
                return filterByFinishedOverdueTasksCommand;
            }
        }
        private void FilterByFinishedOverdueTasks(object obj)
        {
            List<Task> aux = new List<Task>();
            aux.AddRange(Tasks.ToList());
            Tasks.Clear();
            foreach(Task task in aux)
            {
                if(task.TaskStatus == "Done" && task.TaskFinishedDate.Date > task.TaskDeadline.Date) 
                {
                    Tasks.Add(task);
                }
            }
        }
        public ICommand FilterByUnfinishedOverdueTasksCommand
        {
            get
            {
                if (filterByUnfinishedOverdueTasksCommand == null)
                    {
                    filterByUnfinishedOverdueTasksCommand = new RelayCommand<object>(FilterByUnfinishedOverdueTasks);
                }
                return filterByUnfinishedOverdueTasksCommand;
            }
        }
        private void FilterByUnfinishedOverdueTasks(object obj)
        {
            List<Task> aux = new List<Task>();
            aux.AddRange(Tasks.ToList());
            Tasks.Clear();
            foreach (Task task in aux)
            {
                if (DateTime.Now > task.TaskDeadline && task.TaskStatus == "In progress")
                {
                    Tasks.Add(task);
                }
            }
        }
        public ICommand FilterByToDoTasksCommand
        {
            get
            {
                if (filterByToDoTasksCommand == null)
                {
                    filterByToDoTasksCommand = new RelayCommand<object>(FilterByToDoTasks);
                }
                return filterByToDoTasksCommand;
            }
        }
        private void FilterByToDoTasks(object obj)
        {
            List<Task> aux = new List<Task>();
            aux.AddRange(Tasks.ToList());
            Tasks.Clear();
            foreach (Task task in aux)
            {
                if (task.TaskDeadline > DateTime.Now && task.TaskStatus == "In progress")
                {
                    Tasks.Add(task);
                }
            }
        }
        public ICommand OpenAboutCommand
        {
            get
            {
                if (openAboutCommand == null)
                {
					openAboutCommand = new RelayCommand<object>(OpenAbout);
				}
				return openAboutCommand;
            }
        }
        private void OpenAbout(object obj)
        {
            About about = new About();
            about.ShowDialog();
        }
		public int TasksDueToday
		{
            set
            {
                tasksDueToday = value;
                NotifyPropertyChanged("TaskDueToday");
            }
			get 
            { 
                return tasksDueToday;
            }
		}
		public int TasksDueTomorrow
		{
            set
            {
                tasksDueTomorrow = value;
                NotifyPropertyChanged("TasksDueTomorrow");
            }
			get 
            { 
                return tasksDueTomorrow; 
            }
		}
		public int TasksOverdue
		{
            set
            {
                tasksOverdue = value;
                NotifyPropertyChanged("TaskOverdue");
            }
			get
            {
                return tasksOverdue;
            }
		}
		public int TasksDone
		{
            set
            { 
                tasksDone = value;
                NotifyPropertyChanged("TasksDone");
            }
			get 
            { 
                return tasksDone;
            }
		}
		public int TasksToBeDone
		{
            set
            {
                tasksToBeDone = value;
                NotifyPropertyChanged("TasksToBeDone");
            }
			get 
            { 
                return tasksToBeDone;
            }
		}
        public ICommand OpenFindTasksCommand
        {
            get
            {
				if (openFindTasksCommand == null)
                {
					openFindTasksCommand = new RelayCommand<object>(OpenFindTasks);
				}
				return openFindTasksCommand;
			}
        }
        private void OpenFindTasks(object obj)
        {
			FindTask findTasks = new FindTask();
            Mediator.Send("ToDoLists", ToDoLists);
			findTasks.ShowDialog();
            
		}
	}
}
