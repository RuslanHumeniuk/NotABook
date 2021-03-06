using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;
using System.Linq;
using System.Collections.ObjectModel;

using NotABookLibraryStandart.Models;


[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace NotABook
{
	public partial class App : Application
	{
        #region Lists etc
        public static Book currentBook = null;

        public static ObservableCollection<Book> Books
        {
            get => Book.Books;
            set => Book.Books = value;
        }           
       
        public static ObservableCollection<Item> ItemsList => currentBook?.ItemsOfBook;
        public static ObservableCollection<Category> CategoriesList => currentBook?.CategoriesOfBook;
        public static ObservableCollection<CategoryInItem> CategoryInItemsList => currentBook?.CategoryInItemsOfBook;        

        #endregion

        public App ()
		{
            Base.ProjectType = TypeOfRunningProject.Xamarin;

            InitializeComponent();

            SetUp();                     

			MainPage = new Pages.MainPages.StartPage();                                  
		}

        private void SetUp()
        {
            currentBook = new Book("My first book");
            Book secondBook = new Book("Second book");
            Book thirdBook = new Book("Third book");

            Category chocolateCategory = new Category(currentBook, "Chocolate");
            Category flourCategory = new Category(currentBook, "Flour");
            Category eggsCategory = new Category(currentBook, "Eggs");
            Category potatoCategory = new Category(currentBook, "Potato");
            Category tomatoCategory = new Category(currentBook, "Tomato");
            Category chickenCategory = new Category(currentBook, "Chicken");

            Item chocolateBiscuit = new Item(currentBook, "Chocolate biscuit", Description.CreateDescription(App.currentBook, "The best chocolate cake ever"), new ObservableCollection<Category>() { chocolateCategory, flourCategory, eggsCategory });
            Item salatWithPotatoAndTomato = new Item(currentBook, "Salat with potat, tomatos and eggs", Description.CreateDescription(App.currentBook, "Very healthy salat"), new ObservableCollection<Category>() { potatoCategory, tomatoCategory, eggsCategory });
            Item chicken = new Item(currentBook, "Chicken", Description.CreateDescription(App.currentBook, "Chicken like in KFC"), new ObservableCollection<Category>() { chickenCategory, eggsCategory });
        }

        #region StandartFunctions
        protected override void OnStart ()
		{                      
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
        #endregion
    }
}
