using Newtonsoft.Json.Bson;
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
    public class ModifyCategoriesVM: BaseNotification
    {
        private ObservableCollection<string> categories;
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
        public string newCategory;
        public string selectedCategory;
        public string modifiedCategory;
        public ICommand selectedCategoryCommand;
        public ICommand addCategoryCommand;
        public ICommand deleteCategoryCommand;
        public ICommand modifyCategoryCommand;
        public ICommand cancelButtonCommand;
        public ModifyCategoriesVM() 
        {
            Categories = new ObservableCollection<string>();
            Mediator.Register("Categories", SetCategories);
        }
        public void SetCategories(object categories)
        {
            Categories = categories as ObservableCollection<string>;

            Mediator.Unregister("Categories");
        }
        public string NewCategory
        {
            get
            {
                return newCategory;
            }
            set
            {
                newCategory = value;
            }
        }
        public string SelectedCategory
        {
            get
            {
                return selectedCategory;
            }
            set
            {
                selectedCategory = value;
                NotifyPropertyChanged("SelectedCategory");
            }
        }
        public string ModifiedCategory
        {
            get
            {
                return modifiedCategory;
            }
            set
            {
                modifiedCategory = value;
                NotifyPropertyChanged("ModifiedCategory");
            }
        }
        public ICommand SelectedCategoryCommand
        {
            get
            {
                if (selectedCategoryCommand == null)
                {
                    selectedCategoryCommand = new RelayCommand<object>(SelectCategoryMethod);
                }
                return selectedCategoryCommand;
            }
        }
        public void SelectCategoryMethod(object obj)
        {
            SelectedCategory = (string)obj;
            NotifyPropertyChanged("SelectedCategory");
        }
        public ICommand AddCategoryCommand
        {
            get
            {
                if(addCategoryCommand == null)
                {
                    addCategoryCommand = new RelayCommand<object>(addCategory);
                }
                return addCategoryCommand;
            }
        }
        public void addCategory(object obj)
        {
            if(NewCategory != null)
            {
                if(Categories.Contains(NewCategory))
                {
                    MessageBox.Show("This category already exists!");
                }
                else
                {
                    Categories.Add(NewCategory);
                }
            }
            else
            {
                MessageBox.Show("Please insert a name for your new category!");
            }
        }
        public ICommand ModifyCategoryCommand
        {
            get
            {
                if(modifyCategoryCommand == null)
                {
                    modifyCategoryCommand = new RelayCommand<object>(modifyCategory);
                }
                return modifyCategoryCommand;
            }
        }
        public void modifyCategory(object obj)
        {
            if(SelectedCategory == null)
            {
                MessageBox.Show("Please select a category to modify!");
            }
            else
            {
                if(ModifiedCategory == null)
                {
                    MessageBox.Show("Please insert a new name for the category!");
                }
                else
                {
                    if(Categories.Contains(ModifiedCategory))
                    {
                        MessageBox.Show("This category already exists!");
                    }
                    else
                    {
                        Categories.Remove(SelectedCategory);
                        Categories.Add(ModifiedCategory);
                    }
                }
            }
        }
        public ICommand DeleteCategoryCommand
        {
            get
            {
                if(deleteCategoryCommand == null)
                {
                    deleteCategoryCommand = new RelayCommand<object>(deleteCategory);
                }
                return deleteCategoryCommand;
            }
        }
        public void deleteCategory(object obj)
        {
            Categories.Remove(SelectedCategory);
        }
        public ICommand CancelButtonCommand
        {
            get
            {
                if (cancelButtonCommand == null)
                {
                    cancelButtonCommand = new RelayCommand<object>(cancelButton);
                }
                return cancelButtonCommand;
            }
        }
        public void cancelButton(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
            Mediator.Send("NewCategories", Categories);
        }
    }
}
