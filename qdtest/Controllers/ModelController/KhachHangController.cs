using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using qdtest._Library;

namespace qdtest.Controllers.ModelController
{
    public class KhachHangController
    {
        public BanGiayDBContext _db = new BanGiayDBContext();
        public Boolean login(String username,String raw_password)
        {
            if (username == null || raw_password == null) return false;
            //hash input password
            String h_password = TextLibrary.GetSHA1HashData(raw_password);
            //get password from db
            var db_pass = (from u in this._db.ds_khachhang
                              where u.tendangnhap == username
                              && u.active == true
                              select u.matkhau).FirstOrDefault();
            //check error
            if (db_pass == null) return false;
            //validate and return
            return h_password.Equals(db_pass.ToString()) ? true : false;
        }
        public KhachHang get_by_username(String username)
        {
            if(username==null) return null;
            return _db.ds_khachhang.FirstOrDefault(x => x.tendangnhap == username);
        }
        public KhachHang get_by_id(int obj_id)
        {
            return _db.ds_khachhang.FirstOrDefault(x => x.id == obj_id);
        }
        public Boolean change_password(int obj_id, String old_raw_pass, String new_raw_pass)
        {
            //check exist
            if (!this.is_exist(obj_id) || old_raw_pass==null || new_raw_pass==null) return false;
            //get obj
            KhachHang obj = this._db.ds_khachhang.Where(x=>x.id==obj_id).FirstOrDefault();
            //check old pass
            if (TextLibrary.GetSHA1HashData(old_raw_pass) == obj.matkhau)
            {
                obj.matkhau = TextLibrary.GetSHA1HashData(new_raw_pass);
                //commit
                this._db.SaveChanges();
                return true;
            }
            return false;
        }
        public Boolean set_password(int obj_id, String new_raw_or_hash_pass)
        {
            //check
            if (!this.is_exist(obj_id) || new_raw_or_hash_pass==null) return false;
            //get obj
            KhachHang obj = this._db.ds_khachhang.Where(x => x.id == obj_id).FirstOrDefault();
            if (obj.matkhau.Equals(new_raw_or_hash_pass))
            {
                //neu nhu new_raw_pass da dc hash thi bo qua//!!!!!!!!!!!!!!!!!!!!be careful
                return true;
            }
            //hash password
            obj.matkhau = TextLibrary.GetSHA1HashData(new_raw_or_hash_pass);
            //commit
            this._db.SaveChanges();
            return true;
        }
        public KhachHang get_by_id_hash_password(int obj_id, String hash_password)
        {
            if (hash_password == null) return null;
            KhachHang re = (from user in _db.ds_khachhang
                  where user.id == obj_id
                  && user.matkhau == hash_password
                  && user.active == true
                  select user).FirstOrDefault();
            return re;
        }
        public Boolean is_exist(int obj_id)
        {
            KhachHang obj = (from obj_tmp in _db.ds_khachhang
                  where obj_tmp.id == obj_id
                  select obj_tmp).FirstOrDefault();
            return obj==null?false:true;
        }
        public int add(KhachHang obj)
        {
            //hash password first
            obj.matkhau = TextLibrary.GetSHA1HashData(obj.matkhau);
            //call add
            this._db.ds_khachhang.Add(obj);
            //commit
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_khachhang.Max(x => x.id);
        }
        public Boolean delete(int obj_id)
        {
            //get entity
            KhachHang obj = this.get_by_id(obj_id);
            //check null
            if (obj == null) return false;
            //remove
            this._db.ds_khachhang.Remove(obj);
            //commit
            this._db.SaveChanges();
            return true;
        }
        public int timkiem_count(String id = "", String tendangnhap = "", String tendaydu = "", String email = "", String sdt = "", String diachi = "", String active = "")
        {
            return timkiem(id, tendangnhap, tendaydu, email, sdt, diachi, active).Count;
        }
        public List<KhachHang> timkiem(String id = "", String tendangnhap = "", String tendaydu = "", String email = "", String sdt = "", String diachi = "", String active = "", String order_by="id", Boolean order_desc=true, int start_point=0, int count=-1)
        {
            List<KhachHang> obj_list = new List<KhachHang>();
            if (!id.Equals(""))
            {
                //find by id
                int id_i = TextLibrary.ToInt(id);
                obj_list = this._db.ds_khachhang.Where(x => x.id == id_i).ToList();
                if (obj_list == null)
                {
                    return new List<KhachHang>();
                }
            }
            //filter by LIKE element
                obj_list = this._db.ds_khachhang.Where(x => x.tendangnhap.Contains(tendangnhap)
                    && x.tendaydu.Contains(tendaydu)
                    && x.email.Contains(email)
                    && x.sdt.Contains(sdt)
                    && x.diachi.Contains(diachi)
                    ).ToList();

                if (obj_list == null)
                {
                    return new List<KhachHang>();
                }
            //Filter again by by active
                if (!active.Equals(""))
                {
                    Boolean active_b = active.Equals("1") ? true : false;
                    obj_list = obj_list.Where(x => x.active==active_b).ToList();
                }

                if (obj_list == null)
                {
                    return new List<KhachHang>();
                }
            //order
                if (order_desc)
                {
                    obj_list = obj_list.OrderByDescending(x => x.id).ToList();
                }
                else
                {
                    obj_list = obj_list.OrderBy(x => x.id).ToList();
                }

                if (obj_list == null)
                {
                    return new List<KhachHang>();
                }
            //limit
                if (count >= 0)
                {
                    obj_list = obj_list.Skip(start_point).Take(count).ToList();

                    if (obj_list == null)
                    {
                        return new List<KhachHang>();
                    }
                }
            //FINAL return
            return obj_list;
        }
    }
}