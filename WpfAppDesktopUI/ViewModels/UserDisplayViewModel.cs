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

        private List<string> _allRoles;

        public List<string> AllRoles
        {
            get { return _allRoles; }
            set { _allRoles = value; }
        }

        private string _selectedUserRole;

        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set
            {
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
            }
        }

        private string _selectedRoleToAdd;

        public string SelectedRoleToAdd
        {
            get { return _selectedRoleToAdd; }
            set
            {
                _selectedRoleToAdd = value;
                NotifyOfPropertyChange(() => SelectedRoleToAdd);
            }

        }


        private string _selectedRoleToRemove;

        public string SelectedRoleToRemove
        {
            get { return _selectedRoleToRemove; }
            set
            {
                _selectedRoleToRemove = value;
                NotifyOfPropertyChange(() => SelectedRoleToRemove);
            }

        }

        private UserModel _selectedUser;

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                SelectedUserRoles.Clear();
                AvailableRoles.Clear();
                if (value != null)
                {
                    SelectedUserName = value.Email;
                    SelectedUserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                }
                GetAvailableRoles();
                NotifyOfPropertyChange(() => SelectedUser);


            }
        }

        private string _selectedUserName;

        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }

        private BindingList<string> _selectedUserRoles = new BindingList<string>();

        public BindingList<string> SelectedUserRoles
        {
            get { return _selectedUserRoles; }
            set
            {
                _selectedUserRoles = value;
                NotifyOfPropertyChange(() => SelectedUserRoles);
            }
        }

        private BindingList<string> _availableRoles = new BindingList<string>();

        public BindingList<string> AvailableRoles
        {
            get { return _availableRoles; }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
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
                await LoadRoles();
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
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Execption", ex.Message);
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                await _events.PublishOnUIThreadAsync(new UnauthorizedEvent());
            }
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }

        private async Task LoadRoles()
        {
            var roles = await _userEndpoint.GetAllRoles();
            AllRoles = roles.Select(x => x.Value).ToList();
        }

        private void GetAvailableRoles()
        {
            foreach (var role in AllRoles)
            {
                if (SelectedUserRoles.IndexOf(role) < 0)
                {
                    AvailableRoles.Add(role);
                }
            }
        }

        public async Task AddSelectedRole()
        {
            await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedRoleToAdd);
            SelectedUserRoles.Add(SelectedRoleToAdd);
            AvailableRoles.Remove(SelectedRoleToAdd);
        }
        public async Task RemoveSelectedRole()
        {
            await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedRoleToRemove);
            AvailableRoles.Add(SelectedRoleToRemove);
            SelectedUserRoles.Remove(SelectedRoleToRemove);
        }
    }
}
