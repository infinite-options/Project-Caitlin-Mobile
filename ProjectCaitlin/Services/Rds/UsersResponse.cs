using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Services.Rds
{
    public class UsersResponse
    {
        public string message { get; set; }
        //public List<UserDto> result { get; set; }
        public List<UserDto> result { get; set; }

        public List<user> ToUsersList()
        {
            List<user> users = new List<user>();
            if (result != null)
            {
                foreach (UserDto dto in result)
                {
                    users.Add(dto.toUser());
                }
            }
            return users;
        }

        public user ToUser()
        {
            user user = null;
            List<person> importantPeople = new List<person>();
            if (result != null)
            {
                foreach (UserDto dto in result)
                {
                    user usr = dto.toUser();
                    if (usr != null)
                    {
                        user = usr;
                    }
                    else
                    {
                         person p = dto.toPerson();
                        if (p != null)
                        {
                            importantPeople.Add(p);
                        }
                    }
                }
            }
            if (user!=null && importantPeople.Count > 0)
            {
                user.people = importantPeople;
            }
            return user;
        }

        public user ToUser(string id)
        {
            user user = ToUser();
            user.id = id;
            return user;
        }
    }
}
