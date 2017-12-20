using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WTracking.Models
{
    public enum UserGender
    {
        Male = 0,
        Female = 1,
    }

    public class Profile
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public UserGender UserGender { get; set; }

        public int TodayStepCount { get; set; }

        public int AverageStepCountForToday { get; set; } // why you can't store time and than calculate average?

        public string ProfilePicture { get; set; }

        public DateTime LastFetchTime { get; set; }
        //why you don't use lists or array for progress, it will normalize your database
        #region ProgressRegion
        public int FirstDayProgress { get; set; }
        public int SecondDayProgress { get; set; }
        public int ThirdDayProgress { get; set; }
        public int FourthDayProgress { get; set; }
        public int FifthDayProgress { get; set; }
        public int SixthDayProgress { get; set; }
        public int SeventhDayProgress { get; set; }
        #endregion

        #region ScheduleRegion
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime MondaySchedule { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime TuesdaySchedule { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime WednesdaySchedule { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime ThursdaySchedule { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FridaySchedule { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime SaturdaySchedule { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime SundaySchedule { get; set; }
#endregion

        public ApplicationUser User { get; set; } // it created double relation because your already use Profile in ApplicationUser
    }
}
