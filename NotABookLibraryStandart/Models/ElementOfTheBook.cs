﻿using System;
using System.Collections.Generic;
using System.Text;
using NotABookLibraryStandart.Exceptions;

namespace NotABookLibraryStandart.Models
{
    public abstract class ElementOfTheBook : BaseClass
    {
        protected Book currentBook;
        public Book CurrentBook
        {
            get => currentBook;
            protected set
            {
                currentBook = value;
                if (IsTestingOff)
                    OnPropertyChanged(currentBook, "CurrentBook");
            }
        }

        public new string Title
        {
            get => title;
            set
            {
                title = value;
                if (IsTestingOff)
                    OnPropertyChanged(CurrentBook, "Title");
            }
        }

        public ElementOfTheBook(Book book) : base()
        {
            if (IsTestingOff)
                CurrentBook = book ?? new Book("NULL BOOK");
            else
                CurrentBook = book ?? throw new BookNullException();
        }
        public ElementOfTheBook(Book book, string title) : base(title)
        {
            if (IsTestingOff)
                CurrentBook = book ?? new Book("NULL BOOK");
            else
                CurrentBook = book ?? throw new Exceptions.BookNullException();
        }

        public new void OnPropertyChanged(string prop = "")
        {
            base.OnPropertyChanged(prop);
            if (CurrentBook == null)
                throw new Exceptions.BookNullException();
            CurrentBook.DateOfLastChanging = DateTime.Now;
        }
    }
}
