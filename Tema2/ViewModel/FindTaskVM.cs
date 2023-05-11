using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tema2.Command;
using Tema2.Model;
using Tema2.Services;
using Task = Tema2.Model.Task;

namespace Tema2.ViewModel
{
    public class FindTaskVM: BaseNotification
    {
        private ObservableCollection<TDL> toDoLists;
        private string taskName;
        private DateTime taskDeadline;
        private bool byNameChecked;
        private bool byDeadlineChecked;
        private ObservableCollection<Task> foundTasks;
        private ICommand byNameRadioButtonCommand;
        private ICommand byDeadlineRadioButtonCommand;
        private ICommand findButtonCommand;
        private ICommand closeButtonCommand;
        public FindTaskVM()
        {
            byNameChecked = false;
            byDeadlineChecked = false;
            FoundTasks = new ObservableCollection<Task>();
            TaskDeadline = DateTime.Now;
            TaskName = "";
            Mediator.Register("ToDoLists", SetToDoLists);
        }
        public void SetToDoLists(object toDoLists)
        {
            ToDoLists = toDoLists as ObservableCollection<TDL>;
            Mediator.Unregister("ToDoLists");
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
        
        public string TaskName
        {
            get
            {
                return taskName;
            }
            set
            {
                taskName = value;
                NotifyPropertyChanged("TaskName");
            }
        }
        public DateTime TaskDeadline
        {
            get
            {
                return taskDeadline;
            }
            set
            {
                taskDeadline = value;
                NotifyPropertyChanged("TaskDeadline");
            }
        }
        public ICommand ByNameRadioButtonCommand
        {
            get
            {
                if (byNameRadioButtonCommand == null)
                {
                    byNameRadioButtonCommand = new RelayCommand<object>(ByNameRadioButtonMethod);
                }
                return byNameRadioButtonCommand;
            }
        }
        public ObservableCollection<Task> FoundTasks
        {
            get
            {
                return foundTasks;
            }
            set
            {

                foundTasks = value;
                NotifyPropertyChanged("FoundTasks");
            }
        }
        private void ByNameRadioButtonMethod(object obj)
        {
            byNameChecked = true;
            byDeadlineChecked = false;
        }
        public ICommand ByDeadlineRadioButtonCommand
        {
            get
            {
                if (byDeadlineRadioButtonCommand == null)
                {
                    byDeadlineRadioButtonCommand = new RelayCommand<object>(ByDeadlineRadioButtonMethod);
                }
                return byDeadlineRadioButtonCommand;
            }
        }
        private void ByDeadlineRadioButtonMethod(object obj)
        {
            byNameChecked = false;
            byDeadlineChecked = true;
        }
        public ICommand FindButtonCommand
        {
            get
            {
                if (findButtonCommand == null)
                {
                    findButtonCommand = new RelayCommand<object>(FindTaskMethod);
                }
                return findButtonCommand;
            }
        }
        public void SearchByName(string name, ObservableCollection<TDL> currentTDL)
        {
            foreach(TDL tdl in currentTDL)
            {
                foreach(Task task in tdl.Tasks)
                {
                    if(task.TaskName == name)
                    {
                        FoundTasks.Add(task);
                    }
                }
                SearchByName(name, tdl.SubToDoLists);
            }
        }
        public void SearchByDeadline(DateTime date, ObservableCollection<TDL> currentTDL)
        {
            foreach (TDL tdl in currentTDL)
            {
                foreach (Task task in tdl.Tasks)
                {
                    if(task.TaskDeadline.Date == date.Date) 
                    {
                        FoundTasks.Add(task);
                    }
                }
                SearchByDeadline(date, tdl.SubToDoLists);
            }
        }
        public void FindTaskMethod(object obj)
        {
            FoundTasks = new ObservableCollection<Task>();
            if(byNameChecked) 
            {
                if(TaskName == null)
                {
                    MessageBox.Show("Please enter a task name!");
                    return;
                }
                else
                {
                    SearchByName(TaskName, ToDoLists);
                    if(FoundTasks.Count == 0)
                    {
                        MessageBox.Show("No tasks found!");
                    }
                }
            }
            if(byDeadlineChecked)
            {                
                if(TaskDeadline == null)
                    {
                    MessageBox.Show("Please enter a task deadline!");
                    return;
                }
                else
                {
                    SearchByDeadline(TaskDeadline, ToDoLists);
                    if (FoundTasks.Count == 0)
                    {
                        MessageBox.Show("No tasks found!");
                    }
                }
            }
            if(!byDeadlineChecked && !byNameChecked)
            {
                MessageBox.Show("Please enter a search criteria!");
            }
        }
        public ICommand CloseButtonCommand
        {
            get
            {
                if (closeButtonCommand == null)
                {
                    closeButtonCommand = new RelayCommand<object>(CloseMethod);
                }
                return closeButtonCommand;
            }
        }
        public void CloseMethod(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}
