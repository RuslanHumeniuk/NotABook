﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using NotABookLibraryStandart.Models;
using NotABookLibraryStandart.Exceptions;

namespace NotABookTests
{
    [TestClass]
    public class ItemMethodsTest
    {
        #region StartedInfo
        Book CurrentBook = null;
        Item FirstItem = null;
        Item SecondItem = null;
        Category FirstCategory = null;
        Category SecondCategory = null;

        private void SetUp()
        {            
            CurrentBook = new Book("CurBook");

            FirstCategory = new Category(CurrentBook, "Sweet");
            SecondCategory = new Category(CurrentBook, "Salt");

            FirstItem = new Item(CurrentBook, "1 item", Description.CreateDescription("desc1"), new ObservableCollection<Category>() { FirstCategory });
            SecondItem = new Item(CurrentBook, "2 item", Description.CreateDescription("descript 2"), new ObservableCollection<Category>() { SecondCategory, FirstCategory });
        }
        #endregion

        #region Item Constructor
        [TestMethod]
         public void ItemConstr_WhenNotNullBook_ReturnsTrue()
        {
            SetUp();
            Book newBook = new Book("jk");

            Assert.AreEqual(FirstItem.CurrentBook, CurrentBook);
            Assert.AreEqual(newBook, new Item(newBook, "Title", Description.CreateDescription("desk")).CurrentBook);
        }

        [TestMethod]
        [ExpectedException(typeof(BookNullException))]
        public void ItemConstr_WhenBookIsNull_ReturnsBookNullException()
        {
            Item item = new Item(null);
        }
        #endregion

        #region Categories
        [TestMethod]
        public void Categories_WhenRealItem_ReturnTrue()
        {
            SetUp();

            Assert.AreEqual(SecondItem.Categories.Count, 2);
            Assert.AreEqual(FirstItem.Categories.Count, 1);
        }

        [TestMethod]
        public void Categories_WhenSetNewNotNullValue_ReturnTrue()
        {
            SetUp();
            SecondItem.Categories = new ObservableCollection<Category>()
            {
                new Category(CurrentBook, "new1"),
                new Category(CurrentBook, "new2"),
                new Category(CurrentBook, "3new")
            };
            
            Assert.AreEqual(SecondItem.Categories.Count, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Categories_WhenSetNull_ReturnsArgumentNullException()
        {
            SetUp();
            FirstItem.Categories = null;            
        }        
        #endregion

        #region ChangeBook
        [TestMethod]
        public void ChangeBook_WhenRealBook_ReturnsTrue()
        {
            SetUp();
            Book newBook = new Book("NEW");

            Assert.IsTrue(FirstItem.ChangeBook(newBook));
            Assert.AreEqual(FirstItem.CurrentBook, newBook);

            Assert.IsTrue(Item.ChangeBook(newBook, SecondItem));
            Assert.AreEqual(SecondItem.CurrentBook, newBook);
        }

        [TestMethod]
        [ExpectedException(typeof(ItemNullException))]
        public void ChangeBook_WhenItemIsNull_ReturnsItemNullException()
        {
            SetUp();
            Item.ChangeBook(CurrentBook, null);
        }

        [TestMethod]
        [ExpectedException(typeof(BookNullException))]
        public void ChangeBook_WhenBookIsNull_ReturnsBookNullException()
        {
            SetUp();
            FirstItem.ChangeBook(null);
        }
        #endregion

        #region DeleteItem
        [TestMethod]
        public void DeleteItem_WhenRealItem_ReturnsTrue()
        {
            SetUp();
            Assert.IsTrue(FirstItem.DeleteItem());
            Assert.IsTrue(Item.DeleteItem(SecondItem));
            Assert.AreEqual(CurrentBook.ItemsOfBook.Count, 0);
            Assert.AreEqual(CurrentBook.CategoryInItemsOfBook.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ItemNullException))]
        public void DeleteItem_WhenItemIsNull_ReturnsItemNullException()
        {
            Item.DeleteItem(null);
        }
        #endregion
    }
}
