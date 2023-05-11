using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema2.Model
{
    public class TDL: BaseNotification
    {
        private string name;
        private string photo;
        private ObservableCollection<Task> tasks;
        private ObservableCollection<TDL> subToDoLists;

        public TDL(string name, string photo)
        {
            Name = name;
            Photo = photo;
            Tasks = new ObservableCollection<Task>();
            SubToDoLists = new ObservableCollection<TDL>();
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
                NotifyPropertyChanged("Name");
            }
        }
        
        public string Photo
        {
            get
            {
                return photo;
            }
            set
            {
                photo = value;
                NotifyPropertyChanged("Photo");
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
                NotifyPropertyChanged("TDLTasks");
            }
        }

        public ObservableCollection<TDL> SubToDoLists
        {
            get
            {
                return subToDoLists;
            }
            set
            {
                subToDoLists = value;
                NotifyPropertyChanged("SubToDoLists");
            }
        }
    }
}
