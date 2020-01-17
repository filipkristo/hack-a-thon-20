using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace HackathonExample.ViewModel
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private readonly IDictionary<string, object> _propertyValues = new Dictionary<string, object>();
        private readonly IDictionary<string, Command> _commands = new Dictionary<string, Command>();
        private readonly IDictionary<string, bool> _executing = new Dictionary<string, bool>();

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool IsBusy
        {
            get => Get<bool>();
            set => Set(value);
        }

        protected void Set<T>(T value, [CallerMemberName] string name = "", Action afterChanged = null)
        {
            if (_propertyValues.ContainsKey(name))
            {
                var oldValue = (T)_propertyValues[name];
                if (!EqualityComparer<T>.Default.Equals(oldValue, value))
                {
                    _propertyValues[name] = value;

                    afterChanged?.Invoke();
                    OnPropetyChanged(name);
                }
            }
            else
            {
                _propertyValues.Add(name, value);

                afterChanged?.Invoke();
                OnPropetyChanged(name);
            }
        }

        protected T Get<T>(T defaultValue = default, [CallerMemberName] string name = "")
        {
            if (_propertyValues.ContainsKey(name))
            {
                return (T)_propertyValues[name];
            }
            else
            {
                return defaultValue;
            }
        }

        protected void OnPropetyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Command DefaultCommand(Func<Task> Action, [CallerMemberName] string name = "")
        {
            if (_commands.ContainsKey(name))
            {
                return _commands[name];
            }
            else
            {
                var relay = new Command(async () => await ExecuteAction(Action, name).ConfigureAwait(true), () => GetIsExecuting(name));
                _commands.Add(name, relay);

                return relay;
            }
        }

        private async Task ExecuteAction(Func<Task> Action, string name)
        {
            IsBusy = true;
            var Command = _commands[name];

            try
            {
                _executing[name] = true;
                Command.ChangeCanExecute();

                await Action.Invoke().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(ex).ConfigureAwait(true);
            }
            finally
            {
                IsBusy = false;

                _executing[name] = false;
                Command.ChangeCanExecute();
            }
        }

        protected async Task HandleExceptionAsync(Exception ex)
        {
            IsBusy = false;
            await UserDialogs.Instance.AlertAsync(ex.Message, "Error", "Ok");
        }

        private bool GetIsExecuting(string name)
        {
            if (_executing.ContainsKey(name))
            {
                return !_executing[name];
            }
            else
            {
                const bool value = false;
                _executing.Add(name, value);

                return true;
            }
        }        

        public virtual Task OnAppearing() => Task.CompletedTask;

        public virtual Task OnDisappearing() => Task.CompletedTask;
    }
}