using Globalstech.Core.Models;

namespace Nop.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService 
    {
        /// <summary>
        /// ��½
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="createPersistentCookie">A value indicating whether to create a persistent cookie</param>
        void SignIn(Customer customer, bool createPersistentCookie);

        /// <summary>
        /// �ǳ�
        /// </summary>
        void SignOut();

        /// <summary>
        /// ��ȡ��֤���û�
        /// </summary>
        /// <returns>Customer</returns>
        Customer GetAuthenticatedCustomer();
    }
}