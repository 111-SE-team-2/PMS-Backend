using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project_manage_system_backend.Models
{
    public class User
    {
        /// 第三方的帳號 Id
        [Key]
        public string Account { get; set; }
        /// 只有管理員有密碼
        public string Password { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        /// 值有：
        ///     User
        ///     Admin
        public string Authority { get; set; }
        public List<UserProject> Projects { get; set; } = new List<UserProject>();
        public List<UserScheduleOption> ScheduleOptions { get; set; } = new List<UserScheduleOption>();
    }
}
