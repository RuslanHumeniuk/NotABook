﻿using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using NotABookLibraryStandart.Models.Roles;

using System;
using System.Net.Mail;
using System.Windows.Input;

namespace NotABookViewModels
{
    public class SignUpWindowViewModel : ViewModelCustomBase
    {
        private string email;
        public string Email
        {
            get => email;
            set
            {
                try
                {
                    new MailAddress(value);
                    email = value;
                }
                catch (FormatException)
                {
                    Messenger.Default.Send("Wrong email format!");
                }
            }
        }
        public string Username { get; set; }
        public string RealPassword { get; set; }
        public string ReplayPassword { get; set; }
        public string ErrorMessage { get; set; }

        private RelayCommand signUpCommand;
        public ICommand SignUpCommand
        {
            get
            {
                if (signUpCommand == null)
                    signUpCommand = new RelayCommand(SignUp);
                return signUpCommand;
            }
        }

        public SignUpWindowViewModel(IAuthenticationService authenticationService) : base(authenticationService) { }
        private bool CanSignUp()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email)
                || string.IsNullOrWhiteSpace(RealPassword) || string.IsNullOrWhiteSpace(ReplayPassword))
            {
                ErrorMessage = "Error! Please input valid info!";
                return false;
            }
            else if (!RealPassword.Equals(ReplayPassword))
            {
                ErrorMessage = "Error! Your passwords not equal!";
                return false;
            }
            else if (_authenticationService.IsExistUser(Username, RealPassword))
            {
                ErrorMessage = "Error! This user already exist!";
                return false;
            }
            else return true;
        }
        private void SignUp()
        {
            if (CanSignUp())
            {
                _authenticationService.AddUser(Username, Email, RealPassword, new string[] { "Users" });
                Messenger.Default.Send("registered");
            }
            else
                Messenger.Default.Send(ErrorMessage);

        }
    }
}
