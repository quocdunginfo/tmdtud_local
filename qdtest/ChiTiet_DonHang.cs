//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace qdtest
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChiTiet_DonHang
    {
        public int id { get; set; }
        public int soluong { get; set; }
        public int dongia { get; set; }
        public Nullable<int> donhang_id { get; set; }
        public Nullable<int> sanpham_tag_id { get; set; }
    
        public virtual DonHang DonHang { get; set; }
        public virtual SanPham_Tag SanPham_Tag { get; set; }
    }
}