﻿using Newtonsoft.Json;

using System;

using System.Collections.Generic;

using System.IO;

using System.Net;

using System.Net.Sockets;

using System.Text;

using System.Windows;

using System.Security.Cryptography;

using System.Threading;

using System.Threading.Tasks;
using BusinessLayer;

namespace Inventory_Management.API
{
    public class GoogleLogin
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // client configuration

        const string clientID = "581786658708-elflankerquo1a6vsckabbhn25hclla0.apps.googleusercontent.com";

        const string clientSecret = "3f6NggMbPtrmIBpgx-MK2xXK";

        const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";

        const string tokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";

        const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";

        public async void login(object sender)

        {

            // Generates state and PKCE values.

            string state = Utils.randomDataBase64url(32);

            string code_verifier = Utils.randomDataBase64url(32);

            string code_challenge = Utils.base64urlencodeNoPadding(Utils.sha256(code_verifier));

            const string code_challenge_method = "S256";



            // Creates a redirect URI using an available port on the loopback address.

            string redirectURI = string.Format("http://{0}:{1}/", IPAddress.Loopback, Utils.GetRandomUnusedPort());

            output("redirect URI: " + redirectURI);



            // Creates an HttpListener to listen for requests on that redirect URI.

            var http = new HttpListener();

            http.Prefixes.Add(redirectURI);

            output("Listening..");

            http.Start();



            // Creates the OAuth 2.0 authorization request.

            string authorizationRequest = string.Format("{0}?response_type=code&scope=openid%20profile&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",

                authorizationEndpoint,

                System.Uri.EscapeDataString(redirectURI),

                clientID,

                state,

                code_challenge,

                code_challenge_method);



            // Opens request in the browser.

            System.Diagnostics.Process.Start(authorizationRequest);



            // Waits for the OAuth authorization response.

            var context = await http.GetContextAsync();

            // Sends an HTTP response to the browser.

            var response = context.Response;

            string responseString = string.Format("<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>");

            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;

            var responseOutput = response.OutputStream;

            Task responseTask = responseOutput.WriteAsync(buffer, 0, buffer.Length).ContinueWith((task) =>

            {

                responseOutput.Close();

                http.Stop();

                Console.WriteLine("HTTP server stopped.");

            });



            // Checks for errors.

            if (context.Request.QueryString.Get("error") != null)

            {

                output(String.Format("OAuth authorization error: {0}.", context.Request.QueryString.Get("error")));

                return;

            }

            if (context.Request.QueryString.Get("code") == null

                || context.Request.QueryString.Get("state") == null)

            {

                output("Malformed authorization response. " + context.Request.QueryString);

                return;

            }



            // extracts the code

            var code = context.Request.QueryString.Get("code");

            var incoming_state = context.Request.QueryString.Get("state");



            // Compares the receieved state to the expected value, to ensure that

            // this app made the request which resulted in authorization.

            if (incoming_state != state)

            {

                output(String.Format("Received request with invalid state ({0})", incoming_state));

                return;

            }

            output("Authorization code: " + code);



            // Starts the code exchange at the Token Endpoint.

            performCodeExchange(code, code_verifier, redirectURI);

        }



        async void performCodeExchange(string code, string code_verifier, string redirectURI)

        {

            output("Exchanging code for tokens...");



            // builds the  request

            string tokenRequestURI = "https://www.googleapis.com/oauth2/v4/token";

            string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&scope=&grant_type=authorization_code",

                code,

                System.Uri.EscapeDataString(redirectURI),

                clientID,

                code_verifier,

                clientSecret

                );



            // sends the request

            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(tokenRequestURI);

            tokenRequest.Method = "POST";

            tokenRequest.ContentType = "application/x-www-form-urlencoded";

            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            byte[] _byteVersion = Encoding.ASCII.GetBytes(tokenRequestBody);

            tokenRequest.ContentLength = _byteVersion.Length;

            Stream stream = tokenRequest.GetRequestStream();

            await stream.WriteAsync(_byteVersion, 0, _byteVersion.Length);

            stream.Close();



            try

            {

                // gets the response

                WebResponse tokenResponse = await tokenRequest.GetResponseAsync();

                using (StreamReader reader = new StreamReader(tokenResponse.GetResponseStream()))

                {

                    // reads response body

                    string responseText = await reader.ReadToEndAsync();

                    output(responseText);



                    // converts to dictionary

                    Dictionary<string, string> tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);



                    string access_token = tokenEndpointDecoded["access_token"];

                    userinfoCall(access_token);

                }

            }

            catch (WebException ex)

            {

                if (ex.Status == WebExceptionStatus.ProtocolError)

                {

                    var response = ex.Response as HttpWebResponse;

                    if (response != null)

                    {

                        output("HTTP: " + response.StatusCode);

                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))

                        {

                            // reads response body

                            string responseText = await reader.ReadToEndAsync();

                            output(responseText);



                        }

                    }



                }

            }

        }
        async void userinfoCall(string access_token)

        {

            output("Making API Call to Userinfo...");



            // builds the  request

            string userinfoRequestURI = "https://www.googleapis.com/oauth2/v3/userinfo";



            // sends the request

            HttpWebRequest userinfoRequest = (HttpWebRequest)WebRequest.Create(userinfoRequestURI);

            userinfoRequest.Method = "GET";

            userinfoRequest.Headers.Add(string.Format("Authorization: Bearer {0}", access_token));

            userinfoRequest.ContentType = "application/x-www-form-urlencoded";

            userinfoRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";



            // gets the response

            WebResponse userinfoResponse = await userinfoRequest.GetResponseAsync();

            using (StreamReader userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream()))

            {

                // reads response body

                string userinfoResponseText = await userinfoResponseReader.ReadToEndAsync();

                output(userinfoResponseText);

                UserLogin.LoginGoogle(userinfoResponseText);

            }

        }


        /// <summary>

        /// Appends the given string to the on-screen log, and the debug console.

        /// </summary>

        /// <param name="output">string to be appended</param>

        public void output( string output)
        {

            //textBoxOutput.Text = textBoxOutput.Text + output + Environment.NewLine;
            log.Info("Google api: " + output + Environment.NewLine);

        }


    }

}
