﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Essentials;

using PassXYZLib;

using PassXYZ.Vault.Resx;
using PassXYZ.Vault.ViewModels;
using System.Threading.Tasks;

namespace PassXYZ.Vault.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        private ItemDetailViewModel _viewModel;
        private MenuItem showAction = null;
        private const int CONTEXT_ACTIONS_NUM = 4;
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemDetailViewModel();
        }

        private void OnMenuShow(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;

            if (mi.CommandParameter is Field field)
            {
                if (field.IsHide)
                {
                    field.ShowPassword();
                    showAction.Text = AppResources.action_id_hide;
                }
                else
                {
                    field.HidePassword();
                    showAction.Text = AppResources.action_id_show;
                }
            }
        }

        private async void OnMenuCopyAsync(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;

            if (mi.CommandParameter is Field field)
            {
                await Clipboard.SetTextAsync(field.Value);
            }
        }

        private void OnMenuEdit(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;

            if (mi.CommandParameter is Field field)
            {
                _viewModel.Update(field);
            }
        }

        private void OnMenuDeleteAsync(object sender, EventArgs e)
        {
            var mi = (MenuItem)sender;

            if (mi.CommandParameter is Field field)
            {
                _viewModel.Deleted(field);
            }
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            base.OnBindingContextChanged();
            if (BindingContext == null)
            {
                return;
            }

            ViewCell theViewCell = (ViewCell)sender;
            if (theViewCell.BindingContext is Field field)
            {
                // We need to check CONTEXT_ACTIONS_NUM to prevent showAction will be added multiple times.
                if (theViewCell.ContextActions.Count < CONTEXT_ACTIONS_NUM && field.IsProtected)
                {
                    if (showAction == null)
                    {
                        showAction = new MenuItem
                        {
                            Text = AppResources.action_id_show
                        };
                        showAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                        showAction.Clicked += OnMenuShow;
                    }
                    theViewCell.ContextActions.Add(showAction);
                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}