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
        public BanGiayDBContext _db = new BanGiayDBContext();
        public Boolean login(String username,String raw_password)
        {
            if (username == null || raw_password == null) return false;
            //hash input password
            String h_password = TextLibrary.GetSHA1HashData(raw_password);
            //get password from db
            var db_pass = (from u in this._db.ds_nhanvien
                              where u.tendangnhap == username
                              && u.active == true
                              select u.matkhau).FirstOrDefault();
            //check error
            if (db_pass == null) return false;
            //validate and return
            return h_password.Equals(db_pass.ToString()) ? true : false;
        }
        public NhanVien get_by_username(String username)
        {
            if(username==null) return null;
            return _db.ds_nhanvien.FirstOrDefault(x => x.tendangnhap == username);
        }
        public NhanVien get_by_id(int obj_id)
        {
            return _db.ds_nhanvien.FirstOrDefault(x => x.id == obj_id);
        }
        public Boolean change_password(int obj_id, String old_raw_pass, String new_raw_pass)
        {
            //check
            if (!this.is_exist(obj_id) || old_raw_pass==null || new_raw_pass==null) return false;
            //get obj
            NhanVien obj = this.get_by_id(obj_id);
            //check old pass
            if (TextLibrary.GetSHA1HashData(old_raw_pass) == obj.matkhau)
            {
                //hash password
                obj.matkhau = TextLibrary.GetSHA1HashData(new_raw_pass);
                //commit
                this._db.SaveChanges();
                return true;
            }
            return false;
        }
        public Boolean set_password(int obj_id, String new_raw_or_hash_pass)
        {
            //check not exist or null
            if (!this.is_exist(obj_id) || new_raw_or_hash_pass==null) return false;
            //get obj
            NhanVien obj = this.get_by_id(obj_id);
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
        public NhanVien get_by_id_hash_password(int obj_id, String hash_password)
        {
            if (hash_password == null) return null;
            NhanVien re = (from user in _db.ds_nhanvien
                  where user.id == obj_id
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
        public Boolean can_use_email(int obj_id, String email)
        {
            NhanVien u = (from user in _db.ds_nhanvien
                          where user.email.ToUpper().Equals(email.ToUpper())
                          && user.id!=obj_id
                          select user).FirstOrDefault();
            return u == null ? true : false;
        }
        public Boolean can_use_tendangnhap(int obj_id, String tendangnhap)
        {
            NhanVien u = (from user in _db.ds_nhanvien
                          where user.tendangnhap.ToUpper().Equals(tendangnhap.ToUpper())
                          && user.id != obj_id
                          select user).FirstOrDefault();
            return u == null ? true : false;
        }
        public int add(NhanVien obj)
        {
            //hash password first
            obj.matkhau = TextLibrary.GetSHA1HashData(obj.matkhau);
            //call add
            this._db.ds_nhanvien.Add(obj);
            //commit
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_nhanvien.Max(x => x.id);
        }
        public Boolean delete(int id)
        {
            NhanVien obj = this._db.ds_nhanvien.Where(x => x.id==id).FirstOrDefault();
            if (obj == null) return false;
            this._db.ds_nhanvien.Remove(obj);
            this._db.SaveChanges();
            return true;
        }
        public List<NhanVien> timkiem(String id="", String tendangnhap="", String tendaydu="", String email="", String active="", String loainhanvien_id="", String forgot_password_session="")
        {
            List<NhanVien> obj_list = new List<NhanVien>();
            //find by LIKE element
            obj_list = this._db.ds_nhanvien.Where(x => x.tendangnhap.Contains(tendangnhap)
                && x.tendaydu.Contains(tendaydu)
                && x.email.Contains(email)).ToList();
            //filter by id
            if (!id.Equals(""))
            {
                int id_i = TextLibrary.ToInt(id);
                obj_list = obj_list.Where(x => x.id == id_i).ToList();
            }
            
            //filter by session
            if (!forgot_password_session.Equals(""))
            {
                obj_list = obj_list.Where(x => x.forgot_password_session.ToUpper().Equals(forgot_password_session.ToUpper())).ToList();
            }
            
            //Filter again by by active
            if (!active.Equals(""))
            {
                Boolean active_b = active.Equals("1") ? true : false;
                obj_list = obj_list.Where(x => x.active==active_b).ToList();
            }
            
            //Filter again by by group_name
            if (!loainhanvien_id.Equals(""))
            {
                int lnv_id = TextLibrary.ToInt(loainhanvien_id);
                obj_list = obj_list.Where(x => x.loainhanvien.id == lnv_id).ToList();
            }
            
            //FINAL return
            return obj_list;
        }
        public List<String> validate(NhanVien obj, String matkhau = "", String matkhau2 = "")
        {
            //
            List<String> re = new List<string>();
            //check
            if (!this.can_use_tendangnhap(obj.id, obj.tendangnhap))
            {
                re.Add("tendangnhap_exist_fail");
            }
            if (!this.can_use_email(obj.id, obj.email))
            {
                re.Add("email_exist_fail");
            }
            if (obj.email.Equals("") || !ValidateLibrary.is_valid_email(obj.email))
            {
                re.Add("email_fail");
            }
            if (obj.tendangnhap.Equals(""))
            {
                re.Add("tendangnhap_fail");
            }
            if (obj.tendaydu.Equals(""))
            {
                re.Add("tendaydu_fail");
            }
            if (!matkhau.Equals(matkhau2))
            {
                re.Add("matkhau_fail");
            }
            return re;
        }
        public Boolean generate_forgot_password_session(String email, out String session_output)
        {
            NhanVien obj = this.timkiem("", "", "", email, "", "").FirstOrDefault();
            String random = DateTime.Now.ToString() + DateTime.Now.Millisecond;
            String session = TextLibrary.GetSHA1HashData(random);
            session_output = session;
            if(obj!=null)
            {
                
                obj.forgot_password_session = session;
                Debug.WriteLine("Generate new Session " + session + " for email "+email+" ("+random+")");
                this._db.SaveChanges();
                return true;
            }
            return false;
        }
        public Boolean set_password_by_session(int obj_id, String session, String new_pass)
        {
            NhanVien obj = this.timkiem(obj_id.ToString(), "", "", "", "", "", session).FirstOrDefault();
            if(obj!=null)
            {
                this.set_password(obj.id, new_pass);
                //destroy last session
                String tmp;
                this.generate_forgot_password_session(obj.email, out tmp);
                return true;
            }
            return false;
        }
    }
}