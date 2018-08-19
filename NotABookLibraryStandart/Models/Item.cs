﻿using System;
using System.Collections.Generic;
using System.Text;
using NotABookLibraryStandart.Exceptions;
using System.Collections.ObjectModel;

namespace NotABookLibraryStandart.Models
{
    public class Item : ElementOfTheBook
    {
        #region Fields

        private Description description;

        #endregion

        #region Prop        

        public Description Description
        {
            get => description;
            set
            {
                description = value;
                if (IsTestingOff)
                    OnPropertyChanged("Description");
            }
        }

        public string DescriptionText
        {
            get => description.Text;
            set
            {
                description.Text = value;
                if (IsTestingOff)
                    OnPropertyChanged("Description.Text");
            }
        }

        public ObservableCollection<Category> Categories
        {
            get
            {
                if (IsTestingOff)
                {
                    if (CurrentBook == null)
                        return null;
                    if (!CategoryInItem.IsItemHasConnection(this))
                        return null;
                }
                else
                {
                    if (CurrentBook == null)
                        throw new BookNullException();
                    if (!CategoryInItem.IsItemHasConnection(this))
                        throw new ElementIsNotInCollectionException();
                }

                ObservableCollection<Category> categories = new ObservableCollection<Category>();
                foreach (CategoryInItem pair in CurrentBook.CategoryInItemsOfBook)
                {
                    if (pair.GetItemId == Id)
                    {
                        categories.Add(pair.Category);
                    }
                }
                return categories;
            }
            set
            {
                if (IsTestingOff)
                {
                    if (CurrentBook == null || value == null)
                        return;
                }
                else
                {
                    if (CurrentBook == null)
                        throw new BookNullException();
                    if (value == null)
                        throw new ArgumentNullException();
                }

                CategoryInItem.DeleteAllConnectionWithItem(this);
                foreach (var category in value ?? throw new ArgumentNullException())
                {
                    CategoryInItem.CreateCategoryInItem(category, this);
                }

                if (IsTestingOff)
                    OnPropertyChanged("Categories");
            }
        }

        public string CategoriesStr { get => this.GetCategoriesInString(); }
        #endregion

        #region Constr

        public Item(Book curBook) : base(curBook)
        {
            curBook.ItemsOfBook.Add(this);
        }

        public Item(Book curBook, string title) : this(curBook)
        {
            Title = title;
        }

        public Item(Book curBook, string title, Description description) : this(curBook, title)
        {
            Description = description;
        }

        public Item(Book curBook, string title, Description description, ObservableCollection<Category> categories) : this(curBook, title, description)
        {
            Categories = categories;
        }
        #endregion

        #region Methods

        public string ChangeBookStr(Book book)
        {
            if (CurrentBook == null)
                return "Current book is null";
            if (book == null)
                return "new book is null";
            return $"Changing {CurrentBook.Title} on {book.Title} is {ChangeBook(book)}";
        }
        public static string ChangeBookStr(Book newBook, Item item)
        {
            if (item == null)
                return "Item is null";
            if (item.CurrentBook == null)
                return "Current book is null";
            if (newBook == null)
                return "new book is null";
            return $"Changing {item.CurrentBook.Title} on {newBook.Title} is {ChangeBook(newBook, item)}";
        }

        public bool ChangeBook(Book newBook)
        {
            Book lastBook = null;
            if (BaseClass.IsTestingOff)
            {
                if (CurrentBook == null || newBook == null)
                    return false;
                lastBook = this.CurrentBook;
                lastBook.DeleteItem(this);
                CurrentBook = newBook;
            }
            else
            {
                lastBook = this.CurrentBook ?? throw new BookNullException();
                lastBook.DeleteItem(this);
                CurrentBook = newBook ?? throw new BookNullException();
            }

            CurrentBook.ItemsOfBook.Add(this);
            return !lastBook.ItemsOfBook.Contains(this) && newBook.ItemsOfBook.Contains(this);
        }
        public static bool ChangeBook(Book newBook, Item item)
        {
            Book lastBook = null;
            if (BaseClass.IsTestingOff)
            {
                if (item == null || item.CurrentBook == null || newBook == null)
                    return false;
                lastBook = item.CurrentBook;
                lastBook.DeleteItem(item);
                item.CurrentBook = newBook;
            }
            else
            {
                if (item == null)
                    throw new ItemNullException();
                lastBook = item.CurrentBook ?? throw new BookNullException();
                lastBook.DeleteItem(item);
                item.CurrentBook = newBook ?? throw new BookNullException();
            }

            item.CurrentBook.ItemsOfBook.Add(item);
            return !lastBook.ItemsOfBook.Contains(item) && newBook.ItemsOfBook.Contains(item);
        }

