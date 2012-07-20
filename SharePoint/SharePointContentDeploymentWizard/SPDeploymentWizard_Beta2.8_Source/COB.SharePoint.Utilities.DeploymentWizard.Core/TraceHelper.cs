#define TRACE
using System;
using System.Text;
using System.Diagnostics;

namespace COB.SharePoint.Utilities.DeploymentWizard.Core
{
	public class TraceHelper 
	{
		#region -- Private Fields --

		/// <summary>
		/// The name of the owner class is used in all traces so we know where they came from
		/// </summary>
		private string f_OwnerClassName = "";

		#endregion

		#region -- Constructor / Destructor --

		/// <summary>
		/// Creates an instance of the TraceHelper class and readies it for use.
		/// </summary>
		/// <param name="OwnerClass">
		/// A reference to the class which is creating the TraceHelper. Used to fetch the
		/// fully qualified name of the class for the trace output.
		/// </param>
		public TraceHelper(object OwnerClass) {
			f_OwnerClassName = OwnerClass.GetType().ToString();
		}

		/// <summary>
		/// Creates an instance of the TraceHelper class and readies it for use.
		/// </summary>
		/// <param name="OwnerClassName">
		/// The name of the class that is using the helper. For use with static classes
		/// where the "this" keyword is not valid.
		/// </param>
		public TraceHelper(string OwnerClassName) {
			f_OwnerClassName = OwnerClassName;
		}

		#endregion

		#region -- Private methods --

		/// <summary>
		/// Performs the actual trace operation, and writes the correctly formatted data to the
		/// trace stream.
		/// </summary>
		/// <param name="Level">The identifier for the severity level of this trace message</param>
		/// <param name="Message">The message itself</param>
		private void MyTrace(string Level, string Message) {
			StringBuilder sb = new StringBuilder();
			DateTime now = DateTime.Now;
			sb.Append( now.ToString("yyyy-MM-dd, hh:mm:ss.ff") );
			sb.Append(" , ");
			sb.Append( Level );
			sb.Append(" , ");
			sb.Append( f_OwnerClassName );
			sb.Append(" , ");
			sb.Append( Message );

			System.Diagnostics.Trace.WriteLine(sb.ToString());
			System.Diagnostics.Trace.Flush();
		}

		private void MyTrace(string Level, string Message, params object[] args) {
			string strMsg = string.Format(Message, args);
			MyTrace(Level, strMsg);
		}

		#endregion

		#region -- Public Methods --

		/// <summary>
		/// Traces a "verbose" level message. These are usually entry and exit points in
		/// methods
		/// </summary>
		/// <param name="Message">The information write to the trace stream</param>
		public void TraceVerbose(string Message) {
			MyTrace("[VERBOSE]", Message);
		}

		/// <summary>
		/// Traces a "verbose" level message. These are usually entry and exit points in
		/// methods
		/// </summary>
		/// <param name="Message"></param>
		/// <param name="args"></param>
		public void TraceVerbose(string Message, params object[] args) {
			MyTrace("[VERBOSE]", Message, args);
		}

		/// <summary>
		/// Traces an "info" level message. These are usually decision points in the code
		/// or the results of operations that have occured.
		/// </summary>
		/// <param name="Message">The information write to the trace stream</param>
		public void TraceInfo(string Message) {
			MyTrace("[INFO]", Message);
		}

		/// <summary>
		/// Traces an "info" level message. These are usually decision points in the code
		/// or the results of operations that have occured.
		/// </summary>
		/// <param name="Message">The information write to the trace stream</param>
		/// <param name="args">variable argument list</param>
		public void TraceInfo(string Message, params object[] args) {
			MyTrace("[INFO]", Message, args);
		}

		/// <summary>
		/// Traces a "warning" level message. These are usually error conditions which
		/// the code can recover from or handle internally.
		/// </summary>
		/// <param name="Message">The information write to the trace stream</param>
		public void TraceWarning(string Message) {
			MyTrace("[WARNING]", Message);
		}

		/// <summary>
		/// Traces a "warning" level message. These are usually error conditions which
		/// the code can recover from or handle internally.
		/// </summary>
		/// <param name="Message">The information write to the trace stream</param>
		/// <param name="args">variable argument list</param>
		public void TraceWarning(string Message, params object[] args) {
			MyTrace("[WARNING]", Message, args);
		}

		/// <summary>
		/// Traces an "error" level message. These are usually critical errors or failures
		/// which cause a section of code to fail.
		/// </summary>
		/// <param name="Message">The information write to the trace stream</param>
		public void TraceError(string Message) {
			MyTrace("[ERROR]", Message);
		}

		/// <summary>
		/// Traces an "error" level message. These are usually critical errors or failures
		/// which cause a section of code to fail.
		/// </summary>
		/// <param name="Message">The information write to the trace stream</param>
		/// <param name="args">variable argument list</param>
		public void TraceError(string Message, params object[] args) {
			MyTrace("[ERROR]", Message, args);
		}

		#endregion
	}
}
