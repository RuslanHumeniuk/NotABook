﻿using System.Collections.ObjectModel;
using System;
using NotABook.Models.Exceptions;

namespace NotABook.Models
{
    public class Category : ElementOfTheBook
    {

        #region Prop
        public string GetStringCountOfItemsWithCategory
        {
            get
            {
                string text = "Empty";
                try
                {
                    text = ItemsWithThisCategory?.Count.ToString();
                    if (text == null)
                        return "No one item";
                }
                catch (ElementIsNotInCollectionException)
                {
                    text = "el";
                }
                catch (ArgumentException)
                {
                    text = "WTF";
                }
                catch (Exception)
                {
                    text = "unknown exception";
                }
                return text;
            }
        }

        public ObservableCollection<Item> ItemsWithThisCategory
        {
            get
            {
                if (IsTestingOff)
                {
                    if (CurrentBook == null ||
                      CurrentBook.CategoryInItemsOfBook.Count < 1 ||
                      !CategoryInItem.IsCategoryHasConnection(CurrentBook, this))
                        return null;
                }
                else
                {
                    if (CurrentBook == null)
                        throw new BookNullException();
                    if (CurrentBook.CategoryInItemsOfBook.Count < 1)
                        throw new EmptyCollectionException();
                    if (!CategoryInItem.IsCategoryHasConnection(CurrentBook, this))
                        throw new ElementIsNotInCollectionException();
                }                
               

                ObservableCollection<Item> items = new ObservableCollection<Item>();
                foreach (var pair in CurrentBook.CategoryInItemsOfBook)
                {
                    if (pair.GetCategoryId == Id) items.Add(pair.Item);
                }
                return items;
            }
        }

        public int CountOfItemsWithThisCategory
        {
            get => this.ItemsWithThisCategory.Count;
        }

        #endregion

        #region Constr
        public Category(Book curBook) : base(curBook)
        {            
            curBook.CategoriesOfBook.Add(this);
        }

        public Category(Book curBook, string title) : this(curBook)
        {
            Title = title;
        }
        #endregion

        #region Methods
        
        public string DeleteCategoryStr()
        {
            if (CurrentBook == null)
                return $"Book of {this.Title} is null";

            return $"Deleting of {this.Title} is {DeleteCategory().ToString()}";
        }
        public static string DeleteCategoryStr(Category category)
        {
            if (category == null)
                return "u category is null";
            if (category.CurrentBook == null)
                return $"Book of {category.Title} is null";
            return $"Deleting of {category.Title} is {DeleteCategory(category).ToString()}";
        }

        public bool DeleteCategory()
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

            CurrentBook.CategoriesOfBook.Remove(this);
            RemoveCategoryFromAllItems();

            if(IsTestingOff)
                CurrentBook.OnPropertyChanged("DateOfLastChanging");

            return !CurrentBook.CategoriesOfBook.Contains(this) && !CategoryInItem.IsCategoryHasConnection(CurrentBook, this);
        }
        public static bool DeleteCategory(Category category)
        {
            if (BaseClass.IsTestingOff)
            {
                if (category == null || category.CurrentBook == null)
                    return false;
            }
            else
            {
                if (category == null)
                    throw new CategoryNullException();
                if (category.CurrentBook == null)
                    throw new BookNullException();
            }            

            category.CurrentBook.CategoriesOfBook.Remove(category);
            category.RemoveCategoryFromAllItems();

            if (BaseClass.IsTestingOff)
                category.CurrentBook.OnPropertyChanged("DateOfLastChanging");

            return !category.CurrentBook.CategoriesOfBook.Contains(category) && !CategoryInItem.IsCategoryHasConnection(category.CurrentBook, category);
        }
        
        public string RemoveCategoryFromAllItemsStr()
        {
            if (CurrentBook == null)
                return $"Book of {this.Title} is null";
            return $"Removing all connections of {this.Title} is {RemoveCategoryFromAllItems().ToString()}";
        }
        public static string RemoveCategoryFromAllItemsStr(Category category)
        {
            if (category == null)
                return "u category is null";
            if (category.CurrentBook == null)
                return $"Book of {category.Title} is null";

            return $"Removing all connections of {category.Title} is {RemoveCategoryFromAllItems(category).ToString()}";
        }

        public bool RemoveCategoryFromAllItems()
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
                     

            CategoryInItem.DeleteAllConnectionWithCategory(CurrentBook, this);

            if (IsTestingOff)
                CurrentBook.OnPropertyChanged("DateOfLastChanging");

            return !CategoryInItem.IsCategoryHasConnection(CurrentBook, this);
        }
        public static bool RemoveCategoryFromAllItems(Category category)
        {
            if (BaseClass.IsTestingOff)
            {
                if (category == null || category.CurrentBook == null)
                    return false;
            }
            else
            {
                if (category == null)
                    throw new CategoryNullException();
                if (category.CurrentBook == null)
                    throw new BookNullException();
            }            
            
            CategoryInItem.DeleteAllConnectionWithCategory(category.CurrentBook, category);

            if (BaseClass.IsTestingOff)
                category.CurrentBook.OnPropertyChanged("DateOfLastChanging");

            return !CategoryInItem.IsCategoryHasConnection(category.CurrentBook, category);
        }
        #endregion
    }
}
