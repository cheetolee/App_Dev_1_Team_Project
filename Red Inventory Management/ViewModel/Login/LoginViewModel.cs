using System;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using BusinessLayer;
using Inventory_Management.Model;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using Inventory_Management.Views;
using Inventory_Management.API;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using Firebase.Auth;

namespace Inventory_Management.ViewModel
{
    class LoginViewModel : BindableBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // client configuration
        const string clientID = "581786658708-elflankerquo1a6vsckabbhn25hclla0.apps.googleusercontent.com";
        const string clientSecret = "3f6NggMbPtrmIBpgx-MK2xXK";
        const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        const string tokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
        const string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";
        public string webApiKey = "AIzaSyAN7SreA9VJT1qaMuFkQeC6UvxAN7Ti3g8";

        private string _userID;
        private string _password;
        public Window LoginWindow { get; set; }
        public string UserID
        {
            get
            {
                if (_userID == null) _userID = "";
                return _userID;
            }
            set { SetProperty(ref _userID, value); }
        }
        public string Password
        {
            get
            {
                if (_password == null) _password = "";
                return _password;
            }
            set { SetProperty(ref _password, value); }
        }

        private ICommand _click_LoginCommand;
        public ICommand Click_LoginCommand
        {
            get
            {
                if (_click_LoginCommand == null) _click_LoginCommand = new RelayCommand(new Action<object>(Login));
                return _click_LoginCommand;
            }
            set { SetProperty(ref _click_LoginCommand, value); }
        }

