using Microsoft.CodeAnalysis;
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
            if (!(userScheduleOptionDto.ScheduleOptionId > 0))
            {
                throw new Exception("please enter schedule option Id");
            }
            if (userScheduleOptionDto.Availability != "Yes" && userScheduleOptionDto.Availability != "If Need Be" && userScheduleOptionDto.Availability != "Cannot Attend" && userScheduleOptionDto.Availability != "Pending")
            {
                throw new Exception("please enter user schedule option availability");
            }

            var user = _dbContext.Users.Include(user => user.ScheduleOptions).ThenInclude(userScheduleOption => userScheduleOption.ScheduleOption).FirstOrDefault(user => user.Account.Equals(userId));
            if (user != null)
            {
                var userScheduleOptions = user.ScheduleOptions.Where(userScheduleOption => userScheduleOption.ScheduleOptionId == userScheduleOptionDto.ScheduleOptionId).ToList();
                if (userScheduleOptions.Count == 0)
                {
                    //var scheduleOption = _dbContext.ScheduleOptions.Find(userScheduleOptionDto.ScheduleOptionId);
                    var scheduleOption = _dbContext.ScheduleOptions.Where(scheduleOption => scheduleOption.Id.Equals(userScheduleOptionDto.ScheduleOptionId)).Include(scheduleOption => scheduleOption.Users).First();
                    if (scheduleOption != null)
                    {
                        if (userScheduleOptionDto.Availability == "Yes")
                        {
                            scheduleOption.NumberOfYes++;
                        }
                        else if (userScheduleOptionDto.Availability == "If Need Be")
                        {
                            scheduleOption.NumberOfIfNeedBe++;
                        }
                        else if (userScheduleOptionDto.Availability == "Cannot Attend")
                        {
                            scheduleOption.NumberOfCannotAttend++;
                        }
                        else if (userScheduleOptionDto.Availability == "Pending")
                        {
                            scheduleOption.NumberOfPending++;
                        }

                        var userScheduleOption = new Models.UserScheduleOption
                        {
                            User = user,
                            ScheduleOption = scheduleOption,
                            Availability = userScheduleOptionDto.Availability
                        };
                        _dbContext.Add(userScheduleOption);
                        _dbContext.Update(scheduleOption);
                    }
                    else
                    {
                        throw new Exception("schedule option fail, can not find this schedule option");
                    }
                }
                else
                {
                    var scheduleOption = _dbContext.ScheduleOptions.Where(scheduleOption => scheduleOption.Id.Equals(userScheduleOptionDto.ScheduleOptionId)).Include(scheduleOption => scheduleOption.Users).First();

                    if (userScheduleOptionDto.Availability == "Yes")
                    {
                        scheduleOption.NumberOfYes++;
                    }
                    else if (userScheduleOptionDto.Availability == "If Need Be")
                    {
                        scheduleOption.NumberOfIfNeedBe++;
                    }
                    else if (userScheduleOptionDto.Availability == "Cannot Attend")
                    {
                        scheduleOption.NumberOfCannotAttend++;
                    }
                    else if (userScheduleOptionDto.Availability == "Pending")
                    {
                        scheduleOption.NumberOfPending++;
                    }

                    var userScheduleOption = userScheduleOptions.First();

                    if (userScheduleOption.Availability == "Yes")
                    {
                        scheduleOption.NumberOfYes--;
                    }
                    else if (userScheduleOption.Availability == "If Need Be")
                    {
                        scheduleOption.NumberOfIfNeedBe--;
                    }
                    else if (userScheduleOption.Availability == "Cannot Attend")
                    {
                        scheduleOption.NumberOfCannotAttend--;
                    }
                    else if (userScheduleOption.Availability == "Pending")
                    {
                        scheduleOption.NumberOfPending--;
                    }

                    userScheduleOption.Availability = userScheduleOptionDto.Availability;
                    _dbContext.Update(userScheduleOption);
                    _dbContext.Update(scheduleOption);

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

        public List<UserScheduleOption> GetUserListInScheduleOption(int scheduleOptionId)
        {
            var scheduleOption = _dbContext.ScheduleOptions.Where(scheduleOption => scheduleOption.Id.Equals(scheduleOptionId)).Include(scheduleOption => scheduleOption.Users).First();
            return scheduleOption.Users;
        }
    }
}
