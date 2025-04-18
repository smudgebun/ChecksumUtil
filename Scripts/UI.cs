using Godot;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;

public partial class UI : Control
{
	public bool bluh = true;

	public LineEdit Filepath;
	public LineEdit Hash;
	public LineEdit Checktype;

	public static RichTextLabel Output;
	public Timer LoadTime;

	private static string checkinfo;
	private static string typeinfo;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Filepath = GetNode<LineEdit>("/root/UI/Control/VBoxContainer/Filepath");
		Hash = GetNode<LineEdit>("/root/UI/Control/VBoxContainer/Hash");
		Output = GetNode<RichTextLabel>("/root/UI/Control/VBoxContainer/RichTextLabel");
		Checktype = GetNode<LineEdit>("/root/UI/Control/VBoxContainer/Hashtype");
		LoadTime = GetNode<Timer>("/root/UI/LoadTime");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("Enter") && Checksum.can_run){
			Checksum.can_run = false;

			Checksum.typeraw = Checktype.Text;
			Checksum.hashraw = Hash.Text;
			Checksum.pathraw = Filepath.Text;


			Checksum.InputCheck();

			if(Checksum.is_check_valid && Checksum.is_type_valid){
				Checksum.Start();
			}
			else{InvalidHandler();}
		}
	}

	private void InvalidHandler(){
		Output.Text = "";

		if(Checksum.is_check_valid){
		checkinfo = "";
		}
		else{
			checkinfo = string.Format("Checksum: invalid, please re-enter");
		}

		if(Checksum.is_type_valid){
			typeinfo = "";
		}
		else{
			typeinfo = string.Format("Checktype: invalid, please re-enter /nsupported checktypes: MD2 MD4 MD5 SHA1 SHA256 SHA384 SHA512");
		}
		InvalidOutput();
	}
	private void InvalidOutput(){
		if(!Checksum.is_check_valid && !Checksum.is_type_valid){
		Output.Text = string.Format("{0} \n{1}", checkinfo, typeinfo);
		}
		else if(!Checksum.is_check_valid){
			Output.Text = string.Format(checkinfo);
		}
		else{
			Output.Text = string.Format(typeinfo);
		}
		Checksum.can_run = true;
	}


	public static void Success(){
		Output.Text = "you did it ayayaya :3";
	}
}


