param([string]$inputFile=$null, [string]$outputFile=$null, [switch]$verbose, [switch] $debug, [switch]$runtime20, [switch]$x86, [switch]$x64, [switch]$runtime30, [int]$lcid, [switch]$sta, [switch]$mta, [switch]$noConsole, [switch]$nested)


<################################################################################>
<##                                                                            ##>
<##      PS2EXE v0.4.0.0  -  http://ps2exe.codeplex.com                        ##>
<##          written by:                                                       ##>
<##            * Ingo Karstein (http://blog.karstein-consulting.com)           ##>
<##                                                                            ##>
<##      This script is released under Microsoft Public Licence                ##>
<##          that can be downloaded here:                                      ##>
<##          http://www.microsoft.com/opensource/licenses.mspx#Ms-PL           ##>
<##                                                                            ##>
<##      This script was created using PowerGUI (http://www.powergui.org)      ##>
<##             ... and Windows PowerShell ISE v3.0                            ##>
<##                                                                            ##>
<################################################################################>

if( !$nested ) {
    Write-Host "PS2EXE; v0.3.0.0 by Ingo Karstein (http://blog.karstein-consulting.com)"
    Write-Host ""
} else {
    write-host "PowerShell 2.0 environment started..."
    Write-Host ""
}

if( $runtime20 -eq $true -and $runtime30 -eq $true ) {
    write-host "YOU CANNOT USE SWITCHES -runtime20 AND -runtime30 AT THE SAME TIME!"
    exit -1
}

if( $sta -eq $true -and $mta -eq $true ) {
    write-host "YOU CANNOT USE SWITCHES -sta AND -eta AT THE SAME TIME!"
    exit -1
}


if( [string]::IsNullOrEmpty($inputFile) -or [string]::IsNullOrEmpty($outputFile) ) {
Write-Host "Usage:"
Write-Host ""
Write-Host "    powershell.exe -command ""&'.\ps2exe.ps1' [-inputFile] '<file_name>'"
write-host "                   [-outputFile] '<file_name>' "
write-host "                   [-verbose] [-debug] [-runtime20] [-runtime30]"""
Write-Host ""       
Write-Host "       inputFile = PowerShell script that you want to convert to EXE"       
Write-Host "      outputFile = destination EXE file name"       
Write-Host "         verbose = Output verbose informations - if any"       
Write-Host "           debug = generate debug informations for output file" 
Write-Host "           debug = generate debug informations for output file"       
Write-Host "       runtime20 = this switch forces PS2EXE to create a config file for" 
write-host "                   the generated EXE that contains the ""supported .NET"
write-host "                   Framework versions"" setting for .NET Framework 2.0"
write-host "                   for PowerShell 2.0"
Write-Host "       runtime30 = this switch forces PS2EXE to create a config file for" 
write-host "                   the generated EXE that contains the ""supported .NET"
write-host "                   Framework versions"" setting for .NET Framework 4.0"
write-host "                   for PowerShell 3.0"
Write-Host "            lcid = Location ID for the compiled EXE. Current user"
write-host "                   culture if not specified." 
Write-Host "             x86 = Compile for 32-bit runtime only"
Write-Host "             x64 = Compile for 64-bit runtime only"
Write-Host "             sta = Single Thread Apartment Mode"
Write-Host "             mta = Multi Thread Apartment Mode"
write-host "       noConsole = The resulting EXE file starts without a console window just like a Windows Forms app."
write-host ""
}

$psversion = 0
if($PSVersionTable.PSVersion.Major -eq 3) {
    $psversion = 3
    write-host "You are using PowerShell 3.0."
}

if($PSVersionTable.PSVersion.Major -eq 2) {
    $psversion = 2
    write-host "You are using PowerShell 2.0."
}

if( $psversion -eq 0 ) {
    write-host "THE POWERSHELL VERSION IS UNKNOWN!"
    exit -1
}

if( [string]::IsNullOrEmpty($inputFile) -or [string]::IsNullOrEmpty($outputFile) ) {
    write-host ""
    exit -1
}

if( !$runtime20 -and !$runtime30 ) {
    if( $psversion -eq 3 ) {
        $runtime30 = $true
    } else {
        $runtime20 = $true
    }
}

if( $psversion -eq 3 -and $runtime20 ) {
    write-host "To create a EXE file for PowerShell 2.0 on PowerShell 3.0 this script now launces PowerShell 2.0..."
    write-host ""

    $arguments = "-inputFile '$($inputFile)' -outputFile '$($outputFile)' -nested "

    if($verbose) { $arguments += "-verbose "}
    if($debug) { $arguments += "-debug "}
    if($runtime20) { $arguments += "-runtime20 "}
    if($x86) { $arguments += "-x86 "}
    if($x64) { $arguments += "-verbose "}
    if($lcid) { $arguments += "-lcid $lcid "}
    if($sta) { $arguments += "-sta "}
    if($mta) { $arguments += "-mta "}
    if($noconsole) { $arguments += "-noconsole "}

    $jobScript = @"
."$($PSHOME)\powershell.exe" -version 2.0 -command "&'$($MyInvocation.MyCommand.Path)' $($arguments)"
"@
    Invoke-Expression $jobScript

    exit 0
}

if( $psversion -eq 2 -and $runtime30 ) {
    Write-Host "YOU NEED TO RUN PS2EXE IN AN POWERSHELL 3.0 ENVIRONMENT"
    Write-Host "  TO USE PARAMETER -runtime30"
    write-host
    exit -1
}

write-host ""


Set-Location (Split-Path $MyInvocation.MyCommand.Path)

