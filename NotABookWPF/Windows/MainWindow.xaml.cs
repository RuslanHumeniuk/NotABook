﻿using GalaSoft.MvvmLight.Messaging;
using NotABookLibraryStandart.Models.BookElements;
using NotABookViewModels;

using System;
using System.Security.Permissions;
using System.Windows;

namespace NotABookWPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Users")]
    public partial class MainWindow : Window, IWindow
    {
        public ViewModelCustomBase ViewModel
        {
            get { return DataContext as ViewModelCustomBase; }
            set { DataContext = value; }
        }
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            NoteFrame.Navigate(new NotePage(new NotePageViewModel(ViewModel.Service, (ViewModel as MainWindowViewModel).CurrentBook, (ViewModel as MainWindowViewModel).CurrentNote)));
            Messenger.Default.Register(this, new Action<string>(ProcessMessage));
        }

        public void ProcessMessage(string message)
        {
            if (message.Equals("CreateNote"))
            {
                new AddEditINoteWindow(
                                    new NotePageViewModel(
                                        ViewModel.Service,
                                        (ViewModel as MainWindowViewModel).CurrentBook))
                                        .ShowDialog();
            }                                      
            else if (message.Equals("EditNote"))
            {
                new AddEditINoteWindow(
                    new NotePageViewModel(
                        ViewModel.Service,
                        (ViewModel as MainWindowViewModel).CurrentBook,
                        (ViewModel as MainWindowViewModel).CurrentNote)
                    ).Show();
            }
            else if (message.Equals("EditCategory"))
            {
                new AddEditBookElement(
                    new AddEditBookElementViewModel(
                        ViewModel.Service,
                        (ViewModel as MainWindowViewModel).SelectedCategory
                        )).ShowDialog();
            }
            else if (message.Equals("CreateCategory"))
                new AddEditBookElement(new AddEditBookElementViewModel(ViewModel.Service, new Category(String.Empty))).ShowDialog();
            else if (message.Equals("EditBook"))
            {
                new AddEditBookElement(
                   new AddEditBookElementViewModel(
                       ViewModel.Service,
                       (ViewModel as MainWindowViewModel).CurrentBook
                       )).ShowDialog();
            }
            else if (message.Equals("CreateBook"))
                new AddEditBookElement(new AddEditBookElementViewModel(ViewModel.Service, new Book(String.Empty))).ShowDialog();
            else if (message.Equals("AboutPage"))
                MessageBox.Show("Hello! \n I am Ruslan Humeniuk.\n I am KPI student and this is my first WPF project");
            else if (message.Equals("UpdateMain"))
                this.InitializeComponent();
            else if (message.Equals("UpdateNoteFrame"))
                NoteFrame.Navigate(new NotePage(new NotePageViewModel(ViewModel.Service, (ViewModel as MainWindowViewModel).CurrentBook, (ViewModel as MainWindowViewModel).CurrentNote)));
        }
    }
}