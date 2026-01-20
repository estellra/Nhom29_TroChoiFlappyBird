using Godot;
using System;
using System.Threading.Tasks;

public partial class Otpmail : Control
{
	private string currentEmail = "";
	private int timeLeft = 60;

	private Timer timer;

	private TextureButton btnSend;
	private TextureButton btnResend;
	private TextureButton btnOK;
	private TextureButton btnBack;

	private Label lblSend;
	private Label lblResend;
	private Label lblOK;

	private LineEdit inputOTP;
	private LineEdit inputEmail;

	public override void _Ready()
	{
		inputEmail = GetNode<LineEdit>("UserName");
		inputOTP   = GetNode<LineEdit>("OTP");

		btnSend   = GetNode<TextureButton>("btnSendOTP");
		btnResend = GetNode<TextureButton>("btnResendOTP");
		btnOK     = GetNode<TextureButton>("btnOK");
		btnBack   = GetNode<TextureButton>("btnBack");

		lblSend   = btnSend.GetNode<Label>("Label");
		lblResend = btnResend.GetNode<Label>("Label");
		lblOK     = btnOK.GetNode<Label>("Label");

		timer = GetNode<Timer>("CooldownTimer");

		// UI initial state
		inputOTP.Visible = false;
		btnOK.Visible = false;
		btnResend.Visible = false;

		btnBack.Pressed += () =>
			GetTree().ChangeSceneToFile("res://scene/login.tscn");

		btnSend.Pressed   += OnSendPressed;
		btnResend.Pressed += OnSendPressed;

		timer.Timeout += OnCooldownTick;

		btnOK.Pressed += OnConfirmOTP;
	}

	// =============================
	// SEND OTP (MAIN FIX)
	// =============================
	private async void OnSendPressed()
	{
		string email = inputEmail.Text.Trim();

		if (email == "")
		{
			OS.Alert("Vui lòng nhập Email!", "Thiếu thông tin");
			return;
		}

		btnSend.Disabled = true;
		btnResend.Disabled = true;
		lblSend.Text = "Đang gửi...";

		string otp;

		try
		{
			// ⚠️ KHÔNG Task.Run
			otp = await EmailHelper.SendOTPAsync(email);
		}
		catch (Exception e)
		{
			GD.PrintErr(e.Message);
			otp = null;
		}

		if (string.IsNullOrEmpty(otp))
		{
			OS.Alert("Gửi OTP thất bại!", "Lỗi");
			btnSend.Disabled = false;
			lblSend.Text = "Send OTP";
			return;
		}

		// SUCCESS
		DatabaseManager.SaveOTP(email, otp);
		currentEmail = email;

		inputOTP.Visible = true;
		btnOK.Visible = true;
		inputOTP.Text = "";
		inputOTP.GrabFocus();

		OS.Alert("Đã gửi mã OTP! Vui lòng kiểm tra Email.", "Thành công");

		StartCooldown();
	}

	// =============================
	// CONFIRM OTP
	// =============================
	private void OnConfirmOTP()
	{
		string code = inputOTP.Text.Trim();

		if (code == "")
		{
			OS.Alert("Vui lòng nhập mã OTP!", "Thiếu");
			return;
		}

		if (!DatabaseManager.VerifyOTP(currentEmail, code))
		{
			OS.Alert("Mã OTP không đúng!", "Sai mã");
			return;
		}

		GlobalData.ResetEmail = currentEmail;
		GetTree().ChangeSceneToFile("res://scene/newpass.tscn");
	}

	// =============================
	// COOLDOWN
	// =============================
	private void StartCooldown()
	{
		timeLeft = 60;

		btnSend.Visible = false;
		btnResend.Visible = true;
		btnResend.Disabled = true;

		lblResend.Text = $"Gửi lại ({timeLeft}s)";
		timer.Start();
	}

	private void OnCooldownTick()
	{
		timeLeft--;
		lblResend.Text = $"Gửi lại ({timeLeft}s)";

		if (timeLeft <= 0)
		{
			timer.Stop();
			btnResend.Disabled = false;
			lblResend.Text = "Gửi lại";
		}
	}
}
