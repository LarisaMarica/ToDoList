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

namespace Tema2.ViewModel
{
    public class ChangePathVM: BaseNotification
    {
        private ICommand makeRootTDLCommand;
        private ICommand makeSubTDLCommand;
        private ICommand confirmCommand;
        private ICommand cancelCommand;
        private string name;
        private bool makeRootChecked;
        private bool makeSubChecked;
        private ObservableCollection<TDL> tdls;
        private ObservableCollection<string> names;
        private TDL selectedTDL;
        public ChangePathVM()
        {
            Names = new ObservableCollection<string>();
            makeRootChecked = false;
            makeSubChecked = false;
            Mediator.Register("ToDoLists", SetTDLs);
            Mediator.Register("SelectedTDL", SetSelectedTDL);
        }
        private void SetTDLs(object obj)
        {
            TDLS = new ObservableCollection<TDL>();
            TDLS = obj as ObservableCollection<TDL>;
            SetNames(TDLS);
            NotifyPropertyChanged("Names");
            Mediator.Unregister("ToDoLists");
        }
        private void SetSelectedTDL(object obj)
        {
            selectedTDL = obj as TDL;
            Names.Remove(selectedTDL.Name);
            Mediator.Unregister("SelectedTDL");
        }
        public string Name
        {
            get { return name; } 
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }
        public ObservableCollection<TDL> TDLS
        {
            get { return tdls; }
            set
            {
                tdls = value;
            }
        }
        public ObservableCollection<string> Names
        {
            get
            {
                return names;
            }
            set
            {
                names = value;
            }
        }
        public void SetNames(ObservableCollection<TDL> currentTDL)
        {
            foreach(TDL tdl in currentTDL)
            {
                Names.Add(tdl.Name);
                SetNames(tdl.SubToDoLists);
            }
        }
        public ICommand MakeRootTDLCommand
        {
            get
            {
                if (makeRootTDLCommand == null)
                {
                    makeRootTDLCommand = new RelayCommand<object>(MakeRootTDLMethod);
                }
                return makeRootTDLCommand;
            }
        }
        private void MakeRootTDLMethod(object obj)
        {
            makeRootChecked = true;
        }
        public ICommand MakeSubTDLCommand
        {
            get
            {
                if (makeSubTDLCommand == null)
                    {
                    makeSubTDLCommand = new RelayCommand<object>(MakeSubTDLMethod);
                }
                return makeSubTDLCommand;
            }
        }
        private void MakeSubTDLMethod(object obj)
        {
            makeSubChecked = true;
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
        private void Confirm(object obj)
        {
            if(makeSubChecked)
            {
                if(Name ==  null)
                {
                    MessageBox.Show("Please choose a new parent!");
                }
                else
                {
                    DeleteSelectedTDL(selectedTDL, TDLS);
                    InsertTDL(TDLS);
                }
            }
            if(makeRootChecked)
            {
                DeleteSelectedTDL(selectedTDL, TDLS);
                TDLS.Add(selectedTDL);
                Mediator.Send("NewPath", TDLS);
            }
            if(!makeSubChecked && !makeRootChecked)
            {
                MessageBox.Show("Please choose the new path for your To Do List!");
            }
            if (obj is Window window)
            {
                window.Close();
            }
        }
        private void InsertTDL(ObservableCollection<TDL> currentTDL)
        {
            foreach(TDL tdl in currentTDL)
            {
                if(tdl.Name == Name)
                {
                    tdl.SubToDoLists.Add(selectedTDL);
                }
                InsertTDL(tdl.SubToDoLists);
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
        private void Cancel(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
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
    }
}