        private void Login(object parameter)
        {
            log.Debug("Login button");

            PasswordBox pwBox = (PasswordBox)parameter;
            Password = pwBox.Password;

            if (!DatabaseConnection.TestConnection())
            {
                log.Error("Database connection error Please set the database connection, before login.");
            }
            else
            {
           
                try
                {
                    UserLogin.Login(UserID, Password);
                    log.Info(String.Format("Welcome, {0}!", UserID) + "You have succesfully logged in.");
                    LoginWindow.Close();
                }
                catch (ArgumentException e)
                {
                    log.Error("Login error :" + e.Message);
                    MessageBox.Show(e.Message, "Login error ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        private ICommand _click_RegisterCommand;
        public ICommand Click_RegisterCommand
        {
            get
            {
                if (_click_RegisterCommand == null) _click_RegisterCommand = new RelayCommand(new Action<object>(Register));
                return _click_RegisterCommand;
            }
            set { SetProperty(ref _click_RegisterCommand, value); }
        }

        private void Register(object parameter)
        {
            Views.NewUserWindow newUserWindow = new NewUserWindow();
            newUserWindow.ShowDialog();
        }


        private ICommand _click_LoginFBCommand;
        public ICommand Click_LoginFBCommand
        {
            get
            {
                if (_click_LoginFBCommand == null) _click_LoginFBCommand = new RelayCommand(new Action<object>(LoginFB));
                return _click_LoginFBCommand;
            }
            set { SetProperty(ref _click_LoginFBCommand, value); }
        }

        private const string AppId = "798673734733320";
        private const string Scopes = "user_about_me,publish_stream,offline_access";

        private void LoginFB(object sender)
        {
            FBDialog fbd = new FBDialog(AppId, Scopes);
            fbd.Owner = LoginWindow;
            fbd.Show();
        }

        private ICommand _click_LoginGoogleCommand;
        public ICommand Click_LoginGoogleCommand
        {
            get
            {
                if (_click_LoginGoogleCommand == null) _click_LoginGoogleCommand = new RelayCommand(new Action<object>(LoginGoogle));
                return _click_LoginGoogleCommand;
            }
            set { SetProperty(ref _click_LoginGoogleCommand, value); }
        }

        private async void LoginGoogle(object sender)
        {
            // Generates state and PKCE values.
            string state = Utils.randomDataBase64url(32);
            string code_verifier = Utils.randomDataBase64url(32);
            string code_challenge = Utils.base64urlencodeNoPadding(Utils.sha256(code_verifier));
            const string code_challenge_method = "S256";

            // Creates a redirect URI using an available port on the loopback address.
            string redirectURI = string.Format("http://{0}:{1}/", IPAddress.Loopback, Utils.GetRandomUnusedPort());
            log.Info("redirect URI: " + redirectURI);

            // Creates an HttpListener to listen for requests on that redirect URI.
            var http = new HttpListener();
            http.Prefixes.Add(redirectURI);
            log.Info("Listening..");
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

            // Brings this app back to the foreground.
            LoginWindow.Activate();

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
                log.Info(String.Format("OAuth authorization error: {0}.", context.Request.QueryString.Get("error")));
                return;
            }
            if (context.Request.QueryString.Get("code") == null
                || context.Request.QueryString.Get("state") == null)
            {
                log.Info("Malformed authorization response. " + context.Request.QueryString);
                return;
            }

            // extracts the code
            var code = context.Request.QueryString.Get("code");
            var incoming_state = context.Request.QueryString.Get("state");

            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization.
            if (incoming_state != state)
            {
                log.Debug(String.Format("Received request with invalid state ({0})", incoming_state));
                return;
            }
            log.Info("Authorization code: " + code);

            // Starts the code exchange at the Token Endpoint.
            performCodeExchange(code, code_verifier, redirectURI);
        }

        async void performCodeExchange(string code, string code_verifier, string redirectURI)
        {
            log.Info("Exchanging code for tokens...");

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
                    log.Info(responseText);

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
                        log.Info("HTTP: " + response.StatusCode);
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            // reads response body
                            string responseText = await reader.ReadToEndAsync();
                            log.Debug(responseText);
                        }
                    }

                }
            }
        }


        async void userinfoCall(string access_token)
        {
            log.Info("Making API Call to Userinfo...");

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
                try
                {
                    UserLogin.Login("admin", "admin");
                    log.Info(String.Format("Welcome, {0}!", UserID) + "You have succesfully logged in.");
                    LoginWindow.Close();
                }
                catch (ArgumentException e)
                {
                    log.Error("Login error :" + e.Message);
                    MessageBox.Show(e.Message, "Login error ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private ICommand _click_LoginEmailCommand;
        public ICommand Click_LoginEmailCommand
        {
            get
            {
                if (_click_LoginEmailCommand == null) _click_LoginEmailCommand = new RelayCommand(new Action<object>(EmailLoginAsync));
                return _click_LoginEmailCommand;
            }
            set { SetProperty(ref _click_LoginEmailCommand, value); }
        }

        private async void EmailLoginAsync(object parameter)
        {
            log.Debug("Email Login button");

                PasswordBox pwBox = (PasswordBox)parameter;
                Password = pwBox.Password;

            try
            {

                var auth = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                var b = await auth.CreateUserWithEmailAndPasswordAsync(UserID, Password, UserID, false);
                var a = await auth.SignInWithEmailAndPasswordAsync(UserID, Password);
                string token = a.FirebaseToken;
                var user = a.User;
                if (token != "")
                {
                    UserLogin.Login("admin", "admin");
                    log.Info(String.Format("Welcome, {0}!", UserID) + "You have succesfully logged in.");
                    LoginWindow.Close();
                }
            }
            catch (ArgumentException e)
            {
                log.Error("Login error :" + e.Message);
                MessageBox.Show(e.Message, "Login error ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FirebaseAuthException e)
            {
                if (e.Message.Contains("EMAIL_EXISTS"))
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(webApiKey));
                    var a = await auth.SignInWithEmailAndPasswordAsync(UserID, Password);
                    string token = a.FirebaseToken;
                    if (token != "")
                    {
                        UserLogin.Login("admin", "admin");
                        log.Info(String.Format("Welcome, {0}!", UserID) + "You have succesfully logged in.");
                        LoginWindow.Close();
                    }
                }
                else
                {
                    log.Error("Firebase Login error :" + e.Message);
                    MessageBox.Show("Please provide valid login details", "Login error ",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }


    }
}
