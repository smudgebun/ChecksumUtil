using Godot;

public partial class UI : Control
{
//Nodes
	private static LineEdit Filepath;
	private static LineEdit Hash;
	private static LineEdit Checktype;
	private static RichTextLabel Output;
	public Timer LoadTime;
	private static RichTextLabel DebugLabel;
	public static LineEdit OutputLength;

//strings
	private static string checkinfo;
	private static string typeinfo;
	private static string pathinfo;
	private static string lengthinfo;
	private static string greeting;
//colors
	private static Color error = new Color(1,0,0,1);
	private static Color red = new Color(1,0,0,1);
	private static Color green = new Color(0,1,0,1);



	public override void _Ready()
	{
		Filepath = GetNode<LineEdit>("/root/UI/Control/VBoxContainer/Filepath");
		Hash = GetNode<LineEdit>("/root/UI/Control/VBoxContainer/Hash");
		Output = GetNode<RichTextLabel>("/root/UI/Control/VBoxContainer/RichTextLabel");
		Checktype = GetNode<LineEdit>("/root/UI/Control/VBoxContainer/Hashtype");
		LoadTime = GetNode<Timer>("/root/UI/LoadTime");
		DebugLabel = GetNode<RichTextLabel>("/root/UI/DebugLabel");
		OutputLength = GetNode<LineEdit>("/root/UI/Control/VBoxContainer/OutputLength");

		LoadTime.Timeout += OnTimerTimeoutSignal;
		greeting = "Tips: \n[*] Filepath should be from root\n[*] To run with no hashcheck, input ! as the hash \n[*] Output Length is only used for SHAKE algorithms with no check\n[*] Press Enter to Run";
		Output.Text = string.Format(greeting);
	}

	public override void _Process(double delta)
	{
		DebugLabel.Text = "";
		DebugLabel.AddText(string.Format("is_length_valid?: {0} \n", Checksum.is_length_valid));
		DebugLabel.AddText(string.Format("filehash: {0} \n", Checksum.filehash));
		DebugLabel.AddText(string.Format("can_run: {0} \n", Checksum.can_run));
		DebugLabel.AddText(string.Format("load time: {0} \n", LoadTime.TimeLeft.ToString()));
		DebugLabel.AddText(string.Format("can_run: {0} \n", Checksum.is_safe));
		DebugLabel.AddText(string.Format("is_check_valid? {0}\n", Checksum.is_check_valid.ToString()));
		DebugLabel.AddText(string.Format("check value: {0} \n", Checksum.check));
		DebugLabel.AddText(string.Format("is_comparing?: {0} \n", Checksum.is_comparing));

		if(Input.IsActionJustPressed("Enter") && Checksum.can_run){
			Checksum.can_run = false;

			Checksum.typeraw = Checktype.Text;
			Checksum.hashraw = Hash.Text;
			Checksum.pathraw = Filepath.Text;
			Checksum.lengthraw = OutputLength.Text;


			Checksum.InputCheck();

			if(Checksum.is_check_valid && Checksum.is_type_valid && Checksum.is_path_valid && Checksum.is_length_valid){
				Checksum.Start();
			}
			else{
				InvalidHandler();
				}
		}
	}

	private void InvalidHandler(){
		Output.Text = "";
		pathinfo = string.Format("Filepath Exception Occurred:");
		checkinfo = string.Format("Invalid hash. to run with no hashcheck, enter ! for hash");
		typeinfo = string.Format("Invalid checktype\n(supported checktypes: MD5, SHA1, SHA256, SHA384, SHA512, SHA3_256, SHA3_384, SHA3_512, Shake128, Shake256)");
		lengthinfo = string.Format("Invalid output length (note: SHAKE will always output an even number, rounded down)");
		LoadTime.Start();
	}
	private void OnTimerTimeoutSignal(){
		InvalidOutput();
	}
	private void InvalidOutput(){
		if(!Checksum.is_path_valid){
			if(Checksum.is_path_empty){
				Output.AddText("> No Filepath provided\n\n");
			}
			else{
				Output.AddText(string.Format("! {0}\n  {1}\n\n", pathinfo, Checksum.file_error[0]));
			}
		}
		if(!Checksum.is_type_valid){
			if(Checksum.is_type_empty){
				Output.AddText("> No checktype provided\n\n");
			}
			else{
				Output.AddText(string.Format("> {0}\n\n", typeinfo));
			}
		}
		if(!Checksum.is_check_valid){
			if(Checksum.is_hash_empty){
				Output.AddText("> No hash provided. To run with no hashcheck, enter ! for hash\n\n");
			}
			else{
				Output.AddText(string.Format("> {0}\n\n", checkinfo));
			}
		}
		if(!Checksum.is_length_valid){
			if(Checksum.is_length_empty){
				Output.AddText("> No Output Length Provided (note: SHAKE will always round to an even number)");
			}
			else{
			Output.AddText(string.Format("> {0}\n\n", lengthinfo));
			}
		}

		Checksum.can_run = true;
	}

	public static void WriteOutput(){
		if(Checksum.is_comparing){
			Output.Text = "";
			Output.Text = string.Format("Extracted hash: {0}, Compared Hash: {1}\n\n", Checksum.filehash, Checksum.check.ToUpper());
			Output.AddText(string.Format("is file safe? "));
			if(Checksum.is_safe){
				Output.PushColor(green);
				Output.AddText("YES");
			}
			else{
				Output.PushColor(red);
				Output.AddText("NO");
			}
		}

		else{
			Output.Text = "";
			Output.Text = string.Format("Extracted hash: {0}, of type {1}\n", Checksum.filehash, Checksum.checktype);
		}

	}


	public static void ErrorOccurred(){
		Output.Text = "";
		Output.AddText("Hashing error occurred!");
		Output.PushColor(error);
		Output.AddText(Checksum.hash_error);
		Checksum.can_run = true;
	}

	public static void UnknownErrorOccurred(){
		Output.Text = "A completely unkown error occurred... please write up a report on github so I can try to fix";
	}
}


