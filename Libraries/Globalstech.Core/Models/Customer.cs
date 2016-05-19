using System;
using Globalstech.Core.Models.BaseEntity;

namespace Globalstech.Core.Models {
    public class Customer:BaseEntity.BaseEntity {

        /// <summary>
        /// GUID
        /// </summary>
        public Guid CustomerGuid {
            get;
            set;
        }
        /// <summary>
        /// 用户名（登陆）
        /// </summary>
        public string Username {
            get;
            set;
        }
        /// <summary>
        /// 用户名(显示)
        /// </summary>
        public string DisplayName {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email {
            get;
            set;
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password {
            get;
            set;
        }
        /// <summary>
        /// 是否在活动
        /// </summary>
        public bool Active {
            get;
            set;
        }
        /// <summary>
        /// 是否是系统账户
        /// </summary>
        public bool IsSystemAccount {
            get;
            set;
        }
        /// <summary>
        /// 最后登录地址
        /// </summary>
        public string LastIpAddress {
            get;
            set;
        }
        /// <summary>
        ///最后登录时间 
        /// </summary>
        public DateTime LastActivityDateUtc {
            get;
            set;
        }

    }
}