        public bool IsItemContainsWord(string partOfItem)
        {
            if (BaseClass.IsTestingOff)
            {
                if (partOfItem == null)
                    throw new ArgumentNullException();
                if (String.IsNullOrWhiteSpace(partOfItem))
                    throw new ArgumentException();
            }
            else
            {
                if (partOfItem == null ||
                    String.IsNullOrWhiteSpace(partOfItem))
                    return false;
            }

            if (Title.ToUpperInvariant().Contains(partOfItem.ToUpperInvariant()) || this.Description.Text.ToUpperInvariant().Contains(partOfItem.ToUpperInvariant()))
                return true;
            foreach (Category category in this.Categories)
            {
                if (category.IsCategoryContainsWord(partOfItem))
                    return true;
            }

            return false;
        }

        public static bool IsItemContainsWord(Item item, string partOfItem)
        {
            if (BaseClass.IsTestingOff)
            {
                if (item == null)
                    throw new ItemNullException();
                if (partOfItem == null)
                    throw new ArgumentNullException();
                if (String.IsNullOrWhiteSpace(partOfItem))
                    throw new ArgumentException();
            }
            else
            {
                if (item == null ||
                    partOfItem == null ||
                    String.IsNullOrWhiteSpace(partOfItem))
                    return false;
            }

            if (item.Title.Contains(partOfItem) || item.Description.Text.Contains(partOfItem))
                    return true;

            foreach(Category category in item.Categories)
            {
                if (category.IsCategoryContainsWord(partOfItem))
                    return true;
            }

            return false;
        }

        public string GetCategoriesInString()
        {
            if (Categories == null || Categories.Count < 1) return "No one categories";
            
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Category categories in Categories)
            {
                stringBuilder.Append(categories.Title).Append(", ");
            }
            return stringBuilder.Remove(stringBuilder.Length - 2, 2).ToString();
        }

        public string DeleteItemStr()
        {
            if (CurrentBook == null)
                return $"Current book of {Title} is null";
            return $"Deleting of {Title} is {DeleteItem().ToString()}";
        }
        public static string DeleteItemStr(Item item)
        {
            if (item == null)
                return "Item is null";
            if (item.CurrentBook == null)
                return $"Current book of {item.Title} is null";
            return $"Deleting of {item.Title} is {DeleteItem(item).ToString()}";
        }

        public bool DeleteItem()
        {
            if (BaseClass.IsTestingOff)
            {
                if (CurrentBook == null)
                    return false;
            }
            else
            {
                if (CurrentBook == null)
                    throw new BookNullException();
            }

            CurrentBook.ItemsOfBook.Remove(this);
            CategoryInItem.DeleteAllConnectionWithItem(this);

            if (IsTestingOff)
                CurrentBook.OnPropertyChanged("DateOfLastChanging");

            return !CurrentBook.ItemsOfBook.Contains(this) && !CategoryInItem.IsItemHasConnection(this);
        }
        public static bool DeleteItem(Item item)
        {
            if (BaseClass.IsTestingOff)
            {
                if (item == null || item.CurrentBook == null)
                    return false;
            }
            else
            {
                if (item == null)
                    throw new ItemNullException();
                if (item.CurrentBook == null)
                    throw new BookNullException();
            }

            item.CurrentBook.ItemsOfBook.Remove(item);
            CategoryInItem.DeleteAllConnectionWithItem(item);

            if (BaseClass.IsTestingOff)
                item.CurrentBook.OnPropertyChanged("DateOfLastChanging");

            return !item.CurrentBook.ItemsOfBook.Contains(item) && !CategoryInItem.IsItemHasConnection(item);
        }

        #endregion
    }

    public class Description : BaseClass
    {
        public string Text { get; set; }
        public List<Object> Files { get; set; }

        private Description(string text)
        {
            Text = text;
        }
        private Description(string text, List<Object> list) : this(text)
        {
            Files = list;
        }

        public static Description CreateDescription(string text)
        {
            return new Description(text);
        }
        public static Description CreateDescription(string text, List<Object> list)
        {
            return new Description(text, list);
        }

    }
}