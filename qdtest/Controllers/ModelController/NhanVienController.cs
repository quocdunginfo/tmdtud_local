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
        public Boolean login(String username,String raw_password)
        {
            if (username == null || raw_password == null) return false;
            String h_password = TextLibrary.GetSHA1HashData(raw_password);
            var db_pass = (from u in this._db.ds_nhanvien
                              where u.tendangnhap == username
                              && u.active == true
                              select u.matkhau).FirstOrDefault();
            if (db_pass == null) return false;
            Debug.WriteLine("dbpass="+db_pass.ToString());
            Debug.WriteLine("hpass=" + h_password);
            return h_password.Equals(db_pass.ToString()) ? true : false;
        }
        public NhanVien get_by_username(String username)
        {
            if(username==null) return null;
            return _db.ds_nhanvien.FirstOrDefault(x => x.tendangnhap == username);
        }
        public NhanVien get_by_id(int id)
        {
            return _db.ds_nhanvien.FirstOrDefault(x => x.id == id);
        }
        public Boolean change_password(int uid, String old_raw_pass, String new_raw_pass)
        {
            //get nv by id
            if (!this.is_exist(uid)) return false;
            NhanVien nv = this._db.ds_nhanvien.Where(x=>x.id==uid).FirstOrDefault();
            
            if (TextLibrary.GetSHA1HashData(old_raw_pass) == nv.matkhau)
            {
                //xac nhan mat khau cu thanh cong
                nv.matkhau = TextLibrary.GetSHA1HashData(new_raw_pass);
                this._db.SaveChanges();
                return true;
            }
            return false;
        }
        public Boolean set_password(int uid, String new_raw_or_hash_pass)
        {
            //get nv by id
            if (!this.is_exist(uid)) return false;
            NhanVien nv = this._db.ds_nhanvien.Where(x => x.id == uid).FirstOrDefault();
            if (nv.matkhau.Equals(new_raw_or_hash_pass))
            {
                //neu nhu new_raw_pass da dc hash thi bo qua//!!!!!!!!!!!!!!!!!!!!
                return true;
            }
            nv.matkhau = TextLibrary.GetSHA1HashData(new_raw_or_hash_pass);
            this._db.SaveChanges();

            return true;
        }
        public NhanVien get_by_id_hash_password(int uid, String hash_password)
        {
            if (hash_password == null) return null;
            NhanVien re = (from user in _db.ds_nhanvien
                  where user.id == uid
                  && user.matkhau == hash_password
                  && user.active == true
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
        public int add(NhanVien nv)
        {
            this._db.ds_nhanvien.Add(nv);
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_nhanvien.Max(x => x.id);
        }
        public Boolean delete(int id)
        {
            NhanVien nv = this._db.ds_nhanvien.Where(x => x.id==id).FirstOrDefault();
            if (nv == null) return false;
            this._db.ds_nhanvien.Remove(nv);
            this._db.SaveChanges();
            return true;
        }
        public List<NhanVien> timkiem(String id="", String tendangnhap="", String tendaydu="", String email="", String active="", String group_id="")
        {
            List<NhanVien> nhanvien_list = new List<NhanVien>();
            if (!id.Equals(""))
            {
                //find by id
                int id_i = TextLibrary.ToInt(id);
                nhanvien_list = this._db.ds_nhanvien.Where(x => x.id == id_i).ToList();
                if (nhanvien_list == null)
                {
                    nhanvien_list = new List<NhanVien>();
                }
                return nhanvien_list;
            }
            //find by LIKE elament
            nhanvien_list = this._db.ds_nhanvien.Where(x => x.tendangnhap.Contains(tendangnhap)
                && x.tendaydu.Contains(tendaydu)
                && x.email.Contains(email)).ToList();
            if (nhanvien_list == null)
            {
                nhanvien_list = new List<NhanVien>();
            }
            //Filter again by by active
            if (!active.Equals(""))
            {
                Boolean active_b = active.Equals("1") ? true : false;
                nhanvien_list = nhanvien_list.Where(x => x.active==active_b).ToList();
            }
            if (nhanvien_list == null)
            {
                nhanvien_list = new List<NhanVien>();
            }
            //Filter again by by group_id
            if (!group_id.Equals(""))
            {
                int group_id_i = TextLibrary.ToInt(group_id);
                nhanvien_list = nhanvien_list.Where(x => x.group_id == group_id_i).ToList();
            }
            if (nhanvien_list == null)
            {
                nhanvien_list = new List<NhanVien>();
            }
            //FINAL return
            return nhanvien_list;
        }
    }
}