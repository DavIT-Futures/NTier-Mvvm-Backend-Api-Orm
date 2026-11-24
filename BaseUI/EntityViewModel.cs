using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TestApp.Entity;
using System.Linq.Expressions;
using TestApp.Util;
using System.Windows;

namespace TestApp.BaseUI
{
    /// <summary>
    /// Base ViewModel providing reactive change notification and entity binding support for WPF views.
    /// </summary>
    public class EntityViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the associated window view instance.
        /// </summary>
        public Window View { get; set; }

        private EntityObject entity;
        /// <summary>
        /// Gets or sets the bound entity object instance.
        /// </summary>
        public EntityObject Entity
        {
            get { return entity; }
            set 
            { 
                entity = value;
                OnPropertyChange(() => Entity);
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewModel"/> class.
        /// </summary>
        public EntityViewModel()
        {
            this.Entity = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewModel"/> class bound to an entity and view.
        /// </summary>
        /// <param name="entity">Domain entity object instance.</param>
        /// <param name="view">Associated WPF window view.</param>
        public EntityViewModel(EntityObject entity, Window view)
        {
            this.Entity = entity;
            this.View = view;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Attempts to raise the PropertyChanged event, and 
        /// invokes the virtual AfterPropertyChanged method.
        /// </summary>
        /// <typeparam name="T">Class type - no need to provide any more (see example)</typeparam>
        /// <param name="propertyPointer">Property expression, for example (() => CallTypeRestrictionId)</param>
        protected void OnPropertyChange<T>(Expression<Func<T>> property)
        {
            string propertyName = property.GetPropertyName<T>();
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
