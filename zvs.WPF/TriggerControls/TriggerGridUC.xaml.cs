﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using zvs.Entities;


namespace zvs.WPF.TriggerControls
{
    /// <summary>
    /// Interaction logic for TriggerGridUC.xaml
    /// </summary>
    public partial class TriggerGridUC : UserControl
    {
        private zvsContext context;
        private App app = (App)Application.Current;

        public TriggerGridUC()
        {
            context = new zvsContext();

            InitializeComponent();

            zvsContext.ChangeNotifications<DeviceValueTrigger>.onEntityAdded += TriggerGridUC_onEntityAdded;
            zvsContext.ChangeNotifications<DeviceValueTrigger>.onEntityDeleted += TriggerGridUC_onEntityDeleted;
            zvsContext.ChangeNotifications<DeviceValueTrigger>.onEntityUpdated += TriggerGridUC_onEntityUpdated;
        }

        void TriggerGridUC_onEntityUpdated(object sender, NotifyEntityChangeContext.ChangeNotifications<DeviceValueTrigger>.EntityUpdatedArgs e)
        {
            if (context == null)
                return;

            this.Dispatcher.Invoke(new Action(async () =>
            {
                foreach (var ent in context.ChangeTracker.Entries<DeviceValueTrigger>())
                    await ent.ReloadAsync();
            }));
        }

        void TriggerGridUC_onEntityDeleted(object sender, NotifyEntityChangeContext.ChangeNotifications<DeviceValueTrigger>.EntityDeletedArgs e)
        {
            if (context == null)
                return;

            this.Dispatcher.Invoke(new Action(async () =>
            {
                foreach (var ent in context.ChangeTracker.Entries<DeviceValueTrigger>())
                    await ent.ReloadAsync();
            }));
        }

        void TriggerGridUC_onEntityAdded(object sender, NotifyEntityChangeContext.ChangeNotifications<DeviceValueTrigger>.EntityAddedArgs e)
        {
            if (context == null)
                return;

            this.Dispatcher.Invoke(new Action(async () =>
            {
                await context.DeviceValueTriggers.ToListAsync();
            }));
        }

#if DEBUG
        ~TriggerGridUC()
        {
            //Cannot write to log here, it has been disposed. 
            Debug.WriteLine("TriggerGridUC Deconstructed");
        }
#endif

        private async void UserControl_Initialized(object sender, EventArgs e)
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                await context.DeviceValueTriggers
                    .Include(o => o.DeviceValue)
                    .ToListAsync();

                //Load your data here and assign the result to the CollectionViewSource.
                System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["device_value_triggersViewSource"];

                myCollectionViewSource.Source = context.DeviceValueTriggers.Local;
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e) { }
        private void UserControl_Unloaded_1(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            //Check if the parent window is closing  or if this is just being removed from the visual tree temporarily
            if (parent == null || !parent.IsActive)
            {
                zvsContext.ChangeNotifications<DeviceValueTrigger>.onEntityAdded -= TriggerGridUC_onEntityAdded;
                zvsContext.ChangeNotifications<DeviceValueTrigger>.onEntityDeleted -= TriggerGridUC_onEntityDeleted;
                zvsContext.ChangeNotifications<DeviceValueTrigger>.onEntityUpdated -= TriggerGridUC_onEntityUpdated;
            }
        }

        private async void TriggerGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                //have to add , UpdateSourceTrigger=PropertyChanged to have the data updated in time for this event
                var result = await context.TrySaveChangesAsync();
                if (result.HasError)
                    ((App)App.Current).zvsCore.log.Error(result.Message);
            }
        }

        private void SettingBtn_Click_1(object sender, RoutedEventArgs e)
        {
            Object obj = ((FrameworkElement)sender).DataContext;
            if (obj is DeviceValueTrigger)
            {
                var trigger = (DeviceValueTrigger)obj;
                if (trigger != null)
                {
                    TriggerEditorWindow new_window = new TriggerEditorWindow(trigger.Id, context);
                    new_window.Owner = app.zvsWindow;
                    new_window.Title = string.Format("Edit Trigger '{0}', ", trigger.Name);
                    new_window.Show();
                    new_window.Closing += async (s, a) =>
                    {
                        if (!new_window.Canceled)
                        {
                            var result = await context.TrySaveChangesAsync();
                            if (result.HasError)
                                ((App)App.Current).zvsCore.log.Error(result.Message);
                        }
                    };
                }
            }
        }

        private async void Grid_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeviceValueTrigger trigger = (DeviceValueTrigger)TriggerGrid.SelectedItem;
                if (trigger != null)
                {
                    if (MessageBox.Show(string.Format("Are you sure you want to delete the '{0}' trigger?", trigger.Name), "Are you sure?",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        context.DeviceValueTriggers.Local.Remove(trigger);

                        var result = await context.TrySaveChangesAsync();
                        if (result.HasError)
                            ((App)App.Current).zvsCore.log.Error(result.Message);
                    }
                }

                e.Handled = true;
            }
        }

        private void AddTriggerBtn_Click(object sender, RoutedEventArgs e)
        {
            TriggerEditorWindow new_window = new TriggerEditorWindow(0, context);
            new_window.Owner = app.zvsWindow;
            new_window.Title = "Add Trigger";
            new_window.Show();
            new_window.Closing += async (s, a) =>
            {
                if (!new_window.Canceled)
                {
                    context.DeviceValueTriggers.Add(new_window.Trigger);

                    var result = await context.TrySaveChangesAsync();
                    if (result.HasError)
                        ((App)App.Current).zvsCore.log.Error(result.Message);
                }
            };
        }
    }
}
