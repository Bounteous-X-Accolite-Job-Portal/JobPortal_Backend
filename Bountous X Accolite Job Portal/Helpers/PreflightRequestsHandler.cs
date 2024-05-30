using System.Net;

namespace Bountous_X_Accolite_Job_Portal.Helpers
{
    public class PreflightRequestsHandler : DelegatingHandler
    {
        string origins = "http://localhost:4200, https://kind-dune-058eee70f.5.azurestaticapps.net";
        string headers = "*"; // Allow any headers
        string methods = "*"; // Allow any methods
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Origin") && request.Method.Method.Equals("OPTIONS"))
            {
                var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
                // Define and add values to variables: origins, headers, methods (can be global)               
                response.Headers.Add("Access-Control-Allow-Origin", origins);
                response.Headers.Add("Access-Control-Allow-Headers", headers);
                response.Headers.Add("Access-Control-Allow-Methods", methods);
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return tsc.Task;
            }
            return base.SendAsync(request, cancellationToken);
        }

    }
}
