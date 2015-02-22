using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using software.elendil.AstrometryNet.Enum;
using software.elendil.AstrometryNet.Exceptions;
using software.elendil.AstrometryNet.Json;

namespace software.elendil.AstrometryNet
{
	public class Client
	{
		private readonly RequestSender requestSender;

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class.
		/// </summary>
		/// <param name="apiKey">The Astrometry.net API key.</param>
		public Client(string apiKey)
		{
			if (apiKey == null) throw new ArgumentNullException("apiKey");

			requestSender = new RequestSender(apiKey);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Client"/> class.
		/// </summary>
		/// <param name="apiKey">The Astrometry.net API key.</param>
		/// <param name="url">The URL of the Astrometry.net web service</param>
		public Client(string apiKey, string url)
		{
			if (apiKey == null) throw new ArgumentNullException("apiKey");
			if (url == null) throw new ArgumentNullException("url");

			requestSender = new RequestSender(apiKey, url);
		}

		#endregion

		/// <summary>
		/// Log in and set the session key.
		/// 
		/// It returns a <code>LoginResponse</code> that must be checked to know if the connection succeeded. In case of unsuccessful connection an error message is set.
		/// </summary>
		/// <returns>Login status</returns>
		/// <exception cref="AstrometryException">Exception raised if the connection request failed</exception>
		/// <seealso cref="LoginResponse"/>
		public LoginResponse Login()
		{
			return requestSender.Login();
		}

		/// <summary>
		/// Upload an image and start the analysis.
		/// </summary>
		/// <param name="file">The filename (including path) to upload</param>
		/// <param name="uploadArgs">Arguments for the solver. If <c>null</c> the default parameters will be used</param>
		/// <returns>Upload result</returns>
		/// <exception cref="AstrometryException">Exception raised in the following cases : <c>file</c> does not exist, the session is not initialized, something breaks during upload</exception>
		/// <seealso cref="UploadArgs"/>
		public UploadResponse Upload(string file, UploadArgs uploadArgs = null)
		{
			if (file == null) throw new ArgumentNullException("file");

			if (!requestSender.Connected)
			{
				throw new AstrometryNotConnectedException("Service is not connected, call Login first");
			}

			return requestSender.Upload(file, uploadArgs);
		}

		/// <summary>
		/// Get the images id for the given submission ID.
		/// </summary>
		/// <param name="submissionId"></param>
		/// <param name="cancellationToken"></param>
		/// <returns>Submission images response</returns>
		/// <exception cref="AstrometryException">Exception raised if the session is not initialized or something breaks during the request</exception>
		public async Task<SubmissionImagesResponse> GetSubmissionImages(string submissionId,
			CancellationToken cancellationToken)
		{
			if (submissionId == null) throw new ArgumentNullException("submissionId");

			if (!requestSender.Connected)
			{
				throw new AstrometryNotConnectedException("Service is not connected, call Login first");
			}


			return await Task.Factory.StartNew(() =>
			{
				SubmissionImagesResponse submissionImagesResponse;
				string[] images;

				do
				{
#if DEBUG
				Debug.WriteLine("\nWaiting for images submissions...");
#endif

					submissionImagesResponse = requestSender.GetSubmissionImages(submissionId);
					images = submissionImagesResponse.image_ids.Where(x => !string.IsNullOrEmpty(x)).ToArray();
					Thread.Sleep(1000);
				} while (images.Length == 0 && !cancellationToken.IsCancellationRequested);

				return submissionImagesResponse;
			}, cancellationToken);
		}

		/// <summary>
		/// Get the status for the given submission ID.
		/// </summary>
		/// <param name="submissionId">the submission id for which get the status</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Submission status</returns>
		/// <exception cref="AstrometryException">Exception raised if the submission id does not exist or something breaks during the request</exception>
		public async Task<SubmissionStatusResponse> GetSubmissionStatus(string submissionId,
			CancellationToken cancellationToken)
		{
			if (submissionId == null) throw new ArgumentNullException("submissionId");

			if (!requestSender.Connected)
			{
				throw new AstrometryNotConnectedException("Service is not connected, call Login first");
			}

			return await Task.Factory.StartNew(() =>
			{
				SubmissionStatusResponse submissionStatusResponse;
				string[] jobs;

				do
				{
#if DEBUG
				Debug.WriteLine("\nWaiting for jobs...");
#endif
					submissionStatusResponse = requestSender.GetSubmissionStatus(submissionId);
					Thread.Sleep(1000);
					jobs = submissionStatusResponse.jobs.Where(x => !string.IsNullOrEmpty(x)).ToArray();
				} while (jobs.Length == 0);

				return submissionStatusResponse;
			}, cancellationToken);
		}

		/// <summary>
		/// Get the status for the given submission ID.
		/// </summary>
		/// <param name="jobId">the job id for which get the status</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Job status</returns>
		/// <exception cref="AstrometryException">Exception raised if the job id does not exist or something breaks during the request</exception>
		public async Task<JobStatusResponse> GetJobStatus(string jobId, CancellationToken cancellationToken)
		{
			if (jobId == null) throw new ArgumentNullException("jobId");

			if (!requestSender.Connected)
			{
				throw new AstrometryNotConnectedException("Service is not connected, call Login first");
			}

			JobStatusResponse jobStatusResponse;
			return await Task.Factory.StartNew(() =>
			{
				do
				{
#if DEBUG
				Debug.WriteLine("\nSolving...");
#endif
					jobStatusResponse = requestSender.GetJobStatus(jobId);
					Thread.Sleep(1000);
				} while (jobStatusResponse.status.Equals(ResponseJobStatus.solving) ||
				         jobStatusResponse.status.Equals(ResponseJobStatus.processing));

				return jobStatusResponse;
			}, cancellationToken);
		}

		/// <summary>
		/// Get the calibration result for a job id
		/// </summary>
		/// <param name="jobId">the job id for which get the result</param>
		/// <returns>Calibration result</returns>
		/// <exception cref="AstrometryException">Exception raised if the job id does not exist or something breaks during the request</exception>
		public CalibrationResponse GetCalibration(string jobId)
		{
			if (jobId == null) throw new ArgumentNullException("jobId");

			if (!requestSender.Connected)
			{
				throw new AstrometryNotConnectedException("Service is not connected, call Login first");
			}

			var calibrationResponse = requestSender.GetCalibration(jobId);
			return calibrationResponse;
		}

		/// <summary>
		/// Get the images in the field of the image corresponding to the given job id
		/// </summary>
		/// <param name="jobId">the job id for which get the objects</param>
		/// <returns>Objects in field</returns>
		/// <exception cref="AstrometryException">Exception raised if the job id does not exist or something breaks during the request</exception>
		public ObjectsInFieldResponse GetObjectsInField(string jobId)
		{
			if (jobId == null) throw new ArgumentNullException("jobId");

			if (!requestSender.Connected)
			{
				throw new AstrometryNotConnectedException("Service is not connected, call Login first");
			}

			var objectsInFieldResponse = requestSender.GetObjectsInField(jobId);
			return objectsInFieldResponse;
		}
	}
}