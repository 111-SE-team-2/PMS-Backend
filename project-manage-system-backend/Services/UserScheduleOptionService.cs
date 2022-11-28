using Microsoft.EntityFrameworkCore;
using project_manage_system_backend.Dtos;
using project_manage_system_backend.Dtos.Schedule;
using project_manage_system_backend.Models;
using project_manage_system_backend.Shares;
using System;
using System.Collections.Generic;
using System.Linq;

namespace project_manage_system_backend.Services
{
    public class UserScheduleOptionService : BaseService
    {
        public UserScheduleOptionService(PMSContext dbContext) : base(dbContext) { }

        public void VoteScheduleOption(UserScheduleOptionDto userScheduleOptionDto, string userId)
        {
            if (userScheduleOptionDto.Availability != "Yes" || userScheduleOptionDto.Availability != "IfNeedBe" || userScheduleOptionDto.Availability != "CannotAttend" || userScheduleOptionDto.Availability != "Pending")
            {
                throw new Exception("please enter user schedule option availability");
            }

            var user = _dbContext.Users.Find(userId);
            if (user != null)
            {
                var userScheduleOptions = user.ScheduleOptions.Where(userScheduleOption => userScheduleOption.ScheduleOptionId == userScheduleOptionDto.ScheduleOptionId).ToList();
                if (userScheduleOptions.Count == 0)
                {
                    var scheduleOption = _dbContext.ScheduleOptions.Find(userScheduleOptionDto.ScheduleOptionId);
                    if (scheduleOption != null)
                    {
                        var userScheduleOption = new Models.UserScheduleOption
                        {
                            User = user,
                            ScheduleOption = scheduleOption,
                            Availability = userScheduleOptionDto.Availability
                        };
                        _dbContext.Add(userScheduleOption);
                    }
                    else
                    {
                        throw new Exception("schedule option fail, can not find this schedule option");
                    }
                }
                else
                {
                    var userScheduleOption = userScheduleOptions.First();
                    userScheduleOption.Availability = userScheduleOptionDto.Availability;
                    _dbContext.Update(userScheduleOption);
                }
            }
            else
            {
                throw new Exception("user fail, can not find this user");
            }

            if (_dbContext.SaveChanges() == 0)
            {
                throw new Exception("vote schedule option fail");
            }
        }
    }
}
