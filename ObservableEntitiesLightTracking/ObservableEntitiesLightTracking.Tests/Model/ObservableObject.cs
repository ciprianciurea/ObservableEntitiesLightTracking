using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    /// <summary>
    /// Provides an implementation for the <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        /// <summary>
        /// Assigns the specified value to the specified backing store if a change has
        /// been made and, optionally, raises callbacks before and after.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="backingStore">The backing store.</param>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="onChanged">An optional callback to raise just before <see cref="PropertyChanged"/>.</param>
        /// <param name="onChanging">An optional callback to raise just before <see cref="PropertyChanging"/>.</param>
        protected virtual void SetProperty<T>(ref T backingStore, T value, [CallerMemberName]string propertyName = null, Action onChanged = null, Action<T> onChanging = null)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");

            var oldValue = backingStore;
            backingStore = value;

            if (PropertyChanged != null) OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises the <see cref="E:PropertyChanging"/> event.
        /// </summary>
        /// <param name="args">The <see cref="System.ComponentModel.PropertyChangingEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, args);
        }
    }
}
