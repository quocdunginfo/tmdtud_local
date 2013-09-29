using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using qdtest._Library;

namespace qdtest.Controllers.ModelController
{
    public class NhanVienController
    {
        private BanGiayDBContext _db = new BanGiayDBContext();
        public Boolean login(String username,String password)
        {
            if (username == null || password == null) return false;
            var result = (from user in _db.ds_nhanvien
                          where user.tendangnhap == username
                          select new { user.matkhau }).FirstOrDefault();
            if (result == null) return false;
            if (result.matkhau.Equals(password)) return true;
            return false;
        }
        public NhanVien get_by_username(String username)
        {
            if(username==null) return null;
            NhanVien re = _db.ds_nhanvien.FirstOrDefault(x => x.tendangnhap == username);
            return re;
        }
        public NhanVien get_by_id(int id)
        {
            NhanVien re = _db.ds_nhanvien.FirstOrDefault(x => x.id == id);
            return re;
        }
        public Boolean change_password(int uid, String old_pass, String new_pass)
        {
            //String old_pass_sha1 = 
            return true;
        }
        public NhanVien get_by_id_password(String uid, String password)
        {
            if (uid == null || password == null) return null;
            int user_id = Int32.Parse(uid);
            NhanVien re = new NhanVien();
            re = (from user in _db.ds_nhanvien
                  where user.id == user_id
                  && user.matkhau == password
                  select user).FirstOrDefault();
            return re;
        }
        public Boolean is_exist(int id)
        {
            NhanVien u = (from user in _db.ds_nhanvien
                  where user.id == id
                  select user).FirstOrDefault();
            return u==null?false:true;
        }
    }
}