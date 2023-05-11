using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tema2.Model
{
    public class Task: BaseNotification
    {
        private string name;
        private string description;
        private string status;
        private string priority;
        private DateTime deadline;
        private DateTime finishedDate;
        private string category;
        public Task(string name, string description, string status, string priority, DateTime deadline, string category)
        {
            TaskName = name;
            TaskDescription = description;
            TaskStatus = status;
            TaskPriority = priority;
            TaskDeadline = deadline;
            TaskCategory = category;
        }

        public string TaskName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged("TaskName");
            }
        }

        public string TaskDescription
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                NotifyPropertyChanged("TaskDescription");
            }
        }

        public string TaskStatus
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                NotifyPropertyChanged("TaskStatus");
            }
        }

        public string TaskPriority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
                NotifyPropertyChanged("TaskPriority");
            }
        }

        public DateTime TaskDeadline
        {
            get
            {
                return deadline;
            }
            set
            {
                deadline = value;
                NotifyPropertyChanged("TaskDeadline");
            }
        }

        public DateTime TaskFinishedDate
        {
            get
            {
                return finishedDate;
            }
            set
            {
                finishedDate = value;
                NotifyPropertyChanged("TaskFinishedDate");
            }
        }

        public string TaskCategory
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
                NotifyPropertyChanged("TaskCategory");
            }
        }
    }
}
