using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using qdtest._Library;
using System.Linq.Dynamic;

namespace qdtest.Controllers.ModelController
{
    public class KhachHangController
    {
        public BanGiayDBContext _db = new BanGiayDBContext();
        public Boolean login(String username,String raw_password)
        {
            if (username == null || raw_password == null) return false;
            String h_password = TextLibrary.GetSHA1HashData(raw_password);
            var db_pass = (from u in this._db.ds_khachhang
                              where u.tendangnhap == username
                              && u.active == true
                              select u.matkhau).FirstOrDefault();
            if (db_pass == null) return false;
            Debug.WriteLine("dbpass="+db_pass.ToString());
            Debug.WriteLine("hpass=" + h_password);
            return h_password.Equals(db_pass.ToString()) ? true : false;
        }
        public KhachHang get_by_username(String username)
        {
            if(username==null) return null;
            return _db.ds_khachhang.FirstOrDefault(x => x.tendangnhap == username);
        }
        public KhachHang get_by_id(int id)
        {
            return _db.ds_khachhang.FirstOrDefault(x => x.id == id);
        }
        public Boolean change_password(int uid, String old_raw_pass, String new_raw_pass)
        {
            //get nv by id
            if (!this.is_exist(uid)) return false;
            KhachHang nv = this._db.ds_khachhang.Where(x=>x.id==uid).FirstOrDefault();
            
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
            KhachHang nv = this._db.ds_khachhang.Where(x => x.id == uid).FirstOrDefault();
            if (nv.matkhau.Equals(new_raw_or_hash_pass))
            {
                //neu nhu new_raw_pass da dc hash thi bo qua//!!!!!!!!!!!!!!!!!!!!
                return true;
            }
            nv.matkhau = TextLibrary.GetSHA1HashData(new_raw_or_hash_pass);
            this._db.SaveChanges();

            return true;
        }
        public KhachHang get_by_id_hash_password(int uid, String hash_password)
        {
            if (hash_password == null) return null;
            KhachHang re = (from user in _db.ds_khachhang
                  where user.id == uid
                  && user.matkhau == hash_password
                  && user.active == true
                  select user).FirstOrDefault();
            return re;
        }
        public Boolean is_exist(int id)
        {
            KhachHang u = (from user in _db.ds_khachhang
                  where user.id == id
                  select user).FirstOrDefault();
            return u==null?false:true;
        }
        public int add(KhachHang obj)
        {
            this._db.ds_khachhang.Add(obj);
            this._db.SaveChanges();
            //return ma moi nhat
            return this._db.ds_khachhang.Max(x => x.id);
        }
        public Boolean delete(int id)
        {
            KhachHang nv = this._db.ds_khachhang.Where(x => x.id==id).FirstOrDefault();
            if (nv == null) return false;
            this._db.ds_khachhang.Remove(nv);
            this._db.SaveChanges();
            return true;
        }
        public int timkiem_count(String id = "", String tendangnhap = "", String tendaydu = "", String email = "", String sdt = "", String diachi = "", String active = "")
        {
            return timkiem(id, tendangnhap, tendaydu, email, sdt, diachi, active).Count;
        }
        public List<KhachHang> timkiem(String id = "", String tendangnhap = "", String tendaydu = "", String email = "", String sdt = "", String diachi = "", String active = "", String order_by="id", Boolean order_desc=true, int start_point=0, int count=-1)
        {
            List<KhachHang> khachhang_list = new List<KhachHang>();
            if (!id.Equals(""))
            {
                //find by id
                int id_i = TextLibrary.ToInt(id);
                khachhang_list = this._db.ds_khachhang.Where(x => x.id == id_i).ToList();
                if (khachhang_list == null)
                {
                    return new List<KhachHang>();
                }
            }
            //find by LIKE element
                khachhang_list = this._db.ds_khachhang.Where(x => x.tendangnhap.Contains(tendangnhap)
                    && x.tendaydu.Contains(tendaydu)
                    && x.email.Contains(email)
                    && x.sdt.Contains(sdt)
                    && x.diachi.Contains(diachi)
                    ).ToList();
                if (khachhang_list == null)
                {
                    return new List<KhachHang>();
                }
            //Filter again by by active
                if (!active.Equals(""))
                {
                    Boolean active_b = active.Equals("1") ? true : false;
                    khachhang_list = khachhang_list.Where(x => x.active==active_b).ToList();
                }
                if (khachhang_list == null)
                {
                    return new List<KhachHang>();
                }
            //order
                if (order_desc)
                {
                    khachhang_list = khachhang_list.OrderByDescending(x => x.id).ToList();
                }
                else
                {
                    khachhang_list = khachhang_list.OrderBy(x => x.id).ToList();
                }
                if (khachhang_list == null)
                {
                    return new List<KhachHang>();
                }
            //limit
                if (count >= 0)
                {
                    khachhang_list = khachhang_list.Skip(start_point).Take(count).ToList();
                    if (khachhang_list == null)
                    {
                        return new List<KhachHang>();
                    }
                }
            //FINAL return
            return khachhang_list;
        }
    }
}