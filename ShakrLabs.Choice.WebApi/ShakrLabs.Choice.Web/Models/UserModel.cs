using ShakrLabs.Choice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakrLabs.Choice.Web.Models
{
    public class UserModel
    {

        public Guid UserId { get; set; }
        public String AppId { get; set; }

        public UserModel()
        {
        }
        public UserModel(User user)
        {
            this.UserId = user.UserId;
            this.AppId = user.AppId;
        }


    }
}