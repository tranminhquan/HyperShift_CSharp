using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace HyberShift_CSharp.Utilities
{
    public class BringNewItemIntoViewBehavior : Behavior<ItemsControl>
    {
        private INotifyCollectionChanged _notifier;

        protected override void OnAttached()
        {
            base.OnAttached();
            _notifier = AssociatedObject.Items as INotifyCollectionChanged;
            _notifier.CollectionChanged += ItemsControl_CollectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            _notifier.CollectionChanged -= ItemsControl_CollectionChanged;
        }

        private void ItemsControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newIndex = e.NewStartingIndex;
                var newElement = AssociatedObject.ItemContainerGenerator.ContainerFromIndex(newIndex);
                var item = (FrameworkElement)newElement;
                item?.BringIntoView();
            }
        }
    }
}
