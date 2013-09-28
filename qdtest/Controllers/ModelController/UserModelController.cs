using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using qdtest._Library;

namespace qdtest.Controllers.ModelController
{
    public class UserModelController
    {
        private BlogDBContext db = new BlogDBContext();
        public Boolean login(String username,String password)
        {
            var result = (from user in db.Users
                          where user.username == username
                          select new { user.password }).FirstOrDefault();
            if (result == null) return false;
            if (result.password.Equals(password)) return true;
            return false;
        }
        public UserModel get_by_username(String username)
        {
            UserModel re = db.Users.FirstOrDefault(x => x.username==username);
            return re;
        }
        public UserModel get_by_id(int id)
        {
            UserModel re = db.Users.FirstOrDefault(x => x.id == id);
            return re;
        }
        public Boolean change_password(int uid, String old_pass, String new_pass)
        {
            //String old_pass_sha1 = 
            return true;
        }
        public UserModel get_by_id_password(String uid, String password)
        {
            if (uid == null || password == null) return null;
            int user_id = Int32.Parse(uid);
            UserModel re = new UserModel();
            re = (from user in db.Users
                  where user.id == user_id
                  && user.password == password
                  select user).FirstOrDefault();
            return re;
        }
        public Boolean is_exist(int id)
        {
            UserModel u = (from user in db.Users
                  where user.id == id
                  select user).FirstOrDefault();
            return u==null?false:true;
        }
    }
}