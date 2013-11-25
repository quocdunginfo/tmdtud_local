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
        public Boolean can_use_email(int obj_id, String email)
        {
            KhachHang u = (from user in _db.ds_khachhang
                          where user.email.ToUpper().Contains(email.ToUpper())
                          && user.id != obj_id
                          select user).FirstOrDefault();
            return u == null ? true : false;
        }
        public Boolean can_use_tendangnhap(int obj_id, String tendangnhap)
        {
            KhachHang u = (from user in _db.ds_khachhang
                          where user.tendangnhap.ToUpper().Contains(tendangnhap.ToUpper())
                          && user.id != obj_id
                          select user).FirstOrDefault();
            return u == null ? true : false;
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
        public int timkiem_count(String id = "", String tendangnhap = "", String tendaydu = "", String email = "", String sdt = "", String diachi = "", String forgot_password_session="", String active = "")
        {
            return timkiem(id, tendangnhap, tendaydu, email, sdt, diachi, forgot_password_session ,active).Count;
        }
        public List<KhachHang> timkiem(String id = "", String tendangnhap = "", String tendaydu = "", String email = "", String sdt = "", String diachi = "", String forgot_password_session="" ,String active = "", String order_by="id", Boolean order_desc=true, int start_point=0, int count=-1)
        {
            List<KhachHang> obj_list = new List<KhachHang>();
            //find by LIKE element
            obj_list = this._db.ds_khachhang.Where(x => x.tendangnhap.Contains(tendangnhap)
                && x.tendaydu.Contains(tendaydu)
                && x.email.Contains(email)
                && x.sdt.Contains(sdt)
                && x.diachi.Contains(diachi)
                ).ToList();

            //filter by id
            if (!id.Equals(""))
            {
                //find by id
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

            //order
                if (order_desc)
                {
                    obj_list = obj_list.OrderByDescending(x => x.id).ToList();
                }
                else
                {
                    obj_list = obj_list.OrderBy(x => x.id).ToList();
                }

            //limit
                if (count >= 0)
                {
                    obj_list = obj_list.Skip(start_point).Take(count).ToList();
                }
            //FINAL return
            return obj_list;
        }
        public List<string> validate(KhachHang obj, string matkhau, string matkhau2)
        {
            List<string> re = new List<string>();
            string[] forbiden = { "admin", "mod", "moderator", "administrator", "root", "super", "user"};
            if (!ValidateLibrary.is_valid_email(obj.email))
            {
                re.Add("email_fail");
            }
            if (matkhau != matkhau2 || matkhau.Equals(""))
            {
                re.Add("matkhau_fail");
            }
            if (obj.tendangnhap.Equals(""))
            {
                re.Add("tendangnhap_fail");
            }
            if (obj.tendaydu.Equals(""))
            {
                re.Add("tendaydu_fail");
            }
            if (obj.sdt.Equals(""))
            {
                re.Add("sdt_fail");
            }
            /*if (obj.diachi.Equals(""))
            {
                re.Add("diachi_fail");
            }*/
            if (!this.can_use_tendangnhap(obj.id,obj.tendangnhap))
            {
                re.Add("tendangnhap_exist_fail");
            }
            if (forbiden.Contains(obj.tendangnhap.ToLower()))
            {
                re.Add("tendangnhap_exist_fail");
            }
            if (!this.can_use_email(obj.id, obj.email))
            {
                re.Add("email_exist_fail");
            }
            return re;
        }
        public Boolean generate_forgot_password_session(String email, out String session_output)
        {
            KhachHang obj = this.timkiem("", "", "", email).FirstOrDefault();
            String random = DateTime.Now.ToString() + DateTime.Now.Millisecond;
            String session = TextLibrary.GetSHA1HashData(random);
            session_output = session;
            if (obj != null)
            {

                obj.forgot_password_session = session;
                Debug.WriteLine("Generate new Session " + session + " for email " + email + " (" + random + ")");
                this._db.SaveChanges();
                return true;
            }
            return false;
        }
        public Boolean set_password_by_session(int obj_id, String session, String new_pass)
        {
            KhachHang obj = this.timkiem(obj_id.ToString(), "", "", "", "", "", session).FirstOrDefault();
            if (obj != null)
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