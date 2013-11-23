using qdtest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace qdtest._Library
{
    public class GMailLibrary
    {
        public String sender_email = "quocdunginfo@gmail.com";
        public String sender_password = "doyohaanthtode77859";
        public String receive_email = "quocdunginfo@gmail.com";
        public String receive_title = "Test email";
        public String receive_html = "Test body";
        public Boolean Send()
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(receive_email);
            
            mail.Subject = this.receive_title;
            mail.IsBodyHtml = true;
            mail.Body = this.receive_html;
            mail.From = new MailAddress(this.sender_email);
            try
            {
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                // setup Smtp authentication
                NetworkCredential credentials = new NetworkCredential(this.sender_email, this.sender_password);
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// Tạo HTML BODY cho this.receive_title và this.receive_html
        /// </summary>
        /// <param name="reset_link"></param>
        /// <returns></returns>
        public void Generate_ForgotPassword_Html(String reset_link)
        {
            this.receive_title = "Khôi phục mật khẩu Website CuaHangBanGiay";

            this.receive_html = "Bạn hoặc một ai đó đã gửi yêu cầu khôi phục lại mật khẩu tại Website CuaHangBanGiay!";
            this.receive_html += "<br>";
            this.receive_html += "Click vào đường dẫn sau để tiến hành khôi phục lại mật khẩu:";
            this.receive_html += "<br>";
            this.receive_html += "<a href=\"" + reset_link + "\">";
            this.receive_html += reset_link;
            this.receive_html += "</a>";
        }
        public void Generate_DonHang_Html(DonHang obj)
        {
            {
                this.receive_title = "Thông tin đơn hàng mã số ["+obj.id+"] từ Website CuaHangBanGiay";

                this.receive_html = "Xin chào "+obj._get_khachhang_tendaydu();
                this.receive_html += "<br>";
                this.receive_html += "Bạn đã đặt hàng tại của hàng chúng tôi với thông tin chi tiết như sau";
                this.receive_html += "<br><br>";
                this.receive_html += "Mã SP | Tên SP | Màu | Kích thước | Số lượng x Đơn giá = Tổng cộng";
                this.receive_html += "<br>";
                foreach (var item in obj.ds_chitiet_donhang)
                {
                    this.receive_html += item.chitietsp.sanpham.masp;
                    this.receive_html += " | ";
                    this.receive_html += item.chitietsp.sanpham.ten;
                    this.receive_html += " | ";
                    this.receive_html += item.chitietsp.mausac.giatri;
                    this.receive_html += " | ";
                    this.receive_html += item.chitietsp.kichthuoc.giatri;
                    this.receive_html += " | ";
                    this.receive_html += item.soluong;
                    this.receive_html += " x ";
                    this.receive_html += item.dongia + "VNĐ";
                    this.receive_html += " = ";
                    this.receive_html += item._get_total() + "VNĐ";
                    this.receive_html += "<br>";
                }
                this.receive_html += "<br>";
                this.receive_html += "Tổng tiền cho sản phẩm: " + obj._get_tongtien_notinclude_phivanchuyen() + "VNĐ";
                this.receive_html += "<br>";
                this.receive_html += "Phí vận chuyển: " + obj._get_phivanchuyen() + "VNĐ";
                this.receive_html += "<br>";
                this.receive_html += "Giảm giá: " + obj._get_giamgia_tuloaikh() + "VNĐ";
                this.receive_html += "<br>";
                this.receive_html += "Tổng tiền phải thanh toán: " + obj._get_tongtien_include_phivanchuyen() + "VNĐ";
                this.receive_html += "<hr>";
                if (obj.thanhtoan_tructuyen)
                {
                    this.receive_html += "<br>";
                    this.receive_html += "Đơn hàng đã được thanh toán qua cổng thanh toán trực tuyến PnePay";
                }
                else
                {
                    this.receive_html += "<br>";
                    this.receive_html += "Đơn hàng chưa thanh toán, nhân viên chúng tôi sẽ liên lạc với bạn và giao hàng trong thời giân ngắn nhất (tối đa là 72h kể từ khi đơn hàng được ghi nhận), bạn sẽ thanh toán khi nhân viên giao hàng yêu cầu";
                }
            }
        }
    }
}