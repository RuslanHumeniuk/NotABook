﻿ using System;
using System.Collections.ObjectModel;

namespace NotABook.Models
{
    public class CategoryInItem : BaseClass
    {
        #region Fields
        public Book CurrentBook { get; private set; }

        private Guid categoryId;

        private Guid itemId;

        #endregion

        #region Properities

        public Category Category
        {
            get => CurrentBook.CategoriesOfBook[CurrentBook.GetIndexOfCategoryByID(categoryId)];
            set => categoryId = value.Id;
        }
        public Item Item
        {
            get => CurrentBook.ItemsOfBook[CurrentBook.GetIndexOfItemByID(itemId)];
            set => itemId = value.Id;
        }        

        public Guid GetCategoryId { get => categoryId; }
        public Guid GetItemId { get => itemId; }

        #endregion

        #region Constr

        public CategoryInItem(Book curBook)
        {
            CurrentBook = curBook ?? throw new ArgumentNullException();
            CurrentBook.CategoryInItemsOfBook.Add(this);
        }
        public CategoryInItem(Book curBook, Category category, Item item) 
        {
            if(IsContainsThisPair(curBook, category, item)) throw new ArgumentException();
            CurrentBook = curBook;
            categoryId = category.Id;
            itemId = item.Id; 
            CurrentBook.CategoryInItemsOfBook.Add(this);
        }

        #endregion

        #region Methods

        //
        public static CategoryInItem CreateCategoryInItem(Book curBook, Category category, Item item)
        {
            if (CategoryInItem.IsContainsThisPair(curBook, category, item)) throw new ArgumentException();
            if (curBook == null) throw new ArgumentNullException();
            return new CategoryInItem(curBook, category, item);
        }

        //
        public static ObservableCollection<CategoryInItem> GetCIIListByCategory(Book book, Category category)
        {
            if (book == null || category == null) throw new ArgumentNullException();
            ObservableCollection<CategoryInItem> list = new ObservableCollection<CategoryInItem>();
            foreach (var pair in book.CategoryInItemsOfBook)
            {
                if (pair.categoryId == category.Id) list.Add(pair);
            }
            return list;
        }
        public static ObservableCollection<CategoryInItem> GetCIIListByItem(Book book, Item item)
        {
            if (book == null || item == null) throw new ArgumentNullException();
            ObservableCollection<CategoryInItem> list = new ObservableCollection<CategoryInItem>();
            foreach (var pair in book.CategoryInItemsOfBook)
            {
                if (pair.itemId == item.Id) list.Add(pair);
            }
            return list;
        }

        //
        public static bool IsItemHasConnection(Book book, Item item)
        {
            if (book == null) throw new ArgumentNullException();
            if (item == null) throw new ArgumentException();

            foreach(var pair in book.CategoryInItemsOfBook)
            {
                if (pair.GetItemId == item.Id) return true;
            }
            return false;
        }
        public static bool IsCategoryHasConnection(Book book, Category category)
        {
            if (book == null) throw new ArgumentNullException();
            if (category == null) throw new ArgumentException();

            foreach (var pair in book.CategoryInItemsOfBook)
            {
                if (pair.GetCategoryId == category.Id) return true;
            }
            return false;
        }

        //
        public static bool IsContainsThisPair(Book book, Category category, Item item)
        {
            if (book == null) throw new ArgumentNullException();
            if (category == null || item == null) throw new ArgumentException();
            return GetGuidOfPair(book, category, item) != Guid.Empty;
        }
        public static bool IsContainsThisPair(Book book, Guid categoryId, Guid itemId)
        {
            if (book == null) throw new ArgumentNullException();
            if (categoryId == null || itemId == null) throw new ArgumentException();
            return GetGuidOfPair(book, categoryId, itemId) != Guid.Empty;
        }

        //
        public static Guid GetGuidOfPair(Book book, Category category, Item item)
        {
            if (book == null) throw new ArgumentNullException();
            if (category == null || item == null) throw new ArgumentException();
            foreach (var pair in book.CategoryInItemsOfBook)
            {
                if (pair.categoryId == category.Id && pair.itemId == item.Id) return pair.Id;
            }
            return Guid.Empty;
        }
        public static Guid GetGuidOfPair(Book book, Guid currentCategoryId, Guid currentItemId)
        {
            if (book == null) throw new ArgumentNullException();
            if (currentCategoryId == null || currentItemId == null) throw new ArgumentException();
            foreach (var pair in book.CategoryInItemsOfBook)
            {
                if (pair.categoryId == currentCategoryId && pair.itemId == currentItemId) return pair.Id;
            }
            return Guid.Empty;
        }

        //
        public static int GetIndexOfPair(Book book, Guid guid)
        {
            if (book == null) throw new ArgumentNullException();
            for (int i = 0; i < book.CategoryInItemsOfBook.Count; ++i)
            {
                if (book.CategoryInItemsOfBook[i].Id == guid) return i;
            }
            return -1;
        }

        //
        public static int DeleteConnection(Book book, Category category, Item item)
        {
            if (book == null)
                throw new ArgumentNullException();
            if (!IsContainsThisPair(book, category, item))
                throw new ArgumentException();

            Guid guid = CategoryInItem.GetGuidOfPair(book, category, item);
            book.CategoryInItemsOfBook.RemoveAt(GetIndexOfPair(book, guid));

            return !IsContainsThisPair(book, category, item) == true ? 1 : -1;
        }
        public static int DeleteConnection(Book book, Guid categoryId, Guid itemId)
        {
            if (book == null)
                throw new ArgumentNullException();
            if (!IsContainsThisPair(book, categoryId, itemId))
                throw new ArgumentException();

            Guid guid = CategoryInItem.GetGuidOfPair(book, categoryId, itemId);
            book.CategoryInItemsOfBook.RemoveAt(GetIndexOfPair(book, guid));

            return !IsContainsThisPair(book, categoryId, itemId) == true ? 1 : -1;
        }

        //
        public bool DeleteConnection()
        {
            if (CurrentBook == null) throw new ArgumentNullException();
            if (!IsContainsThisPair(CurrentBook, categoryId, itemId)) throw new ArgumentException();

            CurrentBook.CategoryInItemsOfBook.Remove(this);
            return !IsContainsThisPair(CurrentBook, this.categoryId, this.itemId);
        }

        //
        public static bool DeleteAllConnectionWithItem(Book book, Item item)
        {
            if (book == null)
                throw new ArgumentNullException();
            if (item == null)
                throw new ArgumentException();
            if (!CategoryInItem.IsItemHasConnection(book, item))
                return true;

            foreach (var pair in CategoryInItem.GetCIIListByItem(book, item)) 
            {
                if (!pair.DeleteConnection())
                    throw new InvalidOperationException();
            }

            return !IsItemHasConnection(book, item);
        }        
        public static bool DeleteAllConnectionWithCategory(Book book, Category category)
        {
            if (book == null)
                throw new ArgumentNullException();
            if (category == null)
                throw new ArgumentException();
            if (!CategoryInItem.IsCategoryHasConnection(book, category))
                return true;

            foreach (var pair in CategoryInItem.GetCIIListByCategory(book, category)) 
            {
                if (!pair.DeleteConnection())
                    throw new InvalidOperationException();
            }

            return !IsCategoryHasConnection(book, category);
        }        

        public override bool Equals(object obj)
        {
            if (!(obj is CategoryInItem)) return false;
            CategoryInItem pair = (CategoryInItem)obj;
            return pair.categoryId == this.categoryId && pair.itemId == this.itemId;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }

        #endregion
    }
}