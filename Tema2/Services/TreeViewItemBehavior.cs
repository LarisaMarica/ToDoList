using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace Tema2.Services
{
    public static class TreeViewSelectedItemBehavior
    {
        public static readonly DependencyProperty SelectedItemChangedCommandProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItemChangedCommand",
                typeof(ICommand),
                typeof(TreeViewSelectedItemBehavior),
                new PropertyMetadata(null, OnSelectedItemChangedCommandPropertyChanged));

        public static ICommand GetSelectedItemChangedCommand(TreeView treeView)
        {
            return (ICommand)treeView.GetValue(SelectedItemChangedCommandProperty);
        }

        public static void SetSelectedItemChangedCommand(TreeView treeView, ICommand value)
        {
            treeView.SetValue(SelectedItemChangedCommandProperty, value);
        }

        private static void OnSelectedItemChangedCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TreeView treeView)
            {
                treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;

                if (e.NewValue is ICommand command)
                {
                    treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
                }
            }
        }

        private static void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is TreeView treeView)
            {
                ICommand command = GetSelectedItemChangedCommand(treeView);
                if (command != null && command.CanExecute(e.NewValue))
                {
                    command.Execute(e.NewValue);
                }
            }
        }
    }
}
