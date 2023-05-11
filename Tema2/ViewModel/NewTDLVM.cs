using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tema2.Command;
using Tema2.Model;
using Tema2.Services;

namespace Tema2.ViewModel
{
	class NewTDLVM: BaseNotification
	{
		private string tdlName;
		private List<string> icons;
		private int currentIndex;
		private ICommand nextCommand;
		private ICommand prevCommand;
		private ICommand confirmCommand;
		private ICommand cancelCommand;

		public NewTDLVM()
		{
			icons = new List<string>()
			{
				"/Tema2;component/Resources/calendar.png",
				"/Tema2;component/Resources/cooking.png",
				"/Tema2;component/Resources/fitness.png",
				"/Tema2;component/Resources/heart.png",
				"/Tema2;component/Resources/home.png",
				"/Tema2;component/Resources/money.png",
				"/Tema2;component/Resources/mop.png",
				"/Tema2;component/Resources/online-shopping.png",
				"/Tema2;component/Resources/reading-book.png",
				"/Tema2;component/Resources/school.png",
				"/Tema2;component/Resources/suitcase.png",
				"/Tema2;component/Resources/travel.png",
				"/Tema2;component/Resources/user.png"
			};
			currentIndex = 0;
		}
		public string TDLName
		{
			get
			{
				return tdlName;
			}
			set
			{
				tdlName = value;
				NotifyPropertyChanged("TDLName");
			}
		}
		public List<string> Icons
		{
			get
			{
				return icons;
			}
		}
		public int CurrentIndex
		{
			get
			{
				return currentIndex;
			}
			set
			{
				currentIndex = value;
				NotifyPropertyChanged("CurrentIndex");
			}
		}
		public string CurrentIcon
		{
			get { return Icons[CurrentIndex]; }
		}
		public ICommand NextCommand
		{
			get
			{
				if (nextCommand == null)
				{
					nextCommand = new RelayCommand<object>(Next);
				}
				return nextCommand;
			}
		}
		private void Next(object obj)
		{
			CurrentIndex++;
			if (CurrentIndex == Icons.Count)
			{
				CurrentIndex = 0;
			}
			NotifyPropertyChanged("CurrentIcon");
		}
		public ICommand PrevCommand
		{
			get
			{
				if (prevCommand == null)
				{
					prevCommand = new RelayCommand<object>(Prev);
				}
				return prevCommand;
			}
		}
		private void Prev(object obj)
		{
			CurrentIndex--;
			if (CurrentIndex == -1)
			{
				CurrentIndex = Icons.Count - 1;
			}
			NotifyPropertyChanged("CurrentIcon");
		}
		public ICommand ConfirmCommand
		{
			get
			{
				if (confirmCommand == null)
				{
					confirmCommand = new RelayCommand<object>(Confirm);
				}
				return confirmCommand;
			}
		}
		public void Confirm(object obj)
		{
			if(TDLName == null)
			{
				MessageBox.Show("Please enter a name for your new To Do List!");
			}
			else
			{
				TDL newTDL = new TDL(TDLName, CurrentIcon);
				Mediator.Send("NewTDL", newTDL);

                if (obj is Window window)
                {
                    window.Close();
                }
            }
		}
		public ICommand CancelCommand
		{
			get
			{
				if (cancelCommand == null)
				{
					cancelCommand = new RelayCommand<object>(Cancel);
				}
				return cancelCommand;
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
