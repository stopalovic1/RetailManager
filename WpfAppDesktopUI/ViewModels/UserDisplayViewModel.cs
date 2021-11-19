using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppDesktopUI.EventModels;
using WpfAppDesktopUI.Library.Api;
using WpfAppDesktopUI.Library.Models;

namespace WpfAppDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;
        private readonly IEventAggregator _events;
        private readonly IUserEndpoint _userEndpoint;
        public BindingList<UserModel> _users;

        public BindingList<UserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserDisplayViewModel(StatusInfoViewModel status, IWindowManager window, IEventAggregator events, IUserEndpoint userEndpoint)
        {
            _status = status;
            _window = window;
            _events = events;
            _userEndpoint = userEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System error";

                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unathorized", "You do not have rights to interact with the Admin Form.");
                    _window.ShowDialog(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Execption", ex.Message);
                    _window.ShowDialog(_status, null, settings);
                }
                await _events.PublishOnUIThreadAsync(new UnauthorizedEvent());
            }
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }

    }
}
