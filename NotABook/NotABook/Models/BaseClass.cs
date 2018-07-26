﻿using System;
using System.ComponentModel;

namespace NotABook.Models
{
   abstract public class BaseClass : INotifyPropertyChanged
    {
        #region Fields

        protected bool IsTestingOff = false; //To start tests project set "false". 
        private string title;

        #endregion

        #region Prop

        public Guid Id { get; private set; }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                if(IsTestingOff)
                    OnPropertyChanged("Title");
            }
        }

        public DateTime DateOfCreating { get; private set; }

        public DateTime DateOfLastChanging { get; protected set; }

        #endregion

        #region Constr
        public BaseClass()
        {
            Id = Guid.NewGuid();
            DateOfCreating = DateTime.Now;
            DateOfLastChanging = DateTime.Now;
        }

        public BaseClass(string title) : this()
        {
            Title = title;
        }
        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{ this.GetType().Name}: {Title}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
                this.DateOfLastChanging = DateTime.Now;
                if(App.currentBook != null)
                    App.currentBook.DateOfLastChanging = DateTime.Now;
            }
                
        }

        #endregion
    }
}
