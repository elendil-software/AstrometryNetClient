using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using software.elendil.AstrometryNet.Enum;
using software.elendil.AstrometryNet.Exceptions;
using software.elendil.AstrometryNet.Json;

namespace software.elendil.AstrometryNet
{
	/// <summary>
	/// RequestSender for the Astrometry.net service.
	/// 
	/// It provides all necessary method to use the Astrometry.net API. See http://nova.astrometry.net/api_help for more details
	/// </summary>
	public class RequestSender
	{
		#region Properties

		/// <summary>
		/// URL to use to contact the service (by default : http://nova.astrometry.net/api/
		/// </summary>
		private readonly string url;

		/// <summary>
		/// The API key of your account
		/// </summary>
		private readonly string apiKey;

		/// <summary>
		/// Used to store the session key when logged in
		/// </summary>
		private string session;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="apiKey">The API key of your account</param>
		/// <param name="url">URL to use to contact the service (http://nova.astrometry.net/api/ if not given)</param>
		public RequestSender(string apiKey, string url = "http://nova.astrometry.net/api/")
		{
			this.apiKey = apiKey;
			this.url = url;
		}

		#endregion

		#region Public/internal

		internal bool Connected
		{
			get { return !string.IsNullOrEmpty(session); }
		}

		/// <summary>
		/// Log in and set the session key.
		/// 
		/// It returns a <code>LoginResponse</code> that must be checked to know if the connection succeeded. In case of unsuccessful connection an error message is set.
		/// </summary>
		/// <returns>Login status</returns>
		/// <exception cref="AstrometryException">Exception raised if the connection request failed</exception>
		/// <seealso cref="LoginResponse"/>
		internal LoginResponse Login()
		{
			try
			{
				var json = SendRequest("login", JsonConvert.SerializeObject(new Login {apikey = apiKey}));
				var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(json);

				if (loginResponse.status.Equals(ResponseStatus.success))
				{
					session = loginResponse.session;
				}

				return loginResponse;
			}
			catch (Exception e)
			{
				throw new AstrometryException(e.Message, e);
			}
		}

		/// <summary>
		/// Upload an image and start the analysis.
		/// </summary>
		/// <param name="file">The filename (including path) to upload</param>
		/// <param name="args">Arguments for the solver. Must at least have the session defined</param>
		/// <returns>Upload result</returns>
		/// <exception cref="AstrometryException">Exception raised in the following cases : <c>file</c> does not exist, the session is not initialized, something breaks during upload</exception>
		/// <seealso cref="UploadArgs"/>
		internal UploadResponse Upload(string file, UploadArgs args)
		{
			if (!File.Exists(file))
			{
				throw new AstrometryException("File " + file + " does not exist");
			}

			if (String.IsNullOrEmpty(session))
			{
				throw new AstrometryException("session is not initialized");
			}

			if (args == null)
			{
				args = new UploadArgs {session = session};
			}
			else
			{
				args.session = session;
			}


			string boundary = "--------------------------" + DateTime.Now.Ticks.ToString("x");
			var webRequest = (HttpWebRequest) WebRequest.Create(url + "upload");
			webRequest.Method = "POST";
			webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
			boundary = "--" + boundary;

			using (var requestStream = webRequest.GetRequestStream())
			{
				try
				{
					//Write JSON
					var buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
					requestStream.Write(buffer, 0, buffer.Length);

					buffer =
						Encoding.ASCII.GetBytes("Content-Disposition: form-data; name=\"request-json\"" + Environment.NewLine +
						                        Environment.NewLine);
					requestStream.Write(buffer, 0, buffer.Length);
					string serializedArgs = JsonConvert.SerializeObject(args);
					buffer = Encoding.UTF8.GetBytes(serializedArgs + Environment.NewLine);
					requestStream.Write(buffer, 0, buffer.Length);

					buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
					requestStream.Write(buffer, 0, buffer.Length);

					//write file
					buffer =
						Encoding.UTF8.GetBytes("Content-Disposition: form-data; name=\"file\"; filename=\"" + Path.GetFileName(file) +
						                       "\"" + Environment.NewLine);
					requestStream.Write(buffer, 0, buffer.Length);

					buffer =
						Encoding.ASCII.GetBytes("Content-Type: application/octet-stream" + Environment.NewLine + Environment.NewLine);
					requestStream.Write(buffer, 0, buffer.Length);

					var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
					fileStream.CopyTo(requestStream);

					buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
					requestStream.Write(buffer, 0, buffer.Length);

					buffer = Encoding.ASCII.GetBytes(boundary + "--");
					requestStream.Write(buffer, 0, buffer.Length);
				}
				catch (Exception e)
				{
					throw new AstrometryException(e.Message, e);
				}
			}


			string json = "";
			try
			{
				WebResponse response = webRequest.GetResponse();
				Stream stream = response.GetResponseStream();

				if (stream == null)
				{
					return new UploadResponse
					{
						status = ResponseStatus.error,
						errormessage = "response stream is null"
					};
				}

				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						json += reader.ReadLine();
					}
				}

