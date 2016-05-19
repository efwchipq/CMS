using System;
using System.Web;
using System.Web.Security;
using Globalstech.Core.Models;

namespace Nop.Services.Authentication {
    /// <summary>
    /// Authentication service
    /// </summary>
    public partial class FormsAuthenticationService : IAuthenticationService {
        #region Fields

        private readonly HttpContextBase _httpContext;
        //private readonly ICustomerService _customerService;
        //private readonly CustomerSettings _customerSettings;
        private readonly TimeSpan _expirationTimeSpan;

        private Customer _cachedCustomer;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor(IOC���캯��ע��)
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="customerSettings">Customer settings</param>
        public FormsAuthenticationService(HttpContextBase httpContext) {
            this._httpContext = httpContext;
            //this._customerService = customerService;
            //this._customerSettings = customerSettings;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// ͨ��Ʊ֤��ȡ�û�
        /// </summary>
        /// <param name="ticket">Ticket</param>
        /// <returns>Customer</returns>
        protected virtual Customer GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket) {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var usernameOrEmail = ticket.UserData;

            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;
            //var customer = _customerSettings.UsernamesEnabled
            //    ? _customerService.GetCustomerByUsername(usernameOrEmail)
            //    : _customerService.GetCustomerByEmail(usernameOrEmail);
            var customer = new Customer();
            return customer;
        }

        #endregion

        #region Methods

        /// <summary>
        /// ��½
        /// </summary>
        /// <param name="customer">�û�</param>
        /// <param name="createPersistentCookie">�Ƿ񴴽��־���Cookie</param>
        public virtual void SignIn(Customer customer, bool createPersistentCookie) {
            var now = DateTime.UtcNow.ToLocalTime();
            //Ʊ֤
            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                 customer.Username,
                now,
                now.Add(_expirationTimeSpan),
                createPersistentCookie,
                 customer.Username,
                FormsAuthentication.FormsCookiePath);
            //����Ʊ֤
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //��װƱ֤��Cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent) {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null) {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }
            //����Cookie����������
            _httpContext.Response.Cookies.Add(cookie);
            _cachedCustomer = customer;
        }

        /// <summary>
        /// �ǳ�
        /// </summary>
        public virtual void SignOut() {
            _cachedCustomer = null;
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// ��ȡ��֤���û�
        /// </summary>
        /// <returns>Customer</returns>
        public virtual Customer GetAuthenticatedCustomer() {
            if (_cachedCustomer != null)
                return _cachedCustomer;

            if (_httpContext == null ||
                _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity)) {
                return null;
            }

            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);
            if (customer != null && customer.Active && !customer.IsDeleted)
                _cachedCustomer = customer;
            return _cachedCustomer;
        }

        #endregion

    }
}