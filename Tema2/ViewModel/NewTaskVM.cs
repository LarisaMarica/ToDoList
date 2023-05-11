using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class NewTaskVM
    {
        public string name;
        public string priority;
        public DateTime deadline;
        public string category;
        public string description;
        public ObservableCollection<string> Priorities { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>();
        public ICommand addButtonCommand;
        public ICommand cancelButtonCommand;
        public NewTaskVM()
        {
            Priorities.Add("Low");
            Priorities.Add("Medium");
            Priorities.Add("High");
            var category = new Category();
            foreach (var type in category.Types)
            {
                Categories.Add(type);
            }
            Deadline = DateTime.Now;
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
            }
        }
        public DateTime Deadline
        {
            get
            {
                return deadline;
            }
            set
            {
                deadline = value;
            }
        }
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        
        public ICommand AddButtonCommand
        {
            get
            {
                if (addButtonCommand == null)
                {
                    addButtonCommand = new RelayCommand<object>(AddTask);
                }
                return addButtonCommand;
            }
        }
        public void AddTask(object obj)
        {
            if (Name == null || Priority == null || Category == null || Description == null)
            {
                MessageBox.Show("Please fill all the fields!");
                return;
            }
            else
            {
                Task newTask = new Task(Name, Description, "In progress", Priority, Deadline, Category);

                Mediator.Send("AddTask", newTask);

                if (obj is Window window)
                {
                    window.Close();
                }
            }
        }
        public ICommand CancelButtonCommand
        {
            get
            {
                if (cancelButtonCommand == null)
                {
                    cancelButtonCommand = new RelayCommand<object>(Cancel);
                }
                return cancelButtonCommand;
            }
        }
        public void Cancel(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }
    }
}
