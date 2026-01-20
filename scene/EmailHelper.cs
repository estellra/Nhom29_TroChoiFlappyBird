using System;
using System.Net;
using System.Net.Mail;
using Godot;
using System.Threading.Tasks;

public static class EmailHelper
{
	private static string myEmail = "huykiento@gmail.com";
	private static string myPassword = "uzvt fayw dhuq hahl"; 

	public static async Task<string> SendOTPAsync(string toEmail)
{
	try
	{
		Random rand = new Random();
		string otp = rand.Next(100000, 999999).ToString();

		MailMessage mail = new MailMessage();
		mail.From = new MailAddress(myEmail);
		mail.To.Add(toEmail);
		mail.Subject = "Mã xác nhận OTP - Flappy Bird";
		mail.Body = $"Mã OTP của bạn là: {otp}\nVui lòng không chia sẻ mã này cho ai.";

		SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
		{
			Credentials = new NetworkCredential(myEmail, myPassword),
			EnableSsl = true
		};

		await smtp.SendMailAsync(mail);

		GD.Print("OTP đã gửi: " + otp);
		return otp;
	}
	catch (Exception e)
	{
		GD.PrintErr("Lỗi gửi mail: " + e.Message);
		return null;
	}
}

}
