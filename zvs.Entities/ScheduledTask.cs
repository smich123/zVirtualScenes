﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using zvs.DataModel.Tasks;

namespace zvs.DataModel
{
    [Table("ScheduledTasks", Schema = "ZVS")]
    public class ScheduledTask : INotifyPropertyChanged, IIdentity, IStoredCommand, IOneTimeScheduledTask, IWeeklyScheduledTask, IIntervalScheduledTask, IDailyScheduledTask, IMonthlyScheduledTask
    {
        public ScheduledTask()
        {
            StartTime = DateTime.Now;
            RepeatIntervalInSeconds = 120;
            RepeatIntervalInDays = 2;
            RepeatIntervalInWeeks = 1;
            RepeatIntervalInMonths = 1;
        }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        private ScheduledTaskType _taskType;
        public ScheduledTaskType TaskType
        {
            get
            {
                return _taskType;
            }
            set
            {
                if (value == _taskType) return;
                _taskType = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                if (value == _startTime) return;
                _startTime = value;
                NotifyPropertyChanged();
            }
        }

        [NotMapped]
        public DateTimeOffset StartTimeOffset
        {
            // Assume the CreateOn property stores UTC time.
            get { return new DateTimeOffset(StartTime, TimeSpan.FromHours(0)); }
            set { StartTime = value.UtcDateTime; }
        }

        private double _repeatIntervalInSeconds;
        public double RepeatIntervalInSeconds
        {
            get
            {
                return _repeatIntervalInSeconds;
            }
            set
            {
                if (value == _repeatIntervalInSeconds) return;
                _repeatIntervalInSeconds = value;
                NotifyPropertyChanged();
            }
        }

        private int _repeatIntervalInWeeks;
        public int RepeatIntervalInWeeks
        {
            get
            {
                return _repeatIntervalInWeeks;
            }
            set
            {
                if (value == _repeatIntervalInWeeks) return;
                _repeatIntervalInWeeks = value;
                NotifyPropertyChanged();
            }
        }

        private DaysOfWeek _daysOfWeekToActivate;
        public DaysOfWeek DaysOfWeekToActivate
        {
            get
            {
                return _daysOfWeekToActivate;
            }
            set
            {
                if (value == _daysOfWeekToActivate) return;
                _daysOfWeekToActivate = value;
                NotifyPropertyChanged();
            }
        }

        private int _repeatIntervalInMonths;
        public int RepeatIntervalInMonths
        {
            get
            {
                return _repeatIntervalInMonths;
            }
            set
            {
                if (value == _repeatIntervalInMonths) return;
                _repeatIntervalInMonths = value;
                NotifyPropertyChanged();
            }
        }

        private DaysOfMonth _daysOfMonthToActivate;
        public DaysOfMonth DaysOfMonthToActivate
        {
            get
            {
                return _daysOfMonthToActivate;
            }
            set
            {
                if (value == _daysOfMonthToActivate) return;
                _daysOfMonthToActivate = value;
                NotifyPropertyChanged();
            }
        }

        private int _repeatIntervalInDays;
        public int RepeatIntervalInDays
        {
            get
            {
                return _repeatIntervalInDays;
            }
            set
            {
                if (value == _repeatIntervalInDays) return;
                _repeatIntervalInDays = value;
                NotifyPropertyChanged();
            }
        }

        [NotMapped]
        public TimeSpan Inteval
        {
            get { return TimeSpan.FromSeconds(RepeatIntervalInSeconds); }
            set { RepeatIntervalInSeconds = value.TotalSeconds; }
        }

        private string _name;
        [StringLength(255)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (value == _isEnabled) return;
                _isEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private int? _sortOrder;
        public int? SortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                if (value == _sortOrder) return;
                _sortOrder = value;
                NotifyPropertyChanged();
            }
        }

        public int? CommandId { get; set; }
        private Command _command;
        public virtual Command Command
        {
            get
            {
                return _command;
            }
            set
            {
                if (value == _command) return;
                _command = value;
                NotifyPropertyChanged();
            }
        }

        private string _argument;
        [StringLength(512)]
        public string Argument
        {
            get
            {
                return _argument;
            }
            set
            {
                if (value == _argument) return;
                _argument = value;
                NotifyPropertyChanged();
            }
        }

        private string _argument2;
        [StringLength(512)]
        public string Argument2
        {
            get
            {
                return _argument2;
            }
            set
            {
                if (value == _argument2) return;
                _argument2 = value;
                NotifyPropertyChanged();
            }
        }

        private string _targetObjectName;
        public string TargetObjectName
        {
            get { return _targetObjectName; }
            set
            {
                if (value == _targetObjectName) return;
                _targetObjectName = value;
                NotifyPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