				return JsonConvert.DeserializeObject<UploadResponse>(json);
			}
			catch (Exception e)
			{
				throw new AstrometryException(e.Message, e);
			}
		}

		/// <summary>
		/// Get the images id for the given submission ID.
		/// </summary>
		/// <param name="submissionId"></param>
		/// <returns>Submission images response</returns>
		/// <exception cref="AstrometryException">Exception raised if the session is not initialized or something breaks during the request</exception>
		internal SubmissionImagesResponse GetSubmissionImages(string submissionId)
		{
			if (String.IsNullOrEmpty(session))
			{
				throw new AstrometryException("session is not initialized");
			}

			try
			{
				var json = SendRequest("submission_images",
					JsonConvert.SerializeObject(new SubmissionImages {subid = submissionId, session = session}));
				return JsonConvert.DeserializeObject<SubmissionImagesResponse>(json);
			}
			catch (Exception e)
			{
				throw new AstrometryException(e.Message, e);
			}
		}

		/// <summary>
		/// Get the status for the given submission ID.
		/// </summary>
		/// <param name="submissionId">the submission id for which get the status</param>
		/// <returns>Submission status</returns>
		/// <exception cref="AstrometryException">Exception raised if the submission id does not exist or something breaks during the request</exception>
		internal SubmissionStatusResponse GetSubmissionStatus(string submissionId)
		{
			try
			{
				var json = SendRequest("submissions/" + submissionId, "{}");
				return JsonConvert.DeserializeObject<SubmissionStatusResponse>(json);
			}
			catch (WebException we)
			{
				if (HttpStatusCode.NotFound.Equals(((HttpWebResponse) we.Response).StatusCode))
				{
					throw new AstrometryException("This submission ID does not exist", we);
				}

				throw new AstrometryException(we.Message, we);
			}
			catch (Exception e)
			{
				throw new AstrometryException(e.Message, e);
			}
		}

		/// <summary>
		/// Get the status for the given submission ID.
		/// </summary>
		/// <param name="jobId">the job id for which get the status</param>
		/// <returns>Job status</returns>
		/// <exception cref="AstrometryException">Exception raised if the job id does not exist or something breaks during the request</exception>
		internal JobStatusResponse GetJobStatus(string jobId)
		{
			try
			{
				var json = SendRequest("jobs/" + jobId, "{}");
				return JsonConvert.DeserializeObject<JobStatusResponse>(json);
			}
			catch (WebException we)
			{
				if (HttpStatusCode.NotFound.Equals(((HttpWebResponse) we.Response).StatusCode))
				{
					throw new AstrometryException("This Job ID does not exist", we);
				}

				throw new AstrometryException(we.Message, we);
			}
			catch (Exception e)
			{
				throw new AstrometryException(e.Message, e);
			}
		}


		/// <summary>
		/// Get the calibration result for a job id
		/// </summary>
		/// <param name="jobId">the job id for which get the result</param>
		/// <returns>Calibration result</returns>
		/// <exception cref="AstrometryException">Exception raised if the job id does not exist or something breaks during the request</exception>
		internal CalibrationResponse GetCalibration(string jobId)
		{
			try
			{
				var json = SendRequest("jobs/" + jobId + "/calibration", "{}");
				return JsonConvert.DeserializeObject<CalibrationResponse>(json);
			}
			catch (WebException we)
			{
				if (HttpStatusCode.NotFound.Equals(((HttpWebResponse) we.Response).StatusCode))
				{
					throw new AstrometryException("This Job ID does not exist", we);
				}

				throw new AstrometryException(we.Message, we);
			}
			catch (Exception e)
			{
				throw new AstrometryException(e.Message, e);
			}
		}

		/// <summary>
		/// Get the images in the field of the image corresponding to the given job id
		/// </summary>
		/// <param name="jobId">the job id for which get the objects</param>
		/// <returns>Objects in field</returns>
		/// <exception cref="AstrometryException">Exception raised if the job id does not exist or something breaks during the request</exception>
		internal ObjectsInFieldResponse GetObjectsInField(string jobId)
		{
			try
			{
				var result = SendRequest("jobs/" + jobId + "/objects_in_field", "{}");
				return JsonConvert.DeserializeObject<ObjectsInFieldResponse>(result);
			}
			catch (WebException we)
			{
				if (HttpStatusCode.NotFound.Equals(((HttpWebResponse) we.Response).StatusCode))
				{
					throw new AstrometryException("This Job ID does not exist", we);
				}

				throw new AstrometryException(we.Message, we);
			}
			catch (Exception e)
			{
				throw new AstrometryException(e.Message, e);
			}
		}

		#endregion

		#region Private

		private string SendRequest(string service, string json)
		{
			var webRequest = (HttpWebRequest) WebRequest.Create(url + service);

#if DEBUG
			Debug.WriteLine("SendRequest : " + webRequest.Address);
			Debug.WriteLine("json : " + json);
#endif

			webRequest.ContentType = "application/x-www-form-urlencoded;";
			webRequest.Accept = "application/json, text/javascript, */*";
			webRequest.Method = "POST";

			using (var writer = new StreamWriter(webRequest.GetRequestStream()))
			{
				writer.Write("request-json=" + HttpUtility.UrlEncode(json));
				writer.Close();
			}

			json = "";

			WebResponse response = webRequest.GetResponse();
			Stream stream = response.GetResponseStream();

			if (stream != null)
			{
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						json += reader.ReadLine();
					}
				}
			}

#if DEBUG
			Debug.WriteLine("json response : " + json);
#endif

			return json;
		}

		#endregion
	}
}