$type = ('System.Collections.Generic.Dictionary`2') -as "Type"
$type = $type.MakeGenericType( @( ("System.String" -as "Type"), ("system.string" -as "Type") ) )
$o = [Activator]::CreateInstance($type)

if( $runtime30 -eq 3 ) {
    $o.Add("CompilerVersion", "v4.0")
} else {
    $o.Add("CompilerVersion", "v2.0")
}

$referenceAssembies = @("System.dll")
$referenceAssembies += ([System.AppDomain]::CurrentDomain.GetAssemblies() | ? { $_.ManifestModule.Name -ieq "Microsoft.PowerShell.ConsoleHost" } | select -First 1).location
$referenceAssembies += ([System.AppDomain]::CurrentDomain.GetAssemblies() | ? { $_.ManifestModule.Name -ieq "System.Management.Automation.dll" } | select -First 1).location

if( $runtime30 ) {
    $n = new-object System.Reflection.AssemblyName("System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
    [System.AppDomain]::CurrentDomain.Load($n) | Out-Null
    $referenceAssembies += ([System.AppDomain]::CurrentDomain.GetAssemblies() | ? { $_.ManifestModule.Name -ieq "System.Core.dll" } | select -First 1).location
}

if( $noConsole ) {
	$n = new-object System.Reflection.AssemblyName("System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
    if( $runtime30 ) {
		$n = new-object System.Reflection.AssemblyName("System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")
	}
    [System.AppDomain]::CurrentDomain.Load($n) | Out-Null

	$n = new-object System.Reflection.AssemblyName("System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
    if( $runtime30 ) {
		$n = new-object System.Reflection.AssemblyName("System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")
	}
    [System.AppDomain]::CurrentDomain.Load($n) | Out-Null

	
	$referenceAssembies += ([System.AppDomain]::CurrentDomain.GetAssemblies() | ? { $_.ManifestModule.Name -ieq "System.Windows.Forms.dll" } | select -First 1).location
    $referenceAssembies += ([System.AppDomain]::CurrentDomain.GetAssemblies() | ? { $_.ManifestModule.Name -ieq "System.Drawing.dll" } | select -First 1).location
}

$inputFile = [System.IO.Path]::GetFullPath($inputFile) 
$outputFile = [System.IO.Path]::GetFullPath($outputFile) 

$platform = "anycpu"
if( $x64 -and !$x86 ) { $platform = "x64" } else { if ($x86 -and !$x64) { $platform = "x86" }}

$cop = (new-object Microsoft.CSharp.CSharpCodeProvider($o))
$cp = New-Object System.CodeDom.Compiler.CompilerParameters($referenceAssembies, $outputFile)
$cp.GenerateInMemory = $false
$cp.GenerateExecutable = $true
$cp.CompilerOptions = "/platform:$($platform) /target:$( if($noConsole){'winexe'}else{'exe'})"
Write-Host "$( if($noConsole){'winexe'}else{'exe'})" -ForegroundColor red
$cp.IncludeDebugInformation = $debug

if( $debug ) {
	#$cp.TempFiles.TempDir = (split-path $inputFile)
	$cp.TempFiles.KeepFiles = $true
	
}	

Write-Host "Reading input file " -NoNewline 
Write-Host $inputFile 
Write-Host ""
$content = Get-Content -LiteralPath ($inputFile) -Encoding UTF8 -ErrorAction SilentlyContinue
if( $content -eq $null ) {
	Write-Host "No data found. May be read error or file protected."
	exit -2
}
$scriptInp = [string]::Join("`r`n", $content)
$script = [System.Convert]::ToBase64String(([System.Text.Encoding]::UTF8.GetBytes($scriptInp)))

#region program frame
    $culture = ""

    if( $lcid ) {
    $culture = @"
    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo($lcid);
    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo($lcid);
"@
    }
	
	$forms = @"
		    internal class ReadKeyForm 
		    {
		        public KeyInfo key = new KeyInfo();
				public ReadKeyForm() {}
				public void ShowDialog() {}
			}
			
			internal class CredentialForm
		    {
				public class UserPwd
		        {
		            public string User = string.Empty;
		            public string Password = string.Empty;
		            public string Domain = string.Empty;
		        }

				public static UserPwd PromptForPassword(string caption, string message, string target, string user, PSCredentialTypes credTypes, PSCredentialUIOptions options) { return null;}
			}
"@	
	if( $noConsole ) {
	
		$forms = @"
			internal class CredentialForm
		    {
		        // http://www.pinvoke.net/default.aspx/credui/CredUnPackAuthenticationBuffer.html

		        /* >= VISTA 
		        [DllImport("ole32.dll")]
		        public static extern void CoTaskMemFree(IntPtr ptr);

		        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		        private struct CREDUI_INFO
		        {
		            public int cbSize;
		            public IntPtr hwndParent;
		            public string pszMessageText;
		            public string pszCaptionText;
		            public IntPtr hbmBanner;
		        }

		        [DllImport("credui.dll", CharSet = CharSet.Auto)]
		        private static extern bool CredUnPackAuthenticationBuffer(int dwFlags, IntPtr pAuthBuffer, uint cbAuthBuffer, StringBuilder pszUserName, ref int pcchMaxUserName, StringBuilder pszDomainName, ref int pcchMaxDomainame, StringBuilder pszPassword, ref int pcchMaxPassword);

		        [DllImport("credui.dll", CharSet = CharSet.Auto)]
		        private static extern int CredUIPromptForWindowsCredentials(ref CREDUI_INFO notUsedHere, int authError, ref uint authPackage, IntPtr InAuthBuffer, uint InAuthBufferSize, out IntPtr refOutAuthBuffer, out uint refOutAuthBufferSize, ref bool fSave, int flags);

		        public class UserPwd
		        {
		            public string User = string.Empty;
		            public string Password = string.Empty;
		            public string Domain = string.Empty;
		        }

		        public static UserPwd GetCredentialsVistaAndUp(string caption, string message)
		        {
		            CREDUI_INFO credui = new CREDUI_INFO();
		            credui.pszCaptionText = caption;
		            credui.pszMessageText = message;
		            credui.cbSize = Marshal.SizeOf(credui);
		            uint authPackage = 0;
		            IntPtr outCredBuffer = new IntPtr();
		            uint outCredSize;
		            bool save = false;
		            int result = CredUIPromptForWindowsCredentials(ref credui, 0, ref authPackage, IntPtr.Zero, 0, out outCredBuffer, out outCredSize, ref save, 1 / * Generic * /);

		            var usernameBuf = new StringBuilder(100);
		            var passwordBuf = new StringBuilder(100);
		            var domainBuf = new StringBuilder(100);

		            int maxUserName = 100;
		            int maxDomain = 100;
		            int maxPassword = 100;
		            if (result == 0)
		            {
		                if (CredUnPackAuthenticationBuffer(0, outCredBuffer, outCredSize, usernameBuf, ref maxUserName, domainBuf, ref maxDomain, passwordBuf, ref maxPassword))
		                {
		                    //clear the memory allocated by CredUIPromptForWindowsCredentials 
		                    CoTaskMemFree(outCredBuffer);
		                    UserPwd ret = new UserPwd();
		                    ret.User = usernameBuf.ToString();
		                    ret.Password = passwordBuf.ToString();
		                    ret.Domain = domainBuf.ToString();
		                    return ret;
		                }
		            }

		            return null;
		        }
		        */
				
				
				// http://www.pinvoke.net/default.aspx/credui/CredUIPromptForWindowsCredentials.html
				// http://www.pinvoke.net/default.aspx/credui.creduipromptforcredentials#
				
		        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		        private struct CREDUI_INFO
		        {
		            public int cbSize;
		            public IntPtr hwndParent;
		            public string pszMessageText;
		            public string pszCaptionText;
		            public IntPtr hbmBanner;
		        }

		        [Flags]
		        enum CREDUI_FLAGS
		        {
		            INCORRECT_PASSWORD = 0x1,
		            DO_NOT_PERSIST = 0x2,
		            REQUEST_ADMINISTRATOR = 0x4,
		            EXCLUDE_CERTIFICATES = 0x8,
		            REQUIRE_CERTIFICATE = 0x10,
		            SHOW_SAVE_CHECK_BOX = 0x40,
		            ALWAYS_SHOW_UI = 0x80,
		            REQUIRE_SMARTCARD = 0x100,
		            PASSWORD_ONLY_OK = 0x200,
		            VALIDATE_USERNAME = 0x400,
		            COMPLETE_USERNAME = 0x800,
		            PERSIST = 0x1000,
		            SERVER_CREDENTIAL = 0x4000,
		            EXPECT_CONFIRMATION = 0x20000,
		            GENERIC_CREDENTIALS = 0x40000,
		            USERNAME_TARGET_CREDENTIALS = 0x80000,
		            KEEP_USERNAME = 0x100000,
		        }

		        public enum CredUIReturnCodes
		        {
		            NO_ERROR = 0,
		            ERROR_CANCELLED = 1223,
		            ERROR_NO_SUCH_LOGON_SESSION = 1312,
		            ERROR_NOT_FOUND = 1168,
		            ERROR_INVALID_ACCOUNT_NAME = 1315,
		            ERROR_INSUFFICIENT_BUFFER = 122,
		            ERROR_INVALID_PARAMETER = 87,
		            ERROR_INVALID_FLAGS = 1004,
		        }

		        [DllImport("credui")]
		        private static extern CredUIReturnCodes CredUIPromptForCredentials(ref CREDUI_INFO creditUR,
		          string targetName,
		          IntPtr reserved1,
		          int iError,
		          StringBuilder userName,
		          int maxUserName,
		          StringBuilder password,
		          int maxPassword,
		          [MarshalAs(UnmanagedType.Bool)] ref bool pfSave,
		          CREDUI_FLAGS flags);

		        public class UserPwd
		        {
		            public string User = string.Empty;
		            public string Password = string.Empty;
		            public string Domain = string.Empty;
		        }

		        internal static UserPwd PromptForPassword(string caption, string message, string target, string user, PSCredentialTypes credTypes, PSCredentialUIOptions options)
		        {
		            // Setup the flags and variables
		            StringBuilder userPassword = new StringBuilder(), userID = new StringBuilder(user);
		            CREDUI_INFO credUI = new CREDUI_INFO();
		            credUI.cbSize = Marshal.SizeOf(credUI);
		            bool save = false;
		            
		            CREDUI_FLAGS flags = CREDUI_FLAGS.DO_NOT_PERSIST;
		            if ((credTypes & PSCredentialTypes.Domain) != PSCredentialTypes.Domain)
		            {
		                flags |= CREDUI_FLAGS.GENERIC_CREDENTIALS;
		                if ((options & PSCredentialUIOptions.AlwaysPrompt) == PSCredentialUIOptions.AlwaysPrompt)
		                {
		                    flags |= CREDUI_FLAGS.ALWAYS_SHOW_UI;
		                }
		            }

		            // Prompt the user
		            CredUIReturnCodes returnCode = CredUIPromptForCredentials(ref credUI, target, IntPtr.Zero, 0, userID, 100, userPassword, 100, ref save, flags);

		            if (returnCode == CredUIReturnCodes.NO_ERROR)
		            {
		                UserPwd ret = new UserPwd();
		                ret.User = userID.ToString();
		                ret.Password = userPassword.ToString();
		                ret.Domain = "";
		                return ret;
		            }

		            return null;
		        }

		    }
"@

		$forms += @"
		    internal class ReadKeyForm 
		    {
		        public KeyInfo key = new KeyInfo();
				public ReadKeyForm() {}
				public void ShowDialog() {}
			}
"@	
	
	<# NOT FINISHED !!!
		$forms += @"
		    internal class ReadKeyForm : System.Windows.Forms.Form
		    {
		        public KeyInfo key;
		        private System.Windows.Forms.TextBox textBox1;
		        private System.Windows.Forms.Button button1;

		        private void InitializeComponent()
		        {
		            this.textBox1 = new System.Windows.Forms.TextBox();
		            this.button1 = new System.Windows.Forms.Button();
		            this.SuspendLayout();
		            // 
		            // textBox1
		            // 
		            this.textBox1.AcceptsReturn = true;
		            this.textBox1.AcceptsTab = true;
		            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		            this.textBox1.Location = new System.Drawing.Point(0, 0);
		            this.textBox1.Multiline = true;
		            this.textBox1.Name = "textBox1";
		            this.textBox1.Size = new System.Drawing.Size(226, 61);
		            this.textBox1.TabIndex = 0;
		            this.textBox1.ShortcutsEnabled = false;
		            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
		            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
		            // 
		            // button1
		            // 
		            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
		            this.button1.Location = new System.Drawing.Point(226, 0);
		            this.button1.Name = "button1";
		            this.button1.Size = new System.Drawing.Size(75, 61);
		            this.button1.TabIndex = 1;
		            this.button1.Text = "Cancel";
		            this.button1.UseVisualStyleBackColor = true;
		            // 
		            // Form1
		            // 
		            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		            this.ClientSize = new System.Drawing.Size(301, 61);
		            this.Controls.Add(this.textBox1);
		            this.Controls.Add(this.button1);
		            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		            this.Name = "Form1";
		            this.Text = "Press a key...";
		            this.ResumeLayout(false);
		            this.PerformLayout();

		        }


		        private bool alt = false;
		        private bool ctrl = false;
		        private bool shift = false;
		        private System.Windows.Forms.Keys keycode = System.Windows.Forms.Keys.None;
		        private System.Windows.Forms.Keys keydata = System.Windows.Forms.Keys.None;
		        private int keyvalue = 0;

		        private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		        {
		           // key = new KeyInfo(e.
		        }

		        private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		        {
		            alt = e.Alt;
		            ctrl = e.Control;
		            shift = e.Shift;
		            keycode = e.KeyCode;
		            keydata = e.KeyData;
		            keyvalue = e.KeyValue;

		            e.SuppressKeyPress = true;
		            e.Handled = true;

		            if (keyvalue >= 32)
		            {
		                ControlKeyStates k = 0;
		                if(e.Alt )
		                    k |= ControlKeyStates.LeftAltPressed | ControlKeyStates.RightAltPressed;
		                if(e.Control )
		                    k |= ControlKeyStates.LeftCtrlPressed | ControlKeyStates.RightCtrlPressed;
		                if(e.Shift) 
		                    k |= ControlKeyStates.ShiftPressed;
		                if((e.Modifiers & System.Windows.Forms.Keys.CapsLock) > 0)
		                    k |= ControlKeyStates.CapsLockOn;

		                key = new KeyInfo(0, (char)keyvalue, k, false);
		                this.Close();
		            }
		        }

		        public ReadKeyForm()
		        {
		            InitializeComponent();
		            textBox1.Focus();
		        }
		    }
"@
	#>
		}
		

	$programFrame = @"
	//Simple PowerShell host created by Ingo Karstein (http://blog.karstein-consulting.com)
	//   for PS2EXE (http://ps2exe.codeplex.com)


	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Management.Automation;
	using System.Management.Automation.Runspaces;
	using PowerShell = System.Management.Automation.PowerShell;
	using System.Globalization;
	using System.Management.Automation.Host;
	using System.Security;
	using System.Reflection;
	using System.Runtime.InteropServices;

	namespace ik.PowerShell
	{
$forms
		internal class PS2EXEHostRawUI : PSHostRawUserInterface
	    {
			private const bool CONSOLE = $(if($noConsole){"false"}else{"true"});

			public override ConsoleColor BackgroundColor
	        {
	            get
	            {
	                return Console.BackgroundColor;
	            }
	            set
	            {
	                Console.BackgroundColor = value;
	            }
	        }

	        public override Size BufferSize
	        {
	            get
	            {
	                if (CONSOLE)
	                    return new Size(Console.BufferWidth, Console.BufferHeight);
	                else
	                    return new Size(0, 0);
	            }
	            set
	            {
	                Console.BufferWidth = value.Width;
	                Console.BufferHeight = value.Height;
	            }
	        }

	        public override Coordinates CursorPosition
	        {
	            get
	            {
	                return new Coordinates(Console.CursorLeft, Console.CursorTop);
	            }
	            set
	            {
	                Console.CursorTop = value.Y;
	                Console.CursorLeft = value.X;
	            }
	        }

	        public override int CursorSize
	        {
	            get
	            {
	                return Console.CursorSize;
	            }
	            set
	            {
	                Console.CursorSize = value;
	            }
	        }

	        public override void FlushInputBuffer()
	        {
	            throw new Exception("Not implemented: ik.PowerShell.PS2EXEHostRawUI.FlushInputBuffer");
	        }

	        public override ConsoleColor ForegroundColor
	        {
	            get
	            {
	                return Console.ForegroundColor;
	            }
	            set
	            {
	                Console.ForegroundColor = value;
	            }
	        }

	        public override BufferCell[,] GetBufferContents(Rectangle rectangle)
	        {
	            throw new Exception("Not implemented: ik.PowerShell.PS2EXEHostRawUI.GetBufferContents");
	        }

	        public override bool KeyAvailable
	        {
	            get
	            {
	                throw new Exception("Not implemented: ik.PowerShell.PS2EXEHostRawUI.KeyAvailable/Get");
	            }
	        }

	        public override Size MaxPhysicalWindowSize
	        {
	            get { return new Size(Console.LargestWindowWidth, Console.LargestWindowHeight); }
	        }

	        public override Size MaxWindowSize
	        {
	            get { return new Size(Console.BufferWidth, Console.BufferWidth); }
	        }

	        public override KeyInfo ReadKey(ReadKeyOptions options)
	        {
	            if( CONSOLE ) {
		            ConsoleKeyInfo cki = Console.ReadKey();

		            ControlKeyStates cks = 0;
		            if ((cki.Modifiers & ConsoleModifiers.Alt) != 0)
		                cks |= ControlKeyStates.LeftAltPressed | ControlKeyStates.RightAltPressed;
		            if ((cki.Modifiers & ConsoleModifiers.Control) != 0)
		                cks |= ControlKeyStates.LeftCtrlPressed | ControlKeyStates.RightCtrlPressed;
		            if ((cki.Modifiers & ConsoleModifiers.Shift) != 0)
		                cks |= ControlKeyStates.ShiftPressed;
		            if (Console.CapsLock)
		                cks |= ControlKeyStates.CapsLockOn;

		            return new KeyInfo((int)cki.Key, cki.KeyChar, cks, false);
				} else {
					ReadKeyForm f = new ReadKeyForm();
	                f.ShowDialog();
	                return f.key; 
				}
	        }

	        public override void ScrollBufferContents(Rectangle source, Coordinates destination, Rectangle clip, BufferCell fill)
	        {
	            throw new Exception("Not implemented: ik.PowerShell.PS2EXEHostRawUI.ScrollBufferContents");
	        }

	        public override void SetBufferContents(Rectangle rectangle, BufferCell fill)
	        {
	            throw new Exception("Not implemented: ik.PowerShell.PS2EXEHostRawUI.SetBufferContents(1)");
	        }

	        public override void SetBufferContents(Coordinates origin, BufferCell[,] contents)
	        {
	            throw new Exception("Not implemented: ik.PowerShell.PS2EXEHostRawUI.SetBufferContents(2)");
	        }

	        public override Coordinates WindowPosition
	        {
	            get
	            {
	                Coordinates s = new Coordinates();
	                s.X = Console.WindowLeft;
	                s.Y = Console.WindowTop;
	                return s;
	            }
	            set
	            {
	                Console.WindowLeft = value.X;
	                Console.WindowTop = value.Y;
	            }
	        }

	        public override Size WindowSize
	        {
	            get
	            {
	                Size s = new Size();
	                s.Height = Console.WindowHeight;
	                s.Width = Console.WindowWidth;
	                return s;
	            }
	            set
	            {
	                Console.WindowWidth = value.Width;
	                Console.WindowHeight = value.Height;
	            }
	        }

	        public override string WindowTitle
	        {
	            get
	            {
	                return Console.Title;
	            }
	            set
	            {
	                Console.Title = value;
	            }
	        }
	    }
	    internal class PS2EXEHostUI : PSHostUserInterface
	    {
			private const bool CONSOLE = $(if($noConsole){"false"}else{"true"});

			private PS2EXEHostRawUI rawUI = null;

	        public PS2EXEHostUI()
	            : base()
	        {
	            rawUI = new PS2EXEHostRawUI();
	        }

	        public override Dictionary<string, PSObject> Prompt(string caption, string message, System.Collections.ObjectModel.Collection<FieldDescription> descriptions)
	        {
				if( !CONSOLE )
					return new Dictionary<string, PSObject>();
					
	            if (!string.IsNullOrEmpty(caption))
	                WriteLine(caption);
	            if (!string.IsNullOrEmpty(message))
	                WriteLine(message);
	            Dictionary<string, PSObject> ret = new Dictionary<string, PSObject>();
	            foreach (FieldDescription cd in descriptions)
	            {
	                Type t = null;
	                if (string.IsNullOrEmpty(cd.ParameterAssemblyFullName))
	                    t = typeof(string);
	                else t = Type.GetType(cd.ParameterAssemblyFullName);


	                if (t.IsArray)
	                {
	                    Type elementType = t.GetElementType();
	                    Type genericListType = Type.GetType("System.Collections.Generic.List"+((char)0x60).ToString()+"1");
	                    genericListType = genericListType.MakeGenericType(new Type[] { elementType });
	                    ConstructorInfo constructor = genericListType.GetConstructor(BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
	                    object resultList = constructor.Invoke(null);

	                    int index = 0;
	                    string data = "";
	                    do
	                    {
	                        try
	                        {
	                            if (!string.IsNullOrEmpty(cd.Name))
	                                Write(string.Format("{0}[{1}]: ", cd.Name, index));
	                            data = ReadLine();

	                            if (string.IsNullOrEmpty(data))
	                                break;
	                            
	                            object o = System.Convert.ChangeType(data, elementType);

	                            genericListType.InvokeMember("Add", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, resultList, new object[] { o });
	                        }
	                        catch (Exception ex)
	                        {
	                            throw new Exception("Exception in ik.PowerShell.PS2EXEHostUI.Prompt*1");
	                        }
	                        index++;
	                    } while (true);

	                    System.Array retArray = (System.Array )genericListType.InvokeMember("ToArray", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, resultList, null);
	                    ret.Add(cd.Name, new PSObject(retArray));
	                }
	                else
	                {

	                    if (!string.IsNullOrEmpty(cd.Name))
	                        Write(string.Format("{0}: ", cd.Name));
	                    object o = null;

	                    string l = null;
	                    try
	                    {
	                        l = ReadLine();

	                        if (string.IsNullOrEmpty(l))
	                            o = cd.DefaultValue;
	                        if (o == null)
	                        {
	                            o = System.Convert.ChangeType(l, t);
	                        }

	                        ret.Add(cd.Name, new PSObject(o));
	                    }
	                    catch
	                    {
	                        throw new Exception("Exception in ik.PowerShell.PS2EXEHostUI.Prompt*2");
	                    }
	                }
	            }
	            return ret;
	        }

	        public override int PromptForChoice(string caption, string message, System.Collections.ObjectModel.Collection<ChoiceDescription> choices, int defaultChoice)
	        {
				if( !CONSOLE )
					return -1;
					
	            if (!string.IsNullOrEmpty(caption))
	                WriteLine(caption);
	            WriteLine(message);
	            int idx = 0;
	            SortedList<string, int> res = new SortedList<string, int>();
	            foreach (ChoiceDescription cd in choices)
	            {

	                string l = cd.Label;
	                int pos = cd.Label.IndexOf('&');
	                if (pos > -1)
	                {
	                    l = cd.Label.Substring(pos + 1, 1);
	                }
	                res.Add(l.ToLower(), idx);

	                if (idx == defaultChoice)
	                {
	                    Console.ForegroundColor = ConsoleColor.Yellow;
	                    Write(ConsoleColor.Yellow, Console.BackgroundColor, string.Format("[{0}]: ", l, cd.HelpMessage));
	                    WriteLine(ConsoleColor.Gray, Console.BackgroundColor, string.Format("{1}", l, cd.HelpMessage));
	                }
	                else
	                {
	                    Console.ForegroundColor = ConsoleColor.White;
	                    Write(ConsoleColor.White, Console.BackgroundColor, string.Format("[{0}]: ", l, cd.HelpMessage));
	                    WriteLine(ConsoleColor.Gray, Console.BackgroundColor, string.Format("{1}", l, cd.HelpMessage));
	                }
	                idx++;
	            }

	            try
	            {
	                string s = Console.ReadLine().ToLower();
	                if (res.ContainsKey(s))
	                {
	                    return res[s];
	                }
	            }
	            catch { }


	            return -1;
	        }

	        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName, PSCredentialTypes allowedCredentialTypes, PSCredentialUIOptions options)
	        {
	            if (!CONSOLE)
	            {
	                ik.PowerShell.CredentialForm.UserPwd cred = CredentialForm.PromptForPassword(caption, message, targetName, userName, allowedCredentialTypes, options);
	                if (cred != null )
	                {
	                    System.Security.SecureString x = new System.Security.SecureString();
	                    foreach (char c in cred.Password.ToCharArray())
	                        x.AppendChar(c);

	                    return new PSCredential(cred.User, x);
	                }
	                return null;
	            }
					
	            if (!string.IsNullOrEmpty(caption))
	                WriteLine(caption);
	            WriteLine(message);
	            Write("User name: ");
	            string un = ReadLine();
	            SecureString pwd = null;
	            if ((options & PSCredentialUIOptions.ReadOnlyUserName) == 0)
	            {
	                Write("Password: ");
	                pwd = ReadLineAsSecureString();
	            }
	            PSCredential c2 = new PSCredential(un, pwd);
	            return c2;
	        }

	        public override PSCredential PromptForCredential(string caption, string message, string userName, string targetName)
	        {
	            if (!CONSOLE)
	            {
                	ik.PowerShell.CredentialForm.UserPwd cred = CredentialForm.PromptForPassword(caption, message, targetName, userName, PSCredentialTypes.Default, PSCredentialUIOptions.Default);
	                if (cred != null )
	                {
	                    System.Security.SecureString x = new System.Security.SecureString();
	                    foreach (char c in cred.Password.ToCharArray())
	                        x.AppendChar(c);

	                    return new PSCredential(cred.User, x);
	                }
	                return null;
	            }

				if (!string.IsNullOrEmpty(caption))
	                WriteLine(caption);
	            WriteLine(message);
	            Write("User name: ");
	            string un = ReadLine();
	            Write("Password: ");
	            SecureString pwd = ReadLineAsSecureString();
	            PSCredential c2 = new PSCredential(un, pwd);
	            return c2;
	        }

	        public override PSHostRawUserInterface RawUI
	        {
	            get
	            {
	                return rawUI;
	            }
	        }

	        public override string ReadLine()
	        {
	            return Console.ReadLine();
	        }

	        public override System.Security.SecureString ReadLineAsSecureString()
	        {
	            System.Security.SecureString x = new System.Security.SecureString();
	            string l = Console.ReadLine();
	            foreach (char c in l.ToCharArray())
	                x.AppendChar(c);
	            return x;
	        }

	        public override void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
	        {
	            Console.ForegroundColor = foregroundColor;
	            Console.BackgroundColor = backgroundColor;
	            Console.Write(value);
	        }

	        public override void Write(string value)
	        {
	            Console.ForegroundColor = ConsoleColor.White;
	            Console.BackgroundColor = ConsoleColor.Black;
	            Console.Write(value);
	        }

	        public override void WriteDebugLine(string message)
	        {
	            Console.ForegroundColor = ConsoleColor.DarkMagenta;
	            Console.BackgroundColor = ConsoleColor.Black;
	            Console.WriteLine(message);
	        }

	        public override void WriteErrorLine(string value)
	        {
	            Console.ForegroundColor = ConsoleColor.Red;
	            Console.BackgroundColor = ConsoleColor.Black;
	            Console.WriteLine(value);
	        }

	        public override void WriteLine(string value)
	        {
	            Console.ForegroundColor = ConsoleColor.White;
	            Console.BackgroundColor = ConsoleColor.Black;
	            Console.WriteLine(value);
	        }

	        public override void WriteProgress(long sourceId, ProgressRecord record)
	        {

	        }

	        public override void WriteVerboseLine(string message)
	        {
	            Console.ForegroundColor = ConsoleColor.DarkCyan;
	            Console.BackgroundColor = ConsoleColor.Black;
	            Console.WriteLine(message);
	        }

	        public override void WriteWarningLine(string message)
	        {
	            Console.ForegroundColor = ConsoleColor.Yellow;
	            Console.BackgroundColor = ConsoleColor.Black;
	            Console.WriteLine(message);
	        }
	    }



	    internal class PS2EXEHost : PSHost
	    {
			private const bool CONSOLE = $(if($noConsole){"false"}else{"true"});

			private PS2EXEApp parent;
	        private PS2EXEHostUI ui = null;

	        private CultureInfo originalCultureInfo =
	            System.Threading.Thread.CurrentThread.CurrentCulture;

	        private CultureInfo originalUICultureInfo =
	            System.Threading.Thread.CurrentThread.CurrentUICulture;

	        private Guid myId = Guid.NewGuid();

	        public PS2EXEHost(PS2EXEApp app, PS2EXEHostUI ui)
	        {
	            this.parent = app;
	            this.ui = ui;
	        }

	        public override System.Globalization.CultureInfo CurrentCulture
	        {
	            get
	            {
	                return this.originalCultureInfo;
	            }
	        }

	        public override System.Globalization.CultureInfo CurrentUICulture
	        {
	            get
	            {
	                return this.originalUICultureInfo;
	            }
	        }

	        public override Guid InstanceId
	        {
	            get
	            {
	                return this.myId;
	            }
	        }

	        public override string Name
	        {
	            get
	            {
	                return "PS2EXE_Host";
	            }
	        }

	        public override PSHostUserInterface UI
	        {
	            get
	            {
	                return ui;
	            }
	        }

	        public override Version Version
	        {
	            get
	            {
	                return new Version(0, 2, 0, 0);
	            }
	        }

	        public override void EnterNestedPrompt()
	        {
	        }

	        public override void ExitNestedPrompt()
	        {
	        }

	        public override void NotifyBeginApplication()
	        {
	            return;
	        }

	        public override void NotifyEndApplication()
	        {
	            return;
	        }

	        public override void SetShouldExit(int exitCode)
	        {
	            this.parent.ShouldExit = true;
	            this.parent.ExitCode = exitCode;
	        }
	    }



	    internal interface PS2EXEApp
	    {
	        bool ShouldExit { get; set; }
	        int ExitCode { get; set; }
	    }


	    internal class PS2EXE : PS2EXEApp
	    {
			private const bool CONSOLE = $(if($noConsole){"false"}else{"true"});
			
	        private bool shouldExit;

	        private int exitCode;

	        public bool ShouldExit
	        {
	            get { return this.shouldExit; }
	            set { this.shouldExit = value; }
	        }

	        public int ExitCode
	        {
	            get { return this.exitCode; }
	            set { this.exitCode = value; }
	        }

	        $(if($sta){"[STAThread]"})$(if($mta){"[MTAThread]"})
	        private static int Main(string[] args)
	        {
                $culture

	            PS2EXE me = new PS2EXE();

	            bool paramWait = false;
	            string extractFN = string.Empty;

	            PS2EXEHostUI ui = new PS2EXEHostUI();
	            PS2EXEHost host = new PS2EXEHost(me, ui);
	            System.Threading.ManualResetEvent mre = new System.Threading.ManualResetEvent(false);

	            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

	            try
	            {
	                using (Runspace myRunSpace = RunspaceFactory.CreateRunspace(host))
	                {
	                    $(if($sta -or $mta) {"myRunSpace.ApartmentState = System.Threading.ApartmentState."})$(if($sta){"STA"})$(if($mta){"MTA"});
	                    myRunSpace.Open();

	                    using (System.Management.Automation.PowerShell powershell = System.Management.Automation.PowerShell.Create())
	                    {
	                        Console.CancelKeyPress += new ConsoleCancelEventHandler(delegate(object sender, ConsoleCancelEventArgs e)
	                        {
	                            try
	                            {
	                                powershell.BeginStop(new AsyncCallback(delegate(IAsyncResult r)
	                                {
	                                    mre.Set();
	                                    e.Cancel = true;
	                                }), null);
	                            }
	                            catch
	                            {
	                            };
	                        });

	                        powershell.Runspace = myRunSpace;
	                        powershell.Streams.Progress.DataAdded += new EventHandler<DataAddedEventArgs>(delegate(object sender, DataAddedEventArgs e)
	                            {
	                                ui.WriteLine(((PSDataCollection<ProgressRecord>)sender)[e.Index].ToString());
	                            });
	                        powershell.Streams.Verbose.DataAdded += new EventHandler<DataAddedEventArgs>(delegate(object sender, DataAddedEventArgs e)
	                            {
	                                ui.WriteVerboseLine(((PSDataCollection<VerboseRecord>)sender)[e.Index].ToString());
	                            });
	                        powershell.Streams.Warning.DataAdded += new EventHandler<DataAddedEventArgs>(delegate(object sender, DataAddedEventArgs e)
	                            {
	                                ui.WriteWarningLine(((PSDataCollection<WarningRecord>)sender)[e.Index].ToString());
	                            });
	                        powershell.Streams.Error.DataAdded += new EventHandler<DataAddedEventArgs>(delegate(object sender, DataAddedEventArgs e)
	                            {
	                                ui.WriteErrorLine(((PSDataCollection<ErrorRecord>)sender)[e.Index].ToString());
	                            });

	                        PSDataCollection<PSObject> inp = new PSDataCollection<PSObject>();
	                        inp.DataAdded += new EventHandler<DataAddedEventArgs>(delegate(object sender, DataAddedEventArgs e)
	                        {
	                            ui.WriteLine(inp[e.Index].ToString());
	                        });

	                        PSDataCollection<PSObject> outp = new PSDataCollection<PSObject>();
	                        outp.DataAdded += new EventHandler<DataAddedEventArgs>(delegate(object sender, DataAddedEventArgs e)
	                        {
	                            ui.WriteLine(outp[e.Index].ToString());
	                        });

	                        int separator = 0;
	                        int idx = 0;
	                        foreach (string s in args)
	                        {
	                            if (string.Compare(s, "-wait", true) == 0)
	                                paramWait = true;
	                            else if (s.StartsWith("-extract", StringComparison.InvariantCultureIgnoreCase))
	                            {
	                                string[] s1 = s.Split(new string[] { ":" }, 2, StringSplitOptions.RemoveEmptyEntries);
	                                if (s1.Length != 2)
	                                {
	                                    Console.WriteLine("If you specify the -extract option you need to add a file for extraction in this way\r\n   -extract:\"<filename>\"");
	                                    return 1;
	                                }
	                                extractFN = s1[1].Trim(new char[] { '\"' });
	                            }
	                            else if (string.Compare(s, "-end", true) == 0)
	                            {
	                                separator = idx + 1;
	                                break;
	                            }
	                            else if (string.Compare(s, "-debug", true) == 0)
	                            {
	                                System.Diagnostics.Debugger.Launch();
	                                break;
	                            }
	                            idx++;
	                        }

	                        string script = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(@"$($script)"));

	                        if (!string.IsNullOrEmpty(extractFN))
	                        {
	                            System.IO.File.WriteAllText(extractFN, script);
	                            return 0;
	                        }

							List<string> paramList = new List<string>(args);

	                        powershell.AddScript(script);
                        	powershell.AddParameters(paramList.GetRange(separator, paramList.Count - separator));
                        	powershell.AddCommand("out-string");
                        	powershell.AddParameter("-stream");


	                        powershell.BeginInvoke<PSObject, PSObject>(inp, outp, null, new AsyncCallback(delegate(IAsyncResult ar)
	                        {
	                            if (ar.IsCompleted)
	                                mre.Set();
	                        }), null);

	                        while (!me.ShouldExit && !mre.WaitOne(100))
	                        {
	                        };

	                        powershell.Stop();
	                    }

	                    myRunSpace.Close();
	                }
	            }
	            catch (Exception ex)
	            {
	                Console.Write("An exception occured: ");
	                Console.WriteLine(ex.Message);
	            }

	            if (paramWait)
	            {
	                Console.WriteLine("Hit any key to exit...");
	                Console.ReadKey();
	            }
	            return me.ExitCode;
	        }


	        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	        {
	            throw new Exception("Unhandeled exception in PS2EXE");
	        }
	    }
	}
"@
#endregion

#region EXE Config file
  $configFileForEXE2 = "<?xml version=""1.0"" encoding=""utf-8"" ?>`r`n<configuration><startup><supportedRuntime version=""v2.0.50727""/></startup></configuration>"
  $configFileForEXE3 = "<?xml version=""1.0"" encoding=""utf-8"" ?>`r`n<configuration><startup><supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.0"" /></startup></configuration>"
#endregion

Write-Host "Compiling file... " -NoNewline
$cr = $cop.CompileAssemblyFromSource($cp, $programFrame)
if( $cr.Errors.Count -gt 0 ) {
	Write-Host ""
	Write-Host ""
	if( Test-Path $outputFile ) {
		Remove-Item $outputFile -Verbose:$false
	}
	Write-Host -ForegroundColor red "Could not create the PowerShell .exe file because of compilation errors. Use -verbose parameter to see details."
	$cr.Errors | % { Write-Verbose $_ -Verbose:$verbose}
} else {
	Write-Host ""
	Write-Host ""
	if( Test-Path $outputFile ) {
		Write-Host "Output file " -NoNewline 
		Write-Host $outputFile  -NoNewline
		Write-Host " written" 
		
		if( $debug) {
			$cr.TempFiles | ? { $_ -ilike "*.cs" } | select -first 1 | % {
				$dstSrc =  ([System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($outputFile), [System.IO.Path]::GetFileNameWithoutExtension($outputFile)+".cs"))
				Write-Host "Source file name for debug copied: $($dstSrc)"
				Copy-Item -Path $_ -Destination $dstSrc -Force
			}
			$cr.TempFiles | Remove-Item -Verbose:$false -Force -ErrorAction SilentlyContinue
		}
		if( $runtime20 ) {
			$configFileForEXE2 | Set-Content ($outputFile+".config")
			Write-Host "Config file for EXE created."
		}
		if( $runtime30 ) {
			$configFileForEXE3 | Set-Content ($outputFile+".config")
			Write-Host "Config file for EXE created."
		}
	} else {
		Write-Host "Output file " -NoNewline -ForegroundColor Red
		Write-Host $outputFile -ForegroundColor Red -NoNewline
		Write-Host " not written" -ForegroundColor Red
	}
}