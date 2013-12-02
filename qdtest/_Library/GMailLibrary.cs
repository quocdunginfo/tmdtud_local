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
        public String sender_email = "cuahangbangiay@gmail.com";
        public String sender_password = "kienkimkhung";
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
            this.receive_title = "Khôi phục mật khẩu Website Cửa hàng giày dép BigFoot";
            this.receive_html = "<img src=\"https://lh6.googleusercontent.com/-P2R_CIG6H7E/Upzdb6dcFmI/AAAAAAAAABY/rJJOpMz5wJo/w680-h353-no/CHIEW.gif" + "\" alt=\"Cửa hàng BigFoot\" style=\"margin-bottom:20px;box-shadow: 0px 0px 3px 4px #a1a1a1;max-height:110px\">";
            this.receive_html += "<br>";
            this.receive_html += "Bạn hoặc một ai đó đã gửi yêu cầu khôi phục lại mật khẩu tại Website <a href=\"http://localhost:53655/" + "\" title=\"Cửa hàng BigFoot\" target=\"_blank\"> Cửa hàng giày dép BigFoot</a>!";
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
                this.receive_title = "Thông tin đơn hàng mã số ["+obj.id+"] từ Website Cửa hàng giày dép BigFoot";
                this.receive_html = "<div style=\"width:680px\"><a href=\"http://localhost:53655/" + "\" title=\"Cửa hàng BigFoot\" target=\"_blank\"><img src=\"https://lh6.googleusercontent.com/-P2R_CIG6H7E/Upzdb6dcFmI/AAAAAAAAABY/rJJOpMz5wJo/w680-h353-no/CHIEW.gif" + "\" alt=\"Cửa hàng BigFoot\" style=\"margin-bottom:20px;box-shadow: 0px 0px 3px 4px #a1a1a1;max-height:110px\"></a> <p style=\"margin-top:0px;margin-bottom:20px;\">Cám ơn bạn đã quan tâm sản phẩm Cửa hàng BigFoot. Đơn hàng của bạn đang được xử lý, chúng tôi sẽ liên hệ với bạn trong thời gian vòng 72 giờ kể từ khi nhận được email này.</p>";
                this.receive_html += "<table style=\"border-collapse:collapse;width:100%;border-top:1px solid #dddddd;border-left:1px solid #dddddd;margin-bottom:20px\"><thead><tr><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;background-color:#efefef;font-weight:bold;text-align:left;padding:7px;color:#222222\" colspan=\"2\">Chi tiết đơn hàng</td></tr></thead><tbody><tr>";
                this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:left;padding:7px\"><b>Mã Đơn Hàng:</b> "+obj.id+"<br>";
                this.receive_html += "<b>Ngày Đặt:</b> "+obj._get_ngay()+"<br>";
                string thanhtoan = "Thanh toán trực tuyến qua OnePay";
                if(obj.thanhtoan_tructuyen==false) thanhtoan="Thanh toán tại nhà (Chưa thanh toán)";
                this.receive_html += "<b>Phương thức thanh toán:</b>" + thanhtoan + "<br> </td>";
                this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:left;padding:7px\">";
                this.receive_html += "<b>Họ tên khách hàng:</b> "+obj._get_khachhang_tendaydu()+"<br>";
                this.receive_html += "<b>Email:</b> <a href=\"mailto:"+obj._get_khachhang_email()+"\" target=\"_blank\">"+obj._get_khachhang_email()+"</a><br>";
                this.receive_html += "<b>Điện thoại:</b> "+obj.nguoinhan_sdt+"<br></td>";
                this.receive_html += " </tr></tbody>  </table>";
                this.receive_html+="<table style=\"border-collapse:collapse;width:100%;border-top:1px solid #dddddd;border-left:1px solid #dddddd;margin-bottom:20px\"><thead><tr><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;background-color:#efefef;font-weight:bold;text-align:left;padding:7px;color:#222222\">Địa chỉ nhận hàng</td>";
                this.receive_html += " </tr></thead><tbody><tr> <td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:left;padding:7px\">"+obj.nguoinhan_diachi+"<br>"+obj.nguoinhan_diachi_tinhtp.ten.ToString()+"<br>Việt Nam</td></tr></tbody></table>";
                this.receive_html += "<table style=\"border-collapse:collapse;width:100%;border-top:1px solid #dddddd;border-left:1px solid #dddddd;margin-bottom:20px\"><thead><tr><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;background-color:#efefef;font-weight:bold;text-align:left;padding:7px;color:#222222\">Mã sản phẩm</td><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;background-color:#efefef;font-weight:bold;text-align:left;padding:7px;color:#222222\">Tên sản phẩm</td> <td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;background-color:#efefef;font-weight:bold;text-align:left;padding:7px;color:#222222\">Màu sắc</td> <td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;background-color:#efefef;font-weight:bold;text-align:left;padding:7px;color:#222222\">Kích thước</td><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;background-color:#efefef;font-weight:bold;text-align:right;padding:7px;color:#222222\">Số lượng</td><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;background-color:#efefef;font-weight:bold;text-align:right;padding:7px;color:#222222\">Giá</td><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;background-color:#efefef;font-weight:bold;text-align:right;padding:7px;color:#222222\">Tổng cộng</td> </tr></thead>";
                //this.receive_html = "Xin chào "+obj._get_khachhang_tendaydu();
                //this.receive_html += "<br>";
                //this.receive_html += "Bạn đã đặt hàng tại của hàng chúng tôi với thông tin chi tiết như sau";
                //this.receive_html += "<br><br>";
                //this.receive_html += "Mã SP | Tên SP | Màu | Kích thước | Số lượng x Đơn giá = Tổng cộng";
                //this.receive_html += "<br>";
                foreach (var item in obj.ds_chitiet_donhang)
                {
                    this.receive_html += " <tbody><tr>";
                    this.receive_html+="<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:left;padding:7px\">";
                    this.receive_html += item.chitietsp.sanpham.masp;
                    this.receive_html += "          </td> ";
                    this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:left;padding:7px\">";
                    this.receive_html += item.chitietsp.sanpham.ten;
                    this.receive_html += " </td> ";
                    this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:left;padding:7px\">";
                    this.receive_html += item.chitietsp.mausac.giatri;
                    this.receive_html += " </td> ";
                    this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:left;padding:7px\">";
                    this.receive_html += item.chitietsp.kichthuoc.giatri;
                    this.receive_html += " </td> ";
                    this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:left;padding:7px\">";
                    this.receive_html += item.soluong;
                    this.receive_html += " </td> ";
                    this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\">";
                    this.receive_html += item.dongia + "VNĐ";
                    this.receive_html += " </td> ";
                    this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\">";
                    this.receive_html += item._get_total() + "VNĐ";
                    this.receive_html += " </td> ";
                    this.receive_html += "  </tr></tbody>";
                }
                this.receive_html += "  <tfoot>";
                this.receive_html += "<tr><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\" colspan=\"6\"><b>Thành tiền::</b></td>";
                this.receive_html+="<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\">"+obj._get_tongtien_notinclude_phivanchuyen() + "VNĐ </td></tr>";
                this.receive_html += "<tr><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\" colspan=\"6\"><b>Phí vận chuyển::</b></td>";
                this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\">" + obj._get_phivanchuyen() + "VNĐ </td></tr>";
                this.receive_html += "<tr><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\" colspan=\"6\"><b>Giảm giá::</b></td>";
                this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\">" + obj._get_giamgia_tuloaikh() + "VNĐ </td></tr>";
                this.receive_html += "<tr><td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\" colspan=\"6\"><b>Tổng tiền::</b></td>";
                this.receive_html += "<td style=\"font-size:12px;border-right:1px solid #dddddd;border-bottom:1px solid #dddddd;text-align:right;padding:7px\">" + obj._get_tongtien_include_phivanchuyen_giamgia_tuloaikh() + "VNĐ </td></tr>";
               
                //this.receive_html += "<br>";
                //this.receive_html += "Phí vận chuyển: " + obj._get_phivanchuyen() + "VNĐ";
                //this.receive_html += "<br>";
                //this.receive_html += "Giảm giá: " + obj._get_giamgia_tuloaikh() + "VNĐ";
                //this.receive_html += "<br>";
                //this.receive_html += "Tổng tiền phải thanh toán: " + obj._get_tongtien_include_phivanchuyen_giamgia_tuloaikh() + "VNĐ";
                //this.receive_html += "<hr>";
                //if (obj.thanhtoan_tructuyen)
                //{
                //    this.receive_html += "<br>";
                //    this.receive_html += "Đơn hàng đã được thanh toán qua cổng thanh toán trực tuyến PnePay";
                //}
                //else
                //{
                //    this.receive_html += "<br>";
                //    this.receive_html += "Đơn hàng chưa thanh toán, nhân viên chúng tôi sẽ liên lạc với bạn và giao hàng trong thời giân ngắn nhất (tối đa là 72h kể từ khi đơn hàng được ghi nhận), bạn sẽ thanh toán khi nhân viên giao hàng yêu cầu";
                //}
                this.receive_html += "</tfoot></table>";
                this.receive_html += "<p style=\"margin-top:0px;margin-bottom:20px\">Vui lòng trả lời thư này nếu có bất kì câu hỏi nào.</p>";
                this.receive_html+="<p style=\"margin-top:0px;margin-bottom:20px\">Nhân viên của chúng tôi sẽ liên lạc với bạn và giao hàng trong thời giân ngắn nhất (tối đa là 72h kể từ khi đơn hàng được ghi nhận).</p>";
                this.receive_html+="<p style=\"margin-top:0px;margin-bottom:20px\">Đối với đơn hàng thanh toán tại nhà, nhân viên giao hàng sẽ nhận tiền thanh toán khi giao hàng.</p>";
                this.receive_html += "<p style=\"margin-top:0px;margin-bottom:20px\">Copyright © 2013 quocdunginfo ft kienkimkhung Corp. | Special thanks to Joomla for this email template.</p><div class=\"yj6qo\"></div><div class=\"adL\"></div></div>";
            }
        }
    }
}