﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using NotABookLibraryStandart.Interfaces;

namespace NotABook.Pages.MainPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : MasterDetailPage
	{
		public StartPage ()
		{
			InitializeComponent ();
            masterPage.upList.ItemSelected += UpList_ItemSelected;
            masterPage.downList.ItemSelected += DownList_ItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }            
        }

        private void DownList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Pages.MainPages.MasterPageItem item)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.downList.SelectedItem = null;
                IsPresented = false;
            }
        }
        private void UpList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Pages.MainPages.MasterPageItem item)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.upList.SelectedItem = null;
                IsPresented = false;
            }
        }

        private void BtnExit_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IClosingApp>()?.CloseApplication();
        }
    }
}