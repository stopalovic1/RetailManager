using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfAppDesktopUI.EventModels;
using WpfAppDesktopUI.Library.Api;
using WpfAppDesktopUI.Library.Models;

namespace WpfAppDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>, IHandle<UnauthorizedEvent>
    {

        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;
        private LoginViewModel _loginVM;
        private UserDisplayViewModel _userDisplayVM;
        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user,
            IAPIHelper apiHelper, LoginViewModel loginVM, UserDisplayViewModel userDisplayVM)
        {
            _events = events;
            _salesVM = salesVM;
            _loginVM = loginVM;
            _user = user;
            _userDisplayVM = userDisplayVM;
            _apiHelper = apiHelper;
            _events.SubscribeOnPublishedThread(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>(),new CancellationToken());
        }



        public bool IsLoggedIn
        {
            get
            {
                bool output = false;
                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }
                return output;
            }
        }

        public void ExitApplication()
        {
            TryCloseAsync();
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(_userDisplayVM,new CancellationToken());
        }
        public async Task LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM, cancellationToken);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task HandleAsync(UnauthorizedEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_loginVM, cancellationToken);
        }
    }
}